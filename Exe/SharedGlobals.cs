using System;

namespace Q
{
    internal static class SharedGlobals
    {
        public static readonly string[] About = { "PowerShx v1.0", "github.com/iomoath/PowerShx" };

        public static void ShowUsageExamples()
        {
            Console.WriteLine(string.Format("{0,-40} {1,-10}", "PowerShx.exe -i", "Start an interactive console"));
            Console.WriteLine(string.Format("{0,-40} {1,-10}", "PowerShx.exe -e", "<PS script to run>"));
            Console.WriteLine(string.Format("{0,-40} {1,-10}", "PowerShx.exe -f <path>", "Run the script passed as argument"));
            Console.WriteLine(string.Format("{0,-40} {1,-10}", "PowerShx.exe -f <path> -c <PS Cmdlet>", "Load a script and run a PS cmdlet"));
            Console.WriteLine(string.Format("{0,-40} {1,-10}", "PowerShx.exe -s", "Attempt to bypass AMSI. Use with -f and -e"));
        }
    }
}
