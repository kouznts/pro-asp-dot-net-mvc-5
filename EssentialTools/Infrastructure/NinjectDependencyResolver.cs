﻿using EssentialTools.Models;
using Ninject;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EssentialTools.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IValueCalculator>().To<LinqValueCalculator>().InRequestScope();
            
            kernel.Bind<IDiscountHelper>().To<DefaultDiscounterHelper>()
                .WithConstructorArgument("discountSize", 50M);
            kernel.Bind<IDiscountHelper>().To<FlexibleDiscountHelper>().
                WhenInjectedInto<LinqValueCalculator>();
        }
    }
}