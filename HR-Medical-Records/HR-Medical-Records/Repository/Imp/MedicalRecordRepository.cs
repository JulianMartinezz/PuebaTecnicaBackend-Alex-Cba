using AutoMapper;
using HR_Medical_Records.Data;
using HR_Medical_Records.DTOs.BaseResponse;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Helpers;
using HR_Medical_Records.Models;
using HR_Medical_Records.Service.Validator;
using Microsoft.EntityFrameworkCore;

namespace HR_Medical_Records.Repository.Imp
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly HRContext _context;
        private readonly IMapper _mapper;

        public MedicalRecordRepository(HRContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TMedicalRecord?> GetById(int medicalRecordId)
        {
            return await _context.TMedicalRecords.FirstOrDefaultAsync(x => x.MedicalRecordId == medicalRecordId);
        }

        public async Task<BaseResponse<SimpleMedicalRecordDTO>> Register(CreateMedicalRecord request)
        {
            var validator = new CreateMedicalRecordValidator(_context);
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BaseResponseHelper.CreateError<SimpleMedicalRecordDTO>($"Validation failed: {validationErrors}", 400);
            }

            var medicalRecord = _mapper.Map<TMedicalRecord>(request);
            await _context.TMedicalRecords.AddAsync(medicalRecord);

            var result = _mapper.Map<SimpleMedicalRecordDTO>(medicalRecord);
            return BaseResponseHelper.GetSuccessful(result);
        }
    }
}
