using HR_Medical_Records.DTOs.BaseResponse;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.DTOs.PaginationDTO;

namespace HR_Medical_Records.Service.Interface
{
    /// <summary>
    /// Interface for MedicalRecord service operations.
    /// Provides methods to retrieve, add, update, delete, and filter medical records.
    /// </summary>
    public interface IMedicalRecordService
    {
        /// <summary>
        /// Retrieves a medical record by its unique identifier.
        /// </summary>
        /// <param name="medicalRecordId">The ID of the medical record to retrieve.</param>
        /// <returns>A BaseResponse containing a MedicalRecordDTO if found.</returns>
        Task<BaseResponse<MedicalRecordDTO>> GetMedicalRecordById(int medicalRecordId);

        /// <summary>
        /// Adds a new medical record or updates an existing one.
        /// </summary>
        /// <param name="request">The medical record creation or update request.</param>
        /// <param name="userId">The ID of the user performing the action.</param>
        /// <returns>A BaseResponse containing a SimpleMedicalRecordDTO.</returns>
        Task<BaseResponse<SimpleMedicalRecordDTO>> AddUpdateMedicalRecord(CreateAndUpdateMedicalRecord request, Guid userId);

        /// <summary>
        /// Soft deletes a medical record by setting the end date and status to inactive.
        /// </summary>
        /// <param name="request">The soft delete request.</param>
        /// <param name="userId">The ID of the user performing the deletion.</param>
        /// <returns>A BaseResponse containing a SimpleMedicalRecordDTO after deletion.</returns>
        Task<BaseResponse<SimpleMedicalRecordDTO>> DeleteMedicalRecord(SoftDeleteMedicalRecord request, Guid userId);

        /// <summary>
        /// Retrieves a list of medical records based on filter criteria, with pagination.
        /// </summary>
        /// <param name="request">The filter request containing the filter criteria.</param>
        /// <returns>A BaseResponse containing a PaginationDTO with the filtered list of MedicalRecordDTOs.</returns>
        Task<BaseResponse<PaginationDTO<MedicalRecordDTO>>> GetFilterMedicalRecords(MedicalRecordFilterRequest request);
    }
}
