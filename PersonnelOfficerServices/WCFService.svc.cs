using PersonalOfficerLibrary;
using PersonnelOfficerServices.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PersonnelOfficerServices
{
    public class WCFService : IWCFService
    {
        public bool DeleteDepartment(Department department)
        {
            return new PersonnelOfficerPresenter().DeleteDepartment(department);
        }

        public bool DeleteEmployee(Employee employee)
        {
            return new PersonnelOfficerPresenter().DeleteEmployee(employee);
        }

        public bool DeletePosition(Position position)
        {
            return new PersonnelOfficerPresenter().DeletePosition(position);
        }

        public List<Department> GetDepartments()
        {
            return new PersonnelOfficerPresenter().GetDepartments();
        }

        public List<Employee> GetEmployees()
        {
            return new PersonnelOfficerPresenter().GetEmployees();
        }

        public List<Position> GetPositions()
        {
            return new PersonnelOfficerPresenter().GetPositions();
        }

        public bool SaveDepartment(Department department, out int departmentId)
        {
            return new PersonnelOfficerPresenter().SaveDepartment(department, out departmentId);
        }

        public bool SaveEmployee(Employee employee, out int employeeId)
        {
            return new PersonnelOfficerPresenter().SaveEmployee(employee, out employeeId);
        }

        public bool SavePosition(Position position, out int positionId)
        {
            return new PersonnelOfficerPresenter().SavePosition(position, out positionId);
        }
    }
}
