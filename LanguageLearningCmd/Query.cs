
namespace HelloWorld
{
    internal class Query
    {

        public string Language { get; set; }

        public string LanguageCategory { get; set; }

        public WordPair Word { get; set; }

        public Query(string language)
        {
            Language = language;
            LanguageCategory = string.Empty;
            Word = new WordPair();
        }

        public Query(string language, string languageCategory)
        {
            Language = language;
            LanguageCategory = languageCategory;
            Word = new WordPair();
        }

        public Query(string language, string languageCategory, WordPair word)
        {
            Language = language;
            LanguageCategory = languageCategory;
            Word = word;
        }
    }
}
