IF(OBJECT_ID('dbo.VW_ProdutoImagensAnuncio', 'V') IS NOT NULL)
	DROP VIEW dbo.VW_ProdutoImagensAnuncio
GO
-- Imagens/Fotos dos imóveis para os anúncios/site
CREATE VIEW dbo.VW_ProdutoImagensAnuncio
AS
	 SELECT 
		A.ANEP_cd_anexo_produto AS Id
		,UNPR_cd_unidade_pronta AS IdImovel
		,S.STAN_ds_subtipo_anexo_produto AS Descricao
		,A.ANEP_ds_caminho AS NomeArquivo
		,O.ANOR_nr_ordem AS Ordem
	 FROM tb_ANEP_anexo_produto A
		INNER JOIN tb_STAN_subtipo_anexo_produto S WITH(NOLOCK) ON A.STAN_cd_subtipo_anexo_produto = S.STAN_cd_subtipo_anexo_produto      
		INNER JOIN tb_ANOR_anexo_ordem O WITH(NOLOCK) ON O.ANEP_cd_anexo_produto = A.ANEP_cd_anexo_produto       
	 WHERE 
		A.ANEP_fl_internet = 'S'   
GO
		
		select * from VW_ProdutoImagensAnuncio where idimovel = 7502
		--select * from VW_ANEXOS_UNIDADE_PRONTA  where UNPR_cd_unidade_pronta = 14078 

	 --select top 1000 * from tb_STAN_subtipo_anexo_produto where tane_cd_tipo_anexo_produto = 12
	 --select top 1000 * from tb_TANE_tipo_anexo_produto where TANE_cd_tipo_anexo_produto= 12


	/* 
SELECT     
 ANEP.ANEP_cd_anexo_produto AS 'Id',    
 ANEP.UNPR_cd_unidade_pronta AS UNPR_cd_unidade_pronta,      
 ANEP.STAN_cd_subtipo_anexo_produto as MultimediaSubtype_Id,      
 ANEP.ANEP_ds_anexo_produto AS Name,      
 REVERSE(SUBSTRING(REVERSE(ANEP.ANEP_ds_caminho), 0, CHARINDEX('.',REVERSE(ANEP.ANEP_ds_caminho)))) AS Extension,        
 ISNULL(cast(ANEP.UNPR_cd_unidade_pronta as varchar) + '/' + SUBSTRING(ANEP.ANEP_ds_caminho, 0, CHARINDEX('.',ANEP.ANEP_ds_caminho)),'') as FilePath,       
 ISNULL(cast(ANEP.UNPR_cd_unidade_pronta as varchar) + '/' + ANEP.ANEP_ds_caminho,'') as FilePathFull,    
 stan_ds_subtipo_anexo_produto as Description,       
 ANOR.ANOR_nr_ordem as Assortment       
FROM tb_ANEP_anexo_produto ANEP WITH(NOLOCK)       
INNER JOIN tb_ANOR_anexo_ordem ANOR WITH(NOLOCK) ON ANEP.ANEP_cd_anexo_produto = ANOR.ANEP_cd_anexo_produto       
INNER JOIN tb_STAN_subtipo_anexo_produto STAN WITH(NOLOCK) ON ANEP.STAN_cd_subtipo_anexo_produto = STAN.STAN_cd_subtipo_anexo_produto      
WHERE       
 STAN.TANE_cd_tipo_anexo_produto IN (2,12)       
 AND ANEP.ANEP_fl_internet = 'S' 
 */