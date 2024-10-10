using System.Globalization;

namespace Ke.Bee.Localization.Options;

/// <summary>
/// Avalonia 本地化选项
/// </summary>
public class AvaloniaLocalizationOptions : LocalizationOptions
{
    public AvaloniaLocalizationOptions(IEnumerable<CultureInfo> cultures,
        CultureInfo defaultCulture, CultureInfo currentCulture, string assetsPath)
        : base(cultures, defaultCulture, currentCulture)
    {
        AssetsPath = assetsPath;
    }

    /// <summary>
    /// 资源文件路径
    /// </summary>
    public string AssetsPath { get; }
}
