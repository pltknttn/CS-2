using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Xml.Serialization;

namespace PersonnelOfficer.Data
{
    public static class UtilClass 
    {
        public static string Description(this Enum value)
        {
            var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any()) return (attributes.First() as DescriptionAttribute).Description; 
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(ti.ToLower(value.ToString().Replace("_", " ")));
        }

        public static int GetYearDiff(DateTime startDate, DateTime endDate)
        {
            int y = endDate.Year - startDate.Year;
            int startMonth = startDate.Month;
            int endMonth = endDate.Month;
            if (endMonth < startMonth) return y - 1;
            if (endMonth > startMonth) return y;
            return (endDate.Day < startDate.Day ? y - 1 : y);
        }

        public static string[] GetMaritalStatuses(Sex sex)
        {
            return sex == Sex.Female? new [] { "не замужем", "замужем"} : new [] { "не женат", "женат"};
        }

        public static string GetMaritalStatus(Sex sex, bool married)
        {
            return GetMaritalStatuses(sex)[married ? 1 : 0];
        }

        public static bool Save<T>(this List<T> list, string filename)
        { 
            Stream fStream = null;
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<T>));
                fStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                xmlFormat.Serialize(fStream, list); 
            }
            finally
            {
                fStream?.Close();
            }

            return true;
        }

        public static bool Save<T>(this T item, string filename)
        {
            Stream fStream = null;
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                fStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                xmlFormat.Serialize(fStream, item);
            }
            finally
            {
                fStream?.Close();
            }

            return true;
        }

        public static List<T> Load<T>(this List<T> list, string filename)
        {
            Stream fStream = null;
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<T>));
                fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                list = (List<T>)xmlFormat.Deserialize(fStream); 
            }
            finally
            {
                fStream?.Close();
            }

            return list;
        }

        public static T Load<T>(this T item, string filename)
        {
            Stream fStream = null;
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                item = (T)xmlFormat.Deserialize(fStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                fStream?.Close();
            }

            return item;
        }

        public static bool ShowOpenFileDialog(string filter, string defaultFileName, out string filename)
        {
            filename = string.Empty;
            bool isSuccessful = false;

            using (OpenFileDialog dialog = new OpenFileDialog { Filter = filter })
            {
                dialog.FileName = defaultFileName;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    isSuccessful = true;
                    filename = dialog.FileName;
                }
            }

            return isSuccessful;
        }

        public static bool ShowSaveFileDialog(string filter, string defaultFileName, out string filename)
        {
            filename = string.Empty;
            bool isSuccessful = false;

            using (SaveFileDialog dialog = new SaveFileDialog { Filter = filter })
            {
                dialog.FileName = defaultFileName;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    isSuccessful = true;
                    filename = dialog.FileName;
                }
            }

            return isSuccessful;
        }


    }

    public enum Sex
    {
        [Description("М")]
        Man = 'М',
        [Description("Ж")]
        Female = 'Ж'
    }

    public enum EditState
    {
        Insert,
        Edit,
        Delete
    }

    public class SexEnumToCollectionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Sex[] values)
            {
                return values.Select(x => new ValueDescription() { Value = x, Description = x.Description()}).ToList();
            }
            if (value is Sex enumValue)
            {
                return new ValueDescription() { Value = enumValue, Description = enumValue.Description() };
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ValueDescription valueDescription)
            {
                return valueDescription.Value;
            }
            return value;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
