SET
ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories]
(
    [
    Id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [Name] [nvarchar]
(
    50
) NOT NULL
    ) ON [PRIMARY]
    GO
ALTER TABLE [dbo].[Categories] ADD PRIMARY KEY CLUSTERED
    (
    [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[Subcategories]
(
    [
    Id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [Name] [nvarchar]
(
    100
) NOT NULL,
    [CategoryId] [int] NULL
    ) ON [PRIMARY]
    GO
ALTER TABLE [dbo].[Subcategories] ADD PRIMARY KEY CLUSTERED
    (
    [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    GO
ALTER TABLE [dbo].[Subcategories] WITH CHECK ADD FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories] ([Id])
    ON
DELETE
CASCADE
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Username] [nvarchar](100) NOT NULL,
    [Email] [nvarchar](100) NOT NULL,
    [PasswordHash] [nvarchar](100) NOT NULL,
    [CreateDate] [datetimeoffset](7) NOT NULL,
    [UpdateDate] [datetimeoffset](7) NULL,
    [LastName] [nvarchar](20) NOT NULL,
    [FirstName] [nvarchar](20) NOT NULL,
    [LockedOut] [bit] NOT NULL,
    [LoginTries] [smallint] NOT NULL
    ) ON [PRIMARY]
    GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
    (
    [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DEFAULT_Users_LockedOut]  DEFAULT ((0)) FOR [LockedOut]
    GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DEFAULT_Users_LoginTies]  DEFAULT ((0)) FOR [LoginTries]
    GO

    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[Transactions](
    [Id] [uniqueidentifier] NOT NULL,
    [Description] [nvarchar](255) NOT NULL,
    [Amount] [decimal](18, 2) NOT NULL,
    [Date] [date] NOT NULL,
    [IsRecurrent] [bit] NULL,
    [TransactionType] [int] NULL,
    [CategoryId] [int] NULL,
    [SubcategoryId] [int] NULL,
    [UserId] [int] NOT NULL,
    [Currency] [nvarchar](3) NOT NULL,
    [ExchangeRate] [real] NOT NULL
    ) ON [PRIMARY]
    GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT (newsequentialid()) FOR [Id]
    GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT ((0)) FOR [IsRecurrent]
    GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT ((0)) FOR [TransactionType]
    GO
ALTER TABLE [dbo].[Transactions] ADD  CONSTRAINT [DEFAULT_Transactions_Currency]  DEFAULT ('RON') FOR [Currency]
    GO
ALTER TABLE [dbo].[Transactions] ADD  CONSTRAINT [DEFAULT_Transactions_ExchangeRate]  DEFAULT ((1.0)) FOR [ExchangeRate]
    GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([SubcategoryId])
    REFERENCES [dbo].[Subcategories] ([Id])
    ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Categories] FOREIGN KEY([CategoryId])
    REFERENCES [dbo].[Categories] ([Id])
    GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Categories]
    GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Users] FOREIGN KEY([UserId])
    REFERENCES [dbo].[Users] ([Id])
    GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Users]
    GO
