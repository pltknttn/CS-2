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
    public partial class PageEditPosition : Page
    {
        public PageEditPosition()
        {
            InitializeComponent();
            this.DataContext = Application.Current.MainWindow.DataContext;
            if (CurrentModel?.EditedPosition?.LastEditState == EditState.Insert)
                this.Title = $"Должности.Добавление";
        }

        private MainWindowModel CurrentModel => this.DataContext as MainWindowModel;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentModel?.Init();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            CurrentModel?.SaveEditPosition();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            CurrentModel?.CancelEdit();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)e.OriginalSource;

            BindingOperations.GetBindingExpression(comboBox, ComboBox.SelectedValueProperty).UpdateTarget();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9.]+").IsMatch(e.Text); 
        }
    }
}
