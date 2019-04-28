using SiaConsulting.EO.Abstractions;

namespace Spartademo.DTOs.Commands
{
    public class AssignNutritionalValuesCommand : ICommand
    {
        public string ProfilId { get; set; }
        public int EnergyInKCal { get; set; }
    }
}