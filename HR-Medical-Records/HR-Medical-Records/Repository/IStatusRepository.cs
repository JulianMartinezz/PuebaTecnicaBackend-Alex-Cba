using HR_Medical_Records.Models;

namespace HR_Medical_Records.Repository
{
    public interface IStatusRepository
    {
        Task<Status?> GetById(int statusid);
        Task<Status?> GetByName(string name);
    }
}
