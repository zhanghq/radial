BookNine是应用DDD模式的例子，各项目简要说明如下：

领域层
BookNine.Domain

基础设施层
CrossCutting部分：BookNine.Infrastructure
Persistence部分：BookNine.Infrastructure.Persistence

应用程序层

Service层：BookNine.Application，包括领域模型到传输模型的转换，和启动任务
DTO传输层：BookNine.TransferObject，传输模型

表现层
BookNine.Web