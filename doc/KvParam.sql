GO
/****** Object:  Index [IX_Parent]    Script Date: 2014/10/21 18:24:57 ******/
DROP INDEX [IX_Parent] ON [dbo].[KvParam]
GO
/****** Object:  Table [dbo].[KvParam]    Script Date: 2014/10/21 18:24:58 ******/
DROP TABLE [dbo].[KvParam]
GO
/****** Object:  Table [dbo].[KvParam]    Script Date: 2014/10/21 18:24:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[KvParam](
	[Path] [varchar](512) NOT NULL,
	[Parent] [varchar](512) NULL,
	[Description] [nvarchar](512) NULL,
	[Value] [ntext] NULL,
	[HasNext] [bit] NOT NULL,
 CONSTRAINT [PK_KvParam] PRIMARY KEY CLUSTERED 
(
	[Path] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Parent]    Script Date: 2014/10/21 18:24:58 ******/
CREATE NONCLUSTERED INDEX [IX_Parent] ON [dbo].[KvParam]
(
	[Parent] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
