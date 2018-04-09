using System;
using DryIoc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ZebraBellaComponentsUtility.Utility.Extensions
{
    public static class OptionsContainerExtensions
    {
        public static void Configure<TOptions>(this IContainer container, IConfiguration configuration) where TOptions : class, new()
            => container.Configure<TOptions>(Options.DefaultName, configuration);



        public static void Configure<TOptions>(this IContainer container, string name, IConfiguration configuration)
            where TOptions : class, new()
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            container.RegisterOptions();


            container.UseInstance<IOptionsChangeTokenSource<TOptions>>(new ConfigurationChangeTokenSource<TOptions>(name, configuration));

            container.UseInstance<IConfigureOptions<TOptions>>(new NamedConfigureFromConfigurationOptions<TOptions>(name, configuration));


            var options = container.Resolve<IOptions<TOptions>>();

            container.UseInstance(options.Value);
        }



        private static void RegisterOptions(this IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Register(typeof(IOptions<>), typeof(OptionsManager<>), Reuse.Singleton, ifAlreadyRegistered: IfAlreadyRegistered.Keep);
            container.Register(typeof(IOptionsSnapshot<>), typeof(OptionsManager<>), Reuse.InWebRequest, ifAlreadyRegistered: IfAlreadyRegistered.Keep);
            container.Register(typeof(IOptionsMonitor<>), typeof(OptionsMonitor<>), Reuse.Singleton, ifAlreadyRegistered: IfAlreadyRegistered.Keep);
            container.Register(typeof(IOptionsFactory<>), typeof(OptionsFactory<>), Reuse.Transient, ifAlreadyRegistered: IfAlreadyRegistered.Keep);
            container.Register(typeof(IOptionsMonitorCache<>), typeof(OptionsCache<>), Reuse.Singleton, ifAlreadyRegistered: IfAlreadyRegistered.Keep);
        }
    }
}
