using HR_Medical_Records.Data;
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

        public async Task<TMedicalRecord?> GetById(int medicalRecordId)
        {
            return await _context.TMedicalRecords.FirstOrDefaultAsync(x => x.MedicalRecordId == medicalRecordId);
        }

        public async Task<TMedicalRecord> Register(TMedicalRecord request)
        {
            await _context.TMedicalRecords.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}
