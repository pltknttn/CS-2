using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelOfficer.Data
{
    static class UtilClass
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
    }

    public enum Sex
    {
        Man = 'М',
        Female = 'Ж'
    }
}
