USE [master]
GO
/****** Object:  Database [CurrencyXchange]    Script Date: 1/20/2025 11:41:16 AM ******/
CREATE DATABASE [CurrencyXchange]

ALTER DATABASE [CurrencyXchange] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CurrencyXchange].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CurrencyXchange] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CurrencyXchange] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CurrencyXchange] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CurrencyXchange] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CurrencyXchange] SET ARITHABORT OFF 
GO
ALTER DATABASE [CurrencyXchange] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CurrencyXchange] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CurrencyXchange] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CurrencyXchange] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CurrencyXchange] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CurrencyXchange] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CurrencyXchange] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CurrencyXchange] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CurrencyXchange] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CurrencyXchange] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CurrencyXchange] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CurrencyXchange] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CurrencyXchange] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CurrencyXchange] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CurrencyXchange] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CurrencyXchange] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CurrencyXchange] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CurrencyXchange] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CurrencyXchange] SET  MULTI_USER 
GO
ALTER DATABASE [CurrencyXchange] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CurrencyXchange] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CurrencyXchange] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CurrencyXchange] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CurrencyXchange] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CurrencyXchange] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [CurrencyXchange] SET QUERY_STORE = OFF
GO
USE [CurrencyXchange]
GO
/****** Object:  User [user]    Script Date: 1/20/2025 11:41:16 AM ******/
CREATE USER [user] FOR LOGIN [user] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Currency]    Script Date: 1/20/2025 11:41:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[Name] [nvarchar](100) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Currency_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 1/20/2025 11:41:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Name] [nvarchar](100) NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Address] [nvarchar](150) NOT NULL,
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Password] [nvarchar](150) NULL,
 CONSTRAINT [PK_Users_UserId] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserTransaction]    Script Date: 1/20/2025 11:41:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTransaction](
	[WalletId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[TransactionType] [nvarchar](100) NOT NULL,
	[Amount] [decimal](18, 3) NULL,
	[Balance] [decimal](18, 3) NULL,
	[Time] [datetime] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 1/20/2025 11:41:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[UserId] [int] NOT NULL,
	[Balance] [decimal](18, 3) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK__Wallet__3214EC07A2B84444] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Currency] ON 

INSERT [dbo].[Currency] ([Name], [Code], [Id]) VALUES (N'IndianRupee', N'INR', 1)
INSERT [dbo].[Currency] ([Name], [Code], [Id]) VALUES (N'USDollar', N'USD', 2)
INSERT [dbo].[Currency] ([Name], [Code], [Id]) VALUES (N'Euros', N'EUR', 3)
SET IDENTITY_INSERT [dbo].[Currency] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Name], [CurrencyId], [Email], [Mobile], [Address], [UserId], [Password]) VALUES (N'testUser', 1, N'test@gmail.com', N'1234567890', N'test address', 1, N'Test@123')
INSERT [dbo].[Users] ([Name], [CurrencyId], [Email], [Mobile], [Address], [UserId], [Password]) VALUES (N'testUSer2', 3, N'testUSer3@gmail.com', N'1234567890', N'sample address', 2, N'Test@123')
INSERT [dbo].[Users] ([Name], [CurrencyId], [Email], [Mobile], [Address], [UserId], [Password]) VALUES (N'TestUser', 2, N'TestUser3@gmail.com', N'54321432', N'test address 2', 3, NULL)
INSERT [dbo].[Users] ([Name], [CurrencyId], [Email], [Mobile], [Address], [UserId], [Password]) VALUES (N'NewUSer', 3, N'NewUser@gmail.com', N'09876543234567', N'stirng', 4, N'New@123')
INSERT [dbo].[Users] ([Name], [CurrencyId], [Email], [Mobile], [Address], [UserId], [Password]) VALUES (N'test6', 2, N'test6@gmail.com', N'123456788', N'test address', 5, N'Test@123')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[UserTransaction] ON 

INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (1, 1, N'Credit', CAST(855.000 AS Decimal(18, 3)), CAST(1710.000 AS Decimal(18, 3)), CAST(N'2025-01-13T13:37:38.783' AS DateTime), 1, 3)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (1, 1, N'Debit', CAST(427.000 AS Decimal(18, 3)), CAST(2565.000 AS Decimal(18, 3)), CAST(N'2025-01-14T13:38:12.897' AS DateTime), 1, 4)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (3, 3, N'Credit', CAST(10.000 AS Decimal(18, 3)), CAST(0.000 AS Decimal(18, 3)), CAST(N'2025-01-15T13:39:04.130' AS DateTime), 2, 5)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (1, 1, N'Debit', CAST(50.000 AS Decimal(18, 3)), CAST(2088.000 AS Decimal(18, 3)), CAST(N'2025-01-15T14:39:20.603' AS DateTime), 1, 6)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (3, 3, N'Credit', CAST(4275.000 AS Decimal(18, 3)), CAST(4285.000 AS Decimal(18, 3)), CAST(N'2025-01-16T14:39:25.227' AS DateTime), 2, 7)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (1, 1, N'Debit', CAST(1044.000 AS Decimal(18, 3)), CAST(1044.000 AS Decimal(18, 3)), CAST(N'2025-01-15T17:00:45.167' AS DateTime), 1, 8)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (2, 2, N'Credit', CAST(0.000 AS Decimal(18, 3)), CAST(0.000 AS Decimal(18, 3)), CAST(N'2025-01-17T17:00:45.443' AS DateTime), 3, 9)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (3, 3, N'Debit', CAST(1000.000 AS Decimal(18, 3)), CAST(3285.000 AS Decimal(18, 3)), CAST(N'2025-01-14T17:02:17.757' AS DateTime), 2, 10)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (3, 3, N'Debit', CAST(1000.000 AS Decimal(18, 3)), CAST(2285.000 AS Decimal(18, 3)), CAST(N'2025-01-15T17:05:46.567' AS DateTime), 2, 11)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (2, 2, N'Credit', CAST(969.450 AS Decimal(18, 3)), CAST(969.450 AS Decimal(18, 3)), CAST(N'2025-01-16T17:05:52.813' AS DateTime), 3, 12)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (3, 3, N'Debit', CAST(1000.000 AS Decimal(18, 3)), CAST(1285.000 AS Decimal(18, 3)), CAST(N'2025-01-16T18:08:48.913' AS DateTime), 2, 13)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (1, 1, N'Credit', CAST(86576.504 AS Decimal(18, 3)), CAST(87620.504 AS Decimal(18, 3)), CAST(N'2025-01-19T18:08:49.407' AS DateTime), 1, 14)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (3, 3, N'Credit', CAST(100.000 AS Decimal(18, 3)), CAST(1385.000 AS Decimal(18, 3)), CAST(N'2025-01-19T19:58:28.693' AS DateTime), 2, 15)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (1, 1, N'Debit', CAST(1000000.000 AS Decimal(18, 3)), CAST(0.000 AS Decimal(18, 3)), CAST(N'2025-01-19T00:00:00.000' AS DateTime), 1, 16)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (4, 4, N'Credit', CAST(100.000 AS Decimal(18, 3)), CAST(100.000 AS Decimal(18, 3)), CAST(N'2025-01-19T23:25:32.723' AS DateTime), 3, 17)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (5, 5, N'Credit', CAST(100.000 AS Decimal(18, 3)), CAST(100.000 AS Decimal(18, 3)), CAST(N'2025-01-20T10:33:11.913' AS DateTime), 2, 18)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (5, 5, N'Credit', CAST(100.000 AS Decimal(18, 3)), CAST(200.000 AS Decimal(18, 3)), CAST(N'2025-01-20T10:33:40.270' AS DateTime), 2, 19)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (5, 5, N'Debit', CAST(100.000 AS Decimal(18, 3)), CAST(100.000 AS Decimal(18, 3)), CAST(N'2025-01-20T10:36:16.620' AS DateTime), 2, 20)
INSERT [dbo].[UserTransaction] ([WalletId], [UserId], [TransactionType], [Amount], [Balance], [Time], [CurrencyId], [Id]) VALUES (1, 1, N'Credit', CAST(8648.973 AS Decimal(18, 3)), CAST(96269.477 AS Decimal(18, 3)), CAST(N'2025-01-20T10:36:16.650' AS DateTime), 1, 21)
SET IDENTITY_INSERT [dbo].[UserTransaction] OFF
GO
SET IDENTITY_INSERT [dbo].[Wallet] ON 

INSERT [dbo].[Wallet] ([UserId], [Balance], [Id]) VALUES (1, CAST(96269.477 AS Decimal(18, 3)), 1)
INSERT [dbo].[Wallet] ([UserId], [Balance], [Id]) VALUES (2, CAST(969.450 AS Decimal(18, 3)), 2)
INSERT [dbo].[Wallet] ([UserId], [Balance], [Id]) VALUES (3, CAST(1385.000 AS Decimal(18, 3)), 3)
INSERT [dbo].[Wallet] ([UserId], [Balance], [Id]) VALUES (4, CAST(100.000 AS Decimal(18, 3)), 4)
INSERT [dbo].[Wallet] ([UserId], [Balance], [Id]) VALUES (5, CAST(100.000 AS Decimal(18, 3)), 5)
SET IDENTITY_INSERT [dbo].[Wallet] OFF
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_CurrencyId] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_CurrencyId]
GO
ALTER TABLE [dbo].[UserTransaction]  WITH CHECK ADD  CONSTRAINT [CurrencyId_Foreign] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[UserTransaction] CHECK CONSTRAINT [CurrencyId_Foreign]
GO
ALTER TABLE [dbo].[UserTransaction]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserTransaction]  WITH CHECK ADD FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([Id])
GO
/****** Object:  StoredProcedure [dbo].[GetDailyTransactionSummary]    Script Date: 1/20/2025 11:41:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDailyTransactionSummary]
AS
BEGIN
    DECLARE @columns NVARCHAR(MAX), @sql NVARCHAR(MAX)

    SELECT @columns = STRING_AGG(QUOTENAME(c.Code), ', ') 
    FROM (SELECT DISTINCT Code FROM Currency) c;

    SET @sql = '
    SELECT DayName, ' + @columns + '
    FROM (
        SELECT 
            c.Code,
            CASE DATEPART(WEEKDAY, ut.Time)
                WHEN 2 THEN ''Monday''
                WHEN 3 THEN ''Tuesday''
                WHEN 4 THEN ''Wednesday''
                WHEN 5 THEN ''Thursday''
                WHEN 6 THEN ''Friday''
            END AS DayName,
            ut.Amount
        FROM UserTransaction ut
        INNER JOIN Currency c ON c.Id = ut.CurrencyId
        WHERE DATEPART(WEEKDAY, ut.Time) NOT IN (1, 7) -- Exclude Sunday (1) and Saturday (7)
    ) AS SubQuery
    PIVOT (
        SUM(Amount) 
        FOR Code IN (' + @columns + ')
    ) AS PivotedData
    ORDER BY 
        CASE DayName
            WHEN ''Monday'' THEN 1
            WHEN ''Tuesday'' THEN 2
            WHEN ''Wednesday'' THEN 3
            WHEN ''Thursday'' THEN 4
            WHEN ''Friday'' THEN 5
        END';

    EXEC sp_executesql @sql;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetUserProfitOrLoss]    Script Date: 1/20/2025 11:41:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserProfitOrLoss]
    @StartDate DATE 
AS
BEGIN
    SET NOCOUNT ON;

    WITH CTE AS 
    (
        SELECT 
            UserId,
            SUM(CASE 
                    WHEN TransactionType = 'Credit' THEN Amount 
                    WHEN TransactionType = 'Debit' THEN -Amount 
                END) AS NetAmount 
        FROM UserTransaction
        WHERE Time > @StartDate 
        GROUP BY UserId
    )
    -- Select result from the CTE
    SELECT 
        UserId,
        CASE 
            WHEN NetAmount >= 0 THEN 'Profit' 
            ELSE 'Loss'                      
        END AS Type,
        ABS(NetAmount) AS Amount 
    FROM CTE;
END;
GO
USE [master]
GO
ALTER DATABASE [CurrencyXchange] SET  READ_WRITE 
GO
