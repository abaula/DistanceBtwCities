using System;
using System.Data;
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

        public IUnitOfWorkTransactionScope CreateTransactionScope(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            var scope = _serviceProvider.GetRequiredService<IUnitOfWorkTransactionScope>();
            scope.SetIsolationLevel(isolationLevel);
            return scope;
        }

        public IUnitOfWorkScope CreateScope()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWorkScope>();
        }
    }
}
