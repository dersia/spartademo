using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using SiaConsulting.Azure.WebJobs.Extensions.EventStoreExtension.Streams;
using SiaConsulting.EO.Abstractions;
using SiaConsulting.EO;

namespace Spartademo
{
    public partial class HandleActivity
    {
        public static async Task HandleEvent(
            IEvent @event,
            ReadOnlyCollection<ResolvedEvent> eventStream, 
            IAsyncCollector<dynamic> objectStore, 
            Microsoft.Extensions.Logging.ILogger log) 
        {
            foreach(var result in HandleEvents.Handle<dynamic>(@event, eventStream, log)) 
            {
                await objectStore.AddAsync(result);
            }
        }        
    }
}