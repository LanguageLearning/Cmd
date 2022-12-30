namespace HelloWorld
{

    internal interface IService<T>
    {
        bool Has(Query query);

        void Save(Query query, T t);

        void Remove(Query query);

        List<T> Select(Query query);
    }

    internal class LanguageService : IService<Language>
    {

        private ICRUDRepository<Language> LanguageRepository { get; set; }

        public LanguageService(LanguageRepository languageRepository)
        {
            LanguageRepository = languageRepository;
        }

        public bool Has(Query query) => LanguageRepository.Has(query);

        public void Save(Query query, Language language)
        {
            LanguageRepository.Save(query, language);
        }
        public void Remove(Query query) => LanguageRepository.Remove(query);

        public List<Language> Select(Query query) => LanguageRepository.Select(query);
    }

    internal class LanguageCategoryService : IService<LanguageCategory>
    {

        private ICRUDRepository<LanguageCategory> LanguageCategoryRepository { get; set; }

        public LanguageCategoryService(LanguageCategoryRepository languageCategoryRepository)
        {
            LanguageCategoryRepository = languageCategoryRepository;
        }

        public bool Has(Query query) => LanguageCategoryRepository.Has(query);

        public void Save(Query query, LanguageCategory languageCategory)
        {
            LanguageCategoryRepository.Save(query, languageCategory);
        }

        public void Remove(Query query) => LanguageCategoryRepository.Remove(query);

        public List<LanguageCategory> Select(Query query) => LanguageCategoryRepository.Select(query);
    }

    internal class WordPairService
    {

        private ICRUDRepository<WordPair> WordPairRepository { get; set; }

        public WordPairService(WordPairRepository wordPairRepository)
        {
            WordPairRepository = wordPairRepository;
        }

        public bool Has(Query query) => WordPairRepository.Has(query);

        public void Save(Query query, WordPair wordPair)
        {
            WordPairRepository.Save(query, wordPair);
        }

        public void Remove(Query query) => WordPairRepository.Remove(query);

        public List<WordPair> Select(Query query) => WordPairRepository.Select(query);

    }
}
