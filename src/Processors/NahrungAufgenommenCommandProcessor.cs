using SiaConsulting.EO.Abstractions;
using System;
using System.Collections.Generic;

namespace Spartademo.Processors
{
    public class NahrungAufgenommenCommandProcessor : ICommandProcessor
    {
        public IList<IEvent> Process<TCommand, TContext>(TCommand command, TContext context)
            where TCommand : ICommand
            where TContext : IContext
        {
            throw new NotImplementedException();
        }
    }
}
