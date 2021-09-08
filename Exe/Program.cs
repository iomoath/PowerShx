using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Q
{
    public class Program
    {
        #region Main
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintAbout();
                Console.WriteLine();
                SharedGlobals.ShowUsageExamples();
                return;
            }

            var ps = new PsSession();

            try
            {

                var options = ParseUserArgs(args);
                if (options == null)
                    return;

                ps.Start(options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.Read();
                Environment.Exit(1);
            }
        }


        #endregion

        #region Args Parsing

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

            Console.WriteLine(helpText);
        }

        #endregion


        #region Print Version About Text

        private static void PrintAbout()
        {
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