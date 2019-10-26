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

然后使用如下的命令在 WebAPI 子项目目录下创建配置文件 `dbsettings.json`：

```bash
cd BitWaves.WebAPI
touch dbsettings.json
```

配置文件 `dbsettings.json` 中包含了数据库相关配置，例如到数据库实例的连接字符串等。由于该配置文件中包含敏感信息（连接字符串等），因此该文件没有被包含到 git 仓库中。

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
