using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aska.core.tools.Extensions
{
    public class ShellExecute
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public ExecutionResult Execute(string cmd, ApplicationEnvironment[] targetEnvironments)
        {
            if (string.IsNullOrWhiteSpace( cmd)) throw new ArgumentOutOfRangeException(nameof(cmd));
            var result = new ExecutionResult();


            var executable = cmd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            Logger.Debug("{2} >_ {0} on {1}", cmd, string.Join(",", targetEnvironments), executable);

            if (string.IsNullOrWhiteSpace(executable)) return result;
            try
            {
                if (!targetEnvironments.Contains(ApplicationExtensions.GetEnvironment()))
                {
                    Logger.Warn("Skipped due to target platform mismatch.");
                    result.Error = "Skipped due to target platform mismatch.";
                    return result;
                }


                System.Diagnostics.ProcessStartInfo procStartInfo;
                switch (ApplicationExtensions.GetEnvironment())
                {
                    case ApplicationEnvironment.Mono:
                        procStartInfo = new System.Diagnostics.ProcessStartInfo(executable, cmd.Substring(cmd.IndexOf(executable, StringComparison.InvariantCultureIgnoreCase) + executable.Length));
                        break;
                    case ApplicationEnvironment.WindowsCore:
                    case ApplicationEnvironment.WindowsNative:
                        procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + cmd);
                        break;
                        default:
                            throw new Exception("Not supported environment " + ApplicationExtensions.GetEnvironment().ToString("G"));
                }

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.RedirectStandardError = true;
                procStartInfo.UseShellExecute = false;

                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                //proc.BeginOutputReadLine();

                // Get the output into a string
                result.Output = proc.StandardOutput.ReadToEnd();
                result.Error = proc.StandardError.ReadToEnd();

                proc.WaitForExit(120000);

                Logger.Debug("{0} >_ : {1}", executable, result.Output);
                if (result.HasError) Logger.Warn("{0} >_ : {1}", executable, result.Error);
                return result;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                result.Error = e.Message;
                return result;
            }
        }

        public ExecutionResult Execute(string cmd, Dictionary<string, string> tokens, ApplicationEnvironment[] targetEnvironment)
        {
            var command = tokens.Aggregate(cmd, (current, token) => Regex.Replace(current, string.Format("{{{{{0}}}}}", token.Key), token.Value, RegexOptions.CultureInvariant));

            return Execute(command, targetEnvironment);
        }

        public class ExecutionResult
        {
            public string Output { get; set; }
            public string Error { get; set; }

            public bool HasError
            {
                get { return !string.IsNullOrWhiteSpace(Error); }
            }
        }
    }
}