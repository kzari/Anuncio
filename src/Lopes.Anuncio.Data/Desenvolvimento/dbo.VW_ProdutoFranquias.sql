use dbproduto
go
IF OBJECT_ID('VW_ProdutoFranquias', 'V') IS NOT NULL
	DROP VIEW dbo.VW_ProdutoFranquias
GO
-- Retorna as empresas dos gestores do imóvel
CREATE VIEW dbo.VW_ProdutoFranquias
AS
	SELECT DISTINCT EMPE_cd_empresalopes IdFranquia, 
					UNPR_cd_unidade_pronta AS IdProduto
	FROM tb_UNCO_unidade_conta  WITH (NOLOCK) 
	WHERE TCON_cd_tipo_conta IN (14, 15)
GO
select * from VW_ProdutoFranquias where idproduto = 627841