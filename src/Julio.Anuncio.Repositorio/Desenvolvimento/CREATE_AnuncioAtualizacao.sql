USE [Lopes]
GO

/****** Object:  Table [dbo].[AnuncioAtualizacao]    Script Date: 12/09/2022 13:56:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AnuncioAtualizacao](
	[Id] [varchar](40) NOT NULL,
	[IdProduto] [int] NOT NULL,
	[IdEmpresa] [int] NOT NULL,
	[IdPortal] [int] NOT NULL,
	[Acao] [int] NOT NULL,
	[Data] [datetime] NOT NULL,
 CONSTRAINT [PK_AnuncioAtualizacao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


