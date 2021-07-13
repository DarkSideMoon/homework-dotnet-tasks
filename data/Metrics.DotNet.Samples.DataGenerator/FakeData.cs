using Bogus;
using Metrics.DotNet.Samples.Contracts;

namespace Metrics.DotNet.Samples.DataGenerator
{
    public static class FakeData
    {
        public static Faker<Book> Book = new Faker<Book>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            ;
    }
}
