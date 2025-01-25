using AutoMapper;
using FluentValidation;
using HR_Medical_Records.DTOs.BaseResponse;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.DTOs.PaginationDTO;
using HR_Medical_Records.Helpers;
using HR_Medical_Records.Middleware.Exceptions;
using HR_Medical_Records.Models;
using HR_Medical_Records.Repository;
using HR_Medical_Records.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace HR_Medical_Records.Service.Imp
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MedicalRecordService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository,
                                    IStatusRepository statusRepository,
                                    IMapper mapper,
                                    ILogger<MedicalRecordService> logger,
                                    IServiceProvider serviceProvider)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
            _statusRepository = statusRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<MedicalRecordDTO>> GetMedicalRecordById(int medicalRecordId)
        {
            var medicalRecord = await _medicalRecordRepository.GetById(medicalRecordId);

            if (medicalRecord == null)
            {
                throw new KeyNotFoundException($"Medical Record not found with Id:{medicalRecordId}");
            }
            
            var result = _mapper.Map<MedicalRecordDTO>(medicalRecord);

            return BaseResponseHelper.GetSuccessful(result, 1);
        }

        public async Task<BaseResponse<SimpleMedicalRecordDTO>> AddUpdateMedicalRecord(CreateAndUpdateMedicalRecord request, Guid userId)
        {
            TMedicalRecord medicalRecordToMapped = null;

            if (!request.MedicalRecordId.HasValue)
            {
                request.CreatedBy = userId.ToString();
                request.CreationDate = DateOnly.FromDateTime(DateTime.UtcNow);
                await CheckValidator(request);

                var medicalRecord = _mapper.Map<TMedicalRecord>(request);

                medicalRecordToMapped = await _medicalRecordRepository.Register(medicalRecord);
            }
            else
            {
                request.ModifiedBy = userId.ToString();
                request.ModificationDate = DateOnly.FromDateTime(DateTime.UtcNow);
                await CheckValidator(request);

                var medicalRecord = _mapper.Map<TMedicalRecord>(request);

                medicalRecordToMapped = await _medicalRecordRepository.Update(medicalRecord);
            }

            var result = _mapper.Map<SimpleMedicalRecordDTO>(medicalRecordToMapped);
            return BaseResponseHelper.CreateSuccessful(result);
        }

        public async Task<BaseResponse<SimpleMedicalRecordDTO>> DeleteMedicalRecord(SoftDeleteMedicalRecord request, Guid userId)
        {
            await CheckValidator(request);

            var medicalRecord = await _medicalRecordRepository.GetById(request.MedicalRecordId);

            medicalRecord.EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
            medicalRecord.DeletionDate = DateOnly.FromDateTime(DateTime.UtcNow);
            medicalRecord.DeletionReason = request.DeletionReason;

            var statusDb = await _statusRepository.GetByName("Inactive");

            medicalRecord.Status = statusDb;
            medicalRecord.StatusId = statusDb.StatusId;
            medicalRecord.DeletedBy = userId.ToString();

            var updatedMedicalRecord = await _medicalRecordRepository.Update(medicalRecord);

            var result = _mapper.Map<SimpleMedicalRecordDTO>(updatedMedicalRecord);
            return BaseResponseHelper.SoftDeleteSuccessful(result);
        }

        public async Task<BaseResponse<PaginationDTO<MedicalRecordDTO>>> GetFilterMedicalRecords(MedicalRecordFilterRequest request)
        {
            var medicalRecord = await _medicalRecordRepository.GetAllWithFilter(request);

            if (medicalRecord == null || !medicalRecord.Any())
            {
                LogFiltersAsWarning(request);
                throw new KeyNotFoundException($"Medicals Records not found");
            }

            var totalRegister = await medicalRecord.CountAsync();

            medicalRecord = SortingHelper.ApplySortingAndPagination(medicalRecord, request, true);

            var medicalRecordDTOs = _mapper.Map<List<MedicalRecordDTO>>(medicalRecord);

            var result = new PaginationDTO<MedicalRecordDTO>()
            {
                Items = medicalRecordDTOs,
                TotalRegisters = totalRegister
            };

            return BaseResponseHelper.GetSuccessful(result, totalRegister);
        }

        private async Task CheckValidator<T>(T requestCommand) where T : class
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator == null)
            {
                _logger.LogCritical($"Validator Not Found for {typeof(T).Name}");
                throw new Exception($"No validator found for the type {typeof(T).Name}");
            }

            var validationResult = await validator.ValidateAsync(requestCommand);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ExceptionBadRequestClient($"Validation failed: {validationErrors}");
            }
        }

        private void LogFiltersAsWarning(object filter)
        {
            var filterProperties = filter.GetType().GetProperties();
            var logBuilder = new StringBuilder("No medical records found with the provided filters:\n");

            foreach (var property in filterProperties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(filter, null) ?? "null";

                logBuilder.AppendLine($"- {propertyName}: {propertyValue}");
            }

            _logger.LogWarning(logBuilder.ToString());
        }
    }
}
