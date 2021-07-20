using System;

namespace Metrics.DotNet.Samples.Contracts
{
    public class Book : IStorageId
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Price { get; set; }

        public string ISBN { get; set; }

        public int CountOfPages { get; set; }

        public LanguageType Language { get; set; }

        public BookType BookType { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public string AuthorEmail { get; set; }

        string IStorageId.Id => Id.ToString();
    }
}
