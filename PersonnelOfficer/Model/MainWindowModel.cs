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

namespace PersonnelOfficer.Model
{
    public sealed class MainWindowModel : ViewModelBase
    {
        public static Uri UriEmployees = new Uri("pack://application:,,,/Views/PageEmployees.xaml");
        public static Uri UriDepartments = new Uri("pack://application:,,,/Views/PageDepartments.xaml");
        public static Uri UriPositions = new Uri("pack://application:,,,/Views/PagePositions.xaml");
        public static Uri UriEditPosition = new Uri("pack://application:,,,/Views/PageEditPosition.xaml");
        public static Uri UriEditDepartment = new Uri("pack://application:,,,/Views/PageEditDepartment.xaml");
        public static Uri UriEditEmployee = new Uri("pack://application:,,,/Views/PageEditEmployees.xaml");

        private MainPresenter mainPresenter;

        public MainWindowModel() { }

        public void Init()
        {
            mainPresenter = MainPresenter.Instance;
            FillEmployees();
            FillDepartments();
            FillPositions();
        }
        
        public MainWindow View { get; set; }
        public Employee CurrentEmployee { get; set; }
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public Department CurrentDepartment { get; set; }
        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();
        public Position CurrentPosition { get; set; }
        public ObservableCollection<Position> Positions { get; set; } = new ObservableCollection<Position>();

        public void FillEmployees()
        {
            Employees.Clear();
            mainPresenter.GetEmployees().ForEach(Employees.Add);
            CurrentEmployee = Employees.FirstOrDefault();
        }

        public void FillDepartments()
        {
            Departments.Clear();
            mainPresenter.GetDepartments().ForEach(Departments.Add);
            CurrentDepartment = Departments.FirstOrDefault();
            OnPropertyChanged("Departments");
        }

        public void FillPositions()
        {
            Positions.Clear();
            mainPresenter.GetPositions().ForEach(Positions.Add);
            CurrentPosition = Positions.FirstOrDefault();
            OnPropertyChanged("Positions");
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
            CurrentPosition = Positions.Last();
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
                View.MainFrame.Navigate(UriEditPosition);
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
            CurrentDepartment = Departments.Last();
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
                View.MainFrame.Navigate(UriEditDepartment);
                IsCancelEdit = false;
            }
        }

        public Employee EditedEmployee { get; set; }

        public void SaveEditEmployee()
        {
            if(!mainPresenter.SaveEmployee(EditedEmployee)) return;
            Employees.Add(mainPresenter.GetEmployees().Last());
            OnPropertyChanged("Employees");
            CurrentEmployee = Employees.Last();
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
                View.MainFrame.Navigate(UriEditEmployee);
                IsCancelEdit = false;
            }
        }
    }
}
