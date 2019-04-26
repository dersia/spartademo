using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaConsulting.EO.Abstractions;
using SiaConsulting.EO;
using EventStore.ClientAPI;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using SiaConsulting.Azure.WebJobs.Extensions.EventStoreExtension.Streams;

namespace Spartademo
{
    public partial class HandleActivity
    {
        public static async Task HandleCommand( 
            ICommand command,
            ReadOnlyCollection<ResolvedEvent> eventStream, 
            IAsyncCollector<EventData> eventStore, 
            IAsyncCollector<IEvent> eventQueue, 
            Microsoft.Extensions.Logging.ILogger log) 
        {
            foreach(var @event in HandleCommands.Handle(command, eventStream, log)) 
            {
                await eventStore.AddAsync(ToEventData(@event));
                await eventQueue.AddAsync(@event);
            }
        }

        private static EventData ToEventData(IEvent @event) 
            => new EventData(Guid.NewGuid(), @event.GetType().FullName, true, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)), null);
    }
}