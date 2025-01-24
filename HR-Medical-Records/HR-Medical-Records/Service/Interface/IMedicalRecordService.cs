using HR_Medical_Records.DTOs.BaseResponse;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;

namespace HR_Medical_Records.Service.Interface
{
    public interface IMedicalRecordService
    {
        Task<BaseResponse<MedicalRecordDTO>> GetMedicalRecordById(int medicalRecordId);
        Task<BaseResponse<SimpleMedicalRecordDTO>> AddMedicalRecord(CreateMedicalRecord request, Guid userId);
        Task<BaseResponse<SimpleMedicalRecordDTO>> DeleteMedicalRecord(SoftDeleteMedicalRecord request, Guid userId);
    }
}
