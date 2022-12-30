using System.IO.Enumeration;
using System.Text.Json;

namespace HelloWorld
{

    interface ICRUDRepository<T>
    {
        bool Has(Query query);

        T? Find(Query query);

        void Save(Query query, T t);

        List<T> Select(Query query);

        void Remove(Query query);
    }

    internal class LanguageRepository : ICRUDRepository<Language>
    {

        public DDBB DDBB { get; set; }

        public LanguageRepository(DDBB ddbb)
        {
            DDBB = ddbb;
        }

        public Language? Find(Query query) => DDBB.Languages.Find(l => l.Name == query.Language);

        public bool Has(Query query) => DDBB.Languages.Exists(l => l.Name == query.Language);

        public void Remove(Query query) => DDBB.Languages.Remove(Find(query) ?? new Language());

        public void Save(Query query, Language t)
        {
            if (!Has(query))
                DDBB.Languages.Add(t);
        }

        public List<Language> Select(Query query) => DDBB.Languages.Where(l => l.Name.Contains(query.Language)).ToList();
    }

    internal class LanguageCategoryRepository : ICRUDRepository<LanguageCategory>
    {
        public DDBB DDBB { get; set; }

        public LanguageCategoryRepository(DDBB ddbb)
        {
            DDBB = ddbb;
        }

        public LanguageCategory? Find(Query query) => DDBB.Languages.Find(l => l.Name == query.Language)?.Categories.Find(lc => lc.Name == query.LanguageCategory);

        public bool Has(Query query) => Find(query) is not null;

        public void Remove(Query query) => DDBB.Languages.Find(l => l.Name == query.Language)?.Categories.RemoveAll(lc => lc.Name == query.LanguageCategory);

        public void Save(Query query, LanguageCategory lc)
        {
            if (!Has(query))
                DDBB.Languages.Find(l => l.Name == query.Language)?.Categories.Add(lc);
        }

        public List<LanguageCategory> Select(Query query) => DDBB.Languages.
            Find(l => l.Name == query.Language)?.Categories.
            Where(lc => lc.Name.Contains(query.LanguageCategory)).
            ToList() ?? new List<LanguageCategory>();
    }

    internal class WordPairRepository : ICRUDRepository<WordPair>
    {

        private DDBB DDBB { get; set; }

        public WordPairRepository(DDBB ddbb)
        {
            DDBB = ddbb;
        }

        public WordPair? Find(Query query) => DDBB.Languages.
            Find(l => l.Name == query.Language)?.Categories.
            Find(lc => lc.Name == query.LanguageCategory)?.Words.
            Find(w => w.From == query.Word.From);

        public bool Has(Query query) => Find(query) is not null;

        public void Remove(Query query) => DDBB.Languages.
            Find(l => l.Name == query.Language)?.Categories.
            Find(lc => lc.Name == query.LanguageCategory)?.Words.
            RemoveAll(w => w.From.Name == query.Word.From.Name);

        public void Save(Query query, WordPair wordPair)
        {
            if (!Has(query))
                DDBB.Languages.
                    Find(l => l.Name == query.Language)?.Categories.
                    Find(lc => lc.Name == query.LanguageCategory)?.Words.Add(wordPair);
        }

        public List<WordPair> Select(Query query) => DDBB.Languages.
            Find(l => l.Name == query.Language)?.Categories.
            Find(lc => lc.Name == query.LanguageCategory)?.Words.
            Where(w => w.From.Name.Contains(query.Word.From.Name)).ToList() ?? new List<WordPair>();
    }

    internal class DDBB
    {

        public string FileName { get; set; } 

        public List<Language> Languages { get; set; }

        public DDBB(string filename)
        {
            FileName = filename;
            Languages = File.Exists(FileName) ? JsonSerializer.Deserialize<List<Language>>(File.ReadAllText(FileName)) ?? new List<Language>() : new List<Language>();
        }

        public void Persist()
        {
            File.WriteAllText(FileName, JsonSerializer.Serialize(Languages));
        }
    }
}
