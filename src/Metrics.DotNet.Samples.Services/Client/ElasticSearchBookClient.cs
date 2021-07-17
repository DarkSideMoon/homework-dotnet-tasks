using Metrics.DotNet.Samples.Documents;
using Metrics.DotNet.Samples.Services.Settings;
using Microsoft.Extensions.Options;
using Nest;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Services.Client
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
        }

        public Task<IndexResponse> Add(BookDocument data)
        {
            // https://stackoverflow.com/questions/53039564/what-is-the-difference-between-createasync-and-indexasync-methods-on-elasticsear
            //var res = _client.CreateDocumentAsync(data);
            return _client.IndexDocumentAsync(data);
        }

        public async Task<ISearchResponse<BookDocument>> Search(string title)
        {
            return _client.Search<BookDocument>(s => s.Query(q => q.Match(m => m.Field(f => f.Title).Query(title))));
            //return await _client.SearchAsync<BookDocument>(x => x.Query(q => q.MatchAll()));
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
