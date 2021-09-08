using System.Text;

namespace W
{
    internal static class SharedGlobals
    {
        public static readonly string[] About = { "PowerShx v1.0", "github.com/iomoath/PowerShx" };

        public static string UsageExamples()
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("{0,-55} {1,-10}", "rundll32 PowerShx.dll,main -e", "<PS script to run>"));
            sb.AppendLine(string.Format("{0,-55} {1,-10}", "rundll32 PowerShx.dll,main -f <path>", "Run the script passed as argument"));
            sb.AppendLine(string.Format("{0,-55} {1,-10}", "rundll32 PowerShx.dll,main -f <path> -c <PS Cmdlet>", "Load a script and run a PS cmdlet"));
            sb.AppendLine(string.Format("{0,-55} {1,-10}", "rundll32 PowerShx.dll,main -w", "Start an interactive console in a new window"));
            sb.AppendLine(string.Format("{0,-55} {1,-10}", "rundll32 PowerShx.dll,main -i", "Start an interactive console"));
            sb.AppendLine(string.Format("{0,-55} {1,-10}", "rundll32 PowerShx.dll,main -s", "Attempt to bypass AMSI"));
            sb.AppendLine(string.Format("{0,-55} {1,-10}", "rundll32 PowerShx.dll,main -v", "Print Execution Output to the console"));

            return sb.ToString();
        }
    }
}
