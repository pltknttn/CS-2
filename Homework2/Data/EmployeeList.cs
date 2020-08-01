using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2.Data
{
    class EmployeeList : IEnumerable 
    {
        private int _size = 0;
        private BaseEmployee[] _employees;

        public EmployeeList() : this(0) { }

        public EmployeeList(int capacity)
        {
            if (capacity == 0)
                _employees = new BaseEmployee[0];
            else
                _employees = new BaseEmployee[capacity];
        }

        public BaseEmployee this[int index]
        {
            get { return _employees[index]; }
            set
            {
                if (index < 0 || index >= _employees.Length) throw new ArgumentOutOfRangeException();

                _employees[index] = value;

                if (_size < index) _size = index;
            }
        }
                  
        public int Add(BaseEmployee value)
        {
            if (_size == _employees.Length)
            {
                BaseEmployee[] newArray = new BaseEmployee[_employees.Length + 1];

                for (int i = 0; i < _employees.Length; i++)
                    newArray[i] = _employees[i];

                _employees = newArray; 
            }
            _employees[_size++] = value;
            return _size;
        }

        public void Clear()
        {
            for (int i = 0; i < _employees.Length; i++)
                _employees[i] = null;
            _size = 0;
        }

        public bool Contains(BaseEmployee value)
        {
            for (int i = 0; i < _employees.Length; i++)
                if (_employees[i]?.Equals(value) == true)
                    return true;
            return false;
        }

        public void CopyTo(BaseEmployee[] destination, int index)
        {            
            for (int i = index, j = 0; i < destination.Length && i < _employees.Length; i++, j++)
            {
                destination[j] = _employees[i];
            }
        }
                
        public int IndexOf(BaseEmployee value)
        {
            for (int i = 0; i < _employees.Length; i++)
                if (_employees[i]?.Equals(value) == true)
                    return i;
            return -1;
        }

        public void Insert(int index, BaseEmployee value)
        {
            if (index >=_employees.Length)
            {
                BaseEmployee[] newArray = new BaseEmployee[index + 1];

                for (int i = 0; i < _employees.Length; i++)
                    newArray[i] = _employees[i];

                _employees = newArray;  
            }

            if (_employees[index] == null) _employees[index] = value;
            else
            {
                BaseEmployee[] newArray = new BaseEmployee[_employees.Length + 1];

                for (int j = 0; j < index; j++) newArray[j] = _employees[j];

                newArray[index] = value;

                for (int j = index + 1; j < newArray.Length; j++) newArray[j] = _employees[j - 1];

                _employees = newArray;
            }

            if (_size < index) _size = index;
        }

        public void Remove(BaseEmployee value)
        {
            for (int i = 0; i < _employees.Length; i++)
                if (_employees[i]?.Equals(value) == true)
                {
                    _size--;
                    int next = 0;
                    for(int j = i; j < _employees.Length - 1; j++ )
                    {
                        _employees[j] = _employees[j + 1];

                        if (next == 0 && _employees[j + 1]?.Equals(value) == true)
                            next = j + 1;
                    } 
                    _employees[_employees.Length - 1] = null;
                    if (next > 0) i = next;
                    else return;
                } 
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _employees.Length) throw new IndexOutOfRangeException();

            for (int j = index; j < _employees.Length - 1; j++)
            {
                _employees[j] = _employees[j + 1];
            }
            _employees[_employees.Length - 1] = null;

            _size--;
        }

        /// <summary>
        /// Сотрируем объекты, используя Array.Sort
        /// </summary>
        public void Sort()
        {
            Array.Sort(_employees);
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _employees.Length; i++)
                yield return _employees[i];

        }
    }
}
