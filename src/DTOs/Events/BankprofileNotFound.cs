﻿using SiaConsulting.EO.Abstractions;

namespace Spartademo.DTOs.Events
{
    public class BankProfileNotFound: IEvent
    {
        public string ProfilId { get; set; }
        public string ZahlungsId { get; set; }
    }
}
