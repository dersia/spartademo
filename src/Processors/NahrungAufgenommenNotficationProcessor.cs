using System;
using System.Collections.Generic;
using SiaConsulting.EO.Abstractions;

namespace Spartademo.Processors
{
    public class NahrungAufgenommenNotficationProcessor : INotificationProcessor
    {
        public IList<ICommand> Process<TNotification, TContext>(TNotification notification, TContext context)
            where TNotification : INotification
            where TContext : IContext
        {
            throw new NotImplementedException();
        }
    }
}
