USE [master]
GO
/****** Object:  Database [CoffeeHouse]    Script Date: 31.03.2025 7:56:40 ******/
CREATE DATABASE [CoffeeHouse]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CoffeeHouse', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\CoffeeHouse.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CoffeeHouse_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\CoffeeHouse_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CoffeeHouse] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CoffeeHouse].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CoffeeHouse] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CoffeeHouse] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CoffeeHouse] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CoffeeHouse] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CoffeeHouse] SET ARITHABORT OFF 
GO
ALTER DATABASE [CoffeeHouse] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CoffeeHouse] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CoffeeHouse] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CoffeeHouse] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CoffeeHouse] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CoffeeHouse] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CoffeeHouse] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CoffeeHouse] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CoffeeHouse] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CoffeeHouse] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CoffeeHouse] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CoffeeHouse] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CoffeeHouse] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CoffeeHouse] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CoffeeHouse] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CoffeeHouse] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CoffeeHouse] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CoffeeHouse] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CoffeeHouse] SET  MULTI_USER 
GO
ALTER DATABASE [CoffeeHouse] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CoffeeHouse] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CoffeeHouse] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CoffeeHouse] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CoffeeHouse] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CoffeeHouse] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [CoffeeHouse] SET QUERY_STORE = ON
GO
ALTER DATABASE [CoffeeHouse] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CoffeeHouse]
GO
/****** Object:  Table [dbo].[Заказы]    Script Date: 31.03.2025 7:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Заказы](
	[IDЗаказа] [int] IDENTITY(1,1) NOT NULL,
	[ТипЗаказа] [nvarchar](50) NOT NULL,
	[Статус] [nvarchar](50) NOT NULL,
	[Номер_заказа] [nvarchar](50) NOT NULL,
	[ВремяСоздания] [date] NOT NULL,
	[ВремяЗавершения] [date] NULL,
	[ИтоговаяСумма] [decimal](18, 2) NULL,
	[СпособОплаты] [nvarchar](50) NULL,
	[КомментарийКлиента] [nvarchar](50) NULL,
	[IDСотрудника] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IDЗаказа] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ингредиенты]    Script Date: 31.03.2025 7:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ингредиенты](
	[IDИнгредиента] [int] IDENTITY(1,1) NOT NULL,
	[Наименование] [nvarchar](50) NOT NULL,
	[ЕдиницаИзмерения] [nvarchar](50) NOT NULL,
	[ТекущийОстаток] [nvarchar](50) NOT NULL,
	[МинимальныйЗапас] [nvarchar](50) NOT NULL,
	[СебестоимостьЗаЕдиницу] [nvarchar](50) NOT NULL,
	[Поставщик] [nvarchar](50) NULL,
	[СрокГодности] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[IDИнгредиента] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Меню]    Script Date: 31.03.2025 7:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Меню](
	[IDТовара] [int] IDENTITY(1,1) NOT NULL,
	[Название] [nvarchar](100) NOT NULL,
	[Категория] [nvarchar](50) NOT NULL,
	[Описание] [nvarchar](50) NULL,
	[Цена] [decimal](18, 2) NOT NULL,
	[Доступность] [nvarchar](50) NOT NULL,
	[ВремяПриготовления] [nvarchar](20) NULL,
	[ФотоТовара] [nvarchar](50) NULL,
	[ДатаДобавления] [date] NOT NULL,
	[ДатаИзменения] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IDТовара] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ОтчетПерсонала]    Script Date: 31.03.2025 7:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ОтчетПерсонала](
	[IDОтчета] [int] NOT NULL,
	[IDСотрудника] [int] NOT NULL,
	[Показатели] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IDОтчета] ASC,
	[IDСотрудника] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Отчеты]    Script Date: 31.03.2025 7:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Отчеты](
	[IDОтчета] [int] IDENTITY(1,1) NOT NULL,
	[ТипОтчета] [nvarchar](100) NOT NULL,
	[НачалоПериода] [nvarchar](100) NOT NULL,
	[КонецПериода] [nvarchar](100) NOT NULL,
	[ВремяФормирования] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IDОтчета] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Платежи]    Script Date: 31.03.2025 7:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Платежи](
	[IDПлатежа] [int] IDENTITY(1,1) NOT NULL,
	[IDЗаказа] [int] NOT NULL,
	[Сумма] [decimal](18, 2) NOT NULL,
	[СтатусПлатежа] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IDПлатежа] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ПозицииЗаказа]    Script Date: 31.03.2025 7:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ПозицииЗаказа](
	[IDПозиции] [int] IDENTITY(1,1) NOT NULL,
	[IDЗаказа] [int] NOT NULL,
	[IDТовара] [int] NOT NULL,
	[Количество] [nvarchar](50) NOT NULL,
	[Модификация] [nvarchar](50) NULL,
	[ЦенаНаМомент] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IDПозиции] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Роль]    Script Date: 31.03.2025 7:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Роль](
	[IDРоли] [int] IDENTITY(1,1) NOT NULL,
	[Роль] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IDРоли] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[СоставБлюда]    Script Date: 31.03.2025 7:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[СоставБлюда](
	[IDТовара] [int] NOT NULL,
	[IDИнгредиента] [int] NOT NULL,
	[Количество] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IDТовара] ASC,
	[IDИнгредиента] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Сотрудники]    Script Date: 31.03.2025 7:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Сотрудники](
	[IDСотрудника] [int] IDENTITY(1,1) NOT NULL,
	[ФИО] [nvarchar](255) NOT NULL,
	[Роль] [int] NOT NULL,
	[Логин] [nvarchar](255) NOT NULL,
	[ХешПароля] [nvarchar](255) NOT NULL,
	[ГрафикРаботы] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[IDСотрудника] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Заказы] ON 

INSERT [dbo].[Заказы] ([IDЗаказа], [ТипЗаказа], [Статус], [Номер_заказа], [ВремяСоздания], [ВремяЗавершения], [ИтоговаяСумма], [СпособОплаты], [КомментарийКлиента], [IDСотрудника]) VALUES (1, N'в заведении', N'завершен', N'124', CAST(N'2023-05-01' AS Date), NULL, CAST(410.00 AS Decimal(18, 2)), N'карта', N'Быстрее пожалуйста', 4)
INSERT [dbo].[Заказы] ([IDЗаказа], [ТипЗаказа], [Статус], [Номер_заказа], [ВремяСоздания], [ВремяЗавершения], [ИтоговаяСумма], [СпособОплаты], [КомментарийКлиента], [IDСотрудника]) VALUES (2, N'с собой', N'завершен', N'354', CAST(N'2023-05-02' AS Date), NULL, CAST(280.00 AS Decimal(18, 2)), N'наличные', NULL, 4)
INSERT [dbo].[Заказы] ([IDЗаказа], [ТипЗаказа], [Статус], [Номер_заказа], [ВремяСоздания], [ВремяЗавершения], [ИтоговаяСумма], [СпособОплаты], [КомментарийКлиента], [IDСотрудника]) VALUES (3, N'доставка', N'в работе', N'374', CAST(N'2023-05-03' AS Date), NULL, CAST(560.00 AS Decimal(18, 2)), N'онлайн', N'Без сахара', 2)
SET IDENTITY_INSERT [dbo].[Заказы] OFF
GO
SET IDENTITY_INSERT [dbo].[Ингредиенты] ON 

INSERT [dbo].[Ингредиенты] ([IDИнгредиента], [Наименование], [ЕдиницаИзмерения], [ТекущийОстаток], [МинимальныйЗапас], [СебестоимостьЗаЕдиницу], [Поставщик], [СрокГодности]) VALUES (1, N'Кофе арабика', N'граммы', N'5000', N'1000', N'0.5', N'Зёрна Премиум', N'365')
INSERT [dbo].[Ингредиенты] ([IDИнгредиента], [Наименование], [ЕдиницаИзмерения], [ТекущийОстаток], [МинимальныйЗапас], [СебестоимостьЗаЕдиницу], [Поставщик], [СрокГодности]) VALUES (2, N'Молоко 3.2%', N'миллилитры', N'20000', N'5000', N'0.1', N'Молочные продукты', N'5')
INSERT [dbo].[Ингредиенты] ([IDИнгредиента], [Наименование], [ЕдиницаИзмерения], [ТекущийОстаток], [МинимальныйЗапас], [СебестоимостьЗаЕдиницу], [Поставщик], [СрокГодности]) VALUES (3, N'Сахар', N'граммы', N'10000', N'2000', N'0.02', N'Сахарный завод', N'180')
INSERT [dbo].[Ингредиенты] ([IDИнгредиента], [Наименование], [ЕдиницаИзмерения], [ТекущийОстаток], [МинимальныйЗапас], [СебестоимостьЗаЕдиницу], [Поставщик], [СрокГодности]) VALUES (4, N'Шоколадный сироп', N'миллилитры', N'5000', N'1000', N'0.3', N'Десертные ингредиенты', N'90')
INSERT [dbo].[Ингредиенты] ([IDИнгредиента], [Наименование], [ЕдиницаИзмерения], [ТекущийОстаток], [МинимальныйЗапас], [СебестоимостьЗаЕдиницу], [Поставщик], [СрокГодности]) VALUES (5, N'Ваниль', N'граммы', N'500', N'100', N'1.2', N'Пряности и специи', N'365')
SET IDENTITY_INSERT [dbo].[Ингредиенты] OFF
GO
SET IDENTITY_INSERT [dbo].[Меню] ON 

INSERT [dbo].[Меню] ([IDТовара], [Название], [Категория], [Описание], [Цена], [Доступность], [ВремяПриготовления], [ФотоТовара], [ДатаДобавления], [ДатаИзменения]) VALUES (1, N'Эспрессо', N'кофе', N'Крепкий черный кофе', CAST(120.00 AS Decimal(18, 2)), N'доступен', N'2 мин', N'espresso.jpg', CAST(N'2023-01-01' AS Date), CAST(N'2023-01-01' AS Date))
INSERT [dbo].[Меню] ([IDТовара], [Название], [Категория], [Описание], [Цена], [Доступность], [ВремяПриготовления], [ФотоТовара], [ДатаДобавления], [ДатаИзменения]) VALUES (2, N'Американо', N'кофе', N'Черный кофе с водой', CAST(130.00 AS Decimal(18, 2)), N'доступен', N'3 мин', N'americano.jpg', CAST(N'2023-01-01' AS Date), CAST(N'2023-01-01' AS Date))
INSERT [dbo].[Меню] ([IDТовара], [Название], [Категория], [Описание], [Цена], [Доступность], [ВремяПриготовления], [ФотоТовара], [ДатаДобавления], [ДатаИзменения]) VALUES (3, N'Капучино', N'кофе', N'Кофе с молочной пеной', CAST(160.00 AS Decimal(18, 2)), N'доступен', N'4 мин', N'cappuccino.jpg', CAST(N'2023-01-01' AS Date), CAST(N'2023-01-01' AS Date))
INSERT [dbo].[Меню] ([IDТовара], [Название], [Категория], [Описание], [Цена], [Доступность], [ВремяПриготовления], [ФотоТовара], [ДатаДобавления], [ДатаИзменения]) VALUES (4, N'Латте', N'кофе', N'Кофе с молоком', CAST(180.00 AS Decimal(18, 2)), N'доступен', N'5 мин', N'latte.jpg', CAST(N'2023-01-01' AS Date), CAST(N'2023-01-01' AS Date))
INSERT [dbo].[Меню] ([IDТовара], [Название], [Категория], [Описание], [Цена], [Доступность], [ВремяПриготовления], [ФотоТовара], [ДатаДобавления], [ДатаИзменения]) VALUES (5, N'Чёрный чай', N'чай', N'Классический чёрный чай', CAST(100.00 AS Decimal(18, 2)), N'доступен', N'3 мин', N'black_tea.jpg', CAST(N'2023-01-01' AS Date), CAST(N'2023-01-01' AS Date))
INSERT [dbo].[Меню] ([IDТовара], [Название], [Категория], [Описание], [Цена], [Доступность], [ВремяПриготовления], [ФотоТовара], [ДатаДобавления], [ДатаИзменения]) VALUES (6, N'Тирамису', N'десерт', N'Итальянский десерт', CAST(250.00 AS Decimal(18, 2)), N'доступен', N'0 мин', N'tiramisu.jpg', CAST(N'2023-01-01' AS Date), CAST(N'2023-01-01' AS Date))
SET IDENTITY_INSERT [dbo].[Меню] OFF
GO
INSERT [dbo].[ОтчетПерсонала] ([IDОтчета], [IDСотрудника], [Показатели]) VALUES (1, 4, N'обслужено 5 клиентов')
INSERT [dbo].[ОтчетПерсонала] ([IDОтчета], [IDСотрудника], [Показатели]) VALUES (2, 3, N'расход кофе: 500г')
INSERT [dbo].[ОтчетПерсонала] ([IDОтчета], [IDСотрудника], [Показатели]) VALUES (3, 2, N'выручка: 12500.00')
GO
SET IDENTITY_INSERT [dbo].[Отчеты] ON 

INSERT [dbo].[Отчеты] ([IDОтчета], [ТипОтчета], [НачалоПериода], [КонецПериода], [ВремяФормирования]) VALUES (1, N'продажи', N'2023-05-01', N'2023-05-01', CAST(N'2023-05-02' AS Date))
INSERT [dbo].[Отчеты] ([IDОтчета], [ТипОтчета], [НачалоПериода], [КонецПериода], [ВремяФормирования]) VALUES (2, N'запасы', N'2023-05-01', N'2023-05-07', CAST(N'2023-05-08' AS Date))
INSERT [dbo].[Отчеты] ([IDОтчета], [ТипОтчета], [НачалоПериода], [КонецПериода], [ВремяФормирования]) VALUES (3, N'финансы', N'2023-05-01', N'2023-05-31', CAST(N'2023-06-01' AS Date))
SET IDENTITY_INSERT [dbo].[Отчеты] OFF
GO
SET IDENTITY_INSERT [dbo].[Платежи] ON 

INSERT [dbo].[Платежи] ([IDПлатежа], [IDЗаказа], [Сумма], [СтатусПлатежа]) VALUES (1, 1, CAST(410.00 AS Decimal(18, 2)), N'успешно')
INSERT [dbo].[Платежи] ([IDПлатежа], [IDЗаказа], [Сумма], [СтатусПлатежа]) VALUES (2, 2, CAST(280.00 AS Decimal(18, 2)), N'успешно')
INSERT [dbo].[Платежи] ([IDПлатежа], [IDЗаказа], [Сумма], [СтатусПлатежа]) VALUES (3, 3, CAST(560.00 AS Decimal(18, 2)), N'в обработке')
SET IDENTITY_INSERT [dbo].[Платежи] OFF
GO
SET IDENTITY_INSERT [dbo].[ПозицииЗаказа] ON 

INSERT [dbo].[ПозицииЗаказа] ([IDПозиции], [IDЗаказа], [IDТовара], [Количество], [Модификация], [ЦенаНаМомент]) VALUES (1, 1, 3, N'1', N'Дополнительная пенка', CAST(160.00 AS Decimal(18, 2)))
INSERT [dbo].[ПозицииЗаказа] ([IDПозиции], [IDЗаказа], [IDТовара], [Количество], [Модификация], [ЦенаНаМомент]) VALUES (2, 1, 5, N'1', N'С лимоном', CAST(100.00 AS Decimal(18, 2)))
INSERT [dbo].[ПозицииЗаказа] ([IDПозиции], [IDЗаказа], [IDТовара], [Количество], [Модификация], [ЦенаНаМомент]) VALUES (3, 1, 6, N'1', NULL, CAST(250.00 AS Decimal(18, 2)))
INSERT [dbo].[ПозицииЗаказа] ([IDПозиции], [IDЗаказа], [IDТовара], [Количество], [Модификация], [ЦенаНаМомент]) VALUES (4, 2, 1, N'2', NULL, CAST(120.00 AS Decimal(18, 2)))
INSERT [dbo].[ПозицииЗаказа] ([IDПозиции], [IDЗаказа], [IDТовара], [Количество], [Модификация], [ЦенаНаМомент]) VALUES (5, 3, 4, N'2', N'С сиропом', CAST(180.00 AS Decimal(18, 2)))
INSERT [dbo].[ПозицииЗаказа] ([IDПозиции], [IDЗаказа], [IDТовара], [Количество], [Модификация], [ЦенаНаМомент]) VALUES (6, 3, 6, N'1', NULL, CAST(250.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[ПозицииЗаказа] OFF
GO
SET IDENTITY_INSERT [dbo].[Роль] ON 

INSERT [dbo].[Роль] ([IDРоли], [Роль]) VALUES (1, N'Администратор')
INSERT [dbo].[Роль] ([IDРоли], [Роль]) VALUES (2, N'Менеджер')
INSERT [dbo].[Роль] ([IDРоли], [Роль]) VALUES (3, N'Бариста')
INSERT [dbo].[Роль] ([IDРоли], [Роль]) VALUES (4, N'Официант')
SET IDENTITY_INSERT [dbo].[Роль] OFF
GO
INSERT [dbo].[СоставБлюда] ([IDТовара], [IDИнгредиента], [Количество]) VALUES (1, 1, N'7')
INSERT [dbo].[СоставБлюда] ([IDТовара], [IDИнгредиента], [Количество]) VALUES (2, 1, N'7')
INSERT [dbo].[СоставБлюда] ([IDТовара], [IDИнгредиента], [Количество]) VALUES (2, 3, N'5')
INSERT [dbo].[СоставБлюда] ([IDТовара], [IDИнгредиента], [Количество]) VALUES (3, 1, N'7')
INSERT [dbo].[СоставБлюда] ([IDТовара], [IDИнгредиента], [Количество]) VALUES (3, 2, N'150')
INSERT [dbo].[СоставБлюда] ([IDТовара], [IDИнгредиента], [Количество]) VALUES (4, 1, N'7')
INSERT [dbo].[СоставБлюда] ([IDТовара], [IDИнгредиента], [Количество]) VALUES (4, 2, N'200')
INSERT [dbo].[СоставБлюда] ([IDТовара], [IDИнгредиента], [Количество]) VALUES (5, 3, N'5')
GO
SET IDENTITY_INSERT [dbo].[Сотрудники] ON 

INSERT [dbo].[Сотрудники] ([IDСотрудника], [ФИО], [Роль], [Логин], [ХешПароля], [ГрафикРаботы]) VALUES (1, N'Иванов Иван Иванович', 1, N'admin', N'5f4dcc3b5aa765d61d8327deb882cf99', N'Пн-Пт 9:00-18:00')
INSERT [dbo].[Сотрудники] ([IDСотрудника], [ФИО], [Роль], [Логин], [ХешПароля], [ГрафикРаботы]) VALUES (2, N'Петрова Анна Сергеевна', 2, N'manager', N'5f4dcc3b5aa765d61d8327deb882cf99', N'Пн-Сб 10:00-19:00')
INSERT [dbo].[Сотрудники] ([IDСотрудника], [ФИО], [Роль], [Логин], [ХешПароля], [ГрафикРаботы]) VALUES (3, N'Сидоров Алексей Владимирович', 3, N'barista1', N'5f4dcc3b5aa765d61d8327deb882cf99', N'Пн-Вс сменный график')
INSERT [dbo].[Сотрудники] ([IDСотрудника], [ФИО], [Роль], [Логин], [ХешПароля], [ГрафикРаботы]) VALUES (4, N'Кузнецова Елена Дмитриевна', 4, N'waiter1', N'5f4dcc3b5aa765d61d8327deb882cf99', N'Пн-Пт 8:00-17:00')
SET IDENTITY_INSERT [dbo].[Сотрудники] OFF
GO
/****** Object:  Index [IX_Заказ_ВремяСоздания]    Script Date: 31.03.2025 7:56:41 ******/
CREATE NONCLUSTERED INDEX [IX_Заказ_ВремяСоздания] ON [dbo].[Заказы]
(
	[ВремяСоздания] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Заказ_Статус]    Script Date: 31.03.2025 7:56:41 ******/
CREATE NONCLUSTERED INDEX [IX_Заказ_Статус] ON [dbo].[Заказы]
(
	[Статус] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Меню_Категория]    Script Date: 31.03.2025 7:56:41 ******/
CREATE NONCLUSTERED INDEX [IX_Меню_Категория] ON [dbo].[Меню]
(
	[Категория] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Платеж_IDЗаказа]    Script Date: 31.03.2025 7:56:41 ******/
CREATE NONCLUSTERED INDEX [IX_Платеж_IDЗаказа] ON [dbo].[Платежи]
(
	[IDЗаказа] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ПозицииЗаказа_IDЗаказа]    Script Date: 31.03.2025 7:56:41 ******/
CREATE NONCLUSTERED INDEX [IX_ПозицииЗаказа_IDЗаказа] ON [dbo].[ПозицииЗаказа]
(
	[IDЗаказа] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Сотрудни__BC2217D3543E3CB6]    Script Date: 31.03.2025 7:56:41 ******/
ALTER TABLE [dbo].[Сотрудники] ADD UNIQUE NONCLUSTERED 
(
	[Логин] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Сотрудник_Роль]    Script Date: 31.03.2025 7:56:41 ******/
CREATE NONCLUSTERED INDEX [IX_Сотрудник_Роль] ON [dbo].[Сотрудники]
(
	[Роль] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Заказы] ADD  DEFAULT (getdate()) FOR [ВремяСоздания]
GO
ALTER TABLE [dbo].[Меню] ADD  DEFAULT (getdate()) FOR [ДатаДобавления]
GO
ALTER TABLE [dbo].[Меню] ADD  DEFAULT (getdate()) FOR [ДатаИзменения]
GO
ALTER TABLE [dbo].[Отчеты] ADD  DEFAULT (getdate()) FOR [ВремяФормирования]
GO
ALTER TABLE [dbo].[Заказы]  WITH CHECK ADD  CONSTRAINT [FK_Заказ_Сотрудник] FOREIGN KEY([IDСотрудника])
REFERENCES [dbo].[Сотрудники] ([IDСотрудника])
GO
ALTER TABLE [dbo].[Заказы] CHECK CONSTRAINT [FK_Заказ_Сотрудник]
GO
ALTER TABLE [dbo].[ОтчетПерсонала]  WITH CHECK ADD  CONSTRAINT [FK_ОтчетПерсонала_Отчет] FOREIGN KEY([IDОтчета])
REFERENCES [dbo].[Отчеты] ([IDОтчета])
GO
ALTER TABLE [dbo].[ОтчетПерсонала] CHECK CONSTRAINT [FK_ОтчетПерсонала_Отчет]
GO
ALTER TABLE [dbo].[ОтчетПерсонала]  WITH CHECK ADD  CONSTRAINT [FK_ОтчетПерсонала_Сотрудник] FOREIGN KEY([IDСотрудника])
REFERENCES [dbo].[Сотрудники] ([IDСотрудника])
GO
ALTER TABLE [dbo].[ОтчетПерсонала] CHECK CONSTRAINT [FK_ОтчетПерсонала_Сотрудник]
GO
ALTER TABLE [dbo].[Платежи]  WITH CHECK ADD  CONSTRAINT [FK_Платеж_Заказ] FOREIGN KEY([IDЗаказа])
REFERENCES [dbo].[Заказы] ([IDЗаказа])
GO
ALTER TABLE [dbo].[Платежи] CHECK CONSTRAINT [FK_Платеж_Заказ]
GO
ALTER TABLE [dbo].[ПозицииЗаказа]  WITH CHECK ADD  CONSTRAINT [FK_ПозицииЗаказа_Заказ] FOREIGN KEY([IDЗаказа])
REFERENCES [dbo].[Заказы] ([IDЗаказа])
GO
ALTER TABLE [dbo].[ПозицииЗаказа] CHECK CONSTRAINT [FK_ПозицииЗаказа_Заказ]
GO
ALTER TABLE [dbo].[ПозицииЗаказа]  WITH CHECK ADD  CONSTRAINT [FK_ПозицииЗаказа_Меню] FOREIGN KEY([IDТовара])
REFERENCES [dbo].[Меню] ([IDТовара])
GO
ALTER TABLE [dbo].[ПозицииЗаказа] CHECK CONSTRAINT [FK_ПозицииЗаказа_Меню]
GO
ALTER TABLE [dbo].[СоставБлюда]  WITH CHECK ADD  CONSTRAINT [FK_СоставБлюда_Ингредиент] FOREIGN KEY([IDИнгредиента])
REFERENCES [dbo].[Ингредиенты] ([IDИнгредиента])
GO
ALTER TABLE [dbo].[СоставБлюда] CHECK CONSTRAINT [FK_СоставБлюда_Ингредиент]
GO
ALTER TABLE [dbo].[СоставБлюда]  WITH CHECK ADD  CONSTRAINT [FK_СоставБлюда_Меню] FOREIGN KEY([IDТовара])
REFERENCES [dbo].[Меню] ([IDТовара])
GO
ALTER TABLE [dbo].[СоставБлюда] CHECK CONSTRAINT [FK_СоставБлюда_Меню]
GO
ALTER TABLE [dbo].[Сотрудники]  WITH CHECK ADD  CONSTRAINT [FK_Сотрудник_Роль] FOREIGN KEY([Роль])
REFERENCES [dbo].[Роль] ([IDРоли])
GO
ALTER TABLE [dbo].[Сотрудники] CHECK CONSTRAINT [FK_Сотрудник_Роль]
GO
USE [master]
GO
ALTER DATABASE [CoffeeHouse] SET  READ_WRITE 
GO
