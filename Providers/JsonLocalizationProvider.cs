
using System.Globalization;
using System.Text;
using System.Text.Json;
using Ke.Bee.Localization.Options;
using Ke.Bee.Localization.Providers.Abstractions;

namespace Ke.Bee.Localization.Providers;

/// <summary>
/// Json 本地化提供程序
/// </summary>
public abstract class JsonLocalizationProvider : LocalizationProviderBase
{
    private IDictionary<string, string> _activeDictionary = new Dictionary<string, string>();
    private IDictionary<string, string> _fallbackDictionary = new Dictionary<string, string>();

    protected JsonLocalizationProvider(
        LocalizationOptions options,
        IEnumerable<ILocalizationResourceContributor> LocalizationResourceContributors
        ) : base(options, LocalizationResourceContributors)
    {
    }

    /// <summary>
    /// 获取资源字典
    /// </summary>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    private Dictionary<string, string> GetDictionary(CultureInfo culture)
    {
        // 从嵌入文件获取本地化资源
        using var stream = GetResourceStream(culture);

        if (stream != null)
        {
            using var streamReader = new StreamReader(stream, Encoding.UTF8);
            var json = streamReader.ReadToEnd();
            var dic = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];

            // 从贡献者获取本地化资源
            foreach (var contributor in Contributors)
            {
                dic = dic.Concat(contributor.GetResource(culture) ?? []).ToDictionary(x => x.Key, x => x.Value);
            }

            return dic;
        }

        throw new InvalidDataException($"不能载入语言文化 {culture.IetfLanguageTag}");
    }

    /// <summary>
    /// 修改文化
    /// </summary>
    /// <param name="culture">要载入的文化</param>
    protected override void ChangeCulture(CultureInfo culture)
    {
        // 检查文化后再进行更改
        if (AvailableCultures.All(c => c.IetfLanguageTag != culture.IetfLanguageTag))
            culture = Options.DefaultCulture;

        // 检查是否已加载回退机制
        if (!_fallbackDictionary.Any())
            _fallbackDictionary = GetDictionary(Options.DefaultCulture);

        // 载入选中
        _activeDictionary = GetDictionary(culture);

        // 调用基类方法
        base.ChangeCulture(culture);
    }

    /// <summary>
    /// 获取资源
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public override string GetResourceByKey(string key)
    {
        // 从活动字典中提取数据
        if (!_activeDictionary.TryGetValue(key, out var resource))
        {
            _ = _fallbackDictionary.TryGetValue(key, out resource);
        }

        return !string.IsNullOrEmpty(resource)
            ? resource.Replace("\\n", "\n")
            : $"{CurrentCulture.IetfLanguageTag}:{key}"; // 如果回退机制失败，返回的数据
    }
}
