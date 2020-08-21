using PersonalOfficerLibrary;
using PersonnelOfficer.Data;
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
                    MainFrame.Navigate(UtilClass.UriEmployees);
                    break;
                case "Departments":
                    MainFrame.Navigate(UtilClass.UriDepartments);
                    break;
                case "Positions":
                    MainFrame.Navigate(UtilClass.UriPositions);
                    break;  
                case "Add":
                    Model.Edit(EditState.Insert);
                    break;
                case "Edit":
                    Model.Edit(EditState.Edit);
                    break;
                case "Delete":
                    Model.Edit(EditState.Delete);
                    break;
                case "Update": 
                    Model.Fill();
                    break;
                default:
                    break;
            }
        }         
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Model.Init();
        }
    }
}
