using System.Collections.Generic;
using System.Web.Mvc;
using Core;
using SysCEF.Model;

namespace SysCEF.Web.Models
{
    public class LaudoModel
    {
        public int LaudoId { get; set; }
        public string Atividade { get; set; }
        public string Produto { get; set; }
        public string Linha { get; set; }
        public string Fonte { get; set; }
        public string NomeCliente { get; set; }
        public string SiglaLogradouro { get; set; }
        public string Endereco { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public int PrazoExecucaoEmDias { get; set; }
        public string Status { get; set; }
        public string DataHoraVistoria { get; set; }
        public string ResponsavelTecnico { get; set; }
    }

    public class LaudoViewModel
    {
        #region Controle
        public Laudo Laudo { get; set; }
        public int LaudoId { get; set; }
        public string StatusLaudo { get; set; }
        #endregion

        #region Identificação
        public List<SelectListItem> ListaEstados { get; set; }
        public List<SelectListItem> ListaCidades { get; set; }
        public List<SelectListItem> ListaTiposLogradouro { get; set; }

        public List<SelectListItem> ListaFontes { get; set; }
        public List<SelectListItem> ListaLinhas { get; set; }
        public List<SelectListItem> ListaProdutos { get; set; }

        public EnumProduto Produtos { get; set; }
        public EnumLinha Linhas { get; set; }
        public EnumFonte Fontes { get; set; }
        #endregion

        #region Caracterização da Região
        public EnumUsosPredominantes UsosPredominantes { get; set; }
        public List<SelectListItem> ListaServicosPublicosComunitarios { get; set; }
        public List<SelectListItem> ListaInfraEstruturasUrbanas { get; set; }
        public string ServicosSelecionados { get; set; }
        public string InfrasSelecionadas { get; set; }
        #endregion

        #region Terreno
        public EnumFormaTerreno FormaTerreno { get; set; }
        public EnumCotaGreide CotaGreide { get; set; }
        public EnumInclinacaoTerreno InclinacaoTerreno { get; set; }
        public EnumSituacaoTerreno SituacaoTerreno { get; set; }
        public EnumSuperficieTerreno SuperficieTerreno { get; set; }
        #endregion

        #region Edificação
        public EnumTipoEdificacao TiposEdificacao { get; set; }
        public EnumUsosPredio UsosEdificacao { get; set; }
        public EnumQuantidadeMaior QuantidadePavimentosEdificacao { get; set; }
        public EnumQuantidadeMaior IdadeEdificio { get; set; }
        public EnumPosicaoEdificacao PosicoesEdificacao { get; set; }
        public EnumPadraoAcabamento PadroesAcabamento { get; set; }
        public EnumEstadoConservacao EstadosConservacao { get; set; }
        public EnumTetos Tetos { get; set; }
        public EnumFechamentoParedes TiposFechamentoParedes { get; set; }
        public EnumQuantidade NumeroVagasCobertas { get; set; }
        public EnumQuantidade NumeroVagasDescobertas { get; set; }
        public EnumQuantidade NumeroQuartos { get; set; }
        public EnumQuantidade NumeroSalas { get; set; }
        public EnumQuantidade NumeroCirculacao { get; set; }
        public EnumQuantidade NumeroBanheiros { get; set; }
        public EnumQuantidade NumeroSuites { get; set; }
        public EnumQuantidade NumeroClosets { get; set; }
        public EnumQuantidade NumeroCopas { get; set; }
        public EnumQuantidade NumeroCozinhas { get; set; }
        public EnumQuantidade NumeroAreasServico { get; set; }
        public EnumQuantidade NumeroVarandas { get; set; }
        public EnumQuantidade NumeroTerracosCobertos { get; set; }
        public EnumQuantidade NumeroTerracosDescobertos { get; set; }
        public EnumUsosPredio UsosPredio { get; set; }
        public EnumQuantidadeMaior QuantidadePavimentosPredio { get; set; }
        public EnumQuantidade QuantidadeUnidadesPredio { get; set; }
        public EnumQuantidade QuantidadeElevadores { get; set; }
        public EnumPosicaoPredio PosicoesPredio { get; set; }
        public EnumPadraoAcabamento PadroesConstrutivos { get; set; }
        public EnumEstadoConservacao EstadosConservacaoPredio { get; set; }
        public EnumQuantidadeMaior IdadeAparentePredio { get; set; }
        #endregion

        #region Avaliação
        public RadioButtonList<EnumMetodoDefinicaoValor> MetodoDefinicaoValor { get; set; }
        public EnumMetodologiaAvaliacao MetodologiaAvaliacao { get; set; }
        public RadioButtonList<EnumDesempenhoMercado> DesempenhoMercado { get; set; }
        public RadioButtonList<EnumAbsorcaoMercado> AbsorcaoMercado { get; set; }
        public RadioButtonList<EnumNivelImobiliario> NumeroOfertas { get; set; }
        public RadioButtonList<EnumNivelImobiliario> NivelDemanda { get; set; }
        #endregion

        #region Informações, Garantia, Documentação e Observações
        public RadioButtonList<EnumEstabilidadeSimOuNao> EstabilidadeSolidez { get; set; }
        public RadioButtonList<EnumVicioSimOuNao> ViciosConstrucao { get; set; }
        public RadioButtonList<EnumHabitabilidadeSimOuNao> CondicoesHabitabilidade { get; set; }
        public RadioButtonList<EnumFatoresLiquidezValorImovel> FatoresLiquidezValorImovel { get; set; }
        public RadioButtonList<EnumGarantiaSimOuNao> AceitoComoGarantia { get; set; }
        #endregion

        #region Documentação e Observações Finais
        public string DataVistoria { get; set; }
        public string HoraVistoria { get; set; }
        public List<SelectListItem> ListaComarcas { get; set; }
        public RadioButtonList<EnumConformidadeDocumentacaoSimOuNao> ConformidadeDocumentacao { get; set; }
        public List<SelectListItem> ListaRepresentantesLegais { get; set; }
        public List<SelectListItem> ListaStatusLaudo { get; set; }
        public List<SelectListItem> ListaResponsaveisTecnicos { get; set; }
        #endregion
    }

    public class ListaLaudoViewModel
    {
        public ListaLaudoViewModel(string status, IEnumerable<LaudoModel> laudos, string mensagem)
        {
            StatusLaudos = status;
            Laudos = laudos;
            Mensagem = mensagem;
        }

        public int IdLaudoSelecionado { get; set; }
        public string DataVistoria { get; set; }
        public string HoraVistoria { get; set; }
        public string StatusLaudos { get; set; }
        public string Mensagem { get; set; }
        public Usuario ResponsavelTecnico { get; set; }
        public List<SelectListItem> ListaResponsaveisTecnicos { get; set; }
        public IEnumerable<LaudoModel> Laudos { get; set; }
    }
}