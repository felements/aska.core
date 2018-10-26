namespace kd.infrastructure.ShellCommandExecutionProvider
{
    public class ShelCommand
    {
        public string Command { get; set; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Command, Arguments);
        }
    }
}