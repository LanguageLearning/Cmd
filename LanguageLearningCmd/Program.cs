namespace HelloWorld
{
    public class Program
    {

        private static readonly string _fileName = "C:\\Users\\imartinez\\languages.json";

        public static void Main()
        {
            DDBB ddbb = new DDBB(_fileName);
            Cli cli = new Cli();

            cli.AddCommand("l", new LanguageCommand(new LanguageService(new LanguageRepository(ddbb)))); // We need to fix this thing... We are going to use DI eventually.
            cli.AddCommand("lc", new LanguageCategoryCommand(new LanguageCategoryService(new LanguageCategoryRepository(ddbb))));
            cli.AddCommand("w", new WordPairCommand(new WordPairService(new WordPairRepository(ddbb))));

            try
            {
                while (true)
                    cli.ReadLine();
            } catch (CliException e)
            {
                ddbb.Persist();
                Console.WriteLine(e.Message);
            }
        }
    }
}