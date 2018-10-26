using System;
using System.Diagnostics;
using NLog;

namespace kd.infrastructure.ShellCommandExecutionProvider
{
    public class MonoExecutionProvider : IShellCommandExecutionProvider
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ExecutionResult Execute(ShelCommand cmd, TimeSpan executionTimeout)
        {
            var executionResult = new ExecutionResult();

            try
            {
                _logger.Debug("[CMD] >_ {0} {1}", cmd.Command, cmd.Arguments);

                var procStartInfo = new ProcessStartInfo(cmd.Command, cmd.Arguments)
                {
                    // The following commands are needed to redirect the standard output.
                    // This means that it will be redirected to the Process.StandardOutput StreamReader.
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Now we create a process, assign its ProcessStartInfo and start it
                var proc = new Process {StartInfo = procStartInfo};
                proc.Start();

                

                // Get the output into a string
                var result = proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();
                proc.WaitForExit((int)executionTimeout.TotalMilliseconds);

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