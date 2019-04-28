using SiaConsulting.EO.Abstractions;

namespace Spartademo.DTOs.Commands
{
    public class AssignPaymentCommand : ICommand
    {
        public string ProfilId { get; set; }
        public string ZahlungsId { get; set; }
        public decimal Summe { get; set; }
    }
}