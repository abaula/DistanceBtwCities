using System;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWork.Abstractions;

namespace UnitOfWork.Implementation
{
    public class UnitOfWorkScope : IUnitOfWorkScope
    {
        private readonly IServiceScope _serviceScope;
        private readonly IUnitOfWorkScopeConnectionManager _connectionManager;

        public UnitOfWorkScope(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.GetService<IServiceScopeFactory>();
            _serviceScope = scope.CreateScope();
            _connectionManager = _serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkScopeConnectionManager>();
        }

        public T Get<T>()
        {
            var worker = _serviceScope.ServiceProvider.GetRequiredService<T>();
            _connectionManager.CheckAndRegisterConnectionContext(worker.GetType());
            return worker;
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }

    }
}
