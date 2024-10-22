
using System.Globalization;

namespace Ke.Bee.Localization.Providers.Abstractions;

/// <summary>
/// 本地化资源贡献
/// </summary>
public interface ILocalizaitonResourceContributor
{
    /// <summary>
    /// 获取本地化资源
    /// </summary>
    /// <param name="culture">文化</param>
    /// <param name="localizationResource"></param>
    Dictionary<string, string>? GetResource(CultureInfo culture);
}
