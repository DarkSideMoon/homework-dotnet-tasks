using System.Collections.Generic;
using System.Threading.Tasks;
using Homework.Dotnet.Tasks.Documents;
using Nest;

namespace Homework.Dotnet.Tasks.Services.Client
{
    public interface IElasticSearchBookClient
    {
        Task<IndexResponse> Add(BookDocument data);

        Task<ISearchResponse<BookDocument>> Search(string title);

        Task<ISearchResponse<BookDocument>> SearchMatchAll();

        BulkResponse BulkUpdate(List<BookDocument> data);
    }
}
