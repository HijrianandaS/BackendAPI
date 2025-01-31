USE [master]
GO
/****** Object:  Database [JWTAuthentication]    Script Date: 07/01/2025 11:16:08 ******/
CREATE DATABASE [JWTAuthentication]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'JWTAuthentication', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\JWTAuthentication.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'JWTAuthentication_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\JWTAuthentication_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [JWTAuthentication] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JWTAuthentication].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [JWTAuthentication] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [JWTAuthentication] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [JWTAuthentication] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [JWTAuthentication] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [JWTAuthentication] SET ARITHABORT OFF 
GO
ALTER DATABASE [JWTAuthentication] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [JWTAuthentication] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [JWTAuthentication] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [JWTAuthentication] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [JWTAuthentication] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [JWTAuthentication] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [JWTAuthentication] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [JWTAuthentication] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [JWTAuthentication] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [JWTAuthentication] SET  ENABLE_BROKER 
GO
ALTER DATABASE [JWTAuthentication] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [JWTAuthentication] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [JWTAuthentication] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [JWTAuthentication] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [JWTAuthentication] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [JWTAuthentication] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [JWTAuthentication] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [JWTAuthentication] SET RECOVERY FULL 
GO
ALTER DATABASE [JWTAuthentication] SET  MULTI_USER 
GO
ALTER DATABASE [JWTAuthentication] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [JWTAuthentication] SET DB_CHAINING OFF 
GO
ALTER DATABASE [JWTAuthentication] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [JWTAuthentication] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [JWTAuthentication] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [JWTAuthentication] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'JWTAuthentication', N'ON'
GO
ALTER DATABASE [JWTAuthentication] SET QUERY_STORE = OFF
GO
USE [JWTAuthentication]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 07/01/2025 11:16:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeID] [int] NOT NULL,
	[NationalIDNumber] [nvarchar](15) NOT NULL,
	[EmployeeName] [nvarchar](100) NULL,
	[LoginID] [nvarchar](256) NOT NULL,
	[JobTitle] [nvarchar](50) NOT NULL,
	[BirthDate] [date] NOT NULL,
	[MaritalStatus] [nchar](1) NOT NULL,
	[Gender] [nchar](1) NOT NULL,
	[HireDate] [date] NOT NULL,
	[VacationHours] [smallint] NOT NULL,
	[SickLeaveHours] [smallint] NOT NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 07/01/2025 11:16:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Completed] [bit] NOT NULL,
	[Time] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 07/01/2025 11:16:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInfo](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[DisplayName] [varchar](60) NOT NULL,
	[UserName] [varchar](30) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](20) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Employee] ([EmployeeID], [NationalIDNumber], [EmployeeName], [LoginID], [JobTitle], [BirthDate], [MaritalStatus], [Gender], [HireDate], [VacationHours], [SickLeaveHours], [rowguid], [ModifiedDate]) VALUES (1, N'295847284', N'Michael Westover', N'adventure-works\ken0', N'Vice President of Sales', CAST(N'1969-01-29' AS Date), N'S', N'M', CAST(N'2009-01-14' AS Date), 99, 69, N'f01251e5-96a3-448d-981e-0f99d789110d', CAST(N'2014-06-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Employee] ([EmployeeID], [NationalIDNumber], [EmployeeName], [LoginID], [JobTitle], [BirthDate], [MaritalStatus], [Gender], [HireDate], [VacationHours], [SickLeaveHours], [rowguid], [ModifiedDate]) VALUES (2, N'245797967', N'Raeann Santos', N'adventure-works\terri0', N'Vice President of Engineering', CAST(N'1971-08-01' AS Date), N'S', N'F', CAST(N'2008-01-31' AS Date), 1, 20, N'45e8f437-670d-4409-93cb-f9424a40d6ee', CAST(N'2014-06-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Employee] ([EmployeeID], [NationalIDNumber], [EmployeeName], [LoginID], [JobTitle], [BirthDate], [MaritalStatus], [Gender], [HireDate], [VacationHours], [SickLeaveHours], [rowguid], [ModifiedDate]) VALUES (3, N'509647174', N'Pamelas Wambsgans', N'adventure-works\roberto0', N'Engineering Manager', CAST(N'1974-11-12' AS Date), N'M', N'M', CAST(N'2007-11-11' AS Date), 2, 21, N'9bbbfb2c-efbb-4217-9ab7-f97689328841', CAST(N'2014-06-30T00:00:00.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Tasks] ON 

INSERT [dbo].[Tasks] ([Id], [Title], [Completed], [Time], [CreatedAt], [UpdatedAt]) VALUES (1044, N'Makan Siang', 1, N'12:00 PM', CAST(N'2024-12-12T11:52:25.740' AS DateTime), CAST(N'2024-12-24T10:56:33.507' AS DateTime))
INSERT [dbo].[Tasks] ([Id], [Title], [Completed], [Time], [CreatedAt], [UpdatedAt]) VALUES (1047, N'Solat Ashar', 1, N'4:00 PM', CAST(N'2024-12-30T15:31:34.307' AS DateTime), CAST(N'2025-01-03T14:30:24.800' AS DateTime))
INSERT [dbo].[Tasks] ([Id], [Title], [Completed], [Time], [CreatedAt], [UpdatedAt]) VALUES (1048, N'Makan -makan', 0, N'2:30 PM', CAST(N'2025-01-03T14:30:13.060' AS DateTime), CAST(N'2025-01-03T14:30:13.060' AS DateTime))
INSERT [dbo].[Tasks] ([Id], [Title], [Completed], [Time], [CreatedAt], [UpdatedAt]) VALUES (1049, N'Kerja lag', 0, N'1:30 PM', CAST(N'2025-01-06T14:24:24.870' AS DateTime), CAST(N'2025-01-06T17:23:26.117' AS DateTime))
SET IDENTITY_INSERT [dbo].[Tasks] OFF
GO
SET IDENTITY_INSERT [dbo].[UserInfo] ON 

INSERT [dbo].[UserInfo] ([UserId], [DisplayName], [UserName], [Email], [Password], [CreatedDate]) VALUES (1, N'AdminJago', N'Admin', N'admin@abc.com', N'$admin@2022', CAST(N'2022-01-17T14:47:58.207' AS DateTime))
INSERT [dbo].[UserInfo] ([UserId], [DisplayName], [UserName], [Email], [Password], [CreatedDate]) VALUES (2, N'John Doe', N'johndoe', N'johndoe@gmail.com', N'Password01', CAST(N'2024-12-01T10:30:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[UserInfo] OFF
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT ((0)) FOR [Completed]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
USE [master]
GO
ALTER DATABASE [JWTAuthentication] SET  READ_WRITE 
GO
