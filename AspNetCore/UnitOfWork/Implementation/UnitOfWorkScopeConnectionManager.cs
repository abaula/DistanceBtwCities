using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWork.Abstractions;
using System.Reflection;
using System.Linq;

namespace UnitOfWork.Implementation
{
    public class UnitOfWorkScopeConnectionManager : IUnitOfWorkScopeConnectionManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, IDbConnection> _connections;

        public UnitOfWorkScopeConnectionManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _connections = new Dictionary<Type, IDbConnection>();
        }

        public void RegisterConnection(Type scopeWorkerType)
        {
            if (_connections.ContainsKey(scopeWorkerType))
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
            _connections.Add(connectionType, connectionInstance);
        }
    }
}
