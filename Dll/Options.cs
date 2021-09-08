using CommandLine;

namespace W
{
    public class Options
    {
        [Option('e', Required = false, HelpText = "<PS script to run>")]
        public string Script { get; set; }


        [Option('f', Required = false, HelpText = "Run the script passed as argument. -f <path>")]
        public string ScriptPath { get; set; }

        [Option('c', Required = false, HelpText = "Load a script and run a PS cmdlet. -f <path> -c <PS Cmdlet>")]
        public string Cmdlet { get; set; }


        [Option('i', Required = false, HelpText = "Start an interactive console")]
        public bool Interactive { get; set; }


        [Option('s', Required = false, HelpText = "Attempt to bypass AMSI.")]
        public bool BypassAmsi { get; set; }


        [Option('w', Required = false, HelpText = "Start an interactive console in a new window")]
        public bool ConsoleNewWindow { get; set; }


        [Option('v', Required = false, HelpText = "Print Execution Output to the console")]
        public bool ShowConsole { get; set; }
    }
}
