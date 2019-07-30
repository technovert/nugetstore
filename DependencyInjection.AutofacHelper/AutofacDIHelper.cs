using Autofac;
using Autofac.Builder;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DependencyInjection.AutofacHelper
{
    public static class AutofacDIHelper
    {
        private static ContainerBuilder Builder;

        public static void Init(System.Reflection.Assembly assembly)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(assembly);
            Builder = builder;
        }

        public static void Build()
        {
            var container = Builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static void Register<T>(Lifetime? lifetime = null) =>
            AddLifetime(Builder.RegisterType<T>().AsSelf(), lifetime);

        public static void RegisterAs<T1, T2>(Lifetime? lifetime = null) where T1 : Type where T2 : Type => AddLifetime(Builder.RegisterType<T1>().As<T2>(), lifetime);

        public static void RegisterNamed<T1, T2>(T1 service, T2 Names, string name, Lifetime? lifetime = null) where T1 : Type where T2 : Type => AddLifetime(Builder.RegisterType<T1>().Named<T2>(name), lifetime);

        public static void RegisterKeyed<T1, T2>(object key, Lifetime? lifetime = null) where T1 : Type where T2 : Type => AddLifetime(Builder.RegisterType<T1>().Keyed<T2>(key), lifetime);

        private static void AddLifetime<Type>(IRegistrationBuilder<Type, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration, Lifetime? lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.Scoped:
                    registration.InstancePerLifetimeScope();
                    break;
                case Lifetime.Singleton:
                    registration.SingleInstance();
                    break;
                default:
                    registration.InstancePerDependency();
                    break;
            }
        }
    }
}
