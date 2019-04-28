using SiaConsulting.EO.Abstractions;

namespace Spartademo.DTOs.Events
{
    public class FitnessprofileNotFound: IEvent
    {
        public string ProfilId { get; set; }
    }
}
