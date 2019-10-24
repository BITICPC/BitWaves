# BitWaves
BitWaves 是 BITOJ 的后端应用，采用 ASP.Net Core 框架进行编写。数据库采用 MongoDB。

## 模块划分

* 数据层，提供数据库中数据实体抽象的定义。数据层作为一个库维护在子项目 `BitWaves.Data` 中。
* 后端 API 应用，提供访问 BITOJ 中的数据的统一接口，符合 REST 设计规范。后端 API 应用作为一个 ASP.NET Core WebAPI 应用程序维护在子项目 `BitWaves.WebAPI` 中。
* 评测基础服务，为评测调度和评测终端提供公用的组件。该模块作为一个库维护在子项目 `BitWaves.Judge` 中。
* 评测调度，提供中心化的评测机发现、评测机管理和评测任务调度的功能。该模块作为一个 .NET Core 控制台应用维护在子项目 `BitWaves.JudgeCenter` 中（尚未启动编写）。
* 评测终端，提供单个评测机到评测调度的通信和单个评测机的管理功能。该模块作为一个 .NET Core 控制台应用维护在子项目 `BitWaves.JudgeHost` 中（尚未启动编写）。
* 单元测试，维护在子项目 `BitWaves.UnitTest` 中。

注意，评测沙盒作为一个单独的项目维护在仓库 `BITICPC/WaveJudgeWorker` 中，采用 Rust 编写。

## 功能

> 待更新

## 安装

推荐使用 docker 进行部署安装。请确保您的环境中已经安装了 docker。

首先使用如下命令从本仓库拉取 BitWaves 源代码并切换到仓库根目录：

```bash
git clone https://github.com/BITICPC/BitWaves.git
cd BitWaves
```

然后使用如下的命令在 WebAPI 子项目目录下创建配置文件 `nlog.config` 以及 `dbsettings.json`：

```bash
cd BitWaves.WebAPI
touch nlog.config
touch dbsettings.json
```

`nlog.config` 是 `NLog` 的配置文件，用于配置后端 API 服务的日志选项。`dbsettings.json` 中包含了数据库配置，例如到数据库实例的连接字符串等。

您可以使用如下的模板 `nlog.config` 文件：

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

该模板文件将会在目录 `/var/log/bitwaves` 下放置后端 API 服务产生的所有日志。若用户从 TTY 终端启动后端 API 服务，该配置也会在终端上显示筛选后的重要日志。有关 `NLog` 配置的详细信息请参阅 [NLog 文档](https://github.com/NLog/NLog/wiki)。

您可以使用如下的模板 `dbsettings.json` 文件：

```json
{
  "ConnectionStrings": {
    "mongodb": "mongodb://localhost"
  }
}
```

请将 `ConnectionStrings` 配置节中的 `mongodb` 配置项的值修改为到您的数据库实例的连接字符串。

配置好相关的配置文件后，使用如下的命令返回仓库根目录：

```bash
cd ..
```

然后使用如下的命令创建 docker 映像：

```bash
docker build .
```

docker build 的过程中可能需要访问互联网以下载和安装第三方依赖包，因此可能耗时较长，请耐心等待。

> To be further updated.
