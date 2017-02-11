using System;
using System.Data;
using System.Collections.Generic;
using UnitOfWork.Abstractions;
using System.Reflection;
using System.Linq;
using DistanceBtwCities.Common.Connections.Abstractions;
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
                .Where(t => typeof(IAppCommonConnection).IsAssignableFrom(t))
                .ToArray();

            if (parameters.Length != 1)
                throw new NotSupportedException("Указанный тип объекта не поддерживается. Поддерживаются объекты имеющие конструктор с одним параметром IAppCommonConnection.");

            var connectionType = parameters.Single();
            var connectionInstance = (IAppCommonConnection)_serviceProvider.GetRequiredService(connectionType);
            connectionInstance.Connection.Open();
            var transaction = connectionInstance.Connection.BeginTransaction(UseRepeatableRead ? IsolationLevel.RepeatableRead : IsolationLevel.ReadCommitted);
            _transactions.Add(connectionType, transaction);
        }

        public void DoCommit()
        {
            foreach (var transaction in _transactions.Values)
                transaction.Commit();
        }
    }
}
