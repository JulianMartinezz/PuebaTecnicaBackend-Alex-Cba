using HR_Medical_Records.Data;
using HR_Medical_Records.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_Medical_Records.Repository.Imp
{
    public class StatusRepository : IStatusRepository
    {
        private readonly HRContext _context;

        public StatusRepository(HRContext context)
        {
            _context = context;
        }

        public async Task<Status?> GetById(int statusid)
        {
            return await _context.Statuses.AsNoTracking().FirstOrDefaultAsync(x => x.StatusId == statusid);
        }

        public async Task<Status?> GetByName(string name)
        {
            return await _context.Statuses.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
