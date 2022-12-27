using System;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Owin.Hosting;
using Topshelf;

namespace AspNetSelfHostDemo
{
    class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private extern static int ShowWindow(System.IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {

            ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 0);
            StartTopshelf();
        }

        static void StartTopshelf()
        {
            HostFactory.Run(x =>
            {
                x.Service<WebServer>(s =>
                {
                    s.ConstructUsing(name => new WebServer());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("This is a demo of a Windows Service using Topshelf.");
                x.SetDisplayName("Self Host Web API Demo");
                x.SetServiceName("AspNetSelfHostDemo");
            });
        }
    }
}
