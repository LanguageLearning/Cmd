namespace HelloWorld
{
    internal class Language
    {
        public string Name { get; set; }

        public List<LanguageCategory> Categories { get; set; }

        public Language()
        {
            Name = "";
            Categories = new List<LanguageCategory>();
        }

        public Language(string name)
        {
            Name = name;
            Categories = new List<LanguageCategory>();
        }

        public Language(string name, List<LanguageCategory> categories)
        {
            Name = name;
            Categories = categories;
        }
    }

    internal class Word
    {
        public string Name { get; set; }

        public Word()
        {
            Name = string.Empty;
        }

        public Word(string name)
        {
            Name = name;
        }
    }

    internal class WordPair
    {
        public Word From { get; set; }

        public Word To { get; set; }

        public WordPair()
        {
            From = new Word();
            To = new Word();
        }

        public WordPair(Word from, Word to)
        {
            From = from;
            To = to;
        }
    }

    internal class LanguageCategory
    {
        public string Name { get; set; }

        public List<WordPair> Words { get; set; }

        public LanguageCategory(string name)
        {
            Name = name;
            Words = new List<WordPair>();
        }

        public LanguageCategory(string name, List<WordPair> words)
        {
            Name = name;
            Words = words;
        }
    }
}
