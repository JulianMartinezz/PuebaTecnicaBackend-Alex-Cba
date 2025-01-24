
using HR_Medical_Records.DTOs.BaseResponse;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Models;

namespace HR_Medical_Records.Repository
{
    public interface IMedicalRecordRepository
    {
        Task<TMedicalRecord?> GetById(int medicalRecordId);
        Task<TMedicalRecord> Register(TMedicalRecord request);
    }
}
