namespace HelloWorld
{
    internal class Prompt
    {
        public string Language { get; set; }

        public string LanguageCategory { get; set; }

        public Prompt()
        {
            Language = "";
            LanguageCategory = "";
        }

        public Prompt(string language)
        {
            Language = language;
            LanguageCategory = "";
        }

        public Prompt(string language, string languageCategory)
        {
            Language = language;
            LanguageCategory = languageCategory;
        }

        public override string ToString()
        {
            string result = "";

            if (Language.Trim() != "")
                result += $"{Language}";

            if (LanguageCategory.Trim() != "")
                result += $" {LanguageCategory}";

            return result + ">";
        }
    }

    internal class Cli
    {
        private readonly HashSet<string> ExitCommands = new HashSet<string>()
        {
            "exit",
            "e",
            "quit",
            "q"
        };

        private Prompt _prompt { get; set; }

        private Dictionary<string, ICommand> Commands = new Dictionary<string, ICommand>();

        public Cli()
        {
            _prompt = new Prompt();
        }

        public void ReadLine()
        {
            Console.Write(_prompt.ToString());
            string? input = Console.ReadLine();
            _ = input ?? throw new ArgumentNullException(nameof(ReadLine));

            if (ExitCommands.Contains(input.Trim()))
                throw new CliException("Exiting terminal simulator");

            if (string.IsNullOrWhiteSpace(input))
                return;

            ICommand? command;
            string[] inputSplitted = input.Split(" ");
            if (!Commands.TryGetValue(inputSplitted[0], out command))
            {
                Console.WriteLine($"The command that you have specified {inputSplitted[0]} doesn't exists");
                return;
            }

            try
            {
                string output = command.Chooser(_prompt, string.Join(" ", inputSplitted.Skip(1)));
                if (!string.IsNullOrEmpty(output.Trim()))
                    Console.WriteLine(output);
            } catch (CommandException ce)
            {
                Console.WriteLine(ce.Message);
            }
        }

        public void AddCommand(string commandName, ICommand commandAction)
        {
            Commands.Add(commandName, commandAction);
        }

        public string PrintPrompt()
        {
            return _prompt.ToString();
        }
    }

    [Serializable]
    internal class CliException : Exception
    {
        public CliException() : base() { }
        public CliException(string message) : base(message) { }
        public CliException(string message, Exception innerException) : base(message, innerException) { }
    }
}
