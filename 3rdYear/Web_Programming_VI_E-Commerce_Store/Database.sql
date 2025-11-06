USE [master]
GO
/****** Object:  Database [YourDB]    Script Date: 12/11/2024 11:17:35 PM ******/
CREATE DATABASE [YourDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'H60A01EmptyDB', FILENAME = N'E:\MSSQL15.MSSQLSERVER\MSSQL\DATA\YourDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'H60A01EmptyDB_log', FILENAME = N'E:\MSSQL15.MSSQLSERVER\MSSQL\DATA\YourDB.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [YourDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [YourDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [YourDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [YourDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [YourDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [YourDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [YourDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [YourDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [YourDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [YourDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [YourDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [YourDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [YourDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [YourDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [YourDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [YourDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [YourDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [YourDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [YourDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [YourDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [YourDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [YourDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [YourDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [YourDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [YourDB] SET RECOVERY FULL 
GO
ALTER DATABASE [YourDB] SET  MULTI_USER 
GO
ALTER DATABASE [YourDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [YourDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [YourDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [YourDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [YourDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [YourDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'YourDB', N'ON'
GO
ALTER DATABASE [YourDB] SET QUERY_STORE = OFF
GO
USE [YourDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 12/11/2024 11:17:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartItems]    Script Date: 12/11/2024 11:17:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartItems](
	[CartItemId] [int] IDENTITY(1,1) NOT NULL,
	[CartId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_CartItems] PRIMARY KEY CLUSTERED 
(
	[CartItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 12/11/2024 11:17:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[Province] [nvarchar](max) NOT NULL,
	[CreditCard] [nvarchar](max) NOT NULL,
	[UserId] [nvarchar](max) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 12/11/2024 11:17:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[OrderItemId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED 
(
	[OrderItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 12/11/2024 11:17:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL,
	[DateFulfilled] [datetime2](7) NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Taxes] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 12/11/2024 11:17:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProdCatId] [int] NOT NULL,
	[Description] [varchar](80) NOT NULL,
	[Manufacturer] [varchar](80) NOT NULL,
	[Stock] [int] NOT NULL,
	[BuyPrice] [numeric](8, 2) NOT NULL,
	[SellPrice] [numeric](8, 2) NOT NULL,
	[ImageUrl] [nvarchar](max) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 12/11/2024 11:17:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ProdCat] [varchar](60) NOT NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShoppingCarts]    Script Date: 12/11/2024 11:17:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShoppingCarts](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ShoppingCarts] PRIMARY KEY CLUSTERED 
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CartItems] ON 
GO
INSERT [dbo].[CartItems] ([CartItemId], [CartId], [ProductId], [Quantity], [Price]) VALUES (92, 62, 18, 1, CAST(1500.00 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[CartItems] OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON 
GO
INSERT [dbo].[Customers] ([CustomerId], [FirstName], [LastName], [Email], [PhoneNumber], [Province], [CreditCard], [UserId]) VALUES (19, N'Customer', N'Samuel', N'customer@gmail.com', N'1231231234', N'QC', N'1234123412341234', N'd58bcfcd-4b24-400d-a409-717326fed753')
GO
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderItems] ON 
GO
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [Price]) VALUES (141, 94, 18, 1, CAST(1500.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [Price]) VALUES (142, 95, 18, 1, CAST(1500.00 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[OrderItems] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 
GO
INSERT [dbo].[Orders] ([OrderId], [CustomerId], [DateCreated], [DateFulfilled], [Total], [Taxes]) VALUES (94, 19, CAST(N'2024-12-11T00:00:00.0000000' AS DateTime2), CAST(N'2024-12-11T00:00:00.0000000' AS DateTime2), CAST(1724.63 AS Decimal(18, 2)), CAST(224.63 AS Decimal(18, 2)))
GO
INSERT [dbo].[Orders] ([OrderId], [CustomerId], [DateCreated], [DateFulfilled], [Total], [Taxes]) VALUES (95, 19, CAST(N'2024-12-11T00:00:00.0000000' AS DateTime2), CAST(N'2024-12-11T00:00:00.0000000' AS DateTime2), CAST(1724.63 AS Decimal(18, 2)), CAST(224.63 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (3, 2, N'Sp5der Web Hoodie ''Sky Blues''', N'Sp5der', 400, CAST(120.00 AS Numeric(8, 2)), CAST(250.00 AS Numeric(8, 2)), N'/images/hoodie.png')
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (17, 7, N'Air Jordan 1 Low "Reverse Mocha"', N'Nike', 164, CAST(450.00 AS Numeric(8, 2)), CAST(1500.00 AS Numeric(8, 2)), N'/images/reversem.png')
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (18, 7, N'Air Jordan 1 Low "Fragment"', N'Nike', 98, CAST(1300.00 AS Numeric(8, 2)), CAST(1500.00 AS Numeric(8, 2)), N'/images/fragment.png')
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (19, 8, N'Essentials "Cream" T-Shirt', N'Essentials', 118, CAST(80.00 AS Numeric(8, 2)), CAST(120.00 AS Numeric(8, 2)), N'/images/tshirt.png')
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (27, 4, N'Saint Laurent Mid Straight Baggy Jeans', N'Saint Laurent', 14, CAST(100.00 AS Numeric(8, 2)), CAST(300.00 AS Numeric(8, 2)), N'/images/pants.png')
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (29, 2, N'Essentials Hoodie "Black"', N'Essentials', 232, CAST(145.00 AS Numeric(8, 2)), CAST(255.00 AS Numeric(8, 2)), N'/images/essentialsblack.png')
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (30, 8, N'Stussy T-Shirt "White" ', N'Stussy', 500, CAST(80.00 AS Numeric(8, 2)), CAST(120.00 AS Numeric(8, 2)), N'/images/stussy.png')
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (31, 5, N'Supreme Black Puffer Coat', N'Supreme', 50, CAST(400.00 AS Numeric(8, 2)), CAST(800.00 AS Numeric(8, 2)), N'/images/coatone.png')
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (32, 5, N'Supreme Black Puffer Coat "Glossy" ', N'Supreme', 150, CAST(500.00 AS Numeric(8, 2)), CAST(1000.00 AS Numeric(8, 2)), N'/images/coattwo.png')
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (33, 6, N'Supreme Shoulder Bag "Black"', N'Supreme', 99, CAST(1000.00 AS Numeric(8, 2)), CAST(10000.00 AS Numeric(8, 2)), N'/images/bagone.png')
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (34, 6, N'Supreme Shoulder Bag "Red"', N'Supreme', 149, CAST(5000.00 AS Numeric(8, 2)), CAST(15000.00 AS Numeric(8, 2)), N'/images/bagtwo.png')
GO
INSERT [dbo].[Product] ([ProductID], [ProdCatId], [Description], [Manufacturer], [Stock], [BuyPrice], [SellPrice], [ImageUrl]) VALUES (35, 4, N'Off-White Logo Slim Cargo Pants "Black/White"', N'Off-White', 0, CAST(150.00 AS Numeric(8, 2)), CAST(350.00 AS Numeric(8, 2)), N'/images/cargo.png')
GO
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductCategory] ON 
GO
INSERT [dbo].[ProductCategory] ([CategoryID], [ProdCat]) VALUES (2, N'Hoodies')
GO
INSERT [dbo].[ProductCategory] ([CategoryID], [ProdCat]) VALUES (4, N'Bottoms')
GO
INSERT [dbo].[ProductCategory] ([CategoryID], [ProdCat]) VALUES (5, N'Outerwear')
GO
INSERT [dbo].[ProductCategory] ([CategoryID], [ProdCat]) VALUES (6, N'Bags')
GO
INSERT [dbo].[ProductCategory] ([CategoryID], [ProdCat]) VALUES (7, N'Sneakers')
GO
INSERT [dbo].[ProductCategory] ([CategoryID], [ProdCat]) VALUES (8, N'Shirts')
GO
SET IDENTITY_INSERT [dbo].[ProductCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[ShoppingCarts] ON 
GO
INSERT [dbo].[ShoppingCarts] ([CartId], [CustomerId], [DateCreated]) VALUES (62, 19, CAST(N'2024-12-12T03:32:18.3728770' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[ShoppingCarts] OFF
GO
/****** Object:  Index [dbo_IX_CartItems_CartId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [dbo_IX_CartItems_CartId] ON [dbo].[CartItems]
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_IX_CartItems_ProductId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [dbo_IX_CartItems_ProductId] ON [dbo].[CartItems]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_PK_CartItems]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [dbo_PK_CartItems] ON [dbo].[CartItems]
(
	[CartItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CartItems_CartId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_CartItems_CartId] ON [dbo].[CartItems]
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CartItems_ProductId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_CartItems_ProductId] ON [dbo].[CartItems]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_PK_Customers]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [dbo_PK_Customers] ON [dbo].[Customers]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_IX_OrderItems_OrderId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [dbo_IX_OrderItems_OrderId] ON [dbo].[OrderItems]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_IX_OrderItems_ProductId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [dbo_IX_OrderItems_ProductId] ON [dbo].[OrderItems]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_PK_OrderItems]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [dbo_PK_OrderItems] ON [dbo].[OrderItems]
(
	[OrderItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderItems_OrderId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderItems_OrderId] ON [dbo].[OrderItems]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderItems_ProductId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderItems_ProductId] ON [dbo].[OrderItems]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_IX_Orders_CustomerId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [dbo_IX_Orders_CustomerId] ON [dbo].[Orders]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_PK_Orders]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [dbo_PK_Orders] ON [dbo].[Orders]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_CustomerId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_CustomerId] ON [dbo].[Orders]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_IX_Product_ProdCatId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [dbo_IX_Product_ProdCatId] ON [dbo].[Product]
(
	[ProdCatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_PK_Product]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [dbo_PK_Product] ON [dbo].[Product]
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product_ProdCatId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_Product_ProdCatId] ON [dbo].[Product]
(
	[ProdCatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_PK_ProductCategory]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [dbo_PK_ProductCategory] ON [dbo].[ProductCategory]
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_IX_ShoppingCarts_CustomerId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [dbo_IX_ShoppingCarts_CustomerId] ON [dbo].[ShoppingCarts]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [dbo_PK_ShoppingCarts]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [dbo_PK_ShoppingCarts] ON [dbo].[ShoppingCarts]
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShoppingCarts_CustomerId]    Script Date: 12/11/2024 11:17:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShoppingCarts_CustomerId] ON [dbo].[ShoppingCarts]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD  CONSTRAINT [FK_CartItems_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CartItems] CHECK CONSTRAINT [FK_CartItems_Product_ProductId]
GO
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD  CONSTRAINT [FK_CartItems_ShoppingCarts_CartId] FOREIGN KEY([CartId])
REFERENCES [dbo].[ShoppingCarts] ([CartId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CartItems] CHECK CONSTRAINT [FK_CartItems_ShoppingCarts_CartId]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Orders_OrderId]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Product_ProductId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customers_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customers_CustomerId]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductCategory] FOREIGN KEY([ProdCatId])
REFERENCES [dbo].[ProductCategory] ([CategoryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductCategory]
GO
ALTER TABLE [dbo].[ShoppingCarts]  WITH CHECK ADD  CONSTRAINT [FK_ShoppingCarts_Customers_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoppingCarts] CHECK CONSTRAINT [FK_ShoppingCarts_Customers_CustomerId]
GO
USE [master]
GO
ALTER DATABASE [YourDB] SET  READ_WRITE 
GO
