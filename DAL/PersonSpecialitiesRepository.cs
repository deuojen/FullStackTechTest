using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PersonSpecialitiesRepository : IPersonSpecialitiesRepository
    {

        /// <summary>
        /// Get person's specialities 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<List<PersonSpeciality>> GetByIdAsync(int personId)
        {
            var personSpecialityList = new List<PersonSpeciality>();

            var sql = new StringBuilder();
            sql.AppendLine("SELECT ps.Id, ps.PersonId, ps.SpecialityId, s.Name FROM person_specialities as ps");
            sql.AppendLine("LEFT JOIN speciality as s on ps.SpecialityId = s.Id");
            sql.AppendLine("WHERE PersonId = @personId;");

            await using (var connection = new MySqlConnection(Config.DbConnectionString))
            {
                await connection.OpenAsync();

                var command = new MySqlCommand(sql.ToString(), connection);
                command.Parameters.AddWithValue("personId", personId);

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    personSpecialityList.Add(PopulatePersonSpeciality(reader));
                }
            }

            return personSpecialityList;
        }

        /// <summary>
        /// Drop previos specialities for the person and
        /// then insert new ones
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="specialityIds"></param>
        /// <returns></returns>
        public async Task InsertOrDropAsync(int personId, List<int> specialityIds)
        {
            // clean the previous specialities
            var sql = new StringBuilder();
            sql.AppendLine("SET SQL_SAFE_UPDATES = 0;");
            sql.AppendLine("DELETE FROM person_specialities");
            sql.AppendLine("WHERE PersonId = @personId;");
            sql.AppendLine("SET SQL_SAFE_UPDATES = 1;");

            await using (var connection = new MySqlConnection(Config.DbConnectionString))
            {
                await connection.OpenAsync();

                var command = new MySqlCommand(sql.ToString(), connection);
                command.Parameters.AddWithValue("personId", personId);

                await command.ExecuteNonQueryAsync();
            }

            // insert new specialities
            foreach (var specialityId in specialityIds)
            {
                sql = new StringBuilder();
                sql.AppendLine("INSERT INTO person_specialities (PersonId, SpecialityId)");
                sql.AppendLine("VALUES (@personId, @specialityId);");

                await using (var connection = new MySqlConnection(Config.DbConnectionString))
                {
                    await connection.OpenAsync();

                    var command = new MySqlCommand(sql.ToString(), connection);
                    command.Parameters.AddWithValue("personId", personId);
                    command.Parameters.AddWithValue("specialityId", specialityId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }



        private PersonSpeciality PopulatePersonSpeciality(IDataRecord data)
        {
            var personSpeciality = new PersonSpeciality

            {
                Id = int.Parse(data["Id"].ToString()),
                PersonId = data["PersonId"].ToString(),
                SpecialityId = data["SpecialityId"].ToString(),
                SpecialityName = data["Name"].ToString()
            };
            return personSpeciality;
        }
    }
}
