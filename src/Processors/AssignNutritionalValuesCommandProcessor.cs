using System.Collections.Generic;
using System.Linq;
using SiaConsulting.EO.Abstractions;
using Spartademo.Contexts;
using Spartademo.DTOs.Commands;
using Spartademo.DTOs.Events;

namespace Spartademo.Processors
{
    public class AssignNutritionalValuesCommandProcessor : ICommandProcessor
    {
        public IList<IEvent> Process<TCommand, TContext>(TCommand command, TContext context)
            where TCommand : ICommand
            where TContext : IContext 
            => ProcessCommand((AssignNutritionalValuesCommand)(ICommand) command, (AssignNutritionalValuesContext)(IContext)context);

        public IList<IEvent> ProcessCommand(AssignNutritionalValuesCommand command, AssignNutritionalValuesContext context)
        {
            var profilGefunden = context.Stream.Any(s => (s as FitnessProfilRegistered)?.ProfilId == command.ProfilId);

            if (!profilGefunden) return new List<IEvent> {new FitnessprofileNotFound()};

            return new List<IEvent> { new NutritionalValuesAdded {ProfilId = command.ProfilId, EnergyInKCal = command.EnergyInKCal } };
        }
    }
}