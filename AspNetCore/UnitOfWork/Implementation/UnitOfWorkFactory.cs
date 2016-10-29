using System;
using UnitOfWork.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace UnitOfWork.Implementation
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUnitOfWorkTransactionScope CreateTransactionScope(bool useRepeatableRead = false)
        {
            var scope = _serviceProvider.GetRequiredService<IUnitOfWorkTransactionScope>();
            var repeatableReadSupport = scope as IRepeatableReadSupport;

            if (repeatableReadSupport != null)
                repeatableReadSupport.UseRepeatableRead = useRepeatableRead;

            return scope;
        }

        public IUnitOfWorkScope CreateScope()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWorkScope>();
        }
    }
}
