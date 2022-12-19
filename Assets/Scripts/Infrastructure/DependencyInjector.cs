using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public class DependencyInjector
    {
        private Dictionary<Type, object> _dependencies;

        public DependencyInjector() => 
            _dependencies = new Dictionary<Type, object>();
        
        public void Register<T>(T dependency) => 
            _dependencies[typeof(T)] = dependency;
        
        public T Resolve<T>() => (T) 
            _dependencies[typeof(T)];
    }
}
