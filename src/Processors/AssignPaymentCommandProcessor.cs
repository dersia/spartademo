using System;
using System.Collections.Generic;
using System.Linq;
using SiaConsulting.EO.Abstractions;
using Spartademo.Contexts;
using Spartademo.DTOs.Commands;
using Spartademo.DTOs.Events;

namespace Spartademo.Processors
{
    public class AssignPaymentCommandProcessor : ICommandProcessor
    {
        public IList<IEvent> Process<TCommand, TContext>(TCommand command, TContext context)
            where TCommand : ICommand
            where TContext : IContext 
            => ProcessCommand((AssignPaymentCommand)(ICommand) command, (AssignPaymentContext)(IContext)context);

        public IList<IEvent> ProcessCommand(AssignPaymentCommand command, AssignPaymentContext context)
        {
            var profilGefunden = context.Stream.Any(s => (s as BankProfilRegistered)?.ProfilId == command.ProfilId);

            if (!profilGefunden) return new List<IEvent> {new BankProfileNotFound {ProfilId = command.ProfilId, ZahlungsId = command.ZahlungsId } };

            if (command.ZahlungsId == "-99")
            {
                return new List<IEvent> { new PaymentNotExecuted {ProfilId = command.ProfilId, ZahlungsId = command.ZahlungsId } };
            }
            return new List<IEvent> { new PaymentExecuted {ProfilId = command.ProfilId, ZahlungsId = command.ZahlungsId } };
        }
    }
}