using System;

namespace Metrics.DotNet.Samples.Contracts
{
    public class Book : IStorageId
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public double Price { get; set; }

        public string ISBN { get; set; }

        public int CountOfPages { get; set; }

        public LanguageType Language { get; set; }

        public BookType BookType { get; set; }

        public Person Author { get; set; }

        string IStorageId.Id => Id.ToString();
    }
}
