
using System.Globalization;

namespace Ke.Bee.Localization.Options;

/// <summary>
/// 本地化配置选项
/// </summary>
public class LocalizationOptions
{
    public LocalizationOptions(IEnumerable<CultureInfo> cultures,
        CultureInfo defaultCulture, CultureInfo currentCulture)
    {
        Cultures = cultures;
        CurrentCulture = currentCulture;
        DefaultCulture = defaultCulture;
    }

    /// <summary>
    /// 支持的语言文化集合
    /// </summary>
    public IEnumerable<CultureInfo> Cultures { get; }
    /// <summary>
    /// 当前语言文化
    /// </summary>
    public CultureInfo CurrentCulture { get; }
    /// <summary>
    /// 默认语言文化
    /// </summary>
    public CultureInfo DefaultCulture { get; }
}
