using Models;

namespace DAL;

public interface ISpecialityRepository
{
    Task<List<Speciality>> ListAllAsync();
}