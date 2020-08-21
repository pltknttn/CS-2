using PersonalOfficerLibrary;
using PersonnelOfficer.Data;
using PersonnelOfficer.Presenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PersonnelOfficer.Model
{
    public sealed class MainWindowModel : ViewModelBase
    {        
        private MainPresenter mainPresenter;
        public MainWindowModel() { mainPresenter = MainPresenter.Instance; }

        public void Init()
        {           
            FillEmployees();
            FillDepartments();
            FillPositions();
        }
        
        public MainWindow View { get; set; }
        private Employee _currentEmployee;
        public Employee CurrentEmployee { get { return _currentEmployee; } set { _currentEmployee = value; OnPropertyChanged("CurrentEmployee"); } }
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        private Department _currentDepartment;
        public Department CurrentDepartment { get { return _currentDepartment; } set { _currentDepartment = value; OnPropertyChanged("CurrentDepartment"); } }  
        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();
        private Position _currentPosition;
        public Position CurrentPosition { get { return _currentPosition; } set { _currentPosition = value; OnPropertyChanged("CurrentPosition"); } } 
        public ObservableCollection<Position> Positions { get; set; } = new ObservableCollection<Position>();
        
        public Uri CurrentSource { get; set; } = UtilClass.UriEmployees;
        private Page _currentPage = new Page();
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
                OnPropertyChanged("CurrentPage.Title");
            }
        }

        public List<Position> PositionsAtDepartment => Positions?.Where(x => x.DepartmentId == (EditedEmployee?.DepartmentId ?? -1))?.ToList();

        public void FillEmployees(bool reload = false)
        {
            int id = CurrentEmployee?.Id ?? 0;
            Employees.Clear();
            mainPresenter.GetEmployees(reload).ForEach(Employees.Add);
            CurrentEmployee = Employees.FirstOrDefault(x=>x.Id == id) ?? Employees.FirstOrDefault();
            OnPropertyChanged("Employees");
            OnPropertyChanged("CurrentEmployee");
        }

        public void FillDepartments(bool reload = false)
        {
            int id = CurrentDepartment?.Id ?? 0;
            Departments.Clear();
            mainPresenter.GetDepartments(reload).ForEach(Departments.Add);
            CurrentDepartment = Departments.FirstOrDefault(x => x.Id == id) ?? Departments.FirstOrDefault();
            OnPropertyChanged("Departments");
            OnPropertyChanged("CurrentDepartment");
        }

        public void FillPositions(bool reload = false)
        {
            int id = CurrentPosition?.Id ?? 0;
            Positions.Clear();
            mainPresenter.GetPositions(reload).ForEach(Positions.Add);
            CurrentPosition = Positions.FirstOrDefault(x => x.Id == id);
            OnPropertyChanged("Positions");
            OnPropertyChanged("CurrentPosition");
        }

        public void Fill()
        {
            string pagename = CurrentPage.GetType().Name;
            if (!_isCancelEdit) return;

            if (pagename.Contains("Employees"))
            {
                FillEmployees(true);
            }            
            else if (pagename.Contains("Departments"))
            {
                FillDepartments(true);
            }
            else if (pagename.Contains("Positions"))
            {
                FillPositions(true);
            }
        } 

        private bool _isCancelEdit = true;
        public bool IsCancelEdit
        {
            get { return _isCancelEdit; }
            set { _isCancelEdit = value; OnPropertyChanged("IsCancelEdit"); }
        }

        public void CancelEdit()
        {
            FillEmployees();
            FillDepartments();
            FillPositions();
            View.MainFrame.GoBack();
            IsCancelEdit = true; 
        }
                
        public Position EditedPosition { get; set; }

        public void SaveEditPosition()
        {
            if(!mainPresenter.SavePosition(EditedPosition, out var changeEmployees)) return;
            Positions.Add(mainPresenter.GetPositions().Last());
            View.MainFrame.GoBack();
            OnPropertyChanged("Positions");
            CurrentPosition = Positions.FirstOrDefault(x=>x.Id == EditedPosition.Id) ?? Positions.Last();
            OnPropertyChanged("CurrentPosition");
            if(changeEmployees) FillEmployees();
            IsCancelEdit = true;
        }
                
        public void EditPosition(EditState editState)
        {            
            if (editState == EditState.Insert)
            {
                CurrentPosition = new Position();
            }
            else if (editState == EditState.Delete && CurrentPosition != null)
            {
                if(!mainPresenter.DeletePosition(CurrentPosition)) return;
                Positions.Remove(CurrentPosition);
                OnPropertyChanged("Positions");                
                if (Positions.Any()) CurrentPosition = Positions.Last();
                OnPropertyChanged("CurrentPosition");
                return;
            }
            
            if (CurrentPosition != null)
            {
                EditedPosition = CurrentPosition.Clone() as Position;
                EditedPosition.LastEditState = editState;
                View.MainFrame.Navigate(UtilClass.UriEditPosition);
                IsCancelEdit = false;
            }
        }

        public Department EditedDepartment { get; set; }

        public void SaveEditDepartment()
        {
            if(!mainPresenter.SaveDepartment(EditedDepartment)) return;
            Departments.Add(mainPresenter.GetDepartments().Last());
            View.MainFrame.GoBack();
            OnPropertyChanged("Departments");
            CurrentDepartment = Departments.FirstOrDefault(x => x.Id == EditedDepartment.Id) ?? Departments.Last(); 
            OnPropertyChanged("CurrentDepartment");
            IsCancelEdit = true;
        }

        public void EditDepartment(EditState editState)
        {
            if (editState == EditState.Insert)
            {
                CurrentDepartment = new Department();
            }
            else if (editState == EditState.Delete && CurrentDepartment != null)
            {                
                if(!mainPresenter.DeleteDepartment(CurrentDepartment, out var changePositions)) return;
                Departments.Remove(CurrentDepartment);
                OnPropertyChanged("Departments");
                if (Departments.Any()) CurrentDepartment = Departments.Last();
                if (changePositions) FillPositions();
                return;
            }

            if (CurrentDepartment != null)
            {
                EditedDepartment = CurrentDepartment.Clone() as Department;
                EditedDepartment.LastEditState = editState;
                View.MainFrame.Navigate(UtilClass.UriEditDepartment);
                IsCancelEdit = false;
            }
        }

        public Employee EditedEmployee { get; set; }

        public void SaveEditEmployee()
        {
            if(!mainPresenter.SaveEmployee(EditedEmployee)) return;
            Employees.Add(mainPresenter.GetEmployees().Last());
            OnPropertyChanged("Employees");
            CurrentEmployee = Employees.FirstOrDefault(x => x.Id == EditedEmployee.Id) ?? Employees.Last(); 
            OnPropertyChanged("CurrentEmployee");
            View.MainFrame.GoBack();
            IsCancelEdit = true;
        }

        public void EditEmployee(EditState editState)
        {
            if (editState == EditState.Insert)
            { 
                CurrentEmployee = new Employee(); 
            }
            else if (editState == EditState.Delete && CurrentEmployee != null)
            {
                if(!mainPresenter.DeleteEmployee(CurrentEmployee)) return;
                Employees.Remove(CurrentEmployee);
                OnPropertyChanged("Employees");
                if (Employees.Any()) CurrentEmployee = Employees.Last();
                return;
            }

            if (CurrentEmployee != null)
            {
                EditedEmployee = CurrentEmployee.Clone() as Employee;
                EditedEmployee.LastEditState = editState;
                View.MainFrame.Navigate(UtilClass.UriEditEmployee);
                OnPropertyChanged("PositionsAtDepartment");
                IsCancelEdit = false;
            }
        }

        public void Edit(EditState editState)
        {
            if (editState == EditState.Delete)
            {
                if (!mainPresenter.ShowMessageQuestions("Вы уверены ?", $"{CurrentPage.Title}. Удаление"))
                    return;
            }
            if (CurrentSource.OriginalString.Contains(UtilClass.UriPositions.Segments.Last()))
                EditPosition(editState);
            else if (CurrentSource.OriginalString.Contains(UtilClass.UriDepartments.Segments.Last()))
                EditDepartment(editState);
            else if (CurrentSource.OriginalString.Contains(UtilClass.UriEmployees.Segments.Last()))
                EditEmployee(editState);
        }
    }
}
