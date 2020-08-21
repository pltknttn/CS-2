using PersonalOfficerLibrary;
using PersonnelOfficer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            if (CurrentModel?.EditedEmployee?.LastEditState == EditState.Insert)
                this.Title = $"Сотрудники.Добавление";
        }

        private MainWindowModel CurrentModel => this.DataContext as MainWindowModel;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentModel?.Init();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentModel?.EditedEmployee == null) return;
            CurrentModel.SaveEditEmployee();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            CurrentModel?.CancelEdit();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)e.OriginalSource;
            BindingOperations.GetBindingExpression(comboBox, ComboBox.SelectedValueProperty)?.UpdateTarget();
        }
         
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9.]+").IsMatch(e.Text);
        }

        private void TextBoxTelephone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void TextBoxMobilephone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9+]+").IsMatch(e.Text);
        }

        private void cbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindingOperations.GetBindingExpression(cbPosition, ComboBox.ItemsSourceProperty)?.UpdateTarget();
            BindingOperations.GetBindingExpression(cbPosition, ComboBox.SelectedValueProperty)?.UpdateTarget();
        }
    }
}
