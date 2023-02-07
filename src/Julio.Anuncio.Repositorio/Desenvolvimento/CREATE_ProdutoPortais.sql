DROP TABLE Anuncio
GO
CREATE TABLE Anuncio 
(
	IdAnuncio INT NOT NULL
	,IdProduto INT NOT NULL
	,IdEmpresa INT NOT NULL
	,NomeEmpresa VARCHAR(150) 
	,IdCota INT NOT NULL
	,Portal INT NOT NULL
	,IdSubveiculo INT NOT NULL
	,NomeSubveiculo  VARCHAR(150) 
	,CdAnuncioClassificacao INT NOT NULL
	
	,ProdutoUltimaAlteracao DATETIME NULL
	,IdStatusProduto INT NOT NULL
	,NomeStatusProduto  VARCHAR(150) 
	
	,IdStatusAnuncio INT NOT NULL
	,NomeStatusAnuncio VARCHAR(150) 
	
	,IdStatusCota INT NOT NULL
	,NomeStatusCota VARCHAR(150) 
	
	,AnuncioLiberado BIT
	,PodeAnunciarOutraEmpresa BIT
	,CodigoClientePortal  VARCHAR(150)
)