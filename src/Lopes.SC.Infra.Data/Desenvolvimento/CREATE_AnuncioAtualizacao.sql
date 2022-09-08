
/****** Object:  Table [dbo].[AnuncioAtualizacao]    Script Date: 05/09/2022 10:25:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AnuncioAtualizacao](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdImovel] [int] NOT NULL,
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


