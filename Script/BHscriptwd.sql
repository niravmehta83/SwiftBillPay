USE [master]
GO
/****** Object:  Database [BingHousingDbase]    Script Date: 04/30/2014 10:14:45 AM ******/
CREATE DATABASE [BingHousingDbase] ON  PRIMARY 
( NAME = N'BingHousingDbase', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\BingHousingDbase.mdf' , SIZE = 2304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BingHousingDbase_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\BingHousingDbase_log.LDF' , SIZE = 512KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [BingHousingDbase] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BingHousingDbase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BingHousingDbase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BingHousingDbase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BingHousingDbase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BingHousingDbase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BingHousingDbase] SET ARITHABORT OFF 
GO
ALTER DATABASE [BingHousingDbase] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BingHousingDbase] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [BingHousingDbase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BingHousingDbase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BingHousingDbase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BingHousingDbase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BingHousingDbase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BingHousingDbase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BingHousingDbase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BingHousingDbase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BingHousingDbase] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BingHousingDbase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BingHousingDbase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BingHousingDbase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BingHousingDbase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BingHousingDbase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BingHousingDbase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BingHousingDbase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BingHousingDbase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BingHousingDbase] SET  MULTI_USER 
GO
ALTER DATABASE [BingHousingDbase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BingHousingDbase] SET DB_CHAINING OFF 
GO
USE [BingHousingDbase]
GO
/****** Object:  Table [dbo].[CustomerDetails]    Script Date: 04/30/2014 10:14:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomerDetails](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CustomerFirstName] [varchar](100) NULL,
	[CustomerLastName] [varchar](100) NULL,
	[CustomerAddress] [varchar](250) NULL,
	[CustomerAddress2] [varchar](250) NULL,
	[CustomerCity] [varchar](100) NULL,
	[CustomerState] [varchar](100) NULL,
	[CustomerZipCode] [varchar](25) NULL,
	[CustomerCountry] [varchar](100) NULL,
	[CustomerEmail] [varchar](100) NULL,
	[AmountDue] [decimal](10, 2) NULL,
	[BillDescription] [varchar](500) NULL,
	[Payee] [varchar](100) NULL,
	[PayeeAddress] [varchar](250) NULL,
	[PayeeAddress2] [varchar](250) NULL,
	[PayeeCity] [varchar](100) NULL,
	[PayeeState] [varchar](100) NULL,
	[PayeeZipCode] [varchar](25) NULL,
	[PayeeCountry] [varchar](100) NULL,
	[PayeeEmail] [varchar](100) NULL,
	[PayeeComments] [varchar](250) NULL,
	[PayeeContactPerson] [varchar](100) NULL,
	[PayeeWebsite] [varchar](50) NULL,
 CONSTRAINT [PK_CustomerDetails] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 04/30/2014 10:14:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Invoice](
	[InvoiceId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[InvoiceNumber] [varchar](100) NOT NULL,
	[EmailSentDate] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserDetails]    Script Date: 04/30/2014 10:14:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserDetails](
	[UserId] [int] NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[Address] [varchar](250) NOT NULL,
	[Address2] [varchar](250) NULL,
	[City] [varchar](100) NOT NULL,
	[State] [varchar](100) NOT NULL,
	[ZipCode] [varchar](25) NOT NULL,
	[Country] [varchar](100) NOT NULL,
	[PhoneNumber] [varchar](25) NOT NULL,
	[Mobile] [varchar](25) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 04/30/2014 10:14:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserSecurity]    Script Date: 04/30/2014 10:14:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSecurity](
	[SecurityId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Password2] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[Islocked] [bit] NOT NULL,
 CONSTRAINT [PK__UserSecu__9F8B09301920BF5C] PRIMARY KEY CLUSTERED 
(
	[SecurityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 04/30/2014 10:14:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL,
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_OAuthMembership]    Script Date: 04/30/2014 10:14:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_OAuthMembership](
	[Provider] [nvarchar](30) NOT NULL,
	[ProviderUserId] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Provider] ASC,
	[ProviderUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 04/30/2014 10:14:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 04/30/2014 10:14:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[CustomerDetails] ON 

INSERT [dbo].[CustomerDetails] ([CustomerId], [UserId], [CustomerFirstName], [CustomerLastName], [CustomerAddress], [CustomerAddress2], [CustomerCity], [CustomerState], [CustomerZipCode], [CustomerCountry], [CustomerEmail], [AmountDue], [BillDescription], [Payee], [PayeeAddress], [PayeeAddress2], [PayeeCity], [PayeeState], [PayeeZipCode], [PayeeCountry], [PayeeEmail], [PayeeComments], [PayeeContactPerson], [PayeeWebsite]) VALUES (1, 2, N'Nataraj', N'vrun', N'75 Main Street', NULL, N'Flushing', N'NY', N'11366', N'USA', N'kingslaver@gmail.com', CAST(300.76 AS Decimal(10, 2)), N'lease rent', N'31 Seminary Llc', N'76 Jerome Plaza', NULL, N'New York', N'NY', N'11366', N'USA', N'31sem@gmail.com', N'pay as soon as posible', N'Pad', N'abcd.com')
INSERT [dbo].[CustomerDetails] ([CustomerId], [UserId], [CustomerFirstName], [CustomerLastName], [CustomerAddress], [CustomerAddress2], [CustomerCity], [CustomerState], [CustomerZipCode], [CustomerCountry], [CustomerEmail], [AmountDue], [BillDescription], [Payee], [PayeeAddress], [PayeeAddress2], [PayeeCity], [PayeeState], [PayeeZipCode], [PayeeCountry], [PayeeEmail], [PayeeComments], [PayeeContactPerson], [PayeeWebsite]) VALUES (2, 2, N'Thagore', N'Levin', N'37 Front Street', NULL, N'Cedarhurst', N'NY', N'11516', N'USA', N'kingslaver@gmail.com', CAST(450.70 AS Decimal(10, 2)), N'rent', N'33 Seminary LLC', N'100 Flower Street', NULL, N'New York', N'NY', N'11516', N'USA', N'33sem@gmail.com', N'pay as soon as posible', N'John', N'efgh.com')
INSERT [dbo].[CustomerDetails] ([CustomerId], [UserId], [CustomerFirstName], [CustomerLastName], [CustomerAddress], [CustomerAddress2], [CustomerCity], [CustomerState], [CustomerZipCode], [CustomerCountry], [CustomerEmail], [AmountDue], [BillDescription], [Payee], [PayeeAddress], [PayeeAddress2], [PayeeCity], [PayeeState], [PayeeZipCode], [PayeeCountry], [PayeeEmail], [PayeeComments], [PayeeContactPerson], [PayeeWebsite]) VALUES (3, 2, N'Jack', N'Roberts', N'128 Main Street', NULL, N'New York', N'NY', N'10001', N'India', N'kingslaver@gmail.com', CAST(375.00 AS Decimal(10, 2)), N'April rent', N'Jack & Company', N'3 Jackson Avenue', NULL, N'Cresecnt', N'NY', N'10001', N'USA', N'jack@jackco.com', N'pay as soon as posible', N'John', N'jacko.com')
INSERT [dbo].[CustomerDetails] ([CustomerId], [UserId], [CustomerFirstName], [CustomerLastName], [CustomerAddress], [CustomerAddress2], [CustomerCity], [CustomerState], [CustomerZipCode], [CustomerCountry], [CustomerEmail], [AmountDue], [BillDescription], [Payee], [PayeeAddress], [PayeeAddress2], [PayeeCity], [PayeeState], [PayeeZipCode], [PayeeCountry], [PayeeEmail], [PayeeComments], [PayeeContactPerson], [PayeeWebsite]) VALUES (4, 2, N'Jim', N'Fuller', N'3 maple Road', NULL, N'Brooklyn', N'NY', N'13101', N'India', N'kingslaver@gmail.com', CAST(200.00 AS Decimal(10, 2)), N'April rent', N'Jack & Company', N'3 Jackson Avenue', NULL, N'Cresent', N'NY', N'13101', N'USA', N'jack@jackco.com', N'pay as soon as posible', N'John', N'jacko.com')
INSERT [dbo].[CustomerDetails] ([CustomerId], [UserId], [CustomerFirstName], [CustomerLastName], [CustomerAddress], [CustomerAddress2], [CustomerCity], [CustomerState], [CustomerZipCode], [CustomerCountry], [CustomerEmail], [AmountDue], [BillDescription], [Payee], [PayeeAddress], [PayeeAddress2], [PayeeCity], [PayeeState], [PayeeZipCode], [PayeeCountry], [PayeeEmail], [PayeeComments], [PayeeContactPerson], [PayeeWebsite]) VALUES (5, 2, N'Adam', N'Smith', N'3 Maple Road', NULL, N'Brooklyn', N'NY', N'13101', N'USA', N'kingslaver@gmail.com', CAST(700.90 AS Decimal(10, 2)), N'April rent', N'Jack & Company', N'3 Jackson Avenue', NULL, N'Cresent', N'NY', N'13101', N'USA', N'jack@jackco.com', N'good', N'John', N'jacko.com')
SET IDENTITY_INSERT [dbo].[CustomerDetails] OFF
SET IDENTITY_INSERT [dbo].[Invoice] ON 

INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (1, 2, 5, N'A25', CAST(0x5D380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (2, 2, 5, N'A25', CAST(0x62380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (3, 2, 5, N'A25', CAST(0x62380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (4, 2, 5, N'A25', CAST(0x62380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (5, 2, 1, N'N21', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (6, 2, 5, N'A25', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (7, 2, 1, N'N21', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (8, 2, 2, N'T22', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (9, 2, 3, N'J23', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (10, 2, 4, N'J24', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (11, 2, 5, N'A25', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (12, 2, 1, N'N21', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (13, 2, 2, N'T22', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (14, 2, 3, N'J23', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (15, 2, 4, N'J24', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (16, 2, 5, N'A25', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (17, 2, 1, N'N21', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (18, 2, 2, N'T22', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (19, 2, 3, N'J23', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (20, 2, 4, N'J24', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (21, 2, 5, N'A25', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (22, 2, 1, N'N21', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (23, 2, 2, N'T22', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (24, 2, 3, N'J23', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (25, 2, 4, N'J24', CAST(0x63380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (26, 2, 2, N'T22', CAST(0x64380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (27, 2, 3, N'J23', CAST(0x64380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (28, 2, 5, N'A25', CAST(0x64380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (29, 2, 5, N'A25', CAST(0x64380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (30, 2, 5, N'A25', CAST(0x64380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (31, 2, 5, N'A25', CAST(0x64380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (32, 2, 5, N'A25', CAST(0x64380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (33, 2, 5, N'A25', CAST(0x64380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (34, 2, 5, N'A25', CAST(0x6E380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (35, 2, 5, N'A25', CAST(0x6E380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (36, 2, 5, N'A25', CAST(0x6E380B00 AS Date))
INSERT [dbo].[Invoice] ([InvoiceId], [UserId], [CustomerId], [InvoiceNumber], [EmailSentDate]) VALUES (37, 2, 1, N'N21', CAST(0x71380B00 AS Date))
SET IDENTITY_INSERT [dbo].[Invoice] OFF
INSERT [dbo].[UserDetails] ([UserId], [FirstName], [LastName], [Address], [Address2], [City], [State], [ZipCode], [Country], [PhoneNumber], [Mobile]) VALUES (1, N'Raja', N'Dashnamoorthy', N'no:11/33', N'No: 11/ 33 Poombuhar St,Rani anna nagar', N'Chennai', N'Tamilnadu', N'600106', N'India', N'918553227181', N'918553227181')
INSERT [dbo].[UserDetails] ([UserId], [FirstName], [LastName], [Address], [Address2], [City], [State], [ZipCode], [Country], [PhoneNumber], [Mobile]) VALUES (2, N'Raja', N'Dashnamoorthy', N'no:11/33', N'Poombuhar St,Rani anna nagar', N'Chennai', N'Tamilnadu', N'600106', N'India', N'919597141126', N'918553227181')
SET IDENTITY_INSERT [dbo].[UserProfile] ON 

INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (1, N'Admin')
INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (2, N'Raja')
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
SET IDENTITY_INSERT [dbo].[UserSecurity] ON 

INSERT [dbo].[UserSecurity] ([SecurityId], [UserId], [Password2], [Email], [Islocked]) VALUES (1, 1, N'raja2014', N'kingslaver@gmail.com', 0)
INSERT [dbo].[UserSecurity] ([SecurityId], [UserId], [Password2], [Email], [Islocked]) VALUES (2, 2, N'7252433842', N'kingslaver@gmail.com', 0)
SET IDENTITY_INSERT [dbo].[UserSecurity] OFF
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (1, CAST(0x0000A2FF010D769D AS DateTime), N'yB3rO7uPWZJ0Oay_kRjVFQ2', 1, NULL, 0, N'ACuqO090Ix1XyjtRIhpVwjntaVYuAOhoEWjXFufoiuCn6+lgZpHUTZeQx8a7yWpdQg==', CAST(0x0000A2FF010D769D AS DateTime), N'', N'rtx2GF8zrnJRJN422dphVg2', CAST(0x0000A3110041CAD4 AS DateTime))
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (2, CAST(0x0000A2FF010E7AA9 AS DateTime), N'BAxoVElro0P1lstb1Q0wZQ2', 1, NULL, 0, N'APCkWdV/AB6nwK/ABI0QmJYu6qZMz9OVUL5YLjASl/E/nB7BQzlwjgGuclDDjLe5+g==', CAST(0x0000A3000068D0A4 AS DateTime), N'', NULL, NULL)
SET IDENTITY_INSERT [dbo].[webpages_Roles] ON 

INSERT [dbo].[webpages_Roles] ([RoleId], [RoleName]) VALUES (1, N'Admin')
SET IDENTITY_INSERT [dbo].[webpages_Roles] OFF
INSERT [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (1, 1)
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__webpages__8A2B616015502E78]    Script Date: 04/30/2014 10:14:47 AM ******/
ALTER TABLE [dbo].[webpages_Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserSecurity] ADD  CONSTRAINT [DF_UserSecurity_Islocked]  DEFAULT ((0)) FOR [Islocked]
GO
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [IsConfirmed]
GO
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [PasswordFailuresSinceLastSuccess]
GO
ALTER TABLE [dbo].[CustomerDetails]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CustomerDetails] ([CustomerId])
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CustomerDetails] ([CustomerId])
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[UserDetails]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[UserSecurity]  WITH CHECK ADD  CONSTRAINT [FK__UserSecur__UserI__1B0907CE] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[UserSecurity] CHECK CONSTRAINT [FK__UserSecur__UserI__1B0907CE]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_RoleId]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_UserId]
GO
USE [master]
GO
ALTER DATABASE [BingHousingDbase] SET  READ_WRITE 
GO
