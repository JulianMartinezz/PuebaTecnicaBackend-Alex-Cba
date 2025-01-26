using HR_Medical_Records.Models;

namespace HR_Medical_Records.Repository
{
    /// <summary>
    /// Interface for the repository responsible for handling operations related to the <see cref="Status"/> entity.
    /// Provides methods to retrieve <see cref="Status"/> by its ID or name.
    /// </summary>
    public interface IStatusRepository
    {
        /// <summary>
        /// Retrieves a <see cref="Status"/> by its unique identifier.
        /// </summary>
        /// <param name="statusId">The unique identifier of the status.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Status"/> with the specified ID, or null if not found.</returns>
        Task<Status?> GetById(int statusid);

        /// <summary>
        /// Retrieves a <see cref="Status"/> by its name.
        /// </summary>
        /// <param name="name">The name of the status.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Status"/> with the specified name, or null if not found.</returns>
        Task<Status?> GetByName(string name);
    }
}
