using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Azure.WebJobs;
using SiaConsulting.Azure.WebJobs.Extensions.EventStoreExtension.Streams;
using SiaConsulting.EO.Abstractions;
using SiaConsulting.EO;

namespace Spartademo
{
    public partial class HandleActivity
    {
        public static async Task HandleQuery(
            IQuery query,
            ReadOnlyCollection<ResolvedEvent> eventStream, 
            IAsyncCollector<dynamic> objectStore, 
            Microsoft.Extensions.Logging.ILogger log) 
        {
            foreach(var result in HandleQuerys.Handle<dynamic>(query, eventStream, log)) 
            {
                await objectStore.AddAsync(result);
            }
        }
    }
}