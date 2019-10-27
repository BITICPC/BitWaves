# BitWaves

BitWaves 是 BITOJ 的后端 Web API 应用，采用 ASP.Net Core 框架进行编写。

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

docker 镜像构建成功后，执行如下命令启动应用程序：

```bash
docker run -d --network host <image-id> --urls http://0.0.0.0:80
```

请将 `<image-id>` 替换为 docker 构建过程中产生的应用程序镜像 ID。如果您不清楚此 ID，请执行下列命令：

```bash
docker image ls
```

您将看到类似于如下格式的输出：

```
REPOSITORY                             TAG                 IMAGE ID            CREATED             SIZE
<none>                                 <none>              123456789123        7 minutes ago       259MB
<none>                                 <none>              111111111111        7 minutes ago       1.81GB
mcr.microsoft.com/dotnet/core/sdk      2.1                 7eb1ecf4a018        10 days ago         1.74GB
mcr.microsoft.com/dotnet/core/aspnet   2.1                 190467cc5405        10 days ago         253MB
```

使用您的输出中位于该列表第一行的镜像的 `IMAGE ID` 字段（在本例中即为 `123456789123`）替换 `<image-id>` 即可。

如果您需要停止应用程序，请执行如下命令：

```bash
docker stop <container-id>
```

请使用应用程序的容器 ID 替换 `<container-id>` 部分。如果您不清楚您的容器 ID，请执行下列命令：

```bash
docker ps
```

您将看到类似于如下格式的输出：

```
CONTAINER ID        IMAGE               COMMAND                  CREATED             STATUS              PORTS               NAMES
000111222333        123456789123        "dotnet BitWaves.Web…"   10 minutes ago      Up 10 minutes                           silly_bartik
```

请使用与您的应用程序镜像 ID（在本例中为 `123456789123`）所对应的容器 ID（在本例中为 `000111222333`）替换 `<container-id>` 部分。
