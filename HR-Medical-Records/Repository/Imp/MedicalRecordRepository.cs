using HR_Medical_Records.Data;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Helpers;
using HR_Medical_Records.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_Medical_Records.Repository.Imp
{
    /// <summary>
    /// Implements the contract for interacting with <see cref="TMedicalRecord"/> entities in the data storage.
    /// Provides methods for retrieving, registering, updating, and filtering medical records.
    /// </summary>
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly HRContext _context;

        public MedicalRecordRepository(HRContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all <see cref="TMedicalRecord"/> entities that match the provided filter criteria.
        /// </summary>
        /// <param name="filter">The filter criteria for retrieving medical records.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IQueryable{TMedicalRecord}"/> collection of <see cref="TMedicalRecord"/> entities matching the filter criteria.</returns>
        public async Task<IQueryable<TMedicalRecord>> GetAllWithFilter(MedicalRecordFilterRequest filter)
        {
            var query = _context.TMedicalRecords.AsNoTracking()
                                        .Include(x => x.MedicalRecordType)
                                        .Include(x => x.Status)
                                        .AsQueryable();

            if (filter.FieldFilter != null)
                query = query.Where(x => x.FileId == filter.FieldFilter);

            if (filter.StatusId != null)
                query = query.Where(x => x.StatusId == filter.StatusId);

            if (filter.MedicalRecordTypeId != null)
                query = query.Where(x => x.MedicalRecordTypeId == filter.MedicalRecordTypeId);

            if (filter.StartDate.HasValue)
                query = query.Where(x => x.StartDate >= filter.StartDate);

            if (filter.EndDate.HasValue)
                query = query.Where(x => x.EndDate <= filter.EndDate);

            return query;
        }

        /// <summary>
        /// Retrieves a <see cref="TMedicalRecord"/> entity by its unique identifier.
        /// </summary>
        /// <param name="medicalRecordId">The unique identifier of the medical record.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="TMedicalRecord"/> with the specified ID, or null if not found.</returns>
        public async Task<TMedicalRecord?> GetById(int medicalRecordId)
        {
            return await _context.TMedicalRecords.AsNoTracking()
                                            .Include(x => x.Status)
                                            .Include(x => x.MedicalRecordType)
                                            .FirstOrDefaultAsync(x => x.MedicalRecordId == medicalRecordId);
        }

        /// <summary>
        /// Registers a new <see cref="TMedicalRecord"/> entity.
        /// </summary>
        /// <param name="request">The <see cref="TMedicalRecord"/> entity to be registered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the registered <see cref="TMedicalRecord"/> entity.</returns>
        public async Task<TMedicalRecord> Register(TMedicalRecord request)
        {
            await _context.TMedicalRecords.AddAsync(request);
            int affectedRows = await _context.SaveChangesAsync();
            BaseResponseHelper.AffectedRows = affectedRows;
            return request;
        }

        /// <summary>
        /// Updates an existing <see cref="TMedicalRecord"/> entity.
        /// </summary>
        /// <param name="medicalRecord">The <see cref="TMedicalRecord"/> entity to be updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see cref="TMedicalRecord"/> entity.</returns>
        public async Task<TMedicalRecord> Update(TMedicalRecord medicalRecord)
        {
            _context.TMedicalRecords.Update(medicalRecord);
            int affectedRows = await _context.SaveChangesAsync();
            BaseResponseHelper.AffectedRows = affectedRows;
            return medicalRecord;
        }
    }
}
