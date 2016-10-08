using System;
using UnitOfWork.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace UnitOfWork.Implementation
{
    public class UnitOfWorkTransactionScope : IUnitOfWorkTransactionScope
    {
        private readonly IServiceScope _serviceScope;
        private readonly IUnitOfWorkScopeTransactionManager _transactionManager;

        public UnitOfWorkTransactionScope(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.GetService<IServiceScopeFactory>();
            _serviceScope = scope.CreateScope();
            _transactionManager = _serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkScopeTransactionManager>();
        }

        public T Get<T>()
        {
            // Worker-ы создаются из отдельного scope, что гарантирует наличие одного экземляра MyConnection:IDbConnection
            // во всех worker-ах созданных в scope.
            // Время жизни IDbConnection равно времени жизни scope.
            var worker = _serviceScope.ServiceProvider.GetRequiredService<T>();
            _transactionManager.RegisterAndBeginTransaction(worker.GetType());
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