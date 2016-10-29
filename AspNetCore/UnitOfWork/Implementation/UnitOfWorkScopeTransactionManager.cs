using System;
using System.Data;
using System.Collections.Generic;
using UnitOfWork.Abstractions;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace UnitOfWork.Implementation
{
    public class UnitOfWorkScopeTransactionManager : IUnitOfWorkScopeTransactionManager, IRepeatableReadSupport
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, IDbTransaction> _transactions;

        public UnitOfWorkScopeTransactionManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _transactions = new Dictionary<Type, IDbTransaction>();
        }

        public bool UseRepeatableRead { get; set; }

        public void RegisterAndBeginTransaction(Type scopeWorkerType)
        {
            if (_transactions.ContainsKey(scopeWorkerType))
                return;

            var constructors = scopeWorkerType.GetConstructors();

            if (constructors.Length != 1)
                throw new NotSupportedException("Указанный тип объекта не поддерживается. Поддерживаются объекты только с одним конструктором.");

            var parameters = constructors.Single().GetParameters()
                .Select(p => p.ParameterType)
                .Where(t => typeof(IDbConnection).IsAssignableFrom(t))
                .ToArray();

            if (parameters.Length != 1)
                throw new NotSupportedException("Указанный тип объекта не поддерживается. Поддерживаются объекты имеющие конструктор с одним параметром IDbConnection.");

            var connectionType = parameters.Single();
            var connectionInstance = (IDbConnection)_serviceProvider.GetRequiredService(connectionType);
            connectionInstance.Open();
            var transaction = connectionInstance.BeginTransaction(UseRepeatableRead ? IsolationLevel.RepeatableRead : IsolationLevel.ReadCommitted);
            _transactions.Add(connectionType, transaction);
        }

        public void DoCommit()
        {
            foreach (var transaction in _transactions.Values)
                transaction.Commit();
        }
    }
}
