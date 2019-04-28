using SiaConsulting.EO.Abstractions;

namespace Spartademo.DTOs.Commands
{
    public class RegisterFitnessProfilCommand : ICommand
    {
        public string ProfilId { get; set; }
        public string ServiceEndpoint { get; set; }
    }
}