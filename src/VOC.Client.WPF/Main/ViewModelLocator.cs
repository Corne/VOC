using System;
using Autofac;

namespace VOC.Client.WPF.Main
{
    public class ViewModelLocator : IDisposable
    {
        private readonly IContainer container;

        public ViewModelLocator()
        {
            var builder = new ApplicationBuilder();
            container = builder.Build();
        }
        
        public MainViewModel MainViewModel { get { return container.Resolve<MainViewModel>(); } }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}
