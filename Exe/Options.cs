using CommandLine;

namespace Q
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
    }
}
