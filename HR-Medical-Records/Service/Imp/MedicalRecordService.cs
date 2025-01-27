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
    /// <summary>
    /// Service class responsible for handling medical record operations including retrieval, creation, updating, and deletion.
    /// </summary>
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

        /// <summary>
        /// Retrieves a medical record by its ID.
        /// </summary>
        /// <param name="medicalRecordId">The ID of the medical record.</param>
        /// <returns>A task representing the asynchronous operation with a successful response containing the medical record data.</returns>
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

        /// <summary>
        /// Adds or updates a medical record depending on whether the ID is provided in the request.
        /// </summary>
        /// <param name="request">The request containing medical record data to be added or updated.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        /// <returns>A task representing the asynchronous operation with a successful response containing the medical record data.</returns>
        public async Task<BaseResponse<SimpleMedicalRecordDTO>> AddUpdateMedicalRecord(CreateAndUpdateMedicalRecord request, Guid userId)
        {

            if (!request.MedicalRecordId.HasValue)
            {
                request.CreatedBy = userId.ToString();
                request.CreationDate = DateOnly.FromDateTime(DateTime.UtcNow);
                await CheckValidator(request);

                var medicalRecord = _mapper.Map<TMedicalRecord>(request);

                var medicalrecord = await _medicalRecordRepository.Register(medicalRecord);
                var result = _mapper.Map<SimpleMedicalRecordDTO>(medicalrecord);
                return BaseResponseHelper.CreateSuccessful(result);
            }
            else
            {
                request.ModifiedBy = userId.ToString();
                request.ModificationDate = DateOnly.FromDateTime(DateTime.UtcNow);
                await CheckValidator(request);

                var medicalRecord = _mapper.Map<TMedicalRecord>(request);

                var medicalrecord = await _medicalRecordRepository.Update(medicalRecord);

                var result = _mapper.Map<SimpleMedicalRecordDTO>(medicalrecord);
                return BaseResponseHelper.UpdateSuccessful(result);
            }
        }

        /// <summary>
        /// Soft deletes a medical record by setting its status to "Inactive" and updating related fields.
        /// </summary>
        /// <param name="request">The request containing the medical record ID and deletion reason.</param>
        /// <param name="userId">The ID of the user performing the deletion.</param>
        /// <returns>A task representing the asynchronous operation with a successful response indicating the deletion status.</returns>
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

        /// <summary>
        /// Retrieves medical records that match the provided filter criteria.
        /// </summary>
        /// <param name="request">The filter criteria to search for medical records.</param>
        /// <returns>A task representing the asynchronous operation with a successful response containing a paginated list of medical records.</returns>
        public async Task<BaseResponse<PaginationDTO<MedicalRecordDTO>>> GetFilterMedicalRecords(MedicalRecordFilterRequest request)
        {
            var medicalRecordQuery = _medicalRecordRepository.GetAllWithFilter(request);

            if (medicalRecordQuery == null || !medicalRecordQuery.Any())
            {
                LogFiltersAsWarning(request);
                throw new KeyNotFoundException($"Medicals Records not found");
            }

            var totalRegister = await medicalRecordQuery.CountAsync();

            medicalRecordQuery = SortingHelper.ApplySortingAndPagination(medicalRecordQuery, request, true);

            var medicalRecord = await medicalRecordQuery.ToListAsync();
            var medicalRecordDTOs = _mapper.Map<List<MedicalRecordDTO>>(medicalRecord);

            var result = new PaginationDTO<MedicalRecordDTO>()
            {
                Items = medicalRecordDTOs,
                TotalRegisters = totalRegister
            };

            return BaseResponseHelper.GetSuccessful(result, totalRegister);
        }

        /// <summary>
        /// Validates a request using a specific validator.
        /// </summary>
        /// <typeparam name="T">The type of request to be validated.</typeparam>
        /// <param name="requestCommand">The request to be validated.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Logs the filter parameters when no medical records are found with the provided filters.
        /// </summary>
        /// <param name="filter">The filter parameters used for the search.</param>
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
