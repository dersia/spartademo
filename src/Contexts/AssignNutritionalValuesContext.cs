using System.Collections.ObjectModel;
using SiaConsulting.EO.Abstractions;

namespace Spartademo.Contexts
{
    public class AssignNutritionalValuesContext : IContext
    {
        public ReadOnlyCollection<IEvent> Stream { get; set; }
    }
}
