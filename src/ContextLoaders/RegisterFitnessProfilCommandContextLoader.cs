using System.Collections.ObjectModel;
using SiaConsulting.EO;
using SiaConsulting.EO.Abstractions;
using Spartademo.Contexts;

namespace Spartademo.ContextLoaders
{
    public class RegisterFitnessProfilCommandContextLoader : ContextLoaderBase
    {
        public override IContext Render<TMessage>(TMessage message, ReadOnlyCollection<IEvent> stream)
            => new NoneContext();
    }
}