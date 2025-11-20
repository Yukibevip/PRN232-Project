CREATE DATABASE [Callio_Test]
GO
USE [Callio_Test]
GO
/****** Object:  Table [dbo].[Accusations]    Script Date: 10/27/2025 4:47:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accusations](
	[AccusationId] [int] IDENTITY(1,1) NOT NULL,
	[ReportedId] [uniqueidentifier] NOT NULL,
	[AccusedId] [uniqueidentifier] NOT NULL,
	[Category] [varchar](64) NOT NULL,
	[Descriptions] [nvarchar](max) NOT NULL,
	[Status] [varchar](64) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ReviewAt] [datetime] NOT NULL,
	[ReviewedBy] [uniqueidentifier] NOT NULL,
	[ResolutionNote] [nvarchar](max) NULL,
 CONSTRAINT [PK_Accusations] PRIMARY KEY CLUSTERED 
(
	[AccusationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlockList]    Script Date: 10/27/2025 4:47:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlockList](
	[BlockId] [int] IDENTITY(1,1) NOT NULL,
	[BlockerId] [uniqueidentifier] NOT NULL,
	[BlockedId] [uniqueidentifier] NOT NULL,
	[IsPermanent] [bit] NOT NULL,
	[ExpiresAt] [datetime] NULL,
 CONSTRAINT [PK_BlockList] PRIMARY KEY CLUSTERED 
(
	[BlockId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FriendInvitation]    Script Date: 10/27/2025 4:47:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FriendInvitation](
	[InvitationId] [int] IDENTITY(1,1) NOT NULL,
	[SenderId] [uniqueidentifier] NOT NULL,
	[ReceiverId] [uniqueidentifier] NOT NULL,
	[SentAt] [datetime] NOT NULL,
 CONSTRAINT [PK_FriendInvitation] PRIMARY KEY CLUSTERED 
(
	[InvitationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FriendList]    Script Date: 10/27/2025 4:47:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FriendList](
	[UserId1] [uniqueidentifier] NOT NULL,
	[UserId2] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_FriendList] PRIMARY KEY CLUSTERED 
(
	[UserId1] ASC,
	[UserId2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 10/27/2025 4:47:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[Action] [varchar](128) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Status] [varchar](128) NOT NULL,
	[ErrorCode] [varchar](128) NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 10/27/2025 4:47:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[MessageId] [int] IDENTITY(1,1) NOT NULL,
	[SenderId] [uniqueidentifier] NOT NULL,
	[ReceiverId] [uniqueidentifier] NOT NULL,
	[Content] [varchar](max) NOT NULL,
	[SentAt] [datetime] NOT NULL,
	[IsRead] [bit] NOT NULL,
	[ReadAt] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[ReplyToId] [int] NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/27/2025 4:47:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](128) NOT NULL,
	[Password] [varchar](128) NOT NULL,
	[FullName] [nvarchar](128) NULL,
	[Email] [varchar](256) NOT NULL,
	[Gender] [nvarchar](64)  NULL,
	[DateOfBirth] [date]  NULL,
	[AvatarUrl] [varchar](max) NULL,
	[UserRole] [varchar](64) NULL,
	[GoogleId] [nvarchar](64) NULL,
	[Status] [nvarchar](64) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accusations] ADD  CONSTRAINT [DF_Accusations_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Accusations] ADD  CONSTRAINT [DF_Accusations_ReviewAt]  DEFAULT (getdate()) FOR [ReviewAt]
GO
ALTER TABLE [dbo].[BlockList] ADD  CONSTRAINT [DF_BlockList_IsPermanent]  DEFAULT ((0)) FOR [IsPermanent]
GO
ALTER TABLE [dbo].[FriendInvitation] ADD  CONSTRAINT [DF_FriendInvitation_SentAt]  DEFAULT (getdate()) FOR [SentAt]
GO
ALTER TABLE [dbo].[Logs] ADD  CONSTRAINT [DF_Logs_TimeStamp]  DEFAULT (getdate()) FOR [TimeStamp]
GO
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_SentAt]  DEFAULT (getdate()) FOR [SentAt]
GO
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_IsRead]  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_ReadAt]  DEFAULT (getdate()) FOR [ReadAt]
GO
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_UserId]  DEFAULT (newid()) FOR [UserId]
GO
ALTER TABLE [dbo].[Accusations]  WITH CHECK ADD  CONSTRAINT [FK_Accusations_Users] FOREIGN KEY([ReportedId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Accusations] CHECK CONSTRAINT [FK_Accusations_Users]
GO
ALTER TABLE [dbo].[Accusations]  WITH CHECK ADD  CONSTRAINT [FK_Accusations_Users1] FOREIGN KEY([AccusedId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Accusations] CHECK CONSTRAINT [FK_Accusations_Users1]
GO
ALTER TABLE [dbo].[Accusations]  WITH CHECK ADD  CONSTRAINT [FK_Accusations_Users2] FOREIGN KEY([ReviewedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Accusations] CHECK CONSTRAINT [FK_Accusations_Users2]
GO
ALTER TABLE [dbo].[BlockList]  WITH CHECK ADD  CONSTRAINT [FK_BlockList_Users] FOREIGN KEY([BlockerId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[BlockList] CHECK CONSTRAINT [FK_BlockList_Users]
GO
ALTER TABLE [dbo].[BlockList]  WITH CHECK ADD  CONSTRAINT [FK_BlockList_Users1] FOREIGN KEY([BlockedId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[BlockList] CHECK CONSTRAINT [FK_BlockList_Users1]
GO
ALTER TABLE [dbo].[FriendInvitation]  WITH CHECK ADD  CONSTRAINT [FK_FriendInvitation_Users] FOREIGN KEY([SenderId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[FriendInvitation] CHECK CONSTRAINT [FK_FriendInvitation_Users]
GO
ALTER TABLE [dbo].[FriendInvitation]  WITH CHECK ADD  CONSTRAINT [FK_FriendInvitation_Users1] FOREIGN KEY([ReceiverId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[FriendInvitation] CHECK CONSTRAINT [FK_FriendInvitation_Users1]
GO
ALTER TABLE [dbo].[FriendList]  WITH CHECK ADD  CONSTRAINT [FK_FriendList_Users] FOREIGN KEY([UserId1])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[FriendList] CHECK CONSTRAINT [FK_FriendList_Users]
GO
ALTER TABLE [dbo].[FriendList]  WITH CHECK ADD  CONSTRAINT [FK_FriendList_Users1] FOREIGN KEY([UserId2])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[FriendList] CHECK CONSTRAINT [FK_FriendList_Users1]
GO
ALTER TABLE [dbo].[Logs]  WITH CHECK ADD  CONSTRAINT [FK_Logs_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Logs] CHECK CONSTRAINT [FK_Logs_Users]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Messages] FOREIGN KEY([ReplyToId])
REFERENCES [dbo].[Messages] ([MessageId])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Messages]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Users] FOREIGN KEY([SenderId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Users]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Users1] FOREIGN KEY([ReceiverId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Users1]
GO
