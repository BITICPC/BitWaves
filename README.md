# BitWaves
BitWaves 是 BITOJ 的后端 Web 应用，采用 ASP.Net Core 框架进行编写。数据库采用 MongoDB。

## 功能

> 待更新

## 安装
请确保您已经安装了 .NET Core 运行时。最低版本为 .NET Core 2.1。如果尚未安装，请[单击此处](https://dotnet.microsoft.com/download)以安装流程。

### Windows

> 待更新

### Linux 与 macOS
执行如下命令 Clone 代码仓库：

```bash
git clone https://github.com/BITICPC/BitWaves.git
```

然后执行下列命令恢复项目所需的依赖：

```bash
cd BitWaves
dotnet restore
```

然后执行下列命令启动编译：

```bash
cd BitWaves.WebAPI
dotnet publish -c Release -o out
```

## 运行
在编译的输出目录 `path/to/BitWaves/BitWaves.WebAPI/out` 下新建文件 `dbsettings.json`，填入如下内容：

```json
{
  "ConnectionStrings": {
    "mongodb": "Your MongoDB connection string"
  }
}
```

注意填入到您的 MongoDB 实例的连接字符串。

然后再新建文件 `nlog.config`，填入如下内容（供参考）以配置服务端日志：

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogLevel="Info"
      internalLogFile="/var/log/bitwaves/internal.log">

    <!-- enable asp.net core layout renderers -->
    <extensions>
        <add assembly="NLog.Web.AspNetCore"/>
    </extensions>

    <!-- the targets to write to -->
    <targets>
        <!-- write logs to file  -->
        <target xsi:type="File" name="allfile" fileName="/var/log/bitwaves/bitwaves-all-${shortdate}.log"
                layout="${longdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" />

        <!-- another file log, only own logs. Uses some ASP.NET core renderers -->
        <target xsi:type="File" name="ownFile-web" fileName="/var/log/bitwaves/bitwaves-own-${shortdate}.log"
                layout="${longdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}|url: ${aspnet-request-url}|action: ${aspnet-mvc-action}" />

        <!-- console target -->
        <target xsi:type="Console" name="console"
                layout="${shortdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message}"/>
    </targets>

    <!-- rules to map from logger name to target -->
    <rules>
        <!--All logs, including from Microsoft-->
        <logger name="*" minlevel="Trace" writeTo="allfile" />

        <!--Skip non-critical Microsoft logs and so log only own logs-->
        <logger name="Microsoft.*" maxlevel="Info" final="true" /> <!-- BlackHole without writeTo -->
        <logger name="*" minlevel="Trace" writeTo="ownFile-web" />

        <!-- Only own logs and critical Microsoft logs goes to console -->
        <logger name="*" minlevel="Trace" writeTo="console" />
    </rules>
</nlog>
```

配置完成后，使用如下命令启动服务端：

```bash
cd out
dotnet BitWaves.WebAPI.dll
```

> 待更新

```bash
dotnet 
```
