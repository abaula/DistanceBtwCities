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

        public IUnitOfWorkTransactionScope CreateTransactionScope()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWorkTransactionScope>();
        }

        public IUnitOfWorkScope CreateScope()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWorkScope>();
        }
    }
}
