using System.Globalization;
using Ke.Bee.Localization.Options;

namespace Ke.Bee.Localization.Providers.Abstractions;

/// <summary>
/// 本地化服务提供者抽象基类
/// </summary>
public abstract class LocalizationProviderBase : ILocalizationProvider
{
    private CultureInfo? _currentCulture;

    protected LocalizationProviderBase(LocalizationOptions options)
    {
        Options = options;
        ValidateOptions(Options);
        CurrentCulture = Options.CurrentCulture;
    }

    protected LocalizationOptions Options { get; }

    /// <summary>
    /// 获取或设置当前的文化信息
    /// </summary>
    public CultureInfo CurrentCulture
    {
        get => _currentCulture!;
        set => ChangeCulture(value);
    }

    /// <summary>
    /// 获取资源
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public abstract string GetResourceByKey(string key);
    /// <summary>
    /// 获取可用的文化
    /// </summary>
    public IEnumerable<CultureInfo> AvailableCultures => Options.Cultures;

    protected abstract Stream? GetResourceStream(CultureInfo culture);

    /// <summary>
    /// 验证选项
    /// </summary>
    /// <param name="options"></param>
    /// <exception cref="ArgumentException"></exception>
    protected virtual void ValidateOptions(LocalizationOptions options)
    {
        if (!options.Cultures.Any()) throw new ArgumentException("文化列表为空");

        if (options.Cultures.All(x => x.IetfLanguageTag != options.DefaultCulture.IetfLanguageTag))
            throw new ArgumentException(
                $"默认文化 {options.DefaultCulture.IetfLanguageTag} 不在区域性列表中");
    }

    /// <summary>
    /// 改变当前文化
    /// </summary>
    /// <param name="culture"></param>
    protected virtual void ChangeCulture(CultureInfo culture)
    {
        _currentCulture = culture;
    }
}
