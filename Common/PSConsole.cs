using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Common
{
    public class PSConsole
    {
        [DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]

        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]

        private static extern int AllocConsole();
        private const int STD_OUTPUT_HANDLE = -11;
        private const int MY_CODE_PAGE = 437;

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();

        private const int STD_ERROR_HANDLE = -12;
        //private static bool _consoleAttached = false;
        //private static IntPtr consoleWindow;


        public void GetNewConsole()
        {
            try
            {
                AllocConsole();
                var stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                var safeFileHandle = new SafeFileHandle(stdHandle, true);
                var fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                var encoding = Encoding.GetEncoding(MY_CODE_PAGE);
                var standardOutput = new StreamWriter(fileStream, encoding);
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
            catch
            {
                //
            }
        }
        public void StealConsole(Process pp)
        {
            try
            {
                var ppid = pp.Id;
                if (!AttachConsole((uint) ppid)) 
                    return;

                //_consoleAttached = true;
                var stdHandle = GetStdHandle(STD_ERROR_HANDLE);
                var safeFileHandle = new SafeFileHandle(stdHandle, true);
                var fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                var encoding = Encoding.ASCII;
                var standardOutput = new StreamWriter(fileStream, encoding);
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
            catch 
            {
                //
            }
        }
    }
}
