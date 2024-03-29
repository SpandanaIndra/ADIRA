USE [master]
GO
/****** Object:  Database [Adira]    Script Date: 13-02-2024 10:58:24 AM ******/
CREATE DATABASE [Adira]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Adira', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Adira.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Adira_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Adira_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Adira] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Adira].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Adira] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Adira] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Adira] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Adira] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Adira] SET ARITHABORT OFF 
GO
ALTER DATABASE [Adira] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Adira] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Adira] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Adira] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Adira] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Adira] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Adira] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Adira] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Adira] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Adira] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Adira] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Adira] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Adira] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Adira] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Adira] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Adira] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Adira] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Adira] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Adira] SET  MULTI_USER 
GO
ALTER DATABASE [Adira] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Adira] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Adira] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Adira] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Adira] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Adira] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Adira] SET QUERY_STORE = ON
GO
ALTER DATABASE [Adira] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Adira]
GO
/****** Object:  Table [dbo].[Department_L]    Script Date: 13-02-2024 10:58:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department_L](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
	[Description] [varchar](500) NULL,
	[CreatedDate] [date] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[InActiveDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 13-02-2024 10:58:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeId] [varchar](50) NOT NULL,
	[UserId] [int] NULL,
	[Name] [varchar](500) NOT NULL,
	[Email] [varchar](500) NOT NULL,
	[IsActive] [bit] NULL,
	[RoleId] [int] NULL,
	[EntityId] [int] NULL,
	[DepartmentId] [int] NULL,
	[Location] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entity_L]    Script Date: 13-02-2024 10:58:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entity_L](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
	[Description] [varchar](500) NULL,
	[CreatedDate] [date] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[InActiveDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 13-02-2024 10:58:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[IsActive] [bit] NULL,
	[Code] [varchar](50) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SecretSantaData]    Script Date: 13-02-2024 10:58:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecretSantaData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [varchar](50) NOT NULL,
	[SecretSantaEmployeeId] [varchar](50) NOT NULL,
	[CreatedBy] [varchar](20) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [varchar](20) NULL,
	[UpdatedDate] [datetime] NULL,
	[EmailSent] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 13-02-2024 10:58:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](500) NOT NULL,
	[Password] [varchar](500) NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LoginAttempts] [int] NULL,
	[IsBlocked] [bit] NULL,
	[BlockExpirationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Department_L] ON 

INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (1, N'Management', N'This is a sample department', CAST(N'2024-01-17' AS Date), 1, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (2, N'Admin & Facility', N'New entry', CAST(N'2024-01-17' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (5, N'Contracts', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (6, N'Bench Sales', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (7, N'Systems/Network', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (8, N'MIS', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (9, N'India HR', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (10, N'US Finance', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (11, N'US IT Recruitment', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (12, N'Digital Marketing', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (13, N'Learning & Development', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (14, N'Immigration', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (15, N'US HR', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (16, N'Information Technology', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (17, N'OPT', N'New entry', CAST(N'2024-01-22' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (18, N'HR', N'New entry', CAST(N'2024-01-23' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (19, N'IT', N'New entry', CAST(N'2024-01-23' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Department_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (20, N'SalesForce', N'New entry', CAST(N'2024-01-23' AS Date), NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Department_L] OFF
GO
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD008', NULL, N'Santhamma Malikireddy', N'sant.malikireddy@antra.com', 1, 2, 1, 1, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD023', NULL, N'Shrikanth Varma', N'srikanth.varma@antra.com', 1, 2, 1, 2, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD096', NULL, N'Pratyush Banala', N'pratyush.banala@antra.com', 1, 2, 1, 5, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD105', NULL, N'Mohsin Pathan', N'mohsin.khan@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD128', NULL, N'Saichander Gundu', N'sai.chander@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD139', NULL, N'Uday Reddy', N'udaykanth.reddy@antra.com', 1, 2, 1, 7, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD142', NULL, N'Mohammed Basith Abdul', N'basith.abdul@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD159', NULL, N'Ishan Jain', N'ishan.jain@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD184', NULL, N'Rekha Komath', N'rekha.komath@antra.com', 1, 2, 1, 8, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD218', NULL, N'Abhilash Chidara', N'abhilash.chidara@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD229', NULL, N'Sajitha Nair', N'sajitha.nair@antra.com', 1, 2, 1, 9, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD231', NULL, N'Shaik Eliyaz', N'shaik.eliyaz@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD242', NULL, N'Chakravarthy Saya', N'chakravarthy.saya@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD246', NULL, N'Ravender Udanda', N'ravender.udanda@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD250', NULL, N'Dhruva Madupalli', N'dhruva.madupalli@antra.com', 1, 2, 1, 10, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD254', NULL, N'Bodepudi Kiran', N'sasi.kiran@antra.com', 1, 2, 1, 7, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD260', NULL, N'Yogish Kosuru', N'yogish.kosuru@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD263', NULL, N'Sandeep Gandu', N'sandeep.gandu@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD264', NULL, N'Harish Boora', N'harish.boora@antra.com', 1, 2, 1, 7, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD267', NULL, N'Chandana Duvvuri', N'chandana.duvvuri@techwish.com', 1, 2, 1, 11, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD273', NULL, N'Hanumantha Rao Yerragunta', N'hanu.rao@antra.com', 1, 2, 1, 10, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD274', NULL, N'Mohit Lath', N'mohit.lath@antra.com', 1, 2, 1, 9, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD278', NULL, N'Anoop Kasini', N'anoop.kasini@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD280', NULL, N'Vivek Chandaluri', N'vivek.chandaluri@antra.com', 1, 2, 1, 12, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD290', NULL, N'Saneeth Nevuri', N'saneeth.nevuri@antra.com', 1, 2, 1, 2, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD321', NULL, N'Saravanan Baluchamy', N'saran.balu@techwish.com', 1, 2, 1, 11, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD333', NULL, N'Arokiaraj Mahimaidass', N'alex.mahimaidass@techwish.com', 1, 2, 1, 11, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD360', NULL, N'Sri Haril Thungala', N'sriharil.thungala@antra.com', 1, 2, 1, 13, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD366', NULL, N'Vutturi Teja', N'raviteja.vutturi@antra.com', 1, 2, 1, 10, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD367', NULL, N'Bhaskar Reddy Vanteru', N'bhaskar.vanteru@antra.com', 1, 2, 1, 14, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD368', NULL, N'Kyatham Reddy', N'maheshwar.kyatham@antra.com', 1, 2, 1, 10, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD378', NULL, N'Rama Krishna Arapalli', N'ramakrishna.arapalli@antra.com', 1, 2, 1, 5, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD387', NULL, N'Geddadi Lalitha', N'lalitha.geddadi@antra.com', 1, 2, 1, 14, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD396', NULL, N'Srikanth Mogulla', N'srikanth.mogulla@antra.com', 1, 2, 1, 15, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD412', NULL, N'Sanjay Banerjee', N'sanjay.banerjee@techwish.com', 1, 2, 1, 11, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD413', NULL, N'Anjibabu Kari', N'anjibabu.kari@antra.com', 1, 2, 1, 16, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD414', NULL, N'Narendar Gentyala', N'narendar.gentyala@antra.com', 1, 2, 1, 16, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD416', NULL, N'Nagendra Karn', N'nagendra.karn@techwish.com', 1, 2, 1, 11, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD422', NULL, N'Sravan Chada', N'sravan.chada@antra.com', 1, 2, 1, 10, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD433', NULL, N'Yagudum Reddy', N'pavan.yagudum@techwish.com', 1, 2, 1, 10, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD439', NULL, N'Gandugude Swathi', N'swathi.gandugude@antra.com', 1, 2, 1, 6, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD446', NULL, N'Harish Nuthalapati', N'harish.nuthalapati@antra.com', 1, 2, 1, 16, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD450', NULL, N'Venkatesh Peethala', N'venkatesh.peethala@antra.com', 1, 2, 1, 12, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD453', NULL, N'Mohammad Sameer Ahmad', N'sameer.ahmad@antra.com', 1, 2, 1, 16, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD463', NULL, N'Raja Maddala', N'raja.maddala@techwish.com', 1, 2, 1, 8, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD477', NULL, N'Suparna Mandal', N'suparna.mandal@antra.com', 1, 2, 1, 17, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD482', NULL, N'Renuka Ratkal', N'renuka.ratkal@antra.com', 1, 2, 1, 17, N'Hyderabad')
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD485', NULL, N'srinivas reddy', N'sreenivasu.sangabathula@antra.com', 1, 2, 1, 16, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD486', NULL, N'Shalini Nampally', N'shalini.nampally@antra.com', 1, 2, 1, 12, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'ANIS-HYD487', NULL, N'Sownya', N'sowmya.g@antra.com', 1, 2, 1, 17, NULL)
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'EMP001', 1, N'Ram', N'ram@example.com', 1, 1, 1, 1, N'Banglore')
INSERT [dbo].[Employee] ([EmployeeId], [UserId], [Name], [Email], [IsActive], [RoleId], [EntityId], [DepartmentId], [Location]) VALUES (N'EMP002', 2, N'Sita', N'sita@example.com', 1, 2, 1, 1, N'Banglore')
GO
SET IDENTITY_INSERT [dbo].[Entity_L] ON 

INSERT [dbo].[Entity_L] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [InActiveDate]) VALUES (1, N'Antra', N'This is a sample Entity', CAST(N'2024-01-17' AS Date), 1, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Entity_L] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([RoleId], [Name], [IsActive], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Administrator', 1, N'ADM', N'2319187a-aedd-4f37-9eb6-82a376015ae1', CAST(N'2024-01-10T15:09:40.137' AS DateTime), NULL, NULL)
INSERT [dbo].[Role] ([RoleId], [Name], [IsActive], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'User', 1, N'USR', N'b6794e61-b0cd-4a57-ad94-319ce0cdd734', CAST(N'2024-01-10T15:09:40.137' AS DateTime), NULL, NULL)
INSERT [dbo].[Role] ([RoleId], [Name], [IsActive], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'Manager', 0, N'MGR', N'e4da24f9-0cf4-4fe4-ac61-233e31f0eea2', CAST(N'2024-01-10T15:09:40.137' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[SecretSantaData] ON 

INSERT [dbo].[SecretSantaData] ([Id], [EmployeeId], [SecretSantaEmployeeId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [EmailSent]) VALUES (21, N'ANIS-HYD482', N'ANIS-HYD482', N'EMP001', CAST(N'2024-01-31T15:01:53.010' AS DateTime), NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[SecretSantaData] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [Username], [Password], [LastLoginDate], [LoginAttempts], [IsBlocked], [BlockExpirationDate]) VALUES (1, N'Ram', N'123', CAST(N'2024-01-10T15:17:03.180' AS DateTime), 0, 0, NULL)
INSERT [dbo].[Users] ([UserId], [Username], [Password], [LastLoginDate], [LoginAttempts], [IsBlocked], [BlockExpirationDate]) VALUES (2, N'Sita', N'123', CAST(N'2024-01-10T15:17:03.180' AS DateTime), 0, 0, NULL)
INSERT [dbo].[Users] ([UserId], [Username], [Password], [LastLoginDate], [LoginAttempts], [IsBlocked], [BlockExpirationDate]) VALUES (3, N'Karan', N'123', CAST(N'2024-01-10T15:17:03.180' AS DateTime), 0, 0, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [LastLoginDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [LoginAttempts]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsBlocked]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department_L] ([Id])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([EntityId])
REFERENCES [dbo].[Entity_L] ([Id])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[SecretSantaData]  WITH CHECK ADD FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[SecretSantaData]  WITH CHECK ADD FOREIGN KEY([SecretSantaEmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
USE [master]
GO
ALTER DATABASE [Adira] SET  READ_WRITE 
GO
