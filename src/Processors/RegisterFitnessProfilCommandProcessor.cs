using System.Collections.Generic;
using SiaConsulting.EO.Abstractions;
using Spartademo;
using Spartademo.DTOs.Commands;
using Spartademo.DTOs.Events;

namespace Spartademo
{
    public class RegisterFitnessProfilCommandProcessor : ICommandProcessor
    {
        public IList<IEvent> Process<TCommand, TContext>(TCommand command, TContext context)
            where TCommand : ICommand
            where TContext : IContext
            => ProcessCommand((RegisterFitnessProfilCommand)(ICommand)command, context);

        public IList<IEvent> ProcessCommand(RegisterFitnessProfilCommand command, IContext context)
        {
            return new List<IEvent>() { new FitnessProfilRegistered { ProfilId = command.ProfilId, ServiceEndpoint = command.ServiceEndpoint } };
        }
    }
}