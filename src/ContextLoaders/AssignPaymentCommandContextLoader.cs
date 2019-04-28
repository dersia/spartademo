using System.Collections.ObjectModel;
using System.Linq;
using SiaConsulting.EO;
using SiaConsulting.EO.Abstractions;
using Spartademo.Contexts;
using Spartademo.DTOs.Events;

namespace Spartademo.ContextLoaders
{
    public class AssignPaymentCommandContextLoader : ContextLoaderBase
    {
        public override IContext Render<TMessage>(TMessage message, ReadOnlyCollection<IEvent> stream) 
            => new AssignPaymentContext { Stream = stream.Where(s => s is BankProfilRegistered).ToList().AsReadOnly() };
    }
}