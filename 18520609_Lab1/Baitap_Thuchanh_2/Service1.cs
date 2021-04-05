using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace Baitap_Thuchanh_2
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }
        int count = 0;
        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            //System.Diagnostics.Debugger.Launch();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval =5000; //number in miliseconds
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("Service stopped at:" + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {

            if (count%2==0)
            {
                Process.Start("notepad.exe");
                WriteToFile("Process was started at :" + DateTime.Now);
            }
            else
            {
                Process[] count_process = Process.GetProcessesByName("notepad");
                foreach (Process process in count_process)
                {
                    process.Kill();
                    WriteToFile("Process was stopped at:" + DateTime.Now);
                }
            }
            Process[] n_process = Process.GetProcessesByName("notepad");
            if (n_process.Length == 0)
            {
                WriteToFile("notepad isn't running");
            }
            else
            {
                WriteToFile("notepad is running");
            }
            count++;
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory +
            "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') +
            ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
