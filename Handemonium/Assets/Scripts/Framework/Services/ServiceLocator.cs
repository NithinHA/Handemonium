using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPSLS.Framework
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, IService> _services = new Dictionary<Type, IService>();     // Map of all game service- Managers, Controllers, etc.

#region Register & Unregister

        public static void RegisterService<T>(Type serviceType, T instance) where T : IService
        {
            if (!_services.ContainsKey(serviceType))
            {
                _services[serviceType] = instance;
                instance.Start();
            }
        }

        public static void UnregisterAllServices()
        {
            foreach (KeyValuePair<Type, IService> item in _services)
            {
                // UnregisterService(item.Key); // commented out to remove the redundant TryGetValue check.
                item.Value.OnDestroy();
            }
            _services.Clear();
        }

        public static void UnregisterService(Type type)
        {
            if (_services.TryGetValue(type, out var service)) {
                service.OnDestroy();
                _services.Remove(type);
            }
        }

#endregion

        public static T GetService<T>()
        {
            return (T) GetService(typeof(T));
        }

        public static IService GetService(Type type)
        {
            bool found = _services.TryGetValue(type, out IService service);
            if (found)
                return service;
            else
            {
                Debug.LogError($"Could not find the service of type: {type}");
                return null;
            }
        }
    }
}