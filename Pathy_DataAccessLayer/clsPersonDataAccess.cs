using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Pathy_DataAccessLayer
{
    public class clsPersonDataAccess
    {

        public static bool GetPersonByPersonID(int personID , clsPersonDTO personDTO)
        {
            bool isFound = false;
          
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();

                    string cmd = "SP_GetPersonByPersonID";
                    using(SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PersonID",personID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                          
                            if (reader.Read())
                            {
                              
                               isFound = true;

                               personDTO.personID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                               personDTO.firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                               personDTO.secondName = reader.GetString(reader.GetOrdinal("SecondName"));
                               personDTO.thirdName = reader.GetString(reader.GetOrdinal("ThirdName"));
                               personDTO.lastName = reader.GetString(reader.GetOrdinal("LastName"));
                               personDTO.email = reader.GetString(reader.GetOrdinal("Email"));
                               personDTO.phoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"));
                               personDTO.gender = reader.GetInt32(reader.GetOrdinal("Gender"));

                            }
                        }
                    }
                }



            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }


            return isFound;
        }


        public static bool doesEmailExist(string email)
        {
            bool isFound = false;

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();


                    string cmd = "SP_DoesEmailExist";
                    using(SqlCommand command = new SqlCommand(cmd, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"Email",email);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.HasRows)
                            {
                                isFound = true;
                            }

                        }
                    }
                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return isFound;
        }


        public static int addNewPerson(clsPersonDTO personDTO)
        {
            int newPersonID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();


                    string cmd = "SP_AddNewPerson";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue(@"FirstName",personDTO.firstName);
                        command.Parameters.AddWithValue(@"SecondName", personDTO.secondName);
                        command.Parameters.AddWithValue(@"ThirdName", personDTO.thirdName);
                        command.Parameters.AddWithValue(@"LastName", personDTO.lastName);
                        command.Parameters.AddWithValue(@"Email", personDTO.email);
                        command.Parameters.AddWithValue(@"PhoneNumber", personDTO.phoneNumber);
                        command.Parameters.AddWithValue(@"Gender",personDTO.gender);


                        object result = command.ExecuteScalar();

                        if(result != null && int.TryParse(result.ToString(),out int newID))
                        {
                            newPersonID = newID;
                        }

                    }
                }


            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }

            return newPersonID;
        }


        public static bool updatePerson(clsPersonDTO personDTO)
        {
            int numberOfAffectedRows = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.getConnectionString()))
                {
                    connection.Open();


                    string cmd = "SP_UpdatePersonByPersonID";
                    using(SqlCommand command = new SqlCommand(cmd,connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(@"PersonID",personDTO.personID);
                        command.Parameters.AddWithValue(@"FirstName",personDTO.firstName);
                        command.Parameters.AddWithValue(@"SecondName", personDTO.secondName);
                        command.Parameters.AddWithValue(@"ThirdName", personDTO.thirdName);
                        command.Parameters.AddWithValue(@"LastName", personDTO.lastName);
                        command.Parameters.AddWithValue(@"Email", personDTO.email);
                        command.Parameters.AddWithValue(@"PhoneNumber", personDTO.phoneNumber);
                        command.Parameters.AddWithValue(@"Gender", personDTO.gender);

                        numberOfAffectedRows = command.ExecuteNonQuery();

                    }

                }

            } catch(Exception exception)
            {
                Console.WriteLine($"DEBUG: {exception.Message}");
            }




            return numberOfAffectedRows >= 1;
        }


    }
}
