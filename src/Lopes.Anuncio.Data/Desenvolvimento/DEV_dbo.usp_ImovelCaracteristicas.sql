use dbproduto
go
IF OBJECT_ID('dbo.ImovelCaracteristicas', 'P') IS NOT NULL
	DROP PROCEDURE dbo.ImovelCaracteristicas
GO
CREATE PROCEDURE [dbo].ImovelCaracteristicas(
	@idImovel INT,
	@idPortal INT = NULL
)
AS 
	SELECT Id, 
		   Nome, 
		   CONVERT(BIT, MAX(Unidade)) AS Unidade, 
		   CONVERT(BIT, MAX(Empreendimento)) AS Empreendimento,
		   VA.VEAT_ds_tag AS Tag
	FROM 
	(
		SELECT 
			AU.ATRI_cd_atributo AS Id
			,ATRI_nm_atributo AS Nome
			,0 Empreendimento 
			,1 Unidade
		FROM 
			tb_UNPR_X_ATRI AXU
			INNER JOIN tb_ATRI_atributo AU WITH (NOLOCK) ON  AU.ATRI_cd_atributo = AXU.ATRI_cd_atributo
		WHERE 
			UNPR_cd_unidade_pronta = @idImovel
		UNION
		SELECT	
			AE.ATRI_cd_atributo AS Id
			,ATRI_nm_atributo AS Nome
			,1 Empreendimento 
			,0 Unidade
		FROM 
			TB_UNPR_UNIDADE_PRONTA U  
			INNER JOIN TB_MOUN_MODELO_UNIDADE M				 WITH (NOLOCK) ON  U.MOUN_CD_MODELO_UNIDADE = M.MOUN_CD_MODELO_UNIDADE 
			INNER JOIN tb_DIVI_divisao D					 WITH (NOLOCK) ON  M.DIVI_cd_divisao = D.DIVI_cd_divisao 
			INNER JOIN tb_ATEM_atributo_empreendimento AXE	 WITH (NOLOCK) ON  AXE.EMPR_cd_empreendimento = D.EMPR_cd_empreendimento
			INNER JOIN tb_ATRI_atributo AE					 WITH (NOLOCK) ON  AE.ATRI_cd_atributo = AXE.ATRI_cd_atributo
		WHERE 
			U.UNPR_cd_unidade_pronta = @idImovel
	) A
	LEFT JOIN tb_VEIC_x_ATRI_veiculo_atributo VA ON VA.ATRI_cd_atributo = A.Id AND VA.VEIC_cd_veiculo = @idPortal
	WHERE 
		@idPortal IS NULL OR VA.VEIC_cd_veiculo = @idPortal
	GROUP BY 
		Id, Nome, VA.VEAT_ds_tag
GO 
--dbo.ImovelCaracteristicas 345674, 38