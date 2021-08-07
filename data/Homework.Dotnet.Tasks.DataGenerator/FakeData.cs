using System;
using Bogus;
using Homework.Dotnet.Tasks.Contracts;
using Homework.Dotnet.Tasks.Documents;

namespace Homework.Dotnet.Tasks.DataGenerator
{
    public static class FakeData
    {
        public static Faker<BookDocument> BookDocument = new Faker<BookDocument>()
            .RuleFor(x => x.Id, f => f.Random.Number(1, 100000))
            .RuleFor(x => x.Language, f => f.Random.Enum<LanguageType>())
            .RuleFor(x => x.Price, f => f.Random.Double())
            .RuleFor(x => x.Title, f => f.Company.CompanyName())
            .RuleFor(x => x.ISBN, f => f.Random.Guid().ToString())
            .RuleFor(x => x.CountOfPages, f => f.Random.Number())
            .RuleFor(x => x.BookType, f => f.Random.Enum<BookType>())
            .RuleFor(x => x.AuthorFirstName, f => f.Person.FirstName)
            .RuleFor(x => x.AuthorLastName, f => f.Person.LastName)
            .RuleFor(x => x.AuthorEmail, f => f.Person.Email);

        public static Faker<Book> Book = new Faker<Book>()
            .RuleFor(x => x.Language, f => f.Random.Enum<LanguageType>())
            .RuleFor(x => x.Price, f => f.Random.Number(100, 1000))
            .RuleFor(x => x.Title, f => f.Company.CompanyName())
            .RuleFor(x => x.ISBN, f => f.Random.Guid().ToString())
            .RuleFor(x => x.CountOfPages, f => f.Random.Number())
            .RuleFor(x => x.BookType, f => f.Random.Enum<BookType>())
            .RuleFor(x => x.AuthorFirstName, f => f.Person.FirstName)
            .RuleFor(x => x.AuthorLastName, f => f.Person.LastName)
            .RuleFor(x => x.AuthorEmail, f => f.Person.Email)
            .RuleFor(x => x.CreateDate, f => f.Date.Between(new DateTime(2000, 01, 01), DateTime.Now));
    }
}
