using PersonalOfficerLibrary;
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
        private static PersonnelOfficerServiceRef.WCFServiceClient serviceClient = null;
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
            try
            {
                serviceClient = new PersonnelOfficerServiceRef.WCFServiceClient();
            }
            catch(Exception ex)
            {
                ShowMessageError(ex?.InnerException.Message ?? ex.Message, "Подключение сервиса...");
            }
        } 
          
        private List<Employee> _employees;
        public List<Employee> GetEmployees(bool reload = false)
        {
            if (_employees == null || reload)
            {
                try
                {
                    _employees = serviceClient?.GetEmployees()?.ToList();
                }
                catch (Exception ex)
                {
                    ShowMessageError(ex?.InnerException.Message ?? ex.Message);
                }
            }
            return _employees ?? new List<Employee>();
        }

        private List<Department> _departments;
        public List<Department> GetDepartments(bool reload = false)
        {
            if (_departments == null || reload)
            {
                try
                {
                    _departments = serviceClient?.GetDepartments()?.ToList();
                }
                catch (Exception ex)
                {
                    ShowMessageError(ex?.InnerException.Message ?? ex.Message);
                }
            }
            return _departments ?? new List<Department>();
        }

        private List<Position> _positions;
        public List<Position> GetPositions(bool reload = false)
        {
            if (_positions == null || reload)
            {
                try
                {
                    _positions = serviceClient?.GetPositions()?.ToList();
                }
                catch (Exception ex)
                {
                    ShowMessageError(ex?.InnerException.Message ?? ex.Message);
                }
        }
            return _positions ?? new List<Position>();
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

            try
            {
                var employeeId = 0;
                var res = serviceClient?.SaveEmployee(employee, out employeeId);
                if (res == true)
                {
                    employee.Id = employeeId;

                    _employees.RemoveAll(x => x.Id == employee.Id);
                    _employees.Add(employee);

                    return true;
                }
            }
            catch (Exception ex)
            {
                ShowMessageError(ex?.InnerException.Message ?? ex.Message);
            }
            return false;
        }

        public bool DeleteEmployee(Employee employee)
        { 
            if (employee == null)
            {
                ShowMessageError("Заполните данные!");
                return false;
            }
            if (_employees != null && !_employees.Exists(x => x.Id == employee.Id)) return true;
            try
            {
                if (serviceClient?.DeleteEmployee(employee) == true)
                {
                    _employees.RemoveAll(x => x.Id == employee.Id);

                    ShowMessageInfo("Операция выполнена, сотрудник удален");
                    return true;
                }
            }
            catch(Exception ex)
            {
                ShowMessageError(ex?.InnerException.Message ?? ex.Message);
            }
            return false;
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

            try
            {
                var Id = 0;
                var res = serviceClient?.SaveDepartment(department, out Id);
                if (res == true)
                {
                    department.Id = Id;

                    _departments.RemoveAll(x => x.Id == department.Id);
                    _departments.Add(department);

                    return true;
                }
            }
            catch (Exception ex)
            {
                ShowMessageError(ex?.InnerException.Message ?? ex.Message);
            }
            return false; 
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

            try
            {
                if (serviceClient?.DeleteDepartment(department) == true)
                {
                    changePositions = _positions?.Any(x => x.DepartmentId == department.Id) == true;

                    _departments.RemoveAll(x => x.Id == department.Id);
                    _positions?.RemoveAll(x => x.DepartmentId == department.Id);

                    ShowMessageInfo("Операция выполнена, отдел удален");

                    return true;
                }
            }
            catch (Exception ex)
            {
                ShowMessageError(ex?.InnerException.Message ?? ex.Message);
            }
            return false; 
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

            try
            {
                var Id = 0;
                var res = serviceClient?.SavePosition(position, out Id);
                if (res == true)
                {
                    position.Id = Id;

                    _positions.RemoveAll(x => x.Id == position.Id);
                    _positions.Add(position);

                    changeEmployees = _employees?.Any(x => x.PositionId == position.Id && x.DepartmentId != position.DepartmentId) == true;
                    _employees?
                        .FindAll(x => x.PositionId == position.Id && x.DepartmentId != position.DepartmentId)?
                        .ForEach(x => x.DepartmentId = position.DepartmentId);

                    return true;
                }
            }
            catch (Exception ex)
            {
                ShowMessageError(ex?.InnerException.Message ?? ex.Message);
            }
            return false; 
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

            try
            {
                if (serviceClient?.DeletePosition(position) == true)
                {
                    _positions?.RemoveAll(x => x.Id == position.Id);

                    ShowMessageInfo("Операция выполнена, должность удалена");
                    return true;
                }
            }
            catch (Exception ex)
            {
                ShowMessageError(ex?.InnerException.Message ?? ex.Message);
            }  

            return false;
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

        ~MainPresenter()
        {
            try
            {
                if (serviceClient?.State != System.ServiceModel.CommunicationState.Closed)
                    serviceClient?.Close();
            }
            catch { }
            serviceClient = null;
        }
    }
}
