using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWork.Abstractions;
using System.Reflection;
using System.Linq;
using DistanceBtwCities.Common.Connections.Abstractions;

namespace UnitOfWork.Implementation
{
    public class UnitOfWorkScopeConnectionManager : IUnitOfWorkScopeConnectionManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HashSet<Type> _connections;

        public UnitOfWorkScopeConnectionManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _connections = new HashSet<Type>();
        }

        public void RegisterConnection(Type scopeWorkerType)
        {
            if (_connections.Contains(scopeWorkerType))
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
            _connections.Add(connectionType);
        }
    }
}
