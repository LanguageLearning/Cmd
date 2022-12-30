namespace HelloWorld
{
    internal interface ICommand
    {
        void Validate(Prompt prompt);

        void Use(Prompt prompt, string input);

        void Add(Prompt prompt, string input);

        void Remove(Prompt prompt, string input);

        List<string> List(Prompt prompt, string input);

        string Chooser(Prompt prompt, string input);
    }

    internal abstract class AbstractCommand : ICommand
    {
        public abstract void Validate(Prompt prompt);

        public abstract void Use(Prompt prompt, string input);

        public abstract void Add(Prompt prompt, string input);

        public abstract void Remove(Prompt prompt, string input);

        public abstract List<string> List(Prompt prompt, string input);

        public string Chooser(Prompt prompt, string input)
        {
            string[] inputSplitted = input.Split(' ');
            string option = inputSplitted[0].Trim(),
                   args = string.Join(" ", inputSplitted.Skip(1));
            string result = string.Empty;

            Validate(prompt);

            switch (option)
            {
                case "use":
                    Use(prompt, args);
                    break;
                case "add":
                    Add(prompt, args);
                    break;
                case "del":
                    Remove(prompt, args);
                    break;
                case "list":
                    result = string.Join("\n", List(prompt, args));
                    break;
                default:
                    throw new CommandException("Option not found");
            }

            return result;
        }
    }

    internal class LanguageCommand : AbstractCommand
    {

        private LanguageService LanguageService { get; set; }

        public LanguageCommand(LanguageService languageService)
        {
            LanguageService = languageService;
        }

        public override void Validate(Prompt prompt) { }

        public override void Use(Prompt prompt, string input)
        {
            string language = input.Split(" ").First();
            prompt.Language = language;
            prompt.LanguageCategory = string.Empty;

            if (!LanguageService.Has(new Query(prompt.Language)))
                Add(prompt, language);
        }

        public override void Add(Prompt prompt, string input)
        {
            foreach (string language in input.Split(" "))
            {
                LanguageService.Save(new Query(language), new Language(language));
            }
        }

        public override List<string> List(Prompt prompt, string input) => LanguageService.Select(new Query(input.Split(" ").First())).Select(l => l.Name).ToList();

        public override void Remove(Prompt prompt, string input)
        {
            string[] languages = input.Trim().Split(" ");

            if (languages.Length == 0 && !string.IsNullOrWhiteSpace(prompt.Language))
                languages = new string[] { prompt.Language };

            foreach (string language in languages)
            {
                LanguageService.Remove(new Query(language));
            }

            prompt.Language = string.Empty;
        }
    }

    internal class LanguageCategoryCommand : AbstractCommand
    {

        private LanguageCategoryService LanguageCategoryService { get; set; }

        public LanguageCategoryCommand(LanguageCategoryService languageCategoryService)
        {
            LanguageCategoryService = languageCategoryService;
        }

        public override void Validate(Prompt prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt.Language))
                throw new CommandException("you must be using some language to add a new category");
        }

        public override void Use(Prompt prompt, string input)
        {
            string languageCategory = input.Split(" ").First();
            prompt.LanguageCategory = languageCategory;

            if (!LanguageCategoryService.Has(new Query(prompt.LanguageCategory)))
                Add(prompt, languageCategory);
        }


        public override void Add(Prompt prompt, string input)
        {

            foreach (string languageCategory in input.Split(" "))
            {
                LanguageCategoryService.Save(new Query(prompt.Language, languageCategory), new LanguageCategory(languageCategory));
            }
        }

        public override List<string> List(Prompt prompt, string input) => LanguageCategoryService.Select(new Query(prompt.Language, input.Split(" ").First())).Select(l => l.Name).ToList();

        public override void Remove(Prompt prompt, string input)
        {
            string[] languageCategories = input.Trim().Split(" ");

            if (languageCategories.Length == 0 && !string.IsNullOrWhiteSpace(prompt.LanguageCategory))
                languageCategories = new string[] { prompt.LanguageCategory };

            foreach (string languageCategory in languageCategories)
            {
                LanguageCategoryService.Remove(new Query(prompt.Language, languageCategory));
            }

            prompt.LanguageCategory = string.Empty;
        }
    }

    internal class WordPairCommand: AbstractCommand
    {

        private WordPairService WordPairService { get; set; }

        public WordPairCommand(WordPairService wordPairService)
        {
            WordPairService = wordPairService;
        }

        public override void Validate(Prompt prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt.Language))
                throw new CommandException("you must be using some language to add a new word");
            if (string.IsNullOrWhiteSpace(prompt.LanguageCategory))
                throw new CommandException("you must be using some language category to add a new word");
        }

        public override void Use(Prompt prompt, string input)
        {
            throw new CommandException($"You can't use {nameof(Use)} in WordPair command");
        }


        public override void Add(Prompt prompt, string input)
        {
            string[] words = input.Split(" ");
            if (words.Length % 2 != 0)
                throw new CommandException("Please, use an even number of words: first firstTranslation second secondTranslation");

            for (int i = 0; i < words.Length; i += 2)
            {
                WordPairService.Save(new Query(prompt.Language, prompt.LanguageCategory), new WordPair(new Word(words[i]), new Word(words[i + 1])));
            }
        }

        public override List<string> List(Prompt prompt, string input) => WordPairService.
            Select(new Query(prompt.Language, prompt.LanguageCategory, new WordPair(new Word(input.Split(" ").First()), new Word()))).
            Select(pw => $"{pw.From.Name} : {pw.To.Name}").
            ToList();

        public override void Remove(Prompt prompt, string input)
        {
            string[] fromWords = input.Trim().Split(" ");

            foreach (string fromWord in fromWords)
            {
                WordPairService.Remove(new Query(prompt.Language, prompt.LanguageCategory, new WordPair(new Word(fromWord), new Word())));
            }
        }
    }


    internal class CommandException : Exception
    {
        public CommandException() : base() { }
        public CommandException(string message) : base(message) { }

        public CommandException(string message, Exception innerException) : base(message, innerException) { }
    }
}
