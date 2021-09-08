using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Common;

namespace W
{
    public class PsSession
    {
        private readonly PS _ps = new PS();
        private Process _pp = new Process();
        private readonly PSConsole _psConsole = new PSConsole();



        public void Start(Options options)
        {
            try
            {
                if (options == null)
                    options = new Options();

                Handle(options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _ps.Close();
                Cleanup();
                Environment.Exit(1);
            }
        }

        private void Handle(Options options)
        {

            if (options.BypassAmsi)
            {
                _ps.Exe(Payloads.PayloadDict["amsi"]);
            }


            if (options.ConsoleNewWindow)
            {
                _psConsole.GetNewConsole();
                Interact();
                return;
            }


            if (options.Interactive)
            {
                _pp = Process.GetCurrentProcess().Parent();
                _pp.Suspend();

                _psConsole.StealConsole(_pp);
                Console.Title = "cx";
                Console.CancelKeyPress += delegate
                {
                    Cleanup();
                };

                Console.SetCursorPosition(0, Console.CursorTop + 1);
                Console.WriteLine("Press Enter to get started:");
                Console.Write("\n");
                Interact();
                _pp.Resume();

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
                Exec(script, false);
                Exec(options.Cmdlet, options.ShowConsole);
                return;
            }

            if (string.IsNullOrEmpty(options.Cmdlet?.Trim()) && string.IsNullOrWhiteSpace(options.Cmdlet?.Trim()) && !string.IsNullOrEmpty(options.ScriptPath?.Trim()) && !string.IsNullOrWhiteSpace(options.ScriptPath?.Trim()))
            {
                if (!File.Exists(options.ScriptPath))
                    return;

                var script = LoadScript(options.ScriptPath.Trim());

                if (string.IsNullOrEmpty(script) || script == "error")
                    return;

                Exec(options.Cmdlet, options.ShowConsole);
                return;
            }

            if (!string.IsNullOrEmpty(options.Script?.Trim()) && !string.IsNullOrWhiteSpace(options.Script?.Trim()))
            {
                Exec(options.Script, options.ShowConsole);
                return;
            }
        }

        private void Cleanup()
        {
            try
            {
                _pp.Resume();
            }
            catch
            {
                //
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

        private void Exec(string script, bool showConsole)
        {
            if (showConsole)
            {
                var result = _ps.Exe(script?.Trim());
                Print(result);
                Console.WriteLine(result);
            }
            else
            {
                _ps.Exe(script?.Trim());
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
                Print(errorText);
                return "error";
            }
        }



        #region Print, Version About Text

        private void Print(string msg)
        {
            _pp = Process.GetCurrentProcess().Parent();
            _psConsole.StealConsole(_pp);
            Console.CancelKeyPress += delegate
            {
                Cleanup();
            };
            Console.SetCursorPosition(0, Console.CursorTop + 1);

            Console.WriteLine(msg);
        }

      

        #endregion
    }
}
