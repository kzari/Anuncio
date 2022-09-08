DROP TABLE ImovelPortais
GO
CREATE TABLE ImovelPortais 
(
	IdAnuncio INT NOT NULL
	,IdImovel INT NOT NULL
	,IdEmpresa INT NOT NULL
	,NomeEmpresa VARCHAR(150) 
	,IdCota INT NOT NULL
	,Portal INT NOT NULL
	,IdSubveiculo INT NOT NULL
	,NomeSubveiculo  VARCHAR(150) 
	,CdAnuncioClassificacao INT NOT NULL
	
	,ImovelUltimaAlteracao DATETIME NULL
	,IdStatusImovel INT NOT NULL
	,NomeStatusImovel  VARCHAR(150) 
	
	,IdStatusAnuncio INT NOT NULL
	,NomeStatusAnuncio VARCHAR(150) 
	
	,IdStatusCota INT NOT NULL
	,NomeStatusCota VARCHAR(150) 
	
	,AnuncioLiberado BIT
	,PodeAnunciarOutraEmpresa BIT
	,CodigoClientePortal  VARCHAR(150)
)