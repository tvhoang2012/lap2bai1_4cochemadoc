using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Testing
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        System.Timers.Timer timer = new System.Timers.Timer();
        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSSendMessage(
        IntPtr hServer,
        [MarshalAs(UnmanagedType.I4)] int SessionId,
        String pTitle,
        [MarshalAs(UnmanagedType.U4)] int TitleLength,
        String pMessage,
        [MarshalAs(UnmanagedType.U4)] int MessageLength,
        [MarshalAs(UnmanagedType.U4)] int Style,
        [MarshalAs(UnmanagedType.U4)] int Timeout,
        [MarshalAs(UnmanagedType.U4)] out int pResponse,
        bool bWait);
        public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
        //public static int WTS_CURRENT_SESSION = 1;
        public void Start(string userName)
        {
            try
            {
                bool result = false;
                String title = "Okie";
                int tlen = title.Length;
                // lastRebootDaysDiff = 0;
                if (userName != null || userName != "")
                {
                    string msg = "you have been hacked by 18520060";
                    WriteToFile(msg);
                    int mlen = msg.Length;
                    int resp = 7;
                    //result = WTSSendMessage(WTS_CURRENT_SERVER_HANDLE, user_session, title, tlen, msg, mlen, 4,
                    // 0, out resp, true);
                    result = WTSSendMessage(WTS_CURRENT_SERVER_HANDLE, 1, title, tlen, msg, mlen, 4,
                    0, out resp, false);
                    int err = Marshal.GetLastWin32Error();
                    if (err == 0)
                    {
                        if (result) //user responded to box
                        {
                            if (resp == 7) //user clicked no
                            {
                            }
                            else if (resp == 6) //user clicked yes
                            {
                                // write your functionality here
                            }
                            // Debug.WriteLine(“user_session:” + user_session + “ err:” + err + “ resp:” + resp);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Debug.WriteLine(“no such thread exists”, ex);
            }
        }
        protected override void OnStart(string[] args)
        {


            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds
            timer.Enabled = true;
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            Start(userName);

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
        protected override void OnStop()
        {
        }
    }
}
