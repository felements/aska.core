using System;
using System.Text;

namespace kd.infrastructure.ShellCommandExecutionProvider
{
    public class ExecutionResult
    {
        public static ExecutionResult Create(bool succeed, string command, string reason = "")
        {
            return new ExecutionResult
            {
                Succeed = succeed,
                Command = command,
                Reason = reason
            };
        }

        public void AppendException(Exception ex)
        {
            _sb.AppendLine($"{ex.GetType().Name}: {ex.Message}");
            Succeed = false;
        }

        public ExecutionResult()
        {
            Succeed = false;
            Command = string.Empty;

            _sb = new StringBuilder();
        }

        private readonly StringBuilder _sb;

        public bool Succeed { get; set; }
        public string Command { get; set; }

        public string Reason
        {
            get => _sb.ToString();
            set => _sb.AppendLine(value);
        }
    }
}