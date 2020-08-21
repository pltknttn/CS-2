using PersonalOfficerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PersonnelOfficerServices
{
    [ServiceContract]
    public interface IWCFService
    { 
        [OperationContract]
        bool SaveEmployee(Employee employee, out int employeeId);

        [OperationContract]
        bool DeleteEmployee(Employee employee);

        [OperationContract]
        bool SaveDepartment(Department department, out int departmentId);

        [OperationContract]
        bool DeleteDepartment(Department department);

        [OperationContract]
        bool SavePosition(Position position, out int positionId);

        [OperationContract]
        bool DeletePosition(Position position);

        [OperationContract]
        List<Employee> GetEmployees();

        [OperationContract]
        List<Department> GetDepartments();

        [OperationContract]
        List<Position> GetPositions();
    }
}
