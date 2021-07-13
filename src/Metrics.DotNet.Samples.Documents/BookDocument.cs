using Metrics.DotNet.Samples.Contracts;
using Nest;
using System;

namespace Metrics.DotNet.Samples.Documents
{
    [ElasticsearchType(RelationName = "Book", IdProperty = nameof(Id))]
    public class BookDocument
    {
        [Text(Name = "Id")]
        public Guid Id { get; set; }

        [Text(Name = "Title")]
        public string Title { get; set; }

        [Number(NumberType.Double, Name = "Price")]
        public double Price { get; set; }

        [Text(Name = "ISBN")]
        public string ISBN { get; set; }

        [Number(NumberType.Integer, Name = "CountOfPages")]
        public int CountOfPages { get; set; }

        [Keyword(Name = "Language")]
        public LanguageType Language { get; set; }

        [Keyword(Name = "BookType")]
        public BookType BookType { get; set; }

        [Text(Name = "AuthorFirstName")]
        public string AuthorFirstName { get; set; }

        [Text(Name = "AuthorLastName")]
        public string AuthorLastName { get; set; }

        [Text(Name = "AuthorEmail")]
        public string AuthorEmail { get; set; }

        // TODO: Make Author inherited in elastic document
        //public Person Author { get; set; }
    }
}
