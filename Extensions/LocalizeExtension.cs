using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Data.Core;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;

namespace Ke.Bee.Localization.Extensions;

/// <summary>
/// Avalonia 本地化扩展
/// </summary>
public class LocalizeExtension : MarkupExtension
{
    /*
    public LocalizeExtension(string key)
    {
        Key = key;
    }

    public string Key { get; }

    public string? Context { get; }

    /// <summary>
    /// 为 xaml 文件提供值
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var keyToUse = Key;
        if (!string.IsNullOrWhiteSpace(Context))
            keyToUse = $"{Context}/{Key}";

        var binding = new ReflectionBindingExtension($"[{keyToUse}]")
        {
            Mode = BindingMode.OneWay,
            Source = Localizer.Localizer.Instance
        };

        return binding.ProvideValue(serviceProvider);
    }
    */

    private readonly BindingBase[]? _bindings;

    /// <summary>
    /// Gets or sets the key of the localized string.
    /// </summary>
    public object Key { get; set; }

    /// <summary>
    /// Gets or sets the context of the localized string.
    /// </summary>
    public string? Context { get; set; }

    /// <summary>
    /// Gets or sets the default value to return if the localized string is not found.
    /// </summary>
    public string? Default { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizeExtension" /> class with the specified key.
    /// </summary>
    /// <param name="key">The key of the localized string.</param>
    public LocalizeExtension(object key)
    {
        Key = key;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizeExtension" /> class with the specified key and a binding.
    /// </summary>
    /// <param name="key">The key of the localized string.</param>
    /// <param name="binding">The binding to use for string formatting.</param>
    public LocalizeExtension(object key, BindingBase binding) : this(key)
    {
        _bindings = [binding];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizeExtension" /> class with the specified key and two bindings.
    /// </summary>
    /// <param name="key">The key of the localized string.</param>
    /// <param name="binding1">The first binding to use for string formatting.</param>
    /// <param name="binding2">The second binding to use for string formatting.</param>
    public LocalizeExtension(object key, BindingBase binding1, BindingBase binding2) : this(key)
    {
        _bindings = [binding1, binding2];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizeExtension" /> class with the specified key and three bindings.
    /// </summary>
    /// <param name="key">The key of the localized string.</param>
    /// <param name="binding1">The first binding to use for string formatting.</param>
    /// <param name="binding2">The second binding to use for string formatting.</param>
    /// <param name="binding3">The third binding to use for string formatting.</param>
    public LocalizeExtension(object key, BindingBase binding1, BindingBase binding2, BindingBase binding3) : this(key)
    {
        _bindings = [binding1, binding2, binding3];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizeExtension" /> class with the specified key and four bindings.
    /// </summary>
    /// <param name="key">The key of the localized string.</param>
    /// <param name="binding1">The first binding to use for string formatting.</param>
    /// <param name="binding2">The second binding to use for string formatting.</param>
    /// <param name="binding3">The third binding to use for string formatting.</param>
    /// <param name="binding4">The fourth binding to use for string formatting.</param>
    public LocalizeExtension(object key, BindingBase binding1, BindingBase binding2, BindingBase binding3,
        BindingBase binding4) : this(key)
    {
        _bindings = [binding1, binding2, binding3, binding4];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizeExtension" /> class with the specified key and five bindings.
    /// </summary>
    /// <param name="key">The key of the localized string.</param>
    /// <param name="binding1">The first binding to use for string formatting.</param>
    /// <param name="binding2">The second binding to use for string formatting.</param>
    /// <param name="binding3">The third binding to use for string formatting.</param>
    /// <param name="binding4">The fourth binding to use for string formatting.</param>
    /// <param name="binding5">The fifth binding to use for string formatting.</param>
    public LocalizeExtension(object key, BindingBase binding1, BindingBase binding2, BindingBase binding3,
        BindingBase binding4, BindingBase binding5) : this(key)
    {
        _bindings = [binding1, binding2, binding3, binding4, binding5];
    }

    /// <summary>
    /// Provides the localized string.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>The localized string.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Key is string key)
        {
            if (!string.IsNullOrWhiteSpace(Context))
                key = $"{Context}/{Key}";

            ClrPropertyInfo keyInfo = new(
                nameof(Key),
                _ => Localizer.Localizer.Instance?[key],
                null,
                typeof(string));

            CompiledBindingPath path = new CompiledBindingPathBuilder()
                .Property(keyInfo, PropertyInfoAccessorFactory.CreateInpcPropertyAccessor)
                .Build();

            CompiledBindingExtension binding = new(path)
            {
                Mode = BindingMode.OneWay,
                Source = Localizer.Localizer.Instance
            };

            if (_bindings is null || _bindings.Length <= 0)
                return binding;

            BindingBase[] bindingBases = GetBindings(binding);

            MultiBinding multiBinding = new()
            {
                // ReSharper disable once CoVariantArrayConversion
                Bindings = bindingBases,
                Converter = new TranslateConverter()
            };

            return multiBinding;
        }
        else if (Key is BindingBase binding)
        {
            BindingBase[] bindingBases = GetBindings(binding);

            MultiBinding multiBinding = new()
            {
                // ReSharper disable once CoVariantArrayConversion
                Bindings = bindingBases,
                Converter = new BindingTranslateConverter(Context)
            };

            return multiBinding;
        }

        throw new NotSupportedException("Key must be a string or BindingBase");
    }

    private BindingBase[] GetBindings(BindingBase binding)
    {
        if (_bindings is null || _bindings.Length <= 0)
            return [binding];

        BindingBase[] bindingBases = new BindingBase[_bindings.Length + 1];

        bindingBases[0] = binding;

        for (int i = 0; i < _bindings.Length; i++)
            bindingBases[i + 1] = _bindings[i];

        return bindingBases;
    }
}

/// <summary>
/// A converter that translates a binding key and additional arguments into a localized string.
/// </summary>
public class BindingTranslateConverter : IMultiValueConverter
{
    private readonly string? _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="BindingTranslateConverter"/> class.
    /// </summary>
    /// <param name="context">The context for the translation, which will be used to construct the localization key.</param>
    public BindingTranslateConverter(string? context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        List<object> list = new(values!);

        string key = (list[0] as string)!;

        if (!string.IsNullOrWhiteSpace(_context))
            key = $"{_context}/{key}";

        list.RemoveAt(0);

        try
        {
            return Localizer.Localizer.Tr(key, args: list.ToArray());
        }
        catch
        {
            return Localizer.Localizer.Tr(key);
        }
    }
}

/// <summary>
/// A multi-value converter that translates a string using string.Format.
/// The first value in the values list is the translation text, and the remaining values are the arguments to be used
/// in the format string.
/// </summary>
public class TranslateConverter : IMultiValueConverter
{
    /// <summary>
    /// Converts a list of values to a translated string.
    /// </summary>
    /// <param name="values">
    /// A list of values. The first value is the translation text, and the remaining values are the
    /// arguments to be used in the format string.
    /// </param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="culture">The culture to use in the conversion.</param>
    /// <returns>The translated string, or the original translation text if an error occurs during formatting.</returns>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        List<object> list = new(values!);

        string translationText = (list[0] as string)!;

        list.RemoveAt(0);

        try
        {
            return string.Format(translationText, list.ToArray());
        }
        catch
        {
            return translationText;
        }
    }
}
