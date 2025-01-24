using AutoMapper;
using HR_Medical_Records.DTOs.BaseResponse;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Helpers;
using HR_Medical_Records.Models;
using HR_Medical_Records.Repository;
using HR_Medical_Records.Service.Interface;
using HR_Medical_Records.Service.Validator;

namespace HR_Medical_Records.Service.Imp
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MedicalRecordService> _logger;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<MedicalRecordDTO>> GetMedicalRecordById(int medicalRecordId)
        {
            var medicalRecord = await _medicalRecordRepository.GetById(medicalRecordId);

            if (medicalRecord == null)
            {
                throw new KeyNotFoundException($"Medical Record not found with Id:{medicalRecordId}");
            }
            
            var result = _mapper.Map<MedicalRecordDTO>(medicalRecord);

            return BaseResponseHelper.GetSuccessful(result);
        }

        public async Task<BaseResponse<SimpleMedicalRecordDTO>> AddMedicalRecord(CreateMedicalRecord request)
        {
            var newMedicalRecord = await _medicalRecordRepository.Register(request);

            return newMedicalRecord;
        }
    }
}
