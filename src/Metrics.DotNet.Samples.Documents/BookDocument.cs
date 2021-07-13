using Nest;
using System;

namespace Metrics.DotNet.Samples.Documents
{
    [ElasticsearchType(RelationName = "Book", IdProperty = nameof(Id))]
    public class BookDocument
    {
        public Guid Id { get; set; }
    }
}
