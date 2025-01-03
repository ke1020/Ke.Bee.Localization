# 打包

```sh
dotnet pack -c Release
```

发布

```sh
dotnet nuget push .\bin\Release\Ke.Bee.Localization.0.1.4.nupkg --source https://api.nuget.org/v3/index.json --api-key {key}
```
