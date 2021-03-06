CREATE DATABASE [message_board]
GO
USE [message_board]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 7/27/2016 1:35:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[comments]    Script Date: 7/27/2016 1:35:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[comments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[author] [varchar](255) NULL,
	[main_text] [varchar](max) NULL,
	[rating] [int] NULL,
	[post_id] [int] NULL,
	[parent_id] [int] NULL,
	[time] [smalldatetime] NULL,
	[user_id] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[posts]    Script Date: 7/27/2016 1:35:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[posts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[author] [varchar](255) NULL,
	[title] [varchar](255) NULL,
	[main_text] [varchar](max) NULL,
	[rating] [int] NULL,
	[time] [smalldatetime] NULL,
	[user_id] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[posts_categories]    Script Date: 7/27/2016 1:35:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[posts_categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[post_id] [int] NULL,
	[category_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[users]    Script Date: 7/27/2016 1:35:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [varchar](50) NULL,
	[password] [varchar](100) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[voting]    Script Date: 7/27/2016 1:35:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[voting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[voter_id] [int] NULL,
	[comment_id] [int] NULL,
	[post_id] [int] NULL,
	[vote] [int] NULL
) ON [PRIMARY]

GO


CREATE DATABASE [message_board_test]
GO
USE [message_board_test]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 7/27/2016 1:35:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[comments]    Script Date: 7/27/2016 1:35:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[comments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[author] [varchar](255) NULL,
	[main_text] [varchar](max) NULL,
	[rating] [int] NULL,
	[post_id] [int] NULL,
	[parent_id] [int] NULL,
	[time] [smalldatetime] NULL,
	[user_id] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[posts]    Script Date: 7/27/2016 1:35:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[posts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[author] [varchar](255) NULL,
	[title] [varchar](255) NULL,
	[main_text] [varchar](max) NULL,
	[rating] [int] NULL,
	[time] [smalldatetime] NULL,
	[user_id] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[posts_categories]    Script Date: 7/27/2016 1:35:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[posts_categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[post_id] [int] NULL,
	[category_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[users]    Script Date: 7/27/2016 1:35:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [varchar](50) NULL,
	[password] [varchar](100) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[voting]    Script Date: 7/27/2016 1:35:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[voting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[voter_id] [int] NULL,
	[comment_id] [int] NULL,
	[post_id] [int] NULL,
	[vote] [int] NULL
) ON [PRIMARY]

GO
