using System;
using System.Data;
using UnitOfWork.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace UnitOfWork.Implementation
{
    public class UnitOfWorkTransactionScope : IUnitOfWorkTransactionScope
    {
        private readonly IServiceScope _serviceScope;
        private readonly IUnitOfWorkScopeTransactionManager _transactionManager;
        private IsolationLevel _isolationLevel;

        public UnitOfWorkTransactionScope(IServiceProvider serviceProvider)
        {
            _isolationLevel = IsolationLevel.Unspecified;
            var scope = serviceProvider.GetService<IServiceScopeFactory>();
            _serviceScope = scope.CreateScope();
            _transactionManager = _serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkScopeTransactionManager>();
        }

        public void SetIsolationLevel(IsolationLevel isolationlevel)
        {
            _isolationLevel = isolationlevel;
        }

        public T Get<T>()
        {
            var worker = _serviceScope.ServiceProvider.GetRequiredService<T>();
            _transactionManager.CheckAndRegisterConnectionContext(worker.GetType(), _isolationLevel);

            return worker;
        }

        public void Commit()
        {
            _transactionManager.DoCommit();
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}