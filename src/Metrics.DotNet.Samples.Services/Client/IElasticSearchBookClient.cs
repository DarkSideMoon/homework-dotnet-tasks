﻿using Metrics.DotNet.Samples.Documents;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Services.Client
{
    public interface IElasticSearchBookClient
    {
        Task<IndexResponse> Add(BookDocument data);

        Task<ISearchResponse<BookDocument>> Search(string title);

        Task<ISearchResponse<BookDocument>> SearchMatchAll();

        BulkResponse BulkUpdate(List<BookDocument> data);
    }
}
