use dbproduto
go
IF OBJECT_ID('dbo.VW_AnuncioProdutoDados', 'V') IS NOT NULL
	DROP VIEW dbo.VW_AnuncioProdutoDados
GO
CREATE VIEW dbo.VW_AnuncioProdutoDados
AS
	SELECT DISTINCT
		UNPR.UNPR_cd_unidade_pronta AS IdProduto
		,UNDT.UNDT_ds_titulo AS Titulo
		,UNDT.UNDT_ds_texto_site as TextoSite
		,MOUN.MOUN_qt_area_total_m2 AS AreaTotal
		,MOUN.MOUN_qt_area_privativa_m2 AS AreaPrivativa
		,UNPR.UNPR_nr_vagas AS QtdeVagas
		,COALESCE(UNPR.UNPR_nr_vagas, MOUN.MOUN_nr_vagas_demarcadas, 0) AS QtdeVagasDemarcadas
		,COALESCE(UNPR.UNPR_nr_vagas, MOUN.MOUN_nr_vagas_nao_demarcadas, 0) AS QtdeVagasNaoDemarcadas
		,COALESCE(UNPR.UNPR_nr_dormitorios, MOUN.MOUN_nr_dormitorios, 0) AS QtdeQuartos
		,COALESCE(UNPR.UNPR_nr_suites, MOUN.MOUN_nr_suites, 0) AS QtdeSuites
		,COALESCE(UNPR.UNPR_nr_salas, MOUN.MOUN_nr_salas, 0) AS QtdeSalas
		,COALESCE(UNPR.UNPR_nr_banheiros_sociais, MOUN.MOUN_nr_banheiros_sociais, 0) AS QtdeBanheirosSociais
		,UNVL_vl_venda AS ValorVenda
		,UNVL_vl_locacao AS ValorLocacao
		,UNVL_vl_iptu AS ValorIPTU
		,UNVL_vl_condominio AS ValorCondominio
		
		,CASE     
			WHEN UNPR_X_UTPR_cd is not null then 'Comercial'    
			WHEN CADI_ds_categoria_divisao = 'Rural' then 'Propriedade Rural'    
			WHEN CADI_ds_categoria_divisao = 'Flat' then 'Apartamento'    
			WHEN CADI_ds_categoria_divisao = 'Lajes Corporativas' then 'Comercial'    
			WHEN CADI_ds_categoria_divisao = 'Mall' then 'Comercial'    
			WHEN CADI_ds_categoria_divisao = 'Salas' then 'Comercial'    
		ELSE CADI_ds_categoria_divisao END as Tipo
		,CASE    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Apartamento' then 'Padrão'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Casa' then 'Padrão'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Hotel' then 'Padrão'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Terreno' then 'Padrão'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Casa térrea' then 'Térrea'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Condomínio' then 'Condomínio'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Sobrado' then 'Sobrado'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Villagio' then 'Villagio'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Barracão' then 'Galpão'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Lajes Corporativas' then 'Laje'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Loja' then 'Sala'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Conj. Comercial' then 'Sala'    
			WHEN CAMU_ds_categoria_modelo_unidade = 'Salas' then 'Sala'
		ELSE  CAMU_ds_categoria_modelo_unidade END  as Subtipo
		,UNPR.UNPR_nr_score AS Ranking
		,UNPR.AUDT_dt_inclusao AS DataInclusao
		,ENEM.ENEM_ds_logradouro AS Logradouro
		,ENDERECO.Num AS Numero
		,ENDERECO.Complemento AS Complemento
		,ENDERECO.Cidade AS Cidade
		,ENDERECO.Bairro AS Bairro
		,ENDERECO.CEP AS CEP
		,ENEM.ENEM_vl_latitude AS Latitude
		,ENEM.ENEM_vl_longitude AS Longitude
		,ENDERECO.Estado AS SA
		,ESTA.ESTA_nm_estado AS Estado

		--,ZOGE.ZOGE_cd_zona_geografica AS GeographicalZone_Id
		,ZOVA.ZOVA_nm_zona_valor AS ZonaValor

		--,REPLACE(CAST(ENDERECO.Latitude_Y as varchar),'.',',') AS LatitudeRealty
		--,REPLACE(CAST(ENDERECO.Longitude_X as varchar),'.',',') AS LongitudeRealty
		--(
		--	SELECT DISTINCT
		--		a.ATRI_cd_atributo as Id, 
		--		a.ATRI_nm_atributo as Name 
		--	FROM tb_UNPR_X_ATRI t WITH (NOLOCK)
		--		INNER JOIN [dbo].[tb_ATRI_atributo] a WITH (NOLOCK) ON  t.ATRI_cd_atributo = a.ATRI_cd_atributo 
		--	WHERE t.UNPR_cd_unidade_pronta = UNPR.UNPR_cd_unidade_pronta
		--	FOR XML PATH('RealtyAttribute'),  
		--	ROOT('RealtyAttributes') 
		--) AS 'RealtyAttributes',
		--(
		--	SELECT DISTINCT 
		--		ISNULL(VW.MultimediaSubtype_Id,0) AS MultimediaSubtype_Id, 
		--		VW.Name, ISNULL(VW.Extension,'') as extension, 
		--		VW.FilePath as FilePath,
		--		VW.FilePathFull, 
		--		VW.Description as description, 
		--		VW.Assortment
		--	FROM dbo.VW_ANEXOS_UNIDADE_PRONTA VW WITH (NOLOCK)
		--	WHERE VW.UNPR_cd_unidade_pronta = UNPR.UNPR_cd_unidade_pronta
		--	ORDER BY VW.Assortment
		--	FOR XML PATH('RealtyMultimedia'),  
		--	ROOT('RealtyMultimedias')
		--) AS 'RealtyMultimedia',
		,EMDT.EMDT_nr_ano_contrucao AS AnoConstrucao
		,UD.UNDO_nr_contribuinte AS InscricaoMunicipal
	FROM
		TB_UNPR_UNIDADE_PRONTA UNPR WITH (NOLOCK)
		INNER JOIN TB_MOUN_MODELO_UNIDADE MOUN WITH (NOLOCK) ON  UNPR.MOUN_CD_MODELO_UNIDADE = MOUN.MOUN_CD_MODELO_UNIDADE 
		INNER JOIN TB_CAMU_CATEGORIA_MODELO_UNIDADE CAMU WITH (NOLOCK) ON  MOUN.CAMU_CD_CATEGORIA_MODELO_UNIDADE = CAMU.CAMU_CD_CATEGORIA_MODELO_UNIDADE 
		INNER JOIN TB_CDCM_CATEGORIA_DIVISAO_CATEGORIA_MODELO CDCM WITH (NOLOCK) ON  CAMU.CAMU_CD_CATEGORIA_MODELO_UNIDADE = CDCM.CAMU_CD_CATEGORIA_MODELO_UNIDADE 
		INNER JOIN TB_CADI_CATEGORIA_DIVISAO CADI WITH (NOLOCK) ON  CDCM.CADI_CD_CATEGORIA_DIVISAO = CADI.CADI_CD_CATEGORIA_DIVISAO 
		INNER JOIN tb_UNDT_unidade_detalhe UNDT WITH (NOLOCK) ON  UNPR.UNPR_cd_unidade_pronta = UNDT.UNPR_cd_unidade_pronta 
		INNER JOIN tb_DIVI_divisao DIVI WITH (NOLOCK) ON  MOUN.DIVI_cd_divisao = DIVI.DIVI_cd_divisao 
		INNER JOIN tb_EMPR_empreendimento EMPR WITH (NOLOCK) ON  DIVI.EMPR_cd_empreendimento = EMPR.EMPR_cd_empreendimento 
		INNER JOIN tb_EMDT_Empreendimento_Detalhe EMDT WITH (NOLOCK) ON  DIVI.EMPR_cd_empreendimento = EMDT.EMPR_cd_empreendimento 
		INNER JOIN tb_UNVL_unidade_valores UNVL WITH (NOLOCK) ON  UNPR.UNPR_cd_unidade_pronta = UNVL.UNPR_cd_unidade_pronta 
		INNER JOIN vw_VENEM_endereco_empreendimento ENDERECO  WITH (NOLOCK) ON EMPR.EMPR_cd_empreendimento = ENDERECO.Cod_Empreendimento AND ENDERECO.Flag_Principal = 'S' AND ENDERECO.Bairro IS NOT NULL
		INNER JOIN TB_ENEM_ENDERECO_EMPREENDIMENTO ENEM WITH (NOLOCK) ON ENEM.ENEM_cd_endereco_empreendimento = ENDERECO.Cd_endereco_empreendimento
		LEFT JOIN tb_UNDO_unidade_documento UD WITH (NOLOCK) ON UD.UNPR_cd_unidade_pronta = UNDT.UNPR_cd_unidade_pronta 
		LEFT JOIN dbCorreios.dbo.tb_ESTA_estado ESTA WITH (NOLOCK) ON ESTA.ESTA_sg_estado = ENDERECO.Estado
		LEFT JOIN tb_UNPR_X_UTPR UTPR WITH (NOLOCK) ON unpr.UNPR_cd_unidade_pronta = UTPR.UNPR_cd_unidade_pronta and utpr.UTPR_cd_utilizacao_pronto = 2
		LEFT JOIN dbCorreios.dbo.tb_ZOGE_zona_geografica ZOGE WITH (NOLOCK) ON ZOGE.ZOGE_cd_zona_geografica = ISNULL(ENEM.ZOGE_cd_zona_geografica_marketing, ENEM.ZOGE_cd_zona_geografica) 
		LEFT JOIN dbCorreios.dbo.tb_ZOVA_zona_valor ZOVA WITH (NOLOCK) ON ZOVA.ZOVA_cd_zona_valor = COALESCE(unpr.ZOVA_cd_zona_valor_marketing,ENEM.ZOVA_cd_zona_valor_marketing, ENEM.ZOVA_cd_zona_valor)
GO
select * 
from VW_AnuncioProdutoDados 
where idProduto IN (99669)
ORDER BY IdProduto DESC