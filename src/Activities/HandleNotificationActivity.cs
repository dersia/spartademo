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
        public static async Task HandleNotification(
            INotification notification,
            ReadOnlyCollection<ResolvedEvent> eventStream, 
            IAsyncCollector<ICommand> commandStore, 
            Microsoft.Extensions.Logging.ILogger log) 
        {
            foreach(var command in HandleNotifications.Handle(notification, eventStream, log)) 
            {
                await commandStore.AddAsync(command);
            }
        }
    }
}