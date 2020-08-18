using PersonnelOfficer.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows; 

namespace PersonnelOfficer.Presenter
{
    public class MainPresenter
    {
        private static readonly string dbName = Properties.Settings.Default.MsSqlDdName;
        private static string connectionString;
        private static Window Owner;
        private static MainPresenter _mainPresenter;
        public static MainPresenter Instance
        {
            get
            {
                if (_mainPresenter == null)
                    _mainPresenter = new MainPresenter();
                return _mainPresenter;
            }
        }
         
        private MainPresenter()
        {
            Owner = Application.Current.MainWindow;
            connectionString = ConfigurationManager.ConnectionStrings[dbName].ConnectionString; 
        } 
          
        private List<Employee> _employees;
        public List<Employee> GetEmployees(bool reload = false)
        {
            if (_employees == null || reload)
            {
                if (_employees == null) _employees = new List<Employee>();
                else _employees.Clear(); 
               
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
                                        _employees.Add(new Employee()
                                        {
                                            Id = reader.GetInt32(0),
                                            FirstName = reader.GetValue(1)?.ToString(),
                                            Patronymic = reader.GetValue(2)?.ToString(),
                                            Surname = reader.GetValue(3)?.ToString(),
                                            Title = reader.GetValue(4)?.ToString(),
                                            DateOfBirth = reader.GetDateTime(5),
                                            Sex = (Sex)reader.GetString(6)[0],
                                            Married = reader.GetValue(7)?.ToString()=="1",
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
                        ShowMessageError(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        ShowMessageError(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return _employees;
        }

        private List<Department> _departments;
        public List<Department> GetDepartments(bool reload = false)
        {
            if (_departments == null || reload)
            {
                if (_departments == null) _departments = new List<Department>();
                else _departments.Clear();

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
                                        _departments.Add(new Department()
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
                        ShowMessageError(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        ShowMessageError(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return _departments;
        }

        private List<Position> _positions;
        public List<Position> GetPositions(bool reload = false)
        {
            if (_positions == null || reload)
            {
                if (_positions == null) _positions = new List<Position>();
                else _positions.Clear();

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
                                        _positions.Add(new Position()
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
                        ShowMessageError(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        ShowMessageError(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return _positions;
        }

        public bool SaveEmployee(Employee employee)
        { 
            if (employee == null)
            {
                ShowMessageError("Заполните данные!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(employee.FirstName?.Trim()))
            {
                ShowMessageError("Заполните имя!");
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(employee.Surname?.Trim()))
            {
                ShowMessageError("Заполните фамилию!");
                return false;
            }
            
            if (employee.DepartmentId  == 0 || !(_departments?.Any(x => x.Id == employee.DepartmentId) ?? false))
            {
                ShowMessageError("Выберите отдел!");
                return false;
            }

            if (employee.PositionId == 0 || !(_positions?.Any(x => x.Id == employee.PositionId) ?? false))
            {
                ShowMessageError("Выберите должность!");
                return false;
            }

            if (employee.Salary <= 0)
            {
                ShowMessageError("Укажите оклад!");
                return false;
            }

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
                            if (reader.HasRows)
                            {
                                if (reader.Read())
                                {
                                    employee.Id = reader.GetInt32(0);
                                }
                            }
                            reader.Close();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    ShowMessageError(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    ShowMessageError(ex.Message);
                    return false;
                }
                finally
                {
                    connection.Close(); 
                }
            } 

            _employees.RemoveAll(x => x.Id == employee.Id);
            _employees.Add(employee);

            return true;
        }

        public bool DeleteEmployee(Employee employee)
        { 
            if (employee == null)
            {
                ShowMessageError("Заполните данные!");
                return false;
            }
            if (_employees != null && !_employees.Exists(x => x.Id == employee.Id)) return true;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"Org.p_DeleteEmployee @Id", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure; 
                        command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = employee.Id }); 
                        command.ExecuteNonQuery(); 
                    }
                }
                catch (SqlException ex)
                {
                    ShowMessageError(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    ShowMessageError(ex.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
            _employees.RemoveAll(x => x.Id == employee.Id);
            return true;
        }
         
        public bool SaveDepartment(Department department)
        {            
            if (department == null)
            {
                ShowMessageError("Заполните данные!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(department.Name?.Trim()))
            {
                ShowMessageError("Заполните название отдела!");
                return false;
            }

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
                        command.Parameters.Add(new SqlParameter("@Telephone", department.Telephone??""));
                        command.Parameters.Add(new SqlParameter("@RommNumber", department.RommNumber??""));
                        command.Parameters.Add(new SqlParameter("@Address", department.Address??""));  
                        command.Parameters.Add(new SqlParameter("@LastEditState", department.LastEditState.ToString()));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                if (reader.Read())
                                {
                                    department.Id = reader.GetInt32(0);
                                }
                            }
                            reader.Close();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    ShowMessageError(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    ShowMessageError(ex.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }

            _departments.RemoveAll(x => x.Id == department.Id);
            _departments.Add(department);

            return true;
        }

        public bool DeleteDepartment(Department department, out bool changePositions)
        {
            changePositions = false; 

            if (department == null)
            {
                ShowMessageError("Заполните данные!");
                return false;
            }
            if (_departments != null && !_departments.Exists(x => x.Id == department.Id)) return true;

            if (_employees?.Any(x=>x.DepartmentId == department.Id) == true)
            {
                ShowMessageError("Переведите сострудников в другой отдел!");
                return false;
            }

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
                    ShowMessageError(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    ShowMessageError(ex.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
            
             changePositions = _positions?.Any(x => x.DepartmentId == department.Id) == true;
            
            _departments.RemoveAll(x => x.Id == department.Id);
            _positions?.RemoveAll(x => x.DepartmentId == department.Id);

            return true;
        }
         
        public bool SavePosition(Position position, out bool changeEmployees)
        {
            changeEmployees = false; 

            if (position == null)
            {
                ShowMessageError("Заполните данные!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(position.Name?.Trim()))
            {
                ShowMessageError("Заполните название должности!");
                return false;
            }
            if (!(_departments?.Any(x=>x.Id == position.DepartmentId)??false))
            {
                ShowMessageError("Выберите отдел!");
                return false;
            }
            if (position.SalaryFrom > position.SalaryTo)
            {
                ShowMessageError("Потолок оклада не может быть меньше минимального оклада должности!");
                return false;
            }
            if (position.SalaryFrom <= 0)
            {
                ShowMessageError("Укажите минимальный оклад должности!");
                return false;
            }

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
                            if (reader.HasRows)
                            {
                                if (reader.Read())
                                {
                                    position.Id = reader.GetInt32(0);
                                }
                            }
                            reader.Close();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    ShowMessageError(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    ShowMessageError(ex.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }

            _positions.RemoveAll(x => x.Id == position.Id);
            _positions.Add(position);

            changeEmployees = _employees?.Any(x => x.PositionId == position.Id && x.DepartmentId != position.DepartmentId) == true;
            _employees?
                .FindAll(x => x.PositionId == position.Id && x.DepartmentId != position.DepartmentId)?
                .ForEach(x => x.DepartmentId = position.DepartmentId); 
            return true;
        }

        public bool DeletePosition(Position position)
        { 
            if (position == null)
            {
                ShowMessageError("Заполните данные!");
                return false;
            }
            if (_positions != null && !_positions.Exists(x => x.Id == position.Id)) return true;

            if (_employees?.Any(x => x.PositionId == position.Id) == true)
            {
                ShowMessageError("Переведите сострудников на другую должность!");
                return false;
            }

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
                    ShowMessageError(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    ShowMessageError(ex.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }

            _positions?.RemoveAll(x => x.Id == position.Id); 

            return true;
        }
                
        public void ShowMessageWarn(string message, string caption = "Внимание!")
        { 
            MessageBox.Show(Owner, message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ShowMessageInfo(string message, string caption = "Информация!")
        {
            MessageBox.Show(Owner, message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowMessageError(string message, string caption = "Ошибка!")
        {
            MessageBox.Show(Owner, message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool ShowMessageQuestions(string message, string caption = "Ошибка!")
        {
            return MessageBox.Show(Owner, message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        } 
    }
}
