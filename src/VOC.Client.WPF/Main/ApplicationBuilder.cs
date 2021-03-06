﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using VOC.Client.Dashboard.Configuration;
using VOC.Client.Dashboard.Games;
using VOC.Client.Users;
using VOC.Client.WPF.Dashboard.Overview;
using VOC.Client.WPF.Main.Navigation;

namespace VOC.Client.WPF.Main
{
    public class ApplicationBuilder
    {
        private readonly ContainerBuilder builder;

        public ApplicationBuilder()
        {
            builder = new ContainerBuilder();

            //user
            builder.RegisterInstance(new User(Environment.UserName)).As<IUser>();

            //all viewmodels
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf();

            //navigation
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<ContentScope>();

            builder.RegisterType<MainViewModel>().SingleInstance().OnActivated(StartNavigation);

            //data
            builder.RegisterType<GameStore>().As<IGameStore>();
            builder.RegisterType<DummyMapConfigurator>().As<IMapConfigurator>();
            builder.RegisterType<DummyGameConfigurator>().As<IGameConfigurator>();
        }

        private void StartNavigation(IActivatedEventArgs<MainViewModel> args)
        {
            var navigationService = args.Context.Resolve<INavigationService>();
            navigationService.Navigate<Game.GameViewModel>();
        }

        public IContainer Build()
        {
            return builder.Build();
        }

    }
}
