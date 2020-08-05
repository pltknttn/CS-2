using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAsteroid.Data
{
    class GameLog
    {       
        private static readonly object _LockObject = new object();
        private static string _fileName;
        private static GameLog _instance;
        public static GameLog Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameLog();

                return _instance;
            }
        }

        public GameLog()
        {
            var logFolderName =  string.Format(@"{1}\GameLog\{0:yyyy}\{0:MM}\{0:dd}", DateTime.Now, Application.StartupPath);
            if (!Directory.Exists(logFolderName)) Directory.CreateDirectory(logFolderName);

            _fileName = Path.Combine(logFolderName, $@"Log_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log");
        }

        public void WriteFile(string message)
        {
            lock (_LockObject)
            {
                File.AppendAllText(_fileName, $"[{DateTime.Now:yyyy.MM.dd HH:mm:ss}] {message}\r\n");
            }
        }

        public void WriteConsole(string message)
        {
            lock (_LockObject)
            {
                Console.WriteLine(message);
            }
        }
    } 
}
