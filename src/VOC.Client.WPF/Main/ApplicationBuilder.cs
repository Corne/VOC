﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using VOC.Client.Data.Games;
using VOC.Client.WPF.Dashboard;
using VOC.Client.WPF.Main.Navigation;

namespace VOC.Client.WPF.Main
{
    public class ApplicationBuilder
    {
        private readonly ContainerBuilder builder;

        public ApplicationBuilder()
        {
            builder = new ContainerBuilder();

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf();

            //navigation
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<ContentScope>();

            builder.RegisterType<MainViewModel>().SingleInstance().OnActivated(StartNavigation);

            //data
            builder.RegisterType<DummyGameStore>().As<IGameStore>();
        }

        private void StartNavigation(IActivatedEventArgs<MainViewModel> args)
        {
            var navigationService = args.Context.Resolve<INavigationService>();
            navigationService.Navigate<DashboardViewModel>();
        }

        public IContainer Build()
        {
            return builder.Build();
        }

    }
}