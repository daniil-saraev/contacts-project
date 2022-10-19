using Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Desktop.Services.Factories
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
