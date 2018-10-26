using System;
using System.Diagnostics;
using System.IO;
using kd.misc;
using NLog;

namespace kd.infrastructure.ShellCommandExecutionProvider
{
    public class WindowsExecutionProvider : IShellCommandExecutionProvider
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ExecutionResult Execute(ShelCommand cmd, TimeSpan executionTimeout)
        {
            var executionResult = ExecutionResult.Create(false, cmd.ToString());
            try
            {
                _logger.Debug("[CMD] >_ " + cmd);

                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                var procStartInfo = new ProcessStartInfo("cmd", "/c " + cmd.Command + " " + cmd.Arguments)
                {
                    // The following commands are needed to redirect the standard output.
                    // This means that it will be redirected to the Process.StandardOutput StreamReader.

                    WorkingDirectory =  Path.GetFullPath( Path.Combine(AssemblyExtensions.AssemblyDirectory, cmd.WorkingDirectory )),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                // Do not create the black window.
                // Now we create a process, assign its ProcessStartInfo and start it
                var proc = new Process {StartInfo = procStartInfo};
                proc.Start();
                proc.WaitForExit((int)executionTimeout.TotalMilliseconds);
                
                // Get the output into a string
                var result = proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();

                executionResult.Succeed = proc.ExitCode == 0;

                // Display the command output.
                //_logger.Debug("[CMD] >_ " + result);
                executionResult.Reason = result;

            }
            catch (Exception objException)
            {
                _logger.Error(objException);
                executionResult.AppendException(objException);
            }

            return executionResult;
        }
    }
}