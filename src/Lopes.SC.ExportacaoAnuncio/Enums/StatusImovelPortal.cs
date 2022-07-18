namespace Lopes.SC.ExportacaoAnuncio.Domain.Enums
{
    public enum StatusImovelPortal
    {
        /// <summary>
        /// Imóvel deve ser inserido/atualizado
        /// </summary>
        Desatualizado = 1,
        /// <summary>
        /// Imóvel deve ser removido
        /// </summary>
        ARemover = 2,
        Atualizado = 3,
        Removido = 4
    }

}