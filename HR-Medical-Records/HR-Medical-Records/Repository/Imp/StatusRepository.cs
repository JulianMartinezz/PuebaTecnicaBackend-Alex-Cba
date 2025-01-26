using HR_Medical_Records.Data;
using HR_Medical_Records.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_Medical_Records.Repository.Imp
{
    /// <summary>
    /// Repository implementation for accessing and manipulating <see cref="Status"/> entities in the database.
    /// Provides methods to retrieve <see cref="Status"/> by its ID or name.
    /// </summary>
    public class StatusRepository : IStatusRepository
    {
        private readonly HRContext _context;

        public StatusRepository(HRContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a <see cref="Status"/> entity by its unique identifier.
        /// </summary>
        /// <param name="statusid">The unique identifier of the status.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Status"/> with the specified ID, or null if not found.</returns>
        public async Task<Status?> GetById(int statusid)
        {
            return await _context.Statuses.AsNoTracking().FirstOrDefaultAsync(x => x.StatusId == statusid);
        }

        /// <summary>
        /// Retrieves a <see cref="Status"/> entity by its name.
        /// </summary>
        /// <param name="name">The name of the status.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Status"/> with the specified name, or null if not found.</returns>
        public async Task<Status?> GetByName(string name)
        {
            return await _context.Statuses.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
