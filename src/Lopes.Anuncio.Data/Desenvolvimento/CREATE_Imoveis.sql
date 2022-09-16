DROP TABLE dbo.Produtos
GO
CREATE TABLE [dbo].[Produtos](
	[IdImovel] [int] NOT NULL,
	[Titulo] [varchar](MAX) NULL,
	[TextoSite] [varchar](MAX) NULL,
	[AreaTotal] [decimal](18, 0) NULL,
	[AreaPrivativa] [decimal](18, 0) NULL,
	[QtdeVagas] [int] NULL,
	[QtdeVagasDemarcadas] [int] NULL,
	[QtdeVagasNaoDemarcadas] [int] NULL,
	[QtdeQuartos] [int] NULL,
	[QtdeSuites] [int] NULL,
	[QtdeSalas] [int] NULL,
	[QtdeBanheirosSociais] [int] NULL,
	[ValorVenda] [decimal](18, 0) NULL,
	[ValorLocacao] [decimal](18, 0) NULL,
	[ValorIPTU] [decimal](18, 0) NULL,
	[ValorCondominio] [decimal](18, 0) NULL,
	[Tipo] [varchar](200) NULL,
	[Subtipo] [varchar](200) NULL,
	[Ranking] [int] NULL,
	[DataInclusao] [datetime] NULL,
	[Logradouro] [varchar](300) NULL,
	[Numero] [varchar](50) NULL,
	[Complemento] [varchar](100) NULL,
	[Cidade] [varchar](200) NULL,
	[Bairro] [varchar](200) NULL,
	[CEP] [varchar](10) NULL,
	[Latitude] [numeric](18, 0) NULL,
	[Longitude] [numeric](18, 0) NULL,
	[Estado] [varchar](30) NULL,
	[ZonaValor] [varchar](50) NULL,
	[AnoConstrucao] [int] NULL,
	[InscricaoMunicipal] [varchar](50) NULL,
 CONSTRAINT [PK_Produtos] PRIMARY KEY CLUSTERED 
(
	[IdImovel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


