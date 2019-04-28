using SiaConsulting.EO;
using SiaConsulting.EO.Abstractions;
using System;
using System.Collections.ObjectModel;

namespace Spartademo.ContextLoaders
{
    public class NahrungAufgenommenContextLoader : ContextLoaderBase
    {
        public override IContext Render<TMessage>(TMessage message, ReadOnlyCollection<IEvent> stream)
        {
            throw new NotImplementedException();
        }
    }
}
