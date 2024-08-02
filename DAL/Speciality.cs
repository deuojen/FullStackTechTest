using System.Data;
using System.Text;
using Models;
using MySql.Data.MySqlClient;

namespace DAL;

public class SpecialityRepository : ISpecialityRepository
{
    public async Task<List<Speciality>> ListAllAsync()
    {
        var specialityList = new List<Speciality>();
        
        var sql = new StringBuilder();
        sql.AppendLine("SELECT * FROM speciality");

        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();
            
            var command = new MySqlCommand(sql.ToString(), connection);
            
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                specialityList.Add(PopulateSpeciality(reader));
            }
        }

        return specialityList;
    }

    private Speciality PopulateSpeciality(IDataRecord data)
    {
        var speciality = new Speciality
        {
            Id = int.Parse(data["Id"].ToString()),
            Name = data["Name"].ToString()
        };
        return speciality;
    }
}