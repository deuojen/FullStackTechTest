using Models;

namespace DAL;

public interface IPersonSpecialitiesRepository
{
    Task<List<PersonSpeciality>> GetByIdAsync(int personId);
    Task InsertOrDropAsync(int personId, List<int> specialityIds);
}