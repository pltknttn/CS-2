using PersonalOfficerLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PersonnelOfficerServices.Presenter
{
    public class PersonnelOfficerPresenter
    {
        private static readonly string dbName = Properties.Settings.Default.MsSqlDdName;
        private static string connectionString;

        public PersonnelOfficerPresenter()
        {
            connectionString = ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
        }

        public bool SaveEmployee(Employee employee, out int employeeId)
        {
            employeeId = 0;

            if (employee == null) throw new CustomException("Заполните данные!", ErrorCode.InvalidInput); 
            if (string.IsNullOrWhiteSpace(employee.FirstName?.Trim())) throw new CustomException("Не заполнено имя!", ErrorCode.InvalidInput); 
            if (string.IsNullOrWhiteSpace(employee.Surname?.Trim())) throw new CustomException("Не заполнена фамилия!", ErrorCode.InvalidInput);             
            if (employee.DepartmentId  == 0) throw new CustomException("Не заполнен отдел!", ErrorCode.InvalidInput); 
            if (employee.PositionId == 0) throw new CustomException("Не заполнена должность!", ErrorCode.InvalidInput); 
            if (employee.Salary <= 0) throw new CustomException("Не указан оклад!", ErrorCode.InvalidInput); 
            if (string.IsNullOrWhiteSpace(employee.Title?.Trim()))
                employee.Title = $"{employee.FirstName} {employee.Patronymic}".Trim();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(@"Org.p_SaveEmployee", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = employee.Id });
                        command.Parameters.Add(new SqlParameter("@FirstName", employee.FirstName));
                        command.Parameters.Add(new SqlParameter("@Patronymic", employee.Patronymic ?? ""));
                        command.Parameters.Add(new SqlParameter("@Surname", employee.Surname));
                        command.Parameters.Add(new SqlParameter("@Title", employee.Title));
                        command.Parameters.Add(new SqlParameter("@DateOfBirth", System.Data.SqlDbType.DateTime2) { Value = employee.DateOfBirth });
                        command.Parameters.Add(new SqlParameter("@Sex", System.Data.SqlDbType.Char) { Value = (char)employee.Sex });
                        command.Parameters.Add(new SqlParameter("@Married", System.Data.SqlDbType.Bit) { Value = employee.Married });
                        command.Parameters.Add(new SqlParameter("@Telephone", employee.Telephone ?? ""));
                        command.Parameters.Add(new SqlParameter("@Mobilephone", employee.Mobilephone ?? ""));
                        command.Parameters.Add(new SqlParameter("@Address", employee.Address??""));
                        command.Parameters.Add(new SqlParameter("@Email", employee.Email ?? ""));
                        command.Parameters.Add(new SqlParameter("@PositionId", System.Data.SqlDbType.Int) { Value = employee.PositionId });
                        command.Parameters.Add(new SqlParameter("@DepartmentId", System.Data.SqlDbType.Int) { Value = employee.DepartmentId });
                        command.Parameters.Add(new SqlParameter("@Salary", System.Data.SqlDbType.Decimal) { Value = employee.Salary });
                        command.Parameters.Add(new SqlParameter("@LastEditState", employee.LastEditState.ToString()));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read()) employeeId = reader.GetInt32(0);  
                            reader.Close();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new CustomException(ex.Message); 
                }
                catch (Exception ex)
                {
                    throw new CustomException(ex.Message);
                }
                finally
                {
                    connection.Close(); 
                }
            }   
            return true;
        }

        public bool DeleteEmployee(Employee employee)
        {
            if (employee == null) throw new CustomException("Заполните данные!", ErrorCode.InvalidInput); 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"Org.p_DeleteEmployee", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = employee.Id });
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw new CustomException(ex.Message); 
                }
                catch (Exception ex)
                {
                    throw new CustomException(ex.Message); 
                }
                finally
                {
                    connection.Close();
                }
            } 
            return true;
        }

        public bool SaveDepartment(Department department, out int departmentId)
        {
            departmentId = 0;

            if (department == null) throw new CustomException("Заполните данные!", ErrorCode.InvalidInput);
            if (string.IsNullOrWhiteSpace(department.Name?.Trim())) throw new CustomException("Не заполнено название отдела", ErrorCode.InvalidInput); 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"exec Org.p_SaveDepartment @Id, @Name, @Telephone, @Address, @RommNumber, @LastEditState", connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = department.Id });
                        command.Parameters.Add(new SqlParameter("@Name", department.Name));
                        command.Parameters.Add(new SqlParameter("@Telephone", department.Telephone ?? ""));
                        command.Parameters.Add(new SqlParameter("@RommNumber", department.RommNumber ?? ""));
                        command.Parameters.Add(new SqlParameter("@Address", department.Address ?? ""));
                        command.Parameters.Add(new SqlParameter("@LastEditState", department.LastEditState.ToString()));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read()) departmentId = reader.GetInt32(0); 
                            reader.Close();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new CustomException(ex.Message); 
                }
                catch (Exception ex)
                {
                    throw new CustomException(ex.Message); 
                }
                finally
                {
                    connection.Close();
                }
            } 

            return true;
        }

        public bool DeleteDepartment(Department department)
        { 
            if (department == null) throw new CustomException("Заполните данные!", ErrorCode.InvalidInput);  

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"Org.p_DeleteDepartment", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = department.Id });
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw new CustomException(ex.Message); 
                }
                catch (Exception ex)
                {
                    throw new CustomException(ex.Message); 
                }
                finally
                {
                    connection.Close();
                }
            } 

            return true;
        }

        public bool SavePosition(Position position, out int positionId)
        {
            positionId = 0;

            if (position == null) throw new CustomException("Заполните данные!", ErrorCode.InvalidInput);               
            if (string.IsNullOrWhiteSpace(position.Name?.Trim()))  throw new CustomException("Заполните название должности!", ErrorCode.InvalidInput);              
            if (position.SalaryFrom > position.SalaryTo) throw new CustomException("Потолок оклада не может быть меньше минимального оклада должности!", ErrorCode.InvalidInput);
            if (position.SalaryFrom <= 0) throw new CustomException("Укажите минимальный оклад должности!", ErrorCode.InvalidInput);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"Org.p_SavePosition", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = position.Id });
                        command.Parameters.Add(new SqlParameter("@Name", position.Name));
                        command.Parameters.Add(new SqlParameter("@SalaryFrom", System.Data.SqlDbType.Decimal) { Value = position.SalaryFrom });
                        command.Parameters.Add(new SqlParameter("@SalaryTo", System.Data.SqlDbType.Decimal) { Value = position.SalaryTo });
                        command.Parameters.Add(new SqlParameter("@DepartmentId", System.Data.SqlDbType.Int) { Value = position.DepartmentId });
                        command.Parameters.Add(new SqlParameter("@LastEditState", position.LastEditState.ToString()));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read()) positionId = reader.GetInt32(0); 
                            reader.Close();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new CustomException(ex.Message); 
                }
                catch (Exception ex)
                {
                    throw new CustomException(ex.Message); 
                }
                finally
                {
                    connection.Close();
                }
            } 
            return true;
        }

        public bool DeletePosition(Position position)
        {
            if (position == null) throw new CustomException("Заполните данные!", ErrorCode.InvalidInput); 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"Org.p_DeletePosition", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = position.Id });
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw new CustomException(ex.Message); 
                }
                catch (Exception ex)
                {
                    throw new CustomException(ex.Message); 
                }
                finally
                {
                    connection.Close();
                }
            } 

            return true;
        }

        public List<Employee> GetEmployees()
        {
            var emps = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"Org.p_GetEmployee", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) // если есть данные
                            {
                                while (reader.Read()) // построчно считываем данные
                                {
                                    emps.Add(new Employee()
                                    {
                                        Id = reader.GetInt32(0),
                                        FirstName = reader.GetValue(1)?.ToString(),
                                        Patronymic = reader.GetValue(2)?.ToString(),
                                        Surname = reader.GetValue(3)?.ToString(),
                                        Title = reader.GetValue(4)?.ToString(),
                                        DateOfBirth = reader.GetDateTime(5),
                                        Sex = (Sex)reader.GetString(6)[0],
                                        Married = reader.GetValue(7)?.ToString() == "1",
                                        Telephone = reader.GetValue(8)?.ToString(),
                                        Mobilephone = reader.GetValue(9)?.ToString(),
                                        Address = reader.GetValue(10)?.ToString(),
                                        Email = reader.GetValue(11)?.ToString(),
                                        PositionId = reader.GetInt32(12),
                                        DepartmentId = reader.GetInt32(13),
                                        Salary = double.Parse(reader.GetValue(14).ToString()),
                                        LastEditState = string.IsNullOrEmpty(reader.GetValue(15)?.ToString()) || !(reader.GetValue(15) is int) ? EditState.Insert : (EditState)reader.GetInt32(15),
                                    });
                                }
                            }
                            reader.Close();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new CustomException(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new CustomException(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return emps;
        }

        public List<Department> GetDepartments()
        {
            var deps = new List<Department>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(@"Org.p_GetDepartment", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) // если есть данные
                            {
                                while (reader.Read()) // построчно считываем данные
                                {
                                    deps.Add(new Department()
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Telephone = reader.GetValue(2)?.ToString(),
                                        Address = reader.GetValue(3)?.ToString(),
                                        RommNumber = reader.GetValue(4)?.ToString(),
                                        LastEditState = string.IsNullOrEmpty(reader.GetValue(5)?.ToString()) || !(reader.GetValue(5) is int) ? EditState.Insert : (EditState)reader.GetInt32(5),
                                    });
                                }
                            }
                            reader.Close();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new CustomException(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new CustomException(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return deps;
        }

        public List<Position> GetPositions()
        {
            var poss = new List<Position>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"Org.p_GetPosition", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) // если есть данные
                            {
                                while (reader.Read()) // построчно считываем данные
                                {
                                    poss.Add(new Position()
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        SalaryFrom = double.Parse(reader.GetValue(2).ToString()),
                                        SalaryTo = double.Parse(reader.GetValue(3).ToString()),
                                        DepartmentId = reader.GetInt32(4),
                                        LastEditState = string.IsNullOrEmpty(reader.GetValue(5)?.ToString()) || !(reader.GetValue(5) is int) ? EditState.Insert : (EditState)reader.GetInt32(5),
                                    });
                                }
                            }
                            reader.Close();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new CustomException(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new CustomException(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return poss;
        }

    }
}