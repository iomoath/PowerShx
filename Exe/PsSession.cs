using System;
using System.IO;
using System.Text;
using Common;

namespace Q
{
    public class PsSession
    {
        private PS _ps = new PS();

        public void Start(Options options)
        {
            Console.Title = "cx";

            try
            {
                Handle(options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _ps.Close();
            }
        }

        private void Handle(Options options)
        {
            if (options.BypassAmsi)
            {
                _ps.Exe(Payloads.PayloadDict["amsi"]);
            }

            if (options.Interactive)
            {

                Interact();
                return;
            }

            if (!string.IsNullOrEmpty(options.Script?.Trim()) && !string.IsNullOrWhiteSpace(options.Script?.Trim()))
            {
                Console.WriteLine(_ps.Exe(options.Script?.Trim()));
                return;
            }

            if (!string.IsNullOrEmpty(options.Cmdlet?.Trim()) && !string.IsNullOrWhiteSpace(options.Cmdlet?.Trim()) && !string.IsNullOrEmpty(options.ScriptPath?.Trim()) && !string.IsNullOrWhiteSpace(options.ScriptPath?.Trim()))
            {
                if (!File.Exists(options.ScriptPath))
                    return;

                var script = LoadScript(options.ScriptPath.Trim());

                if (string.IsNullOrEmpty(script) || script == "error")
                    return;

                _ps.Exe(script);
                Console.WriteLine(_ps.Exe(options.Cmdlet));
            }

            if (string.IsNullOrEmpty(options.Cmdlet?.Trim()) && string.IsNullOrWhiteSpace(options.Cmdlet?.Trim()) && !string.IsNullOrEmpty(options.ScriptPath?.Trim()) && !string.IsNullOrWhiteSpace(options.ScriptPath?.Trim()))
            {
                if (!File.Exists(options.ScriptPath))
                    return;

                var script = LoadScript(options.ScriptPath.Trim());

                if (string.IsNullOrEmpty(script) || script == "error")
                    return;

                Console.WriteLine(_ps.Exe(script));
            }
        }

        private void Interact()
        {
            var cmd = "";
            Console.WriteLine();
            while (cmd != null && cmd.ToLower() != "exit")
            {
                Console.Write("PS " + _ps.Exe("$(get-location).Path").Replace(Environment.NewLine, string.Empty) + ">");
                cmd = Console.ReadLine();
                Console.WriteLine(_ps.Exe(cmd));
            }
        }

        private string LoadScript(string filename)
        {
            try
            {
                using (var sr = new StreamReader(filename))
                {
                    var fileContents = new StringBuilder();
                    string curLine;
                    while ((curLine = sr.ReadLine()) != null)
                    {
                        fileContents.Append(curLine + "\n");
                    }
                    return fileContents.ToString();
                }
            }
            catch (Exception e)
            {
                var errorText = e.Message + "\n";
                Console.WriteLine(errorText);
                return "error";
            }
        }
    }

}
