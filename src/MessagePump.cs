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
            var message = new MessageBase(messagePayload, typeof(MessagePump));

            var pumps = default(Task);
            switch(message.Payload) 
            {
                case INotification notification: 
                        pumps = HandleActivity.HandleNotification(notification, eventStream, commandStore, log);
                        break;
                case IEvent @event: 
                        pumps = HandleActivity.HandleEvent(@event, eventStream, objectStore, log);
                        break;
                case ICommand command: 
                        pumps = HandleActivity.HandleCommand(command, eventStream, eventStore, eventQueue, log);
                        break;
                case IQuery query: 
                        pumps = HandleActivity.HandleQuery(query, eventStream, objectStore, log);
                        break;
                default: 
                        throw new IndexOutOfRangeException();
            };

            await pumps.ConfigureAwait(false);
        }
    }
}