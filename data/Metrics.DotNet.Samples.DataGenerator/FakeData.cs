﻿using Bogus;
using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Documents;

namespace Metrics.DotNet.Samples.DataGenerator
{
    public static class FakeData
    {
        public static Faker<BookDocument> BookDocument = new Faker<BookDocument>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Language, f => f.Random.Enum<LanguageType>())
            .RuleFor(x => x.Price, f => f.Random.Double())
            .RuleFor(x => x.Title, f => f.Company.CompanyName())
            .RuleFor(x => x.ISBN, f => f.Random.Guid().ToString())
            .RuleFor(x => x.CountOfPages, f => f.Random.Number())
            .RuleFor(x => x.BookType, f => f.Random.Enum<BookType>())
            .RuleFor(x => x.AuthorFirstName, f => f.Person.FirstName)
            .RuleFor(x => x.AuthorLastName, f => f.Person.LastName)
            .RuleFor(x => x.AuthorEmail, f => f.Person.Email);
    }
}
