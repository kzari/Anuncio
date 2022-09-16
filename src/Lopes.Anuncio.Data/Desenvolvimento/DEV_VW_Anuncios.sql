use Lopes
go
IF OBJECT_ID('VW_Anuncios', 'V') IS NOT NULL
	DROP VIEW dbo.VW_Anuncios
GO
-- Retorna as informações do Anuncio
CREATE VIEW dbo.VW_Anuncios
AS
	SELECT DISTINCT
		 IdAnuncio 
		,IP.IdProduto
		,IP.IdEmpresa
		,NomeEmpresa	
		,IdCota
		,Portal
		,IdSubveiculo
		,NomeSubveiculo
		,CdAnuncioClassificacao
		
		,ProdutoUltimaAlteracao
		,IdStatusProduto
		,NomeStatusProduto
		
		,IdStatusAnuncio
		,NomeStatusAnuncio

		,IdStatusCota
		,NomeStatusCota

		,AnuncioLiberado
		,PodeAnunciarOutraEmpresa
		,CodigoClientePortal

		,AnuncioAtualizacao.Data AS DataAtualizacaoAnuncioPortal
		,AnuncioAtualizacao.Acao AS AcaoAtualizacaoAnuncioPortal
	FROM 
		Anuncio IP
		OUTER APPLY
			(SELECT TOP 1 * 
			 FROM AnuncioAtualizacao AA 
			 WHERE IP.IdProduto = AA.IdProduto AND IP.IdEmpresa = AA.IdEmpresa AND IP.Portal = AA.IdPortal 
			 ORDER BY AA.Data DESC) AS AnuncioAtualizacao
GO
--select TOP 100 * from VW_Anuncios where IdProduto = 627841
select * from VW_Anuncios where Portal = 68
