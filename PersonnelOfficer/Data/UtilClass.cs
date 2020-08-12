using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PersonnelOfficer.Data
{
    public static class UtilClass 
    {
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
            catch(Exception ex)
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
        Man = 'М',
        Female = 'Ж'
    }

    public enum EditState
    {
        Insert,
        Edit,
        Delete
    }
}
