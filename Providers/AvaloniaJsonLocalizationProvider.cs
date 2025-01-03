using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Avalonia.Platform;
using Ke.Bee.Localization.Options;
using Ke.Bee.Localization.Providers.Abstractions;

namespace Ke.Bee.Localization.Providers;

/// <summary>
/// Avalonia 本地化资源提供程序
/// </summary>
public class AvaloniaJsonLocalizationProvider : JsonLocalizationProvider
{
    public AvaloniaJsonLocalizationProvider(
        LocalizationOptions options, 
        IEnumerable<ILocalizationResourceContributor> LocalizationResourceContributors
        ) : base(options, LocalizationResourceContributors)
    {
        if (Options is not AvaloniaLocalizationOptions)
            throw new ArgumentException("选项与请求的提供程序不兼容");
    }

    /// <summary>
    /// 获取选项
    /// </summary>
    private new AvaloniaLocalizationOptions Options => (AvaloniaLocalizationOptions)base.Options;

    /// <summary>
    /// 获取资源文件路径
    /// </summary>
    /// <param name="culture"></param>
    /// <returns></returns>
    private Uri GetAssetUri(CultureInfo culture)
    {
        return new Uri($"avares://{Options.AssetsPath}/{culture.IetfLanguageTag}.json");
    }

    /// <summary>
    /// 验证选项
    /// </summary>
    /// <param name="options"></param>
    /// <exception cref="ValidationException"></exception>
    protected override void ValidateOptions(LocalizationOptions options)
    {
        base.ValidateOptions(options);

        options.Cultures.ToList().ForEach(culture =>
        {
            var uri = GetAssetUri(culture);
            if (!AssetLoader.Exists(uri))
                throw new ValidationException($"不能找到资源文件\n{uri}");
        });
    }

    /// <summary>
    /// 获取资源流
    /// </summary>
    /// <param name="culture"></param>
    /// <returns></returns>
    protected override Stream? GetResourceStream(CultureInfo culture)
    {
        var uri = GetAssetUri(culture);

        if (AssetLoader.Exists(uri))
            return AssetLoader.Open(uri);

        return null;
    }
}
