using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ImportDataRepository : IImportDataRepository
    {

        /// <summary>
        /// Only inserts GMC value not exist in the people's table
        /// If new person does not have address details insert empty data with current personId.
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        public async Task<int> Insert(List<PersonWithAddress> persons)
        {
            var result = 0;

            foreach (var person in persons)
            {
                int personId = 0;
                var sql = new StringBuilder();
                sql.AppendLine("INSERT INTO people (FirstName, LastName, GMC)");
                sql.AppendLine("SELECT @firstName, @lastName, @gmc FROM DUAL");
                sql.AppendLine("WHERE NOT EXISTS (SELECT * FROM people WHERE GMC = @gmc LIMIT 1);");
                sql.AppendLine("SELECT LAST_INSERT_ID() as Id;");

                await using (var connection = new MySqlConnection(Config.DbConnectionString))
                {
                    await connection.OpenAsync();

                    var command = new MySqlCommand(sql.ToString(), connection);
                    command.Parameters.AddWithValue("firstName", person.FirstName);
                    command.Parameters.AddWithValue("lastName", person.LastName);
                    command.Parameters.AddWithValue("gmc", person.GMC);

                    var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        personId = int.Parse(reader["Id"].ToString());
                    }
                }

                if (personId != 0)
                {
                    person.Id = personId;
                    result++;

                    sql = new StringBuilder();
                    sql.AppendLine("INSERT INTO addresses (PersonId, Line1, City, Postcode)");
                    sql.AppendLine("VALUES (@personId, @line1, @city, @postcode);");

                    await using (var connection = new MySqlConnection(Config.DbConnectionString))
                    {
                        await connection.OpenAsync();

                        var command = new MySqlCommand(sql.ToString(), connection);
                        command.Parameters.AddWithValue("personId", person.Id);
                        command.Parameters.AddWithValue("line1", person.Address?[0].Line1 ?? "");
                        command.Parameters.AddWithValue("city", person.Address?[0].City ?? "");
                        command.Parameters.AddWithValue("postcode", person.Address?[0].Postcode ?? "");

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }

            return result;
        }
    }
}
