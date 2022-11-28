using Microsoft.Extensions.DependencyInjection;
using Desktop.Common.ViewModels;
using System;
using Desktop.Common.Services;

namespace Desktop.Main.Common.Services
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
