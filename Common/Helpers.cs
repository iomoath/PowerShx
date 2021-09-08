using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace Common
{
    public static class Helpers
    {
        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LocalFree(IntPtr hMem);



        /// <summary>
        /// Credits: https://stackoverflow.com/a/49214724
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] ParseCommandLineArgs(string input)
        {
            var ptrToSplitArgs = CommandLineToArgvW(input, out var numberOfArgs);

            // CommandLineToArgvW returns NULL upon failure.
            if (ptrToSplitArgs == IntPtr.Zero)
                throw new ArgumentException("Unable to split argument.", new Win32Exception());


            // Make sure the memory ptrToSplitArgs to is freed, even upon failure.
            try
            {
                var splitArgs = new string[numberOfArgs];

                // ptrToSplitArgs is an array of pointers to null terminated Unicode strings.
                // Copy each of these strings into our split argument array.
                for (var i = 0; i < numberOfArgs; i++)
                {
                    splitArgs[i] = Marshal.PtrToStringUni(
                        Marshal.ReadIntPtr(ptrToSplitArgs, i * IntPtr.Size));
                }

                return splitArgs;
            }
            finally
            {
                // Free memory obtained by CommandLineToArgW.
                LocalFree(ptrToSplitArgs);
            }
        }

        public static string DecodeB64(string s, Encoding encoding)
        {
            try
            {
                var textAsBytes = Convert.FromBase64String(s);
                var decodedText = encoding.GetString(textAsBytes);
                return decodedText;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }

}
