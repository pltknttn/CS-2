using PersonnelOfficer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows; 

namespace PersonnelOfficer.Presenter
{
    public class MainPresenter
    {
        private Database _database = new Database();
        private static string DefaultFileName = $"{App.AppDir}\\{Properties.Settings.Default.DbFileName}";
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

        public MainPresenter()
        {
            Owner = Application.Current.MainWindow;
            LoadDB();
        }
                 
        public List<Employee> GetEmployees()
        {
            return _database.Employees;
        }

        public List<Department> GetDepartments()
        {
            return _database.Departments;
        }

        public List<Position> GetPositions()
        {
             return _database.Positions;
        }

        public bool SaveEmployee(Employee employee)
        {
            if (_database.Departments == null)
            {
                LoadDB();
            }
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
            if (!(_database.Departments?.Any(x => x.Id == employee.DepartmentId) ?? false))
            {
                ShowMessageError("Выберите отдел!");
                return false;
            }
            if (!(_database.Positions?.Any(x => x.Id == employee.PositionId) ?? false))
            {
                ShowMessageError("Выберите должность!");
                return false;
            }
            if (_database.Employees != null && !_database.Employees.Exists(x=>x.Id == employee.Id))
                employee.Id = _database.NextEmployeId;

            _database.Employees.RemoveAll(x => x.Id == employee.Id);
            _database.Employees.Add(employee); 
            
            return SaveDB();
        }

        public bool DeleteEmployee(Employee employee)
        {
            if (_database.Departments == null)
            {
                LoadDB();
            }
            if (employee == null)
            {
                ShowMessageError("Заполните данные!");
                return false;
            }
            if (_database.Employees != null && !_database.Employees.Exists(x => x.Id == employee.Id)) return true;

            _database.Employees.RemoveAll(x => x.Id == employee.Id);
            return SaveDB();
        }

        public int NextEmployeId => _database.NextEmployeId;

        public bool SaveDepartment(Department department)
        {
            if (_database.Departments == null)
            {
                LoadDB();
            }
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
            if (_database.Departments != null && !_database.Departments.Exists(x => x.Id == department.Id))
                department.Id = _database.NextDepartmentId;

            _database.Departments.RemoveAll(x => x.Id == department.Id);
            _database.Departments.Add(department);

            return SaveDB();
        }

        public bool DeleteDepartment(Department department, out bool changePositions)
        {
            changePositions = false;
            if (_database.Departments == null)
            {
                LoadDB();
            }
            if (department == null)
            {
                ShowMessageError("Заполните данные!");
                return false;
            }
            if (_database.Departments != null && !_database.Departments.Exists(x => x.Id == department.Id)) return true;

            if (_database.Employees?.Any(x=>x.DepartmentId == department.Id) == true)
            {
                ShowMessageError("Переведите сострудников в другой отдел!");
                return false;
            }
            changePositions = _database.Positions?.Any(x => x.DepartmentId == department.Id) == true;
            _database.Departments.RemoveAll(x => x.Id == department.Id);
            _database.Positions?.RemoveAll(x => x.DepartmentId == department.Id);

            return SaveDB();
        }

        public int NextDepartmentId => _database.NextDepartmentId;

        public bool SavePosition(Position position, out bool changeEmployees)
        {
            changeEmployees = false;

            if (_database.Positions == null)
            {
                LoadDB();
            }
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
            if (!(_database.Departments?.Any(x=>x.Id == position.DepartmentId)??false))
            {
                ShowMessageError("Выберите отдел!");
                return false;
            }
            if (_database.Positions != null && !_database.Positions.Exists(x => x.Id == position.Id))
                position.Id = _database.NextPositionId;

            _database.Positions.RemoveAll(x => x.Id == position.Id);
            _database.Positions.Add(position);

            changeEmployees = _database.Employees?.Any(x => x.PositionId == position.Id && x.DepartmentId != position.DepartmentId) == true;
            _database.Employees?
                .FindAll(x => x.PositionId == position.Id && x.DepartmentId != position.DepartmentId)?
                .ForEach(x => x.DepartmentId = position.DepartmentId); 
            return SaveDB();
        }

        public bool DeletePosition(Position position)
        {
            if (_database.Positions == null)
            {
                LoadDB();
            }
            if (position == null)
            {
                ShowMessageError("Заполните данные!");
                return false;
            }
            if (_database.Positions != null && !_database.Positions.Exists(x => x.Id == position.Id)) return true;

            if (_database.Employees?.Any(x => x.PositionId == position.Id) == true)
            {
                ShowMessageError("Переведите сострудников на другую должность!");
                return false;
            }
                       
            _database.Positions?.RemoveAll(x => x.Id == position.Id); 

            return SaveDB();
        }

        public int NextPositionId => _database.NextPositionId;

        private bool SaveDB()
        {
            return _database.Save(DefaultFileName);
        }

        private void LoadDB()
        {
            _database = new Database().Load(DefaultFileName) ?? new Database();
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
