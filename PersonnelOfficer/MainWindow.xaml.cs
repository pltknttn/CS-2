using PersonnelOfficer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonnelOfficer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {            
            InitializeComponent();
            Model = new MainWindowModel { View = this };
        }

        public MainWindowModel Model
        {
            get => (MainWindowModel)DataContext;
            set => DataContext = value;
        }
         
        private void Button_Click(object sender, RoutedEventArgs e)
        {           
            var button = sender as ToggleButton; 
            switch ((string)button.Tag)
            {
                case "Employees":
                    MainFrame.Navigate(MainWindowModel.UriEmployees);
                    break;
                case "Departments":
                    MainFrame.Navigate(MainWindowModel.UriDepartments);
                    break;
                case "Positions":
                    MainFrame.Navigate(MainWindowModel.UriPositions);
                    break; 
                case "Add" when CurrentUri.OriginalString.Contains(MainWindowModel.UriEmployees.Segments.Last()):
                    Model.EditEmployee(Data.EditState.Insert);
                    break;
                case "Edit" when CurrentUri.OriginalString.Contains(MainWindowModel.UriEmployees.Segments.Last()):
                    Model.EditEmployee(Data.EditState.Edit);
                    break;
                case "Delete" when CurrentUri.OriginalString.Contains(MainWindowModel.UriEmployees.Segments.Last()):
                    Model.EditEmployee(Data.EditState.Delete);
                    break;
                case "Add" when CurrentUri.OriginalString.Contains(MainWindowModel.UriDepartments.Segments.Last()):
                    Model.EditDepartment(Data.EditState.Insert);
                    break;
                case "Edit" when CurrentUri.OriginalString.Contains(MainWindowModel.UriDepartments.Segments.Last()):
                    Model.EditDepartment(Data.EditState.Edit);
                    break;
                case "Delete" when CurrentUri.OriginalString.Contains(MainWindowModel.UriDepartments.Segments.Last()):
                    Model.EditDepartment(Data.EditState.Delete);
                    break;
                case "Add" when CurrentUri.OriginalString.Contains(MainWindowModel.UriPositions.Segments.Last()):
                    Model.EditPosition(Data.EditState.Insert);
                    break;
                case "Edit" when CurrentUri.OriginalString.Contains(MainWindowModel.UriPositions.Segments.Last()):
                    Model.EditPosition(Data.EditState.Edit);
                    break;
                case "Delete" when CurrentUri.OriginalString.Contains(MainWindowModel.UriPositions.Segments.Last()):
                    Model.EditPosition(Data.EditState.Delete);
                    break;
                default:
                    break;
            }
        }

        private Page CurrentPage;
        private Uri CurrentUri;
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
           CurrentUri = e.Uri;
           CurrentPage = e.Content as Page;
           TextInfo.Text = CurrentPage?.Title;
           TextInfo.Tag = CurrentPage?.Name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Model.Init();
        }
    }
}
