using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;
using System.EnterpriseServices;
using CommandLine;
using CommandLine.Text;
using Common;

[assembly: ApplicationActivation(ActivationOption.Server)]
[assembly: ApplicationAccessControl(false)]

namespace W
{

    public static class Program
    {
        private static Process _pp = new Process();
        private static readonly PSConsole PSConsole = new PSConsole();

        [DllExport("main", CallingConvention = CallingConvention.Cdecl)]
        public static void main(IntPtr hwnd, IntPtr hinst, string lpszCmdLine, int nCmdShow)
        {
            if (string.IsNullOrEmpty(lpszCmdLine))
            {
               Print("");
               PrintAbout();
               Print(SharedGlobals.UsageExamples());
               return;
            }

            var ps = new PsSession();
            var args = Helpers.ParseCommandLineArgs(lpszCmdLine);
            var options = ParseUserArgs(args);
            ps.Start(options);
        }

        [DllExport("DllRegisterServer", CallingConvention = CallingConvention.StdCall)]
        public static void DllRegisterServer()
        {
            var ps = new PsSession();
            ps.Start(new Options { Interactive = true });
        }

        [DllExport("DllUnregisterServer", CallingConvention = CallingConvention.StdCall)]
        public static void DllUnregisterServer()
        {
            var ps = new PsSession();
            ps.Start(new Options{Interactive = true});
        }

        [ComVisible(true)]
        [Guid("31D2B969-7608-426E-9D8E-A09FC9A51680")]
        [ClassInterface(ClassInterfaceType.None)]
        [ProgId("dllguest.Bypass")]
        [Transaction(TransactionOption.Required)]
        public class Bypass : ServicedComponent
        {
            [ComRegisterFunction] //This executes if registration is successful
            public static void RegisterClass(string key)
            {
                var ps = new PsSession();
                ps.Start(new Options { Interactive = true });
            }

            [ComUnregisterFunction] //This executes if registration fails
            public static void UnRegisterClass(string key)
            {
                var ps = new PsSession();
                ps.Start(new Options { Interactive = true });
            }

            public void Exec()
            {
                var ps = new PsSession();
                ps.Start(new Options { Interactive = true });
            }
        }

        #region User Args parser


        private static Options ParseUserArgs(string[] args)
        {
            // Parse arguments passed
            var parser = new Parser(with =>
            {
                with.CaseInsensitiveEnumValues = true;
                with.CaseSensitive = false;
                with.HelpWriter = null;
            });


            Options options = null;


            var parserResult = parser.ParseArguments<Options>(args);
            parserResult.WithParsed(o => { options = o; }).WithNotParsed(errs => DisplayHelp(parserResult, errs));

            return options;
        }

        private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.AutoVersion = false;
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);

            Print(helpText);
        }


        #endregion


        private static void Cleanup()
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


        private static void Print(string msg)
        {
            _pp = Process.GetCurrentProcess().Parent();
            PSConsole.StealConsole(_pp);
            Console.CancelKeyPress += delegate
            {
                Cleanup();
            };
            Console.SetCursorPosition(0, Console.CursorTop + 1);

            Console.WriteLine(msg);
        }


        #region Print Version About Text

        private static void PrintAbout()
        {
            _pp = Process.GetCurrentProcess().Parent();
            PSConsole.StealConsole(_pp);
            Console.CancelKeyPress += delegate
            {
                Cleanup();
            };

            Console.WriteLine();
            if (Console.BackgroundColor == ConsoleColor.Black)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            foreach (var s in SharedGlobals.About)
            {
                Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
                Console.WriteLine(s);
            }

            Console.WriteLine();
            Console.ResetColor();
        }

        #endregion
    }
}
