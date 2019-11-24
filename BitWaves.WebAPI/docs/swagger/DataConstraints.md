# 数据约束

本文档列出 BitWaves 中所有数据字段的约束条件。如果某数据字段没有在在本文档中列出，则说明该数据字段上没有数据约束且该数据字段可空。

## 用户数据

### 用户名

* 字段名称：`username`
* 是否可空：否
* 类型：`string`
* 最短长度：3
* 最长长度：64

用户名相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/UsernameAttribute.cs`。

### 密码

* 字段名称：`password`
* 是否可空：否
* 类型：`string`
* 最短长度：6

密码相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/PasswordAttribute.cs`。

### 昵称

* 字段名称：`nickname`
* 是否可空：否
* 类型：`string`
* 最短长度：1
* 最长长度：64

昵称相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/NicknameAttribute.cs`。

### 学校名称

* 字段名称：`school`
* 是否可空：是
* 类型：`string`
* 最短长度：1
* 最长长度：64

学校名称相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/SchoolAttribute.cs`。

### 学号

* 字段名称：`studentId`
* 是否可空：是
* 类型：`string`
* 最短长度：3
* 最长长度：30

学号相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/StudentIdAttribute.cs`。

### 博客 URL

* 字段名称：`blogUrl`
* 是否可空：是
* 类型：`string`
* 正则匹配：`^(https?:\/\/)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&\/\/=]*)$`

博客 URL 相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/UrlAttribute.cs`。

## 全站公告数据

### 全站公告标题

* 字段名称：`title`
* 是否可空：否
* 类型：`string`
* 最短长度：1
* 最长长度：128

全站公告标题相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/AnnouncementTitleAttribute.cs`。

### 全站公告内容

* 字段名称：`content`
* 是否可空：否
* 类型：`string`
* 最短长度：1

全站公告内容相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/AnnouncementContentAttribute.cs`。

## 题目数据

### 题目标题

* 字段名称：`title`
* 是否可空：否
* 类型：`string`
* 最短长度：1
* 最长长度：128

题目标题相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/ProblemTitleAttribute.cs`。

### 公开题目集题目 ID

* 字段名称：`archiveId`
* 是否可空：是
* 类型：`integer`
* 最小值：0

公开题目集 ID 的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/ArchiveIdAttribute.cs`。

### 难度系数

* 字段名称：`difficulty`
* 是否可空：否
* 类型：`integer`
* 最小值：0
* 最大值：100

难度系数相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/DifficultyAttribute.cs`。

### 时间限制

* 字段名称：`timeLimit`
* 是否可空：否
* 类型：`integer`
* 最小值：500
* 最大值：10000

时间限制相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/TimeLimitAttribute.cs`。

### 内存限制

* 字段名称：`memoryLimit`
* 是否可空：否
* 类型：`integer`
* 最小值：32
* 最大值：1024

内存限制相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/MemoryLimitAttribute.cs`。

## 语言数据

### 语言标识符

* 字段名称：`langId`
* 是否可空：否
* 类型：`string`
* 最小长度：1
* 最大长度：32

语言标识符相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/LanguageIdentifierAttribute.cs`。

### 语言方言名称

* 字段名称：`dialect`
* 是否可空：否
* 类型：`string`
* 最小长度：1
* 最大长度：32

语言方言名称相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/LanguageDialectAttribute.cs`。

### 语言版本名称

* 字段名称：`version`
* 是否可空：否
* 类型：`string`
* 最小长度：1
* 最大长度：32

语言版本相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/LanguageVersionAttribute.cs`。

### 语言显示名称

* 字段名称：`displayName`
* 是否可空：否
* 类型：`string`
* 最小长度：1
* 最大长度：128

语言方言名称相关的数据验证逻辑定义在 `BitWaves.WebAPI/Validation/LanguageDisplayNameAttribute.cs`。
