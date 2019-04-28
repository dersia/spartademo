using SiaConsulting.EO.Abstractions;

namespace Spartademo.DTOs.Commands
{
    public class RegisterBankProfilCommand : ICommand
    {
        public string ProfilId { get; set; }
        public string ServiceEndpoint { get; set; }
    }
}