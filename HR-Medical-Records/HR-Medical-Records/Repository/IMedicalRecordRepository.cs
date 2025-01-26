using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Models;

namespace HR_Medical_Records.Repository
{
    /// <summary>
    /// Defines the contract for interacting with <see cref="TMedicalRecord"/> entities in the data storage.
    /// Provides methods for retrieving, registering, updating, and filtering medical records.
    /// </summary>
    public interface IMedicalRecordRepository
    {
        /// <summary>
        /// Retrieves a <see cref="TMedicalRecord"/> entity by its unique identifier.
        /// </summary>
        /// <param name="medicalRecordId">The unique identifier of the medical record.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="TMedicalRecord"/> with the specified ID, or null if not found.</returns>
        Task<TMedicalRecord?> GetById(int medicalRecordId);

        /// <summary>
        /// Registers a new <see cref="TMedicalRecord"/> entity.
        /// </summary>
        /// <param name="request">The <see cref="TMedicalRecord"/> entity to be registered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the registered <see cref="TMedicalRecord"/> entity.</returns>
        Task<TMedicalRecord> Register(TMedicalRecord request);

        /// <summary>
        /// Updates an existing <see cref="TMedicalRecord"/> entity.
        /// </summary>
        /// <param name="medicalRecord">The <see cref="TMedicalRecord"/> entity to be updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see cref="TMedicalRecord"/> entity.</returns>
        Task<TMedicalRecord> Update(TMedicalRecord medicalRecord);

        /// <summary>
        /// Retrieves all <see cref="TMedicalRecord"/> entities that match the given filter criteria.
        /// </summary>
        /// <param name="filter">The filter criteria for retrieving medical records.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IQueryable{TMedicalRecord}"/> collection of <see cref="TMedicalRecord"/> entities matching the filter criteria.</returns>
        Task<IQueryable<TMedicalRecord>> GetAllWithFilter(MedicalRecordFilterRequest filter);
    }
}
