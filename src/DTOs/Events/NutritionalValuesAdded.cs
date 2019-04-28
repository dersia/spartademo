using SiaConsulting.EO.Abstractions;

namespace Spartademo.DTOs.Events
{
    public class NutritionalValuesAdded : IEvent
    {
        public string ProfilId { get; set; }
        public decimal EnergyInKCal { get; set; }
    }
}