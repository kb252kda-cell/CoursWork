using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OOPWPFProject
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                MessageBox.Show("Директорію створено");
            }
            else
            {
                MessageBox.Show("Директорія вже існує");
            }
            
        }
    }
    }
