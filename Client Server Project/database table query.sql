
USE [SimpleGame]
GO
/****** Object:  Table [dbo].[tblSqlErrors]    Script Date: 8/12/2022 1:20:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSqlErrors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ErrorQueryString] [nvarchar](max) NOT NULL,
	[StackTrace] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_tblSqlErrors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAuths]    Script Date: 8/12/2022 1:20:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAuths](
	[UserId] [int] NOT NULL,
	[AuthCode] [char](64) NOT NULL,
	[LoginDate] [datetime] NOT NULL,
 CONSTRAINT [PK_UserAuths] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[AuthCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserBattleLogs]    Script Date: 8/12/2022 1:20:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserBattleLogs](
	[BattleLogId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[BattleTime] [datetime] NOT NULL,
	[MonsterLevel] [int] NOT NULL,
	[UserGainedExp] [int] NOT NULL,
	[TurnCount] [int] NOT NULL,
 CONSTRAINT [PK_UserBattleLogs] PRIMARY KEY CLUSTERED 
(
	[BattleLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserCharacters]    Script Date: 8/12/2022 1:20:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserCharacters](
	[CharacterId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[HP] [int] NOT NULL,
	[Attack] [int] NOT NULL,
	[Defense] [int] NOT NULL,
	[Char Level] [int] NOT NULL,
	[CharExp] [int] NOT NULL,
 CONSTRAINT [PK_UserCharacters] PRIMARY KEY CLUSTERED 
(
	[CharacterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/12/2022 1:20:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[UserPassword] [char](64) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [char_userid]    Script Date: 8/12/2022 1:20:25 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [char_userid] ON [dbo].[UserCharacters]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblSqlErrors] ADD  CONSTRAINT [DF_tblSqlErrors_ErrorQueryString]  DEFAULT ('null') FOR [ErrorQueryString]
GO
ALTER TABLE [dbo].[tblSqlErrors] ADD  CONSTRAINT [DF_tblSqlErrors_StackTrace]  DEFAULT ('none') FOR [StackTrace]
GO
ALTER TABLE [dbo].[UserAuths] ADD  CONSTRAINT [DF_UserAuths_LoginDate]  DEFAULT (sysutcdatetime()) FOR [LoginDate]
GO
ALTER TABLE [dbo].[UserBattleLogs] ADD  CONSTRAINT [DF_UserBattleLogs_BattleTime]  DEFAULT (sysutcdatetime()) FOR [BattleTime]
GO
ALTER TABLE [dbo].[UserCharacters] ADD  CONSTRAINT [DF_UserCharacters_HP]  DEFAULT ((50)) FOR [HP]
GO
ALTER TABLE [dbo].[UserCharacters] ADD  CONSTRAINT [DF_UserCharacters_Attack]  DEFAULT ((10)) FOR [Attack]
GO
ALTER TABLE [dbo].[UserCharacters] ADD  CONSTRAINT [DF_UserCharacters_Defense]  DEFAULT ((10)) FOR [Defense]
GO
ALTER TABLE [dbo].[UserCharacters] ADD  CONSTRAINT [DF_UserCharacters_Char Level]  DEFAULT ((1)) FOR [Char Level]
GO
ALTER TABLE [dbo].[UserCharacters] ADD  CONSTRAINT [DF_UserCharacters_CharExp]  DEFAULT ((0)) FOR [CharExp]
GO
USE [master]
GO
ALTER DATABASE [SimpleGame] SET  READ_WRITE 
GO
