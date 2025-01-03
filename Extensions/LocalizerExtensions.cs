using Ke.Bee.Localization.Localizer.Abstractions;
using Ke.Bee.Localization.Options;
using Ke.Bee.Localization.Providers;
using Ke.Bee.Localization.Providers.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ke.Bee.Localization.Extensions;

public static class LocalizerExtensions
{
    /// <summary>
    /// 注册本地化服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsDelegate"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IServiceCollection AddLocalization<T>(this IServiceCollection services,
        Func<LocalizationOptions> optionsDelegate)
        where T : LocalizationProviderBase
    {
        // 本地化资源贡献者集合
        var localizationResourceContributors = services.BuildServiceProvider().GetService<IEnumerable<ILocalizationResourceContributor>>();

        // 调用委托方法获取配置选项
        var options = optionsDelegate?.Invoke();
        // 创建 T 类型的实例，并将配置选项传递给构造函数
        if (Activator.CreateInstance(typeof(T), options, localizationResourceContributors) is ILocalizationProvider provider)
        {
            // 初始化服务实例
            var localizer = Localizer.Localizer.Initialize(provider);
            // 注册 ILocalizer 接口实现
            services.AddSingleton(impl => localizer);
        }
        else
        {
            throw new Exception("不能创建本地化服务实例");
        }

        return services;
    }
}
