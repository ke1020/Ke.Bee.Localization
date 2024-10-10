using System.Globalization;

namespace Ke.Bee.Localization.Providers.Abstractions;

/// <summary>
/// 本地化提供程序接口
/// </summary>
public interface ILocalizationProvider
{
    /// <summary>
    /// 获取所有可用的文化集合
    /// </summary>
    public IEnumerable<CultureInfo> AvailableCultures { get; }
    /// <summary>
    /// 获取当前文化
    /// </summary>
    public CultureInfo CurrentCulture { get; set; }
    /// <summary>
    /// 获取资源
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetResourceByKey(string key);
}
