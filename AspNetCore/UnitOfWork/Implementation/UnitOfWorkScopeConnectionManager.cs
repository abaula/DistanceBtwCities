using System;
using System.Collections.Generic;
using UnitOfWork.Abstractions;
using System.Reflection;
using System.Linq;
using DistanceBtwCities.Common.Connections.Abstractions;

namespace UnitOfWork.Implementation
{
    public class UnitOfWorkScopeConnectionManager : IUnitOfWorkScopeConnectionManager
    {
        private readonly HashSet<Type> _connections;

        public UnitOfWorkScopeConnectionManager()
        {
            _connections = new HashSet<Type>();
        }

        public void CheckAndRegisterConnectionContext(Type scopeWorkerType)
        {
            if (_connections.Contains(scopeWorkerType))
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

            var connectionType = parameters.Single();
            _connections.Add(connectionType);
        }
    }
}