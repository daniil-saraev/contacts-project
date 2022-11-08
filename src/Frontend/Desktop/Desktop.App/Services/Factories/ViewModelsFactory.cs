using Microsoft.Extensions.DependencyInjection;
using System;

namespace Desktop.App.Services.Factories
{
    public class ViewModelsFactory : IViewModelsFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelsFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public BaseViewModel GetViewModel<T>() where T : BaseViewModel
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
