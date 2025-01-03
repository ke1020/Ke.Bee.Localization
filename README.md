# Avalonia 基于 `JSON` 的本地化库

一个带有自身抽象层的本地化库，提供了创建本地化基础设施的可能性。它包含了基本的 `JSON` 本地化提供程序实现。项目是 [TekDeq.Localization](https://github.com/semack/TekDeq.Localization) 的一个衍生版本，主要进行了以下修改：

- 升级了依赖库；
- 添加了中文注释；
- 调整了代码结构。

## 使用

要创建额外的本地化提供程序，请参考 `ILocalizationProvider` 和 `LocalizationProviderBase` 抽象层。作为用法示例，请查看 `JsonLocalizationProvider` 和 `AvaloniaJsonLocalizationProvider` 的实现。

以下是 `Avalonia` 项目的使用示例：

```cs
public override void OnFrameworkInitializationCompleted()
{
    BindingPlugins.DataValidators.RemoveAt(0);

    // 注册应用程序运行所需的所有服务
    var collection = new ServiceCollection();
    // 注入主窗口视图模型
    collection.AddTransient<MainWindowViewModel>();
    // 注册本地化
    collection.AddLocalization<AvaloniaJsonLocalizationProvider>(() =>
    {
        var options = new AvaloniaLocalizationOptions(
            // 支持的本地化语言文化
            new List<CultureInfo>
            {
                new("en-US"),
                new("zh-CN")
            },
            // defaultCulture, 用于设置当前文化（currentCulture）不在 cultures 列表中时的情况以及作为缺失的本地化条目的备用文化（fallback culture）
            new CultureInfo("en-US"),
            // currentCulture 在基础设施加载时设置，可以从应用程序设置或其他地方获取
            Thread.CurrentThread.CurrentCulture,
            // 包含本地化 JSON 文件的资源路径
            $"{typeof(App).Namespace}/Assets/i18n");
        return options;
    });

    // 从 collection 提供的 IServiceCollection 中创建包含服务的 ServiceProvider
    var services = collection.BuildServiceProvider();
    var vm = services.GetRequiredService<MainWindowViewModel>();

    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        desktop.MainWindow = new MainWindow
        {
            DataContext = vm,
        };
    }

    base.OnFrameworkInitializationCompleted();
}
```

要在 `Avalonia` 中使用 `Localize` 标记扩展进行标记本地化，首先需要在 `XAML` 标记文件中添加相应的命名空间声明。这一步骤是为了让标记文件能够识别并使用 `Localize` 扩展的功能。

```xml
<Window xmlns="https://github.com/avaloniaui"
...
    xmlns:i18n="clr-namespace:Ke.Bee.Localization.Extensions;assembly=Ke.Bee.Localization"
...
>
```

完成命名空间的添加后，即可利用 `Localize` 标记扩展来实现 `UI` 元素的本地化。

```xml
   <StackPanel>
    ...
        <TextBlock Text="{i18n:Localize Greeting, {Binding Value}}" />
    ...
    </StackPanel>
```

除了在 `XAML` 标记中使用 `Localize` 扩展外，还可以在后台代码中实现本地化功能。具体做法如下：

```cs
/// <summary>
/// 主窗口视图模型
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ILocalizer _l;

    /// <summary>
    /// 通过构造函数注入本地化器
    /// </summary>
    public MainWindowViewModel(ILocalizer localizer)
    {
        _l = localizer;

        // 获取本地化字符串
        var localString = _l["Greeting"];
    }
}
```

## 许可

本项目根据 [MIT](https://github.com/semack/TekDeq.Localization/blob/master/LICENSE.md) 许可证条款进行授权。
