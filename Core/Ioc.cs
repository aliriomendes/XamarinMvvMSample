using System;
using Autofac;
using Autofac.Core;

namespace XamarinMvvMSample
{
    public static class Ioc
    {
        private static readonly Lazy<ContainerBuilder> _instance = new Lazy<ContainerBuilder>(() => new ContainerBuilder());
        private static Container _container;

        public static ContainerBuilder Instance
        {
            get { return _instance.Value; }
        }

        public static Container Container
        {
            get { return _container ?? (_container = Instance.Build() as Container); }
        }
    }
}
