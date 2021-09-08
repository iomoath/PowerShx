using System;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Common
{
    public class PS
    {
        readonly Runspace _runspace;

        public PS()
        {
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();
        }

        public string Exe(string cmd)
        {
            try
            {
                var pipeline = _runspace.CreatePipeline();
                pipeline.Commands.AddScript(cmd);
                pipeline.Commands.Add("Out-String");
                var results = pipeline.Invoke();
                var stringBuilder = new StringBuilder();
                foreach (var obj in results)
                {
                    foreach (var line in obj.ToString().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                    {
                        stringBuilder.AppendLine(line.TrimEnd());
                    }
                }
                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                var errorText = e.Message + "\n";
                return (errorText);
            }
        }


        public void Close()
        {
            try
            {
                _runspace.Close();
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
