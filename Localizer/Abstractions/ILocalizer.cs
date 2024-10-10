using System.Globalization;

namespace Ke.Bee.Localization.Localizer.Abstractions;

/// <summary>
/// 本地化接口
/// </summary>
public interface ILocalizer
{
    /// <summary>
    /// 获取本地化字符串
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string this[string key] { get; }
    /// <summary>
    /// 获取当前语言文化
    /// </summary>
    public CultureInfo CurrentCulture { get; set; }
    /// <summary>
    /// 获取可用语言文化
    /// </summary>
    public IEnumerable<CultureInfo> AvailableCultures { get; }
}
