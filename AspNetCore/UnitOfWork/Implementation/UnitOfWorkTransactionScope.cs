using System;
using UnitOfWork.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace UnitOfWork.Implementation
{
    public class UnitOfWorkTransactionScope : IUnitOfWorkTransactionScope, IRepeatableReadSupport
    {
        private readonly IServiceScope _serviceScope;
        private readonly IUnitOfWorkScopeTransactionManager _transactionManager;
        private bool _registerRepeatableReadSupport;

        public UnitOfWorkTransactionScope(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.GetService<IServiceScopeFactory>();
            _serviceScope = scope.CreateScope();
            _transactionManager = _serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkScopeTransactionManager>();
            _registerRepeatableReadSupport = true;
        }

        public bool UseRepeatableRead { get; set; }

        public T Get<T>()
        {
            // Worker-ы создаются из отдельного scope, что гарантирует наличие одного экземляра MyConnection:IDbConnection
            // во всех worker-ах созданных в scope.
            // Время жизни IDbConnection равно времени жизни scope.
            var worker = _serviceScope.ServiceProvider.GetRequiredService<T>();
            RegiterRepeatableReadSupport();
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

        private void RegiterRepeatableReadSupport()
        {
            if (!_registerRepeatableReadSupport)
                return;

            var repeatableReadSupport = _transactionManager as IRepeatableReadSupport;

            if (repeatableReadSupport != null)
                repeatableReadSupport.UseRepeatableRead = UseRepeatableRead;

            _registerRepeatableReadSupport = false;
        }
    }
}