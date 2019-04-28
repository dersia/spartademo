using System.Collections.Generic;
using SiaConsulting.EO.Abstractions;
using Spartademo.DTOs.Commands;
using Spartademo.DTOs.Events;

namespace Spartademo.Processors
{
    public class RegisterBankProfilCommandProcessor : ICommandProcessor
    {
        public IList<IEvent> Process<TCommand, TContext>(TCommand command, TContext context)
            where TCommand : ICommand
            where TContext : IContext
            => ProcessCommand((RegisterBankProfilCommand)(ICommand)command, context);

        public IList<IEvent> ProcessCommand(RegisterBankProfilCommand command, IContext context)
        {
            return new List<IEvent> { new BankProfilRegistered { ProfilId = command.ProfilId, ServiceEndpoint = command.ServiceEndpoint } };
        }
    }
}