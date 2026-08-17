USE [master]
GO

/****** Object:  Database [BusinessERP]    Script Date: 8/06/2026 12:37:04 ******/
CREATE DATABASE [BusinessERP]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BusinessERP', FILENAME = N'D:\SQLDatabases\Data\BusinessERP.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BusinessERP_log', FILENAME = N'D:\SQLDatabases\Log\BusinessERP.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BusinessERP].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [BusinessERP] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [BusinessERP] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [BusinessERP] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [BusinessERP] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [BusinessERP] SET ARITHABORT OFF 
GO

ALTER DATABASE [BusinessERP] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [BusinessERP] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [BusinessERP] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [BusinessERP] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [BusinessERP] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [BusinessERP] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [BusinessERP] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [BusinessERP] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [BusinessERP] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [BusinessERP] SET  ENABLE_BROKER 
GO

ALTER DATABASE [BusinessERP] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [BusinessERP] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [BusinessERP] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [BusinessERP] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [BusinessERP] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [BusinessERP] SET READ_COMMITTED_SNAPSHOT ON 
GO

ALTER DATABASE [BusinessERP] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [BusinessERP] SET RECOVERY FULL 
GO

ALTER DATABASE [BusinessERP] SET  MULTI_USER 
GO

ALTER DATABASE [BusinessERP] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [BusinessERP] SET DB_CHAINING OFF 
GO

ALTER DATABASE [BusinessERP] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [BusinessERP] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [BusinessERP] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [BusinessERP] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [BusinessERP] SET QUERY_STORE = ON
GO

ALTER DATABASE [BusinessERP] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

ALTER DATABASE [BusinessERP] SET  READ_WRITE 
GO

GO
GO
GO



USE [BusinessERP]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 8/06/2026 12:43:01 ******/
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
/****** Object:  Table [dbo].[Almacenes]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Almacenes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](450) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Ubicacion] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Almacenes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorias]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RazonSocial] [nvarchar](max) NOT NULL,
	[RUC] [nvarchar](450) NOT NULL,
	[Direccion] [nvarchar](max) NOT NULL,
	[Telefono] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[TipoDocumento] [nvarchar](max) NOT NULL,
	[LimiteCredito] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CotizacionDetalles]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CotizacionDetalles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CotizacionId] [int] NOT NULL,
	[ProductoId] [int] NOT NULL,
	[Cantidad] [decimal](18, 4) NOT NULL,
	[PrecioUnitario] [decimal](18, 2) NOT NULL,
	[Descuento] [decimal](18, 2) NOT NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_CotizacionDetalles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cotizaciones]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cotizaciones](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Numero] [nvarchar](max) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[FechaVencimiento] [datetime2](7) NOT NULL,
	[ClienteId] [int] NOT NULL,
	[Estado] [nvarchar](max) NOT NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[Igv] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Cotizaciones] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresas]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RazonSocial] [nvarchar](max) NOT NULL,
	[RUC] [nvarchar](450) NOT NULL,
	[Direccion] [nvarchar](max) NOT NULL,
	[Telefono] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[LogoUrl] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Empresas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FacturaDetalles]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FacturaDetalles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FacturaId] [int] NOT NULL,
	[ProductoId] [int] NOT NULL,
	[Cantidad] [decimal](18, 4) NOT NULL,
	[PrecioUnitario] [decimal](18, 2) NOT NULL,
	[Descuento] [decimal](18, 2) NOT NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_FacturaDetalles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Facturas]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Facturas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Serie] [nvarchar](max) NOT NULL,
	[Numero] [nvarchar](max) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[ClienteId] [int] NOT NULL,
	[PedidoId] [int] NULL,
	[Estado] [nvarchar](max) NOT NULL,
	[TipoDocumento] [nvarchar](max) NOT NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[Igv] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Facturas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Icon] [nvarchar](max) NOT NULL,
	[Route] [nvarchar](max) NOT NULL,
	[Order] [int] NOT NULL,
	[ParentId] [int] NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovimientosInventario]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovimientosInventario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Tipo] [nvarchar](max) NOT NULL,
	[ProductoId] [int] NOT NULL,
	[AlmacenId] [int] NOT NULL,
	[Cantidad] [decimal](18, 4) NOT NULL,
	[PrecioUnitario] [decimal](18, 2) NOT NULL,
	[Referencia] [nvarchar](max) NULL,
	[Observacion] [nvarchar](max) NULL,
	[FechaMovimiento] [datetime2](7) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_MovimientosInventario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdenCompraDetalles]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdenCompraDetalles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrdenCompraId] [int] NOT NULL,
	[ProductoId] [int] NOT NULL,
	[Cantidad] [decimal](18, 4) NOT NULL,
	[CantidadRecibida] [decimal](18, 4) NOT NULL,
	[PrecioUnitario] [decimal](18, 2) NOT NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_OrdenCompraDetalles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdenesCompra]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdenesCompra](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Numero] [nvarchar](max) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[FechaEntrega] [datetime2](7) NOT NULL,
	[ProveedorId] [int] NOT NULL,
	[Estado] [nvarchar](max) NOT NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[Igv] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_OrdenesCompra] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parametros]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parametros](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](max) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Valor] [nvarchar](max) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Modulo] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Parametros] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PedidoDetalles]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PedidoDetalles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PedidoId] [int] NOT NULL,
	[ProductoId] [int] NOT NULL,
	[Cantidad] [decimal](18, 4) NOT NULL,
	[PrecioUnitario] [decimal](18, 2) NOT NULL,
	[Descuento] [decimal](18, 2) NOT NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_PedidoDetalles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pedidos]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedidos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Numero] [nvarchar](max) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[ClienteId] [int] NOT NULL,
	[CotizacionId] [int] NULL,
	[Estado] [nvarchar](max) NOT NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[Igv] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Pedidos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
	[Module] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Productos]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Productos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](450) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[PrecioCompra] [decimal](18, 2) NOT NULL,
	[PrecioVenta] [decimal](18, 2) NOT NULL,
	[Unidad] [nvarchar](max) NOT NULL,
	[CategoriaId] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proveedores]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedores](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RazonSocial] [nvarchar](max) NOT NULL,
	[RUC] [nvarchar](450) NOT NULL,
	[Direccion] [nvarchar](max) NOT NULL,
	[Telefono] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[TipoDocumento] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Proveedores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecepcionCompraDetalles]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecepcionCompraDetalles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RecepcionCompraId] [int] NOT NULL,
	[ProductoId] [int] NOT NULL,
	[CantidadEsperada] [decimal](18, 4) NOT NULL,
	[CantidadRecibida] [decimal](18, 4) NOT NULL,
	[PrecioUnitario] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_RecepcionCompraDetalles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecepcionesCompra]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecepcionesCompra](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Numero] [nvarchar](max) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[OrdenCompraId] [int] NOT NULL,
	[Estado] [nvarchar](max) NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_RecepcionesCompra] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleMenus]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMenus](
	[RoleId] [int] NOT NULL,
	[MenuId] [int] NOT NULL,
 CONSTRAINT [PK_RoleMenus] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolePermissions]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePermissions](
	[RoleId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL,
 CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stocks]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stocks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductoId] [int] NOT NULL,
	[AlmacenId] [int] NOT NULL,
	[CantidadActual] [decimal](18, 4) NOT NULL,
	[StockMinimo] [decimal](18, 4) NOT NULL,
	[StockMaximo] [decimal](18, 4) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Stocks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sucursales]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sucursales](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Codigo] [nvarchar](450) NOT NULL,
	[Direccion] [nvarchar](max) NOT NULL,
	[Telefono] [nvarchar](max) NOT NULL,
	[EmpresaId] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Sucursales] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/06/2026 12:43:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](450) NOT NULL,
	[Email] [nvarchar](450) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[RoleId] [int] NULL,
	[EmpresaId] [int] NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[RefreshTokenExpiry] [datetime2](7) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Almacenes_Codigo]    Script Date: 8/06/2026 12:43:01 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Almacenes_Codigo] ON [dbo].[Almacenes]
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Clientes_RUC]    Script Date: 8/06/2026 12:43:01 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Clientes_RUC] ON [dbo].[Clientes]
(
	[RUC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CotizacionDetalles_CotizacionId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_CotizacionDetalles_CotizacionId] ON [dbo].[CotizacionDetalles]
(
	[CotizacionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CotizacionDetalles_ProductoId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_CotizacionDetalles_ProductoId] ON [dbo].[CotizacionDetalles]
(
	[ProductoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cotizaciones_ClienteId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_Cotizaciones_ClienteId] ON [dbo].[Cotizaciones]
(
	[ClienteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Empresas_RUC]    Script Date: 8/06/2026 12:43:01 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Empresas_RUC] ON [dbo].[Empresas]
(
	[RUC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FacturaDetalles_FacturaId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_FacturaDetalles_FacturaId] ON [dbo].[FacturaDetalles]
(
	[FacturaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FacturaDetalles_ProductoId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_FacturaDetalles_ProductoId] ON [dbo].[FacturaDetalles]
(
	[ProductoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Facturas_ClienteId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_Facturas_ClienteId] ON [dbo].[Facturas]
(
	[ClienteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Facturas_PedidoId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_Facturas_PedidoId] ON [dbo].[Facturas]
(
	[PedidoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Menus_ParentId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_Menus_ParentId] ON [dbo].[Menus]
(
	[ParentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MovimientosInventario_AlmacenId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_MovimientosInventario_AlmacenId] ON [dbo].[MovimientosInventario]
(
	[AlmacenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MovimientosInventario_ProductoId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_MovimientosInventario_ProductoId] ON [dbo].[MovimientosInventario]
(
	[ProductoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrdenCompraDetalles_OrdenCompraId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_OrdenCompraDetalles_OrdenCompraId] ON [dbo].[OrdenCompraDetalles]
(
	[OrdenCompraId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrdenCompraDetalles_ProductoId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_OrdenCompraDetalles_ProductoId] ON [dbo].[OrdenCompraDetalles]
(
	[ProductoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrdenesCompra_ProveedorId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_OrdenesCompra_ProveedorId] ON [dbo].[OrdenesCompra]
(
	[ProveedorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PedidoDetalles_PedidoId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_PedidoDetalles_PedidoId] ON [dbo].[PedidoDetalles]
(
	[PedidoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PedidoDetalles_ProductoId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_PedidoDetalles_ProductoId] ON [dbo].[PedidoDetalles]
(
	[ProductoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pedidos_ClienteId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_Pedidos_ClienteId] ON [dbo].[Pedidos]
(
	[ClienteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pedidos_CotizacionId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_Pedidos_CotizacionId] ON [dbo].[Pedidos]
(
	[CotizacionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Productos_CategoriaId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_Productos_CategoriaId] ON [dbo].[Productos]
(
	[CategoriaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Productos_Codigo]    Script Date: 8/06/2026 12:43:01 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Productos_Codigo] ON [dbo].[Productos]
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Proveedores_RUC]    Script Date: 8/06/2026 12:43:01 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Proveedores_RUC] ON [dbo].[Proveedores]
(
	[RUC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RecepcionCompraDetalles_ProductoId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_RecepcionCompraDetalles_ProductoId] ON [dbo].[RecepcionCompraDetalles]
(
	[ProductoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RecepcionCompraDetalles_RecepcionCompraId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_RecepcionCompraDetalles_RecepcionCompraId] ON [dbo].[RecepcionCompraDetalles]
(
	[RecepcionCompraId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RecepcionesCompra_OrdenCompraId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_RecepcionesCompra_OrdenCompraId] ON [dbo].[RecepcionesCompra]
(
	[OrdenCompraId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RoleMenus_MenuId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_RoleMenus_MenuId] ON [dbo].[RoleMenus]
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RolePermissions_PermissionId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_RolePermissions_PermissionId] ON [dbo].[RolePermissions]
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Stocks_AlmacenId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_Stocks_AlmacenId] ON [dbo].[Stocks]
(
	[AlmacenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Stocks_ProductoId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_Stocks_ProductoId] ON [dbo].[Stocks]
(
	[ProductoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Sucursales_EmpresaId_Codigo]    Script Date: 8/06/2026 12:43:01 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Sucursales_EmpresaId_Codigo] ON [dbo].[Sucursales]
(
	[EmpresaId] ASC,
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_Email]    Script Date: 8/06/2026 12:43:01 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_EmpresaId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_Users_EmpresaId] ON [dbo].[Users]
(
	[EmpresaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_RoleId]    Script Date: 8/06/2026 12:43:01 ******/
CREATE NONCLUSTERED INDEX [IX_Users_RoleId] ON [dbo].[Users]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_Username]    Script Date: 8/06/2026 12:43:01 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Username] ON [dbo].[Users]
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CotizacionDetalles]  WITH CHECK ADD  CONSTRAINT [FK_CotizacionDetalles_Cotizaciones_CotizacionId] FOREIGN KEY([CotizacionId])
REFERENCES [dbo].[Cotizaciones] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CotizacionDetalles] CHECK CONSTRAINT [FK_CotizacionDetalles_Cotizaciones_CotizacionId]
GO
ALTER TABLE [dbo].[CotizacionDetalles]  WITH CHECK ADD  CONSTRAINT [FK_CotizacionDetalles_Productos_ProductoId] FOREIGN KEY([ProductoId])
REFERENCES [dbo].[Productos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CotizacionDetalles] CHECK CONSTRAINT [FK_CotizacionDetalles_Productos_ProductoId]
GO
ALTER TABLE [dbo].[Cotizaciones]  WITH CHECK ADD  CONSTRAINT [FK_Cotizaciones_Clientes_ClienteId] FOREIGN KEY([ClienteId])
REFERENCES [dbo].[Clientes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cotizaciones] CHECK CONSTRAINT [FK_Cotizaciones_Clientes_ClienteId]
GO
ALTER TABLE [dbo].[FacturaDetalles]  WITH CHECK ADD  CONSTRAINT [FK_FacturaDetalles_Facturas_FacturaId] FOREIGN KEY([FacturaId])
REFERENCES [dbo].[Facturas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FacturaDetalles] CHECK CONSTRAINT [FK_FacturaDetalles_Facturas_FacturaId]
GO
ALTER TABLE [dbo].[FacturaDetalles]  WITH CHECK ADD  CONSTRAINT [FK_FacturaDetalles_Productos_ProductoId] FOREIGN KEY([ProductoId])
REFERENCES [dbo].[Productos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FacturaDetalles] CHECK CONSTRAINT [FK_FacturaDetalles_Productos_ProductoId]
GO
ALTER TABLE [dbo].[Facturas]  WITH CHECK ADD  CONSTRAINT [FK_Facturas_Clientes_ClienteId] FOREIGN KEY([ClienteId])
REFERENCES [dbo].[Clientes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Facturas] CHECK CONSTRAINT [FK_Facturas_Clientes_ClienteId]
GO
ALTER TABLE [dbo].[Facturas]  WITH CHECK ADD  CONSTRAINT [FK_Facturas_Pedidos_PedidoId] FOREIGN KEY([PedidoId])
REFERENCES [dbo].[Pedidos] ([Id])
GO
ALTER TABLE [dbo].[Facturas] CHECK CONSTRAINT [FK_Facturas_Pedidos_PedidoId]
GO
ALTER TABLE [dbo].[Menus]  WITH CHECK ADD  CONSTRAINT [FK_Menus_Menus_ParentId] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Menus] ([Id])
GO
ALTER TABLE [dbo].[Menus] CHECK CONSTRAINT [FK_Menus_Menus_ParentId]
GO
ALTER TABLE [dbo].[MovimientosInventario]  WITH CHECK ADD  CONSTRAINT [FK_MovimientosInventario_Almacenes_AlmacenId] FOREIGN KEY([AlmacenId])
REFERENCES [dbo].[Almacenes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MovimientosInventario] CHECK CONSTRAINT [FK_MovimientosInventario_Almacenes_AlmacenId]
GO
ALTER TABLE [dbo].[MovimientosInventario]  WITH CHECK ADD  CONSTRAINT [FK_MovimientosInventario_Productos_ProductoId] FOREIGN KEY([ProductoId])
REFERENCES [dbo].[Productos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MovimientosInventario] CHECK CONSTRAINT [FK_MovimientosInventario_Productos_ProductoId]
GO
ALTER TABLE [dbo].[OrdenCompraDetalles]  WITH CHECK ADD  CONSTRAINT [FK_OrdenCompraDetalles_OrdenesCompra_OrdenCompraId] FOREIGN KEY([OrdenCompraId])
REFERENCES [dbo].[OrdenesCompra] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrdenCompraDetalles] CHECK CONSTRAINT [FK_OrdenCompraDetalles_OrdenesCompra_OrdenCompraId]
GO
ALTER TABLE [dbo].[OrdenCompraDetalles]  WITH CHECK ADD  CONSTRAINT [FK_OrdenCompraDetalles_Productos_ProductoId] FOREIGN KEY([ProductoId])
REFERENCES [dbo].[Productos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrdenCompraDetalles] CHECK CONSTRAINT [FK_OrdenCompraDetalles_Productos_ProductoId]
GO
ALTER TABLE [dbo].[OrdenesCompra]  WITH CHECK ADD  CONSTRAINT [FK_OrdenesCompra_Proveedores_ProveedorId] FOREIGN KEY([ProveedorId])
REFERENCES [dbo].[Proveedores] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrdenesCompra] CHECK CONSTRAINT [FK_OrdenesCompra_Proveedores_ProveedorId]
GO
ALTER TABLE [dbo].[PedidoDetalles]  WITH CHECK ADD  CONSTRAINT [FK_PedidoDetalles_Pedidos_PedidoId] FOREIGN KEY([PedidoId])
REFERENCES [dbo].[Pedidos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PedidoDetalles] CHECK CONSTRAINT [FK_PedidoDetalles_Pedidos_PedidoId]
GO
ALTER TABLE [dbo].[PedidoDetalles]  WITH CHECK ADD  CONSTRAINT [FK_PedidoDetalles_Productos_ProductoId] FOREIGN KEY([ProductoId])
REFERENCES [dbo].[Productos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PedidoDetalles] CHECK CONSTRAINT [FK_PedidoDetalles_Productos_ProductoId]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_Pedidos_Clientes_ClienteId] FOREIGN KEY([ClienteId])
REFERENCES [dbo].[Clientes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_Pedidos_Clientes_ClienteId]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_Pedidos_Cotizaciones_CotizacionId] FOREIGN KEY([CotizacionId])
REFERENCES [dbo].[Cotizaciones] ([Id])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_Pedidos_Cotizaciones_CotizacionId]
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_Productos_Categorias_CategoriaId] FOREIGN KEY([CategoriaId])
REFERENCES [dbo].[Categorias] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Productos] CHECK CONSTRAINT [FK_Productos_Categorias_CategoriaId]
GO
ALTER TABLE [dbo].[RecepcionCompraDetalles]  WITH CHECK ADD  CONSTRAINT [FK_RecepcionCompraDetalles_Productos_ProductoId] FOREIGN KEY([ProductoId])
REFERENCES [dbo].[Productos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RecepcionCompraDetalles] CHECK CONSTRAINT [FK_RecepcionCompraDetalles_Productos_ProductoId]
GO
ALTER TABLE [dbo].[RecepcionCompraDetalles]  WITH CHECK ADD  CONSTRAINT [FK_RecepcionCompraDetalles_RecepcionesCompra_RecepcionCompraId] FOREIGN KEY([RecepcionCompraId])
REFERENCES [dbo].[RecepcionesCompra] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RecepcionCompraDetalles] CHECK CONSTRAINT [FK_RecepcionCompraDetalles_RecepcionesCompra_RecepcionCompraId]
GO
ALTER TABLE [dbo].[RecepcionesCompra]  WITH CHECK ADD  CONSTRAINT [FK_RecepcionesCompra_OrdenesCompra_OrdenCompraId] FOREIGN KEY([OrdenCompraId])
REFERENCES [dbo].[OrdenesCompra] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RecepcionesCompra] CHECK CONSTRAINT [FK_RecepcionesCompra_OrdenesCompra_OrdenCompraId]
GO
ALTER TABLE [dbo].[RoleMenus]  WITH CHECK ADD  CONSTRAINT [FK_RoleMenus_Menus_MenuId] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menus] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleMenus] CHECK CONSTRAINT [FK_RoleMenus_Menus_MenuId]
GO
ALTER TABLE [dbo].[RoleMenus]  WITH CHECK ADD  CONSTRAINT [FK_RoleMenus_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleMenus] CHECK CONSTRAINT [FK_RoleMenus_Roles_RoleId]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Permissions_PermissionId] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permissions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Permissions_PermissionId]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Roles_RoleId]
GO
ALTER TABLE [dbo].[Stocks]  WITH CHECK ADD  CONSTRAINT [FK_Stocks_Almacenes_AlmacenId] FOREIGN KEY([AlmacenId])
REFERENCES [dbo].[Almacenes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Stocks] CHECK CONSTRAINT [FK_Stocks_Almacenes_AlmacenId]
GO
ALTER TABLE [dbo].[Stocks]  WITH CHECK ADD  CONSTRAINT [FK_Stocks_Productos_ProductoId] FOREIGN KEY([ProductoId])
REFERENCES [dbo].[Productos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Stocks] CHECK CONSTRAINT [FK_Stocks_Productos_ProductoId]
GO
ALTER TABLE [dbo].[Sucursales]  WITH CHECK ADD  CONSTRAINT [FK_Sucursales_Empresas_EmpresaId] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sucursales] CHECK CONSTRAINT [FK_Sucursales_Empresas_EmpresaId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Empresas_EmpresaId] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Empresas_EmpresaId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles_RoleId]
GO
USE [master]
GO
ALTER DATABASE [BusinessERP] SET  READ_WRITE 
GO
