using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homework.Dotnet.Tasks.Documents;
using Homework.Dotnet.Tasks.Services.Settings;
using Microsoft.Extensions.Options;
using Nest;
using Serilog;

namespace Homework.Dotnet.Tasks.Services.Client
{
    public class ElasticSearchBookClient : IElasticSearchBookClient
    {
        private readonly ILogger _logger = Log.ForContext<ElasticSearchBookClient>();

        private readonly ElasticClient _client;
        private readonly IOptions<ElasticSearchSetting> _setting;

        public ElasticSearchBookClient(IOptions<ElasticSearchSetting> setting)
        {
            _setting = setting;

            var connectionSettings = new ConnectionSettings(new Uri(setting.Value.Uri));
            connectionSettings.DefaultIndex(setting.Value.IndexName);

            _client = new ElasticClient(connectionSettings);
            _client.Indices.Create(setting.Value.IndexName, x =>
                x.Map<BookDocument>(b =>
                    b.AutoMap<BookDocument>().Properties(props =>
                        props.Completion(c => c.Name(n => n.Suggest).Analyzer("simple").SearchAnalyzer("simple"))))
            );
        }

        public async Task<IndexResponse> Add(BookDocument data)
        {
            // https://stackoverflow.com/questions/53039564/what-is-the-difference-between-createasync-and-indexasync-methods-on-elasticsear
            //var res = _client.CreateDocumentAsync(data);
            return await _client.IndexDocumentAsync(data);
        }

        public async Task<IEnumerable<BookSuggestResponse>> Search(string searchString)
        {
            var searchResponse = await _client.SearchAsync<BookDocument>(
                s => s.Index(_setting.Value.IndexName).Suggest(su =>
                    su.Completion("suggest",
                        cs => cs.Field(f => f.Suggest).Prefix(searchString).Fuzzy(f => f.Fuzziness(Fuzziness.Auto))
                            .Analyzer("simple")
                            .Size(10)
                            .SkipDuplicates())));

            var suggestions =
                from suggest in searchResponse.Suggest["suggest"]
                from option in suggest.Options
                select new BookSuggestResponse
                {
                    Id = option.Id,
                    SuggestName = option.Text,
                    Score = option.Score,
                    Frequency = option.Frequency,

                    BookTitle = option.Source.Title,
                    BookId = option.Source.Id,
                    AuthorEmail = option.Source.AuthorEmail,
                    AuthorLastName = option.Source.AuthorLastName
                };

            return suggestions;
        }

        public Task<ISearchResponse<BookDocument>> SearchAll(string searchString)
        {
            throw new NotImplementedException();
        }

        public async Task<ISearchResponse<BookDocument>> SearchMatchAll()
        {
            return await _client.SearchAsync<BookDocument>(x => x.Query(q => q.MatchAll()));
        }

        //public async Task<ISearchResponse<BookDocument>> Search(BookDocument search)
        //{
        //    return _client.Search<BookDocument>(s => s.Query(q => q.Match(m => m.Field(f => f.Title).Query(search.Title))));
        //    //return await _client.SearchAsync<BookDocument>(x => x.Query(q => q.MatchAll()));
        //}

        public BulkResponse BulkUpdate(List<BookDocument> data)
        {
            try
            {
                var bulkDescriptor = new BulkDescriptor();
                foreach (var item in data)
                {
                    var updateDescriptor = new BulkUpdateDescriptor<BookDocument, BookDocument>()
                        .Index(_setting.Value.IndexName)
                        .Id(item.Id)
                        .Doc(item)
                        .DocAsUpsert()
                        .RetriesOnConflict(1);

                    bulkDescriptor.AddOperation(updateDescriptor);
                }

                return _client.Bulk(bulkDescriptor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Could not bulk update book data in elasticsearch");
                throw;
            }
        }
    }
}
