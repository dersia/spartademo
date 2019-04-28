using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using SiaConsulting.Azure.WebJobs.Extensions.EventStoreExtension.Streams;
using SiaConsulting.EO.Abstractions;
using SiaConsulting.EO;
using Spartademo.ContextLoaders;
using Spartademo.DTOs.Commands;
using Spartademo.Processors;
using Spartademo.DTOs.Notifications;

namespace Spartademo
{
    public static class Triggers
    {
        static Triggers()
        {
            var registerBankProfilContextLoader = new RegisterBankProfilCommandContextLoader();
            var registerBankProfilProcessor = new RegisterBankProfilCommandProcessor();
            EoRegistry.RegisterCommandPump<RegisterBankProfilCommand>(
                new Pump
                (registerBankProfilContextLoader, registerBankProfilProcessor));

            var registerFitnessProfilContextLoader = new RegisterFitnessProfilCommandContextLoader();
            var registerFitnessProfilProcessor = new RegisterFitnessProfilCommandProcessor();
            EoRegistry.RegisterCommandPump<RegisterFitnessProfilCommand>(
                new Pump
                (registerFitnessProfilContextLoader, registerFitnessProfilProcessor));

            var assignPaymentCommandContextLoader = new AssignPaymentCommandContextLoader();
            var assignPaymentProcessor = new AssignPaymentCommandProcessor();
            EoRegistry.RegisterCommandPump<AssignPaymentCommand>(
                new Pump
                (assignPaymentCommandContextLoader, assignPaymentProcessor));

            var assignNutritionalValuesCommandContextLoader = new AssignNutritionalValuesCommandContextLoader();
            var assignNutritionalValuesProcessor = new AssignNutritionalValuesCommandProcessor();
            EoRegistry.RegisterCommandPump<AssignNutritionalValuesCommand>(
                new Pump
                (assignNutritionalValuesCommandContextLoader, assignNutritionalValuesProcessor));

            EoRegistry.RegisterNotificationPump<NahrungAufgenommenNotification>(
                new Pump(
                    new NahrungAufgenommenContextLoader(), 
                    new NahrungAufgenommenNotficationProcessor()
                )
            );
        }

        [FunctionName("MessagePump_HttpTrigger")]
        public static async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [EventStoreStreams(ConnectionStringSetting = "EventStoreEndpoint", StreamName = "eoStream")] IList<ResolvedEvent> eventStream,
            [EventStoreStreams(ConnectionStringSetting = "EventStoreEndpoint", StreamName = "eoStream")] IAsyncCollector<EventData> eventStore,
            [Queue("event-queue2", Connection = "StorageEndpoint")] IAsyncCollector<IEvent> eventQueue,
            [CosmosDB("eo","readModels", ConnectionStringSetting = "CosmosEndpoint")]IAsyncCollector<dynamic> objectStore,
            [ServiceBus("command-queue", Connection = "SbEndpoint")] IAsyncCollector<ICommand> commandStore,
            Microsoft.Extensions.Logging.ILogger log)
        {
            // Function input comes from the request content.
            await StartMessagePump(
                await req.Content.ReadAsStringAsync().ConfigureAwait(false),
                eventStream.ToList().AsReadOnly(),
                eventStore,
                eventQueue,
                objectStore,
                commandStore,
                log
                );

            log.LogInformation($"Started orchestration from Http");

            return new AcceptedResult();
        }

        [FunctionName("MessagePump_ServiceBusTrigger")]
        public static async Task SbStart(
            [ServiceBusTrigger("command-queue", Connection = "SbEndpoint")] Message message,
            [EventStoreStreams(ConnectionStringSetting = "EventStoreEndpoint", StreamName = "eoStream")] IList<ResolvedEvent> eventStream,
            [EventStoreStreams(ConnectionStringSetting = "EventStoreEndpoint", StreamName = "eoStream")] IAsyncCollector<EventData> eventStore,
            [Queue("event-queue2", Connection = "StorageEndpoint")] IAsyncCollector<IEvent> eventQueue,
            [CosmosDB("eo","readModels", ConnectionStringSetting = "CosmosEndpoint")]IAsyncCollector<dynamic> objectStore,
            [ServiceBus("command-queue", Connection = "SbEndpoint")] IAsyncCollector<ICommand> commandStore,
            Microsoft.Extensions.Logging.ILogger log) 
        {
            await StartMessagePump(
                Encoding.UTF8.GetString(message.Body),
                eventStream.ToList().AsReadOnly(),
                eventStore,
                eventQueue,
                objectStore,
                commandStore,
                log
                );

            log.LogInformation($"Started orchestration from SB");
        }

        [Singleton(Mode = SingletonMode.Listener)]
        [FunctionName("MessagePump_QueueTrigger")]
        public static async Task QueueStart(
            [QueueTrigger("event-queue2", Connection = "StorageEndpoint")] string message,
            [EventStoreStreams(ConnectionStringSetting = "EventStoreEndpoint", StreamName = "eoStream")] IList<ResolvedEvent> eventStream,
            [EventStoreStreams(ConnectionStringSetting = "EventStoreEndpoint", StreamName = "eoStream")] IAsyncCollector<EventData> eventStore,
            [Queue("event-queue2", Connection = "StorageEndpoint")] IAsyncCollector<IEvent> eventQueue,
            [CosmosDB("eo", "readModels", ConnectionStringSetting = "CosmosEndpoint")]IAsyncCollector<dynamic> objectStore,
            [ServiceBus("command-queue", Connection = "SbEndpoint")] IAsyncCollector<ICommand> commandStore,
            Microsoft.Extensions.Logging.ILogger log)
        {
            await StartMessagePump(
                message,
                eventStream.ToList().AsReadOnly(),
                eventStore,
                eventQueue,
                objectStore,
                commandStore,
                log
                );

            log.LogInformation($"Started orchestration from Queue");
        }

        private static Task StartMessagePump(
            string message,
            ReadOnlyCollection<ResolvedEvent> eventStream,
            IAsyncCollector<EventData> eventStore,
            IAsyncCollector<IEvent> eventQueue, 
            IAsyncCollector<dynamic> objectStore,
            IAsyncCollector<ICommand> commandStore,
            Microsoft.Extensions.Logging.ILogger log)
        {
            return MessagePump.RunOrchestrator(message, eventStream, eventStore, eventQueue, objectStore, commandStore, log);
        }
    }
}