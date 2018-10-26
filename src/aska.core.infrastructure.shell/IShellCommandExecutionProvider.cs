using System;

namespace kd.infrastructure.ShellCommandExecutionProvider
{
    public interface IShellCommandExecutionProvider
    {
        ExecutionResult Execute(ShelCommand cmd, TimeSpan executionTimeout);
    }
}