using System.ComponentModel;
using System.Globalization;
using Ke.Bee.Localization.Localizer.Abstractions;
using Ke.Bee.Localization.Providers.Abstractions;

namespace Ke.Bee.Localization.Localizer;

/// <summary>
/// 本地化实现
/// </summary>
public sealed class Localizer : ILocalizer, INotifyPropertyChanged
{
    /// <summary>
    /// 本地化字典索引器名称
    /// </summary>
    private const string IndexerName = "Item";
    private const string IndexerArrayName = "Item[]";

    private Localizer(ILocalizationProvider provider)
    {
        Provider = provider;
    }

    /// <summary>
    /// 本地化接口实例
    /// </summary>
    public static ILocalizer? Instance { get; private set; }
    /// <summary>
    /// 本地化提供程序实例
    /// </summary>
    private ILocalizationProvider Provider { get; }
    /// <summary>
    /// 可用的本地化语言
    /// </summary>
    public IEnumerable<CultureInfo> AvailableCultures => Provider.AvailableCultures;
    /// <summary>
    /// 获取本地化资源索引器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string this[string key] => Provider.GetResourceByKey(key);
    /// <summary>
    /// 获取或设置本地化资源索引器
    /// </summary>
    public CultureInfo CurrentCulture
    {
        get => Provider.CurrentCulture;
        set
        {
            Provider.CurrentCulture = value;
            Invalidate();
        }
    }

    /// <summary>
    /// 属性变更事件委托函数
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// 触发属性变更事件
    /// </summary>
    private void Invalidate()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentCulture)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerName));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerArrayName));
    }

    /// <summary>
    /// 初始化本地化服务实例
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static ILocalizer Initialize(ILocalizationProvider provider)
    {
        Instance = new Localizer(provider);
        return Instance;
    }
}
