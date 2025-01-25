using HR_Medical_Records.DTOs.BaseResponse;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.DTOs.PaginationDTO;

namespace HR_Medical_Records.Service.Interface
{
    public interface IMedicalRecordService
    {
        Task<BaseResponse<MedicalRecordDTO>> GetMedicalRecordById(int medicalRecordId);
        Task<BaseResponse<SimpleMedicalRecordDTO>> AddUpdateMedicalRecord(CreateAndUpdateMedicalRecord request, Guid userId);
        Task<BaseResponse<SimpleMedicalRecordDTO>> DeleteMedicalRecord(SoftDeleteMedicalRecord request, Guid userId);
        Task<BaseResponse<PaginationDTO<MedicalRecordDTO>>> GetFilterMedicalRecords(MedicalRecordFilterRequest request);
    }
}
