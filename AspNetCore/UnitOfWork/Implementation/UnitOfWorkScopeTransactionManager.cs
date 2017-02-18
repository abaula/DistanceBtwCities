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
    public class UnitOfWorkScopeTransactionManager : IUnitOfWorkScopeTransactionManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, IConnectionContext> _contexts;

        public UnitOfWorkScopeTransactionManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _contexts = new Dictionary<Type, IConnectionContext>();
        }

        public void CheckAndRegisterConnectionContext(Type scopeWorkerType, IsolationLevel isolationLevel)
        {
            if (_contexts.ContainsKey(scopeWorkerType))
                return;

            var constructors = scopeWorkerType.GetConstructors();

            if (constructors.Length != 1)
                throw new NotSupportedException("Указанный тип объекта не поддерживается. Поддерживаются объекты только с одним конструктором.");

            var parameters = constructors.Single().GetParameters()
                .Select(p => p.ParameterType)
                .Where(t => typeof(IConnectionContext).IsAssignableFrom(t))
                .ToArray();

            if (parameters.Length != 1)
                throw new NotSupportedException("Указанный тип объекта не поддерживается. Поддерживаются объекты имеющие конструктор с одним параметром IConnectionContext.");

            var contextType = parameters.Single();
            var contextInstance = (IConnectionContext)_serviceProvider.GetRequiredService(contextType);
            ((IConnectionContextSettings)contextInstance).SetTransactionIsolationLevel(isolationLevel);
            _contexts.Add(contextType, contextInstance);
        }

        public void DoCommit()
        {
            foreach (var context in _contexts.Values)
                context.CommitTransaction();
        }
    }
}
