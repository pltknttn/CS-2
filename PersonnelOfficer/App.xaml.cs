﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace PersonnelOfficer
{
     
    public partial class App : Application
    {
        public static string AppDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string Shell = Assembly.GetExecutingAssembly().Location; 
    }
}
