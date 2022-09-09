use dbproduto
go
IF OBJECT_ID('VW_ImovelEmpresas', 'V') IS NOT NULL
	DROP VIEW dbo.VW_ImovelEmpresas
GO
-- Retorna as empresas dos gestores do imóvel
CREATE VIEW dbo.VW_ImovelEmpresas
AS
	SELECT DISTINCT EMPE_cd_empresalopes IdEmpresa, 
					UNPR_cd_unidade_pronta AS IdImovel
	FROM tb_UNCO_unidade_conta  WITH (NOLOCK) 
	WHERE TCON_cd_tipo_conta IN (14, 15)
GO
select * from VW_ImovelEmpresas where idimovel = 627841