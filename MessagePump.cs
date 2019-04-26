using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SiaConsulting.Azure.WebJobs.Extensions.EventStoreExtension.Streams;
using SiaConsulting.EO.Abstractions;
using SiaConsulting.EO;

namespace Spartademo
{
    public static class MessagePump
    {
        public static async Task RunOrchestrator(
            string messagePayload,
            ReadOnlyCollection<ResolvedEvent> eventStream,
            IAsyncCollector<EventData> eventStore,
            IAsyncCollector<IEvent> eventQueue, 
            IAsyncCollector<dynamic> objectStore,
            IAsyncCollector<ICommand> commandStore,
            Microsoft.Extensions.Logging.ILogger log)
        {
            log.LogInformation($"Pump started with message: `{ messagePayload }`");
            var message = new MessageBase(messagePayload);

            var pumps = message.Payload switch 
            {
                INotification notification => HandleActivity.HandleNotification(notification, eventStream, commandStore, log),
                IEvent @event => HandleActivity.HandleEvent(@event, eventStream, objectStore, log),
                ICommand command => HandleActivity.HandleCommand(command, eventStream, eventStore, eventQueue, log),
                IQuery query => HandleActivity.HandleQuery(query, eventStream, objectStore, log),
                _ => throw new IndexOutOfRangeException()
            };

            await pumps.ConfigureAwait(false);
        }
    }
}