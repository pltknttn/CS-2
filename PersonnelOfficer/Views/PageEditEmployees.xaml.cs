using PersonnelOfficer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonnelOfficer.Views
{
    /// <summary>
    /// Interaction logic for PageEditEmployees.xaml
    /// </summary>
    public partial class PageEditEmployees : Page
    {
        public PageEditEmployees()
        {
            InitializeComponent();
            this.DataContext = Application.Current.MainWindow.DataContext;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainWindowModel)?.Init();
           
            cbPosition.SelectedValue = (this.DataContext as MainWindowModel)?.EditedEmployee.PositionId;
            cbDepartment.SelectedValue = (this.DataContext as MainWindowModel)?.EditedEmployee.DepartmentId;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if ((this.DataContext as MainWindowModel)?.EditedEmployee == null) return;

            (this.DataContext as MainWindowModel).EditedEmployee.PositionId = int.Parse(cbPosition.SelectedValue.ToString());
            (this.DataContext as MainWindowModel).EditedEmployee.DepartmentId = int.Parse(cbDepartment.SelectedValue.ToString()); 
            (this.DataContext as MainWindowModel).SaveEditEmployee();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainWindowModel)?.CancelEdit();
        }
    }
}
