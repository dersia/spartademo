using SiaConsulting.EO.Abstractions;

namespace Spartademo.DTOs.Events
{
    public class BankProfilRegistered : IEvent
    {
        public string ProfilId { get; set; }
        public string ServiceEndpoint { get; set; }
    }
}