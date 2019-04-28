using System.Collections.ObjectModel;
using SiaConsulting.EO.Abstractions;

namespace Spartademo.Contexts
{
    public class NoneContext : IContext
    {
        public ReadOnlyCollection<IEvent> Stream { get; }
    }
}