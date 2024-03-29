USE [master]
GO
/****** Object:  Database [EMS]    Script Date: 2020-02-24 2:35:04 PM ******/
CREATE DATABASE [EMS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EMS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\EMS.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'EMS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\EMS_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [EMS] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EMS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EMS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EMS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EMS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EMS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EMS] SET ARITHABORT OFF 
GO
ALTER DATABASE [EMS] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EMS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EMS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EMS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EMS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EMS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EMS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EMS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EMS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EMS] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EMS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EMS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EMS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EMS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EMS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EMS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EMS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EMS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EMS] SET  MULTI_USER 
GO
ALTER DATABASE [EMS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EMS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EMS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EMS] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [EMS] SET DELAYED_DURABILITY = DISABLED 
GO
USE [EMS]
GO
USE [EMS]
GO
/****** Object:  Sequence [dbo].[seq_INV_INFO_INV_ID]    Script Date: 2020-02-24 2:35:04 PM ******/
CREATE SEQUENCE [dbo].[seq_INV_INFO_INV_ID] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
USE [EMS]
GO
/****** Object:  Sequence [dbo].[seqForItemInfoItemId]    Script Date: 2020-02-24 2:35:04 PM ******/
CREATE SEQUENCE [dbo].[seqForItemInfoItemId] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
/****** Object:  Table [dbo].[ASSET_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ASSET_INFO](
	[ASSET_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ASSET_TITLE] [nvarchar](50) NOT NULL,
	[REMARKS] [nvarchar](50) NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_ASSET_INFO] PRIMARY KEY CLUSTERED 
(
	[ASSET_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ASSET_MANAGEMENT]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ASSET_MANAGEMENT](
	[ASSET_MNG_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EMP_ID] [bigint] NOT NULL,
	[EQUP_ID] [bigint] NOT NULL,
	[DATE_ASSN] [nvarchar](50) NOT NULL,
	[DATE_RTN] [nvarchar](50) NULL,
	[REMARKS] [nvarchar](50) NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_ASSET_ASSIGNMENT] PRIMARY KEY CLUSTERED 
(
	[ASSET_MNG_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ATTENDANCE_DETAILS]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ATTENDANCE_DETAILS](
	[ATNDNC_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EMPLOYEE_ID] [bigint] NOT NULL,
	[ATT_DATE] [date] NOT NULL,
	[CHECK_IN_TIME] [time](0) NULL,
	[CHECK_OUT_TIME] [time](0) NULL,
	[SL_NO] [int] NULL,
	[STATUS] [char](1) NULL,
 CONSTRAINT [PK_ATTENDANCE_DETAILS] PRIMARY KEY CLUSTERED 
(
	[ATNDNC_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ATTENDANCE_DETAILS_MONTHLY]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ATTENDANCE_DETAILS_MONTHLY](
	[ATTN_MONTHLY_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EMP_ID] [bigint] NOT NULL,
	[DATE] [date] NOT NULL,
	[CHECK_IN] [time](7) NULL,
	[CHECK_OUT] [time](7) NULL,
	[WORKING_HOUR] [time](7) NULL,
 CONSTRAINT [PK_ATTENDANCE_DETAILS_MONTHLY] PRIMARY KEY CLUSTERED 
(
	[ATTN_MONTHLY_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CARD_ASSIGN_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CARD_ASSIGN_INFO](
	[CARD_ASSGN_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EMP_ID] [bigint] NOT NULL,
	[CARD_NO] [nchar](10) NOT NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_CARD_ASSIGN_INFO] PRIMARY KEY CLUSTERED 
(
	[CARD_ASSGN_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DEPARTMENT_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DEPARTMENT_INFO](
	[DEPT_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DEPT_TITLE] [nvarchar](50) NOT NULL,
	[REMARKS] [nvarchar](50) NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_DEPARTMENT_INFO] PRIMARY KEY CLUSTERED 
(
	[DEPT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DIVISION_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DIVISION_INFO](
	[DIV_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DIV_TITLE] [nvarchar](50) NOT NULL,
	[DEPT_ID] [bigint] NOT NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_DIVISION_INFO] PRIMARY KEY CLUSTERED 
(
	[DIV_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EMPLOYEE_APPLICATION]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EMPLOYEE_APPLICATION](
	[APP_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[APP_SUB] [nvarchar](100) NOT NULL,
	[APP_BODY] [nvarchar](1000) NOT NULL,
	[EMP_ID] [bigint] NOT NULL,
 CONSTRAINT [PK_EMPLOYEE_APPLICATION] PRIMARY KEY CLUSTERED 
(
	[APP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EMPLOYEE_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EMPLOYEE_INFO](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EMPLOYEE_NAME] [nvarchar](50) NOT NULL,
	[ADDRESS] [nvarchar](100) NULL,
	[CONTACT] [varchar](50) NULL,
	[EMAIL] [varchar](50) NULL,
	[NID] [nvarchar](50) NULL,
	[IMAGE] [nvarchar](500) NULL,
	[CITY] [nvarchar](50) NULL,
	[POSTAL_CODE] [nvarchar](50) NULL,
	[DOB] [nvarchar](50) NOT NULL,
	[GENDER] [char](1) NOT NULL,
	[BLOOD_GROUP] [char](3) NULL,
	[IS_DELETED] [char](1) NOT NULL,
	[JOINING_DATE] [nvarchar](50) NULL,
	[RESIGNING_DATE] [nvarchar](50) NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
	[MARITALA_STATUS] [char](1) NULL,
	[NATIONALITY] [nvarchar](50) NULL,
 CONSTRAINT [PK_EMPLOYEE_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EQUEPMENTS_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EQUEPMENTS_INFO](
	[EQUEPMENT_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EQP_TITLE] [nvarchar](50) NOT NULL,
	[ASSET_ID] [bigint] NOT NULL,
	[MODEL_NO] [nvarchar](50) NULL,
	[SERIAL_NO] [nvarchar](50) NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
	[ASSET_FOR] [char](1) NULL,
 CONSTRAINT [PK_EQUEPMENTS_INFO] PRIMARY KEY CLUSTERED 
(
	[EQUEPMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[INV_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[INV_INFO](
	[INV_ID] [bigint] NOT NULL,
	[EQP_ID] [bigint] NOT NULL,
	[VENDOR_ID] [bigint] NOT NULL,
	[UNIT] [int] NOT NULL,
	[DATE] [nvarchar](50) NOT NULL,
	[STOCK_TYPE] [char](1) NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_ITEM_INFO] PRIMARY KEY CLUSTERED 
(
	[INV_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LEAVE_APPLICATION]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LEAVE_APPLICATION](
	[LEAVE_APP_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LEAVE_TYPE_ID] [bigint] NOT NULL,
	[EMPLOYEE_ID] [bigint] NOT NULL,
	[START_DATE] [nvarchar](50) NOT NULL,
	[END_DATE] [nvarchar](50) NULL,
	[APPROVED_START_DATE] [nvarchar](50) NULL,
	[APPROVED_END_DATE] [nvarchar](50) NULL,
	[STATUS] [char](1) NOT NULL,
	[REMARKS] [nvarchar](50) NULL,
	[ACTIVE_BY] [bigint] NOT NULL,
	[ACTIVE_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_LEAVE_APPLICATION] PRIMARY KEY CLUSTERED 
(
	[LEAVE_APP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LEAVE_TYPE]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LEAVE_TYPE](
	[LEAVE_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LEAVE_TITLE] [nvarchar](50) NOT NULL,
	[REMARKS] [nvarchar](500) NULL,
	[ACTIVE_BY] [bigint] NOT NULL,
	[ACTIVE_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_LEAVE_TYPE] PRIMARY KEY CLUSTERED 
(
	[LEAVE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NOTICE_BOARD]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NOTICE_BOARD](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[NOTICE] [nvarchar](1000) NOT NULL,
	[DEPT_ID] [bigint] NULL,
	[DIV_ID] [bigint] NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
	[STATUS] [char](1) NOT NULL,
 CONSTRAINT [PK_NOTICE_BOARD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[POSITIONAL_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[POSITIONAL_INFO](
	[POSITION_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[POSITION_TITLE] [nvarchar](50) NOT NULL,
	[EMPLOYEE_ID] [bigint] NOT NULL,
	[BASIC_SALARY] [decimal](18, 2) NOT NULL,
	[DUTY_TYPE] [char](1) NOT NULL,
	[RATE_TYPE] [char](1) NOT NULL,
	[PAY_FREQ] [char](1) NOT NULL,
	[DIV_ID] [bigint] NOT NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
	[CHANGE_TYPE] [char](1) NOT NULL,
 CONSTRAINT [PK_POSITIONAL_INFO] PRIMARY KEY CLUSTERED 
(
	[POSITION_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SALARY_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SALARY_INFO](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EMPLOYEE_ID] [bigint] NOT NULL,
	[GROSS_SALARY] [decimal](18, 2) NOT NULL,
	[BONUS] [decimal](18, 2) NULL,
	[OTHERS] [decimal](18, 2) NULL,
	[SALARY_PAID] [nvarchar](50) NOT NULL,
	[TOTAL] [decimal](18, 2) NULL,
	[STATUS] [char](1) NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
	[REMARKS] [nvarchar](50) NULL,
 CONSTRAINT [PK_SALARY_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SALARY_SETUP]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SALARY_SETUP](
	[SALARY_SET_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EMP_ID] [bigint] NOT NULL,
	[PAY_TYPE] [char](1) NOT NULL,
	[GROSS_SALARY] [decimal](18, 2) NOT NULL,
	[SALARY_GRADE_SETUP] [nvarchar](500) NOT NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
	[CANGE_TYPE] [char](1) NULL,
	[POSITION_ID] [bigint] NULL,
 CONSTRAINT [PK_SALARY_SETUP] PRIMARY KEY CLUSTERED 
(
	[SALARY_SET_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SALARY-GRADE]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SALARY-GRADE](
	[GRADE_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[GRADE_TITLE] [nvarchar](50) NOT NULL,
	[GRADE_TYPE] [char](1) NOT NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_SALARY-GRADE] PRIMARY KEY CLUSTERED 
(
	[GRADE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[STOCK_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[STOCK_INFO](
	[STOCK_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EQP_ID] [bigint] NOT NULL,
	[UNIT] [int] NOT NULL,
	[STOCK_TYPE] [char](1) NOT NULL,
	[STOCK_FOR] [char](1) NOT NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
	[INV_ID] [bigint] NOT NULL,
 CONSTRAINT [PK_STOCK_INFO] PRIMARY KEY CLUSTERED 
(
	[STOCK_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TEAM_DETAILS]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TEAM_DETAILS](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TEAM_ID] [bigint] NOT NULL,
	[MEMBER] [bigint] NOT NULL,
	[REMARKS] [nvarchar](50) NULL,
	[STATUS] [char](1) NOT NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_TEAM_DETAILS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TEAM_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TEAM_INFO](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TEAM_TITLE] [nvarchar](50) NOT NULL,
	[TEAM_LEADER] [bigint] NOT NULL,
	[REMARKS] [nvarchar](50) NULL,
	[STATUS] [char](1) NOT NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_TEAM] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TRANSACTION_ITEM]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TRANSACTION_ITEM](
	[TRNS_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TRNS_TITLE] [nvarchar](50) NOT NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
	[TYPE] [char](1) NOT NULL,
 CONSTRAINT [PK_EXPENSES_ITEM] PRIMARY KEY CLUSTERED 
(
	[TRNS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TRANSACTION_SHEET]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TRANSACTION_SHEET](
	[TRNS_S_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DATE] [nvarchar](50) NOT NULL,
	[TRNS_ID] [bigint] NOT NULL,
	[AMOUNT] [decimal](18, 2) NOT NULL,
	[PAY_TYPE] [char](1) NOT NULL,
	[VOUCHER_NO] [nvarchar](50) NULL,
	[TYPE] [char](1) NULL,
	[REMARKS] [nvarchar](50) NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
 CONSTRAINT [PK_EXPENSES_SHEET] PRIMARY KEY CLUSTERED 
(
	[TRNS_S_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USER_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USER_INFO](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EMPLOYEE_ID] [bigint] NULL,
	[USER_NAME] [nvarchar](50) NOT NULL,
	[CONTACT] [varchar](50) NULL,
	[USER_ID] [nvarchar](50) NOT NULL,
	[PASSWORD] [nvarchar](50) NOT NULL,
	[USER_LEVEL] [char](1) NOT NULL,
	[IS_DELETED] [char](1) NULL,
	[ACTION_DATE] [datetime] NULL,
	[ACTION_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
	[UPDATE_BY] [bigint] NULL,
 CONSTRAINT [PK_USER_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VENDOR_INFO]    Script Date: 2020-02-24 2:35:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VENDOR_INFO](
	[VENDOR_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[VENDOR_NAME] [nvarchar](50) NOT NULL,
	[ADDRESS] [nvarchar](100) NOT NULL,
	[CONTACT] [nvarchar](50) NOT NULL,
	[VENDOR_TYPE] [char](1) NOT NULL,
	[ACTION_BY] [bigint] NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
	[UPDATE_BY] [bigint] NULL,
	[UPDATE_DATE] [datetime] NULL,
	[STATUS] [char](1) NOT NULL,
 CONSTRAINT [PK_VENDOR_INFO] PRIMARY KEY CLUSTERED 
(
	[VENDOR_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[ASSET_MANAGEMENT]  WITH CHECK ADD  CONSTRAINT [FK_ASSET_ASSIGNMENT_EMPLOYEE_INFO] FOREIGN KEY([EMP_ID])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[ASSET_MANAGEMENT] CHECK CONSTRAINT [FK_ASSET_ASSIGNMENT_EMPLOYEE_INFO]
GO
ALTER TABLE [dbo].[ASSET_MANAGEMENT]  WITH CHECK ADD  CONSTRAINT [FK_ASSET_MANAGEMENT_EQUEPMENTS_INFO] FOREIGN KEY([EQUP_ID])
REFERENCES [dbo].[EQUEPMENTS_INFO] ([EQUEPMENT_ID])
GO
ALTER TABLE [dbo].[ASSET_MANAGEMENT] CHECK CONSTRAINT [FK_ASSET_MANAGEMENT_EQUEPMENTS_INFO]
GO
ALTER TABLE [dbo].[ATTENDANCE_DETAILS]  WITH NOCHECK ADD  CONSTRAINT [FK_ATTENDANCE_DETAILS_EMPLOYEE_INFO] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[ATTENDANCE_DETAILS] CHECK CONSTRAINT [FK_ATTENDANCE_DETAILS_EMPLOYEE_INFO]
GO
ALTER TABLE [dbo].[ATTENDANCE_DETAILS_MONTHLY]  WITH CHECK ADD  CONSTRAINT [FK_ATTENDANCE_DETAILS_MONTHLY_EMPLOYEE_INFO] FOREIGN KEY([EMP_ID])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[ATTENDANCE_DETAILS_MONTHLY] CHECK CONSTRAINT [FK_ATTENDANCE_DETAILS_MONTHLY_EMPLOYEE_INFO]
GO
ALTER TABLE [dbo].[CARD_ASSIGN_INFO]  WITH CHECK ADD  CONSTRAINT [FK_CARD_ASSIGN_INFO_EMPLOYEE_INFO] FOREIGN KEY([EMP_ID])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[CARD_ASSIGN_INFO] CHECK CONSTRAINT [FK_CARD_ASSIGN_INFO_EMPLOYEE_INFO]
GO
ALTER TABLE [dbo].[DIVISION_INFO]  WITH CHECK ADD  CONSTRAINT [FK_DIVISION_INFO_DEPARTMENT_INFO] FOREIGN KEY([DEPT_ID])
REFERENCES [dbo].[DEPARTMENT_INFO] ([DEPT_ID])
GO
ALTER TABLE [dbo].[DIVISION_INFO] CHECK CONSTRAINT [FK_DIVISION_INFO_DEPARTMENT_INFO]
GO
ALTER TABLE [dbo].[EMPLOYEE_APPLICATION]  WITH CHECK ADD  CONSTRAINT [FK_EMPLOYEE_APPLICATION_EMPLOYEE_INFO] FOREIGN KEY([APP_ID])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[EMPLOYEE_APPLICATION] CHECK CONSTRAINT [FK_EMPLOYEE_APPLICATION_EMPLOYEE_INFO]
GO
ALTER TABLE [dbo].[EQUEPMENTS_INFO]  WITH CHECK ADD  CONSTRAINT [FK_EQUEPMENTS_INFO_EQUEPMENTS_INFO] FOREIGN KEY([ASSET_ID])
REFERENCES [dbo].[ASSET_INFO] ([ASSET_ID])
GO
ALTER TABLE [dbo].[EQUEPMENTS_INFO] CHECK CONSTRAINT [FK_EQUEPMENTS_INFO_EQUEPMENTS_INFO]
GO
ALTER TABLE [dbo].[INV_INFO]  WITH CHECK ADD  CONSTRAINT [FK_INV_INFO_INV_INFO] FOREIGN KEY([EQP_ID])
REFERENCES [dbo].[EQUEPMENTS_INFO] ([EQUEPMENT_ID])
GO
ALTER TABLE [dbo].[INV_INFO] CHECK CONSTRAINT [FK_INV_INFO_INV_INFO]
GO
ALTER TABLE [dbo].[INV_INFO]  WITH CHECK ADD  CONSTRAINT [FK_INV_INFO_VENDOR_INFO] FOREIGN KEY([VENDOR_ID])
REFERENCES [dbo].[VENDOR_INFO] ([VENDOR_ID])
GO
ALTER TABLE [dbo].[INV_INFO] CHECK CONSTRAINT [FK_INV_INFO_VENDOR_INFO]
GO
ALTER TABLE [dbo].[LEAVE_APPLICATION]  WITH CHECK ADD  CONSTRAINT [FK_LEAVE_APPLICATION_EMPLOYEE_INFO] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[LEAVE_APPLICATION] CHECK CONSTRAINT [FK_LEAVE_APPLICATION_EMPLOYEE_INFO]
GO
ALTER TABLE [dbo].[LEAVE_APPLICATION]  WITH CHECK ADD  CONSTRAINT [FK_LEAVE_APPLICATION_LEAVE_APPLICATION] FOREIGN KEY([LEAVE_TYPE_ID])
REFERENCES [dbo].[LEAVE_TYPE] ([LEAVE_ID])
GO
ALTER TABLE [dbo].[LEAVE_APPLICATION] CHECK CONSTRAINT [FK_LEAVE_APPLICATION_LEAVE_APPLICATION]
GO
ALTER TABLE [dbo].[POSITIONAL_INFO]  WITH CHECK ADD  CONSTRAINT [FK_POSITIONAL_INFO_DIVISION_INFO] FOREIGN KEY([DIV_ID])
REFERENCES [dbo].[DIVISION_INFO] ([DIV_ID])
GO
ALTER TABLE [dbo].[POSITIONAL_INFO] CHECK CONSTRAINT [FK_POSITIONAL_INFO_DIVISION_INFO]
GO
ALTER TABLE [dbo].[POSITIONAL_INFO]  WITH CHECK ADD  CONSTRAINT [FK_POSITIONAL_INFO_EMPLOYEE_INFO] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[POSITIONAL_INFO] CHECK CONSTRAINT [FK_POSITIONAL_INFO_EMPLOYEE_INFO]
GO
ALTER TABLE [dbo].[SALARY_INFO]  WITH CHECK ADD  CONSTRAINT [FK_SALARY_INFO_EMPLOYEE_INFO] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[SALARY_INFO] CHECK CONSTRAINT [FK_SALARY_INFO_EMPLOYEE_INFO]
GO
ALTER TABLE [dbo].[SALARY_SETUP]  WITH CHECK ADD  CONSTRAINT [FK_SALARY_SETUP_EMPLOYEE_INFO] FOREIGN KEY([EMP_ID])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[SALARY_SETUP] CHECK CONSTRAINT [FK_SALARY_SETUP_EMPLOYEE_INFO]
GO
ALTER TABLE [dbo].[SALARY_SETUP]  WITH CHECK ADD  CONSTRAINT [FK_SALARY_SETUP_POSITIONAL_INFO] FOREIGN KEY([POSITION_ID])
REFERENCES [dbo].[POSITIONAL_INFO] ([POSITION_ID])
GO
ALTER TABLE [dbo].[SALARY_SETUP] CHECK CONSTRAINT [FK_SALARY_SETUP_POSITIONAL_INFO]
GO
ALTER TABLE [dbo].[STOCK_INFO]  WITH CHECK ADD  CONSTRAINT [FK_STOCK_INFO_EQUEPMENTS_INFO] FOREIGN KEY([EQP_ID])
REFERENCES [dbo].[EQUEPMENTS_INFO] ([EQUEPMENT_ID])
GO
ALTER TABLE [dbo].[STOCK_INFO] CHECK CONSTRAINT [FK_STOCK_INFO_EQUEPMENTS_INFO]
GO
ALTER TABLE [dbo].[STOCK_INFO]  WITH CHECK ADD  CONSTRAINT [FK_STOCK_INFO_INV_INFO] FOREIGN KEY([INV_ID])
REFERENCES [dbo].[INV_INFO] ([INV_ID])
GO
ALTER TABLE [dbo].[STOCK_INFO] CHECK CONSTRAINT [FK_STOCK_INFO_INV_INFO]
GO
ALTER TABLE [dbo].[TEAM_DETAILS]  WITH CHECK ADD  CONSTRAINT [FK_TEAM_DETAILS_EMPLOYEE_INFO] FOREIGN KEY([MEMBER])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[TEAM_DETAILS] CHECK CONSTRAINT [FK_TEAM_DETAILS_EMPLOYEE_INFO]
GO
ALTER TABLE [dbo].[TEAM_DETAILS]  WITH CHECK ADD  CONSTRAINT [FK_TEAM_DETAILS_TEAM] FOREIGN KEY([TEAM_ID])
REFERENCES [dbo].[TEAM_INFO] ([ID])
GO
ALTER TABLE [dbo].[TEAM_DETAILS] CHECK CONSTRAINT [FK_TEAM_DETAILS_TEAM]
GO
ALTER TABLE [dbo].[TEAM_INFO]  WITH CHECK ADD  CONSTRAINT [FK_TEAM_EMPLOYEE_INFO] FOREIGN KEY([TEAM_LEADER])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[TEAM_INFO] CHECK CONSTRAINT [FK_TEAM_EMPLOYEE_INFO]
GO
ALTER TABLE [dbo].[TRANSACTION_SHEET]  WITH CHECK ADD  CONSTRAINT [FK_TRANSACTION_SHEET_TRANSACTION_ITEM] FOREIGN KEY([TRNS_ID])
REFERENCES [dbo].[TRANSACTION_ITEM] ([TRNS_ID])
GO
ALTER TABLE [dbo].[TRANSACTION_SHEET] CHECK CONSTRAINT [FK_TRANSACTION_SHEET_TRANSACTION_ITEM]
GO
ALTER TABLE [dbo].[USER_INFO]  WITH CHECK ADD  CONSTRAINT [FK_USER_INFO_EMPLOYEE_INFO] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[EMPLOYEE_INFO] ([ID])
GO
ALTER TABLE [dbo].[USER_INFO] CHECK CONSTRAINT [FK_USER_INFO_EMPLOYEE_INFO]
GO
USE [master]
GO
ALTER DATABASE [EMS] SET  READ_WRITE 
GO
