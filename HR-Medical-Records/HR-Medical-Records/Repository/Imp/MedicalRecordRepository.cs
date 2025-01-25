using HR_Medical_Records.Data;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Helpers;
using HR_Medical_Records.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_Medical_Records.Repository.Imp
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly HRContext _context;

        public MedicalRecordRepository(HRContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<TMedicalRecord>> GetAllWithFilter(MedicalRecordFilterRequest filter)
        {
            var query = _context.TMedicalRecords.AsNoTracking()
                                                            .Include(x => x.MedicalRecordType)
                                                            .Include(x => x.Status)
                                                            .Where(x =>
                                                                    filter.FieldFilter != null && x.FileId == filter.FieldFilter ||
                                                                    filter.StatusId != null && x.StatusId == filter.StatusId ||
                                                                    filter.MedicalRecordTypeId != null && x.MedicalRecordTypeId == filter.MedicalRecordTypeId ||
                                                                    filter.StartDate.HasValue && x.StartDate >= filter.StartDate ||
                                                                    filter.EndDate.HasValue && x.EndDate <= filter.EndDate)
                                                            .AsQueryable();

            return query;
        }

        public async Task<TMedicalRecord?> GetById(int medicalRecordId)
        {
            return await _context.TMedicalRecords.AsNoTracking()
                                            .Include(x => x.Status)
                                            .Include(x => x.MedicalRecordType)
                                            .FirstOrDefaultAsync(x => x.MedicalRecordId == medicalRecordId);
        }

        public async Task<TMedicalRecord> Register(TMedicalRecord request)
        {
            await _context.TMedicalRecords.AddAsync(request);
            int affectedRows = await _context.SaveChangesAsync();
            BaseResponseHelper.AffectedRows = affectedRows;
            return request;
        }

        public async Task<TMedicalRecord> Update(TMedicalRecord medicalRecord)
        {
            _context.TMedicalRecords.Update(medicalRecord);
            int affectedRows = await _context.SaveChangesAsync();
            BaseResponseHelper.AffectedRows = affectedRows;
            return medicalRecord;
        }
    }
}
