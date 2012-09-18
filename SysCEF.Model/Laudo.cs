//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

//Generated using version %PRODUCTVERSION% of the NHibernate DSL Tool

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SysCEF.Model
{
	[Serializable]
	[DataContract]
	// ReSharper disable PartialTypeWithSinglePart
	public partial class Laudo
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Constructors

		partial void BeforeInitialized();
		partial void AfterInitialized();

		public Laudo()
		{
			BeforeInitialized();
			
			Solicitante = @"";
			
			AfterInitialized();
		}

		#endregion Constructors

		#region Private Fields

		#pragma warning disable 0169

		[DataMember]
		// ReSharper disable InconsistentNaming
		///<summary>Field to facilitate optimistic locking</summary>
		private int __Version;
		// ReSharper restore InconsistentNaming

		#pragma warning restore 0169

		#endregion Private Fields

		#region Properties

		[DataMember]
		public virtual int LaudoID { get; set; }

		[DataMember]
		public virtual int Produto { get; set; }

		[DataMember]
		public virtual int Linha { get; set; }

		[DataMember]
		public virtual int Fonte { get; set; }

		[DataMember]
		public virtual int Status { get; set; }

		[DataMember]
		public virtual DateTime? DataHoraVistoria { get; set; }

		[DataMember]
		public virtual int UsosPredominantes { get; set; }

		[DataMember]
		public virtual int FormaTerreno { get; set; }

		[DataMember]
		public virtual int CotaGreideTerreno { get; set; }

		[DataMember]
		public virtual int InclinacaoTerreno { get; set; }

		[DataMember]
		public virtual int SituacaoTerreno { get; set; }

		[DataMember]
		public virtual int SuperficieTerreno { get; set; }

		[DataMember]
		public virtual decimal MedidaAreaTerreno { get; set; }

		[DataMember]
		public virtual decimal MedidaFrenteTerreno { get; set; }

		[DataMember]
		public virtual decimal MedidaFundosTerreno { get; set; }

		[DataMember]
		public virtual decimal MedidaDireitaTerreno { get; set; }

		[DataMember]
		public virtual decimal MedidaEsquerdaTerreno { get; set; }

		[DataMember]
		public virtual double FracaoIdealTerreno { get; set; }

		[DataMember]
		public virtual int TipoEdificacao { get; set; }

		[DataMember]
		public virtual int UsoEdificacao { get; set; }

		[DataMember]
		public virtual int NumeroPavimentos { get; set; }

		[DataMember]
		public virtual int IdadeEdificio { get; set; }

		[DataMember]
		public virtual int PosicaoEdificacao { get; set; }

		[DataMember]
		public virtual int PadraoAcabamento { get; set; }

		[DataMember]
		public virtual int EstadoConservacao { get; set; }

		[DataMember]
		public virtual int Tetos { get; set; }

		[DataMember]
		public virtual int FechamentoParedes { get; set; }

		[DataMember]
		public virtual int NumeroVagasCobertas { get; set; }

		[DataMember]
		public virtual int NumeroVagasDescobertas { get; set; }

		[DataMember]
		public virtual decimal AreaUnidadePrivativa { get; set; }

		[DataMember]
		public virtual decimal AreaUnidadeComum { get; set; }

		[DataMember]
		public virtual decimal AreaUnidadeTotal { get; set; }

		[DataMember]
		public virtual decimal AreaEstacionamentoPrivativa { get; set; }

		[DataMember]
		public virtual decimal AreaEstacionamentoComum { get; set; }

		[DataMember]
		public virtual decimal AreaEstacionamentoTotal { get; set; }

		[DataMember]
		public virtual decimal AreaOutrosPrivativa { get; set; }

		[DataMember]
		public virtual decimal AreaOutrosComum { get; set; }

		[DataMember]
		public virtual decimal AreaOutrosTotal { get; set; }

		[DataMember]
		public virtual decimal AreaTotalPrivativa { get; set; }

		[DataMember]
		public virtual decimal AreaTotalComum { get; set; }

		[DataMember]
		public virtual decimal AreaTotalAverbada { get; set; }

		[DataMember]
		public virtual decimal AreaTotalNaoAverbada { get; set; }

		[DataMember]
		public virtual decimal SomatorioAreas { get; set; }

		[DataMember]
		public virtual int NumeroQuartos { get; set; }

		[DataMember]
		public virtual int NumeroSalas { get; set; }

		[DataMember]
		public virtual int NumeroCirculacao { get; set; }

		[DataMember]
		public virtual int NumeroBanheiros { get; set; }

		[DataMember]
		public virtual int NumeroSuites { get; set; }

		[DataMember]
		public virtual int NumeroCopas { get; set; }

		[DataMember]
		public virtual int NumeroCozinhas { get; set; }

		[DataMember]
		public virtual int NumeroAreasServico { get; set; }

		[DataMember]
		public virtual int NumeroVarandas { get; set; }

		[DataMember]
		public virtual int NumeroTerracosCobertos { get; set; }

		[DataMember]
		public virtual int NumeroTerracosDescobertos { get; set; }

		[DataMember]
		public virtual int UsoPredio { get; set; }

		[DataMember]
		public virtual int NumeroPavimentosPredio { get; set; }

		[DataMember]
		public virtual int NumeroElevadoresPredio { get; set; }

		[DataMember]
		public virtual int PosicaoPredio { get; set; }

		[DataMember]
		public virtual int PadraoConstrutivoPredio { get; set; }

		[DataMember]
		public virtual int EstadoConservacaoPredio { get; set; }

		[DataMember]
		public virtual string IdentificacaoPavimentosPredio { get; set; }

		[DataMember]
		public virtual int IdadeAparentePredio { get; set; }

		[DataMember]
		public virtual decimal ValorAvaliacao { get; set; }

		[DataMember]
		public virtual string ValorAvaliacaoExtenso { get; set; }

		[DataMember]
		public virtual int MetodoDefinicaoValor { get; set; }

		[DataMember]
		public virtual decimal AreaGlobal { get; set; }

		[DataMember]
		public virtual decimal ValorMetroQuadradoGlobal { get; set; }

		[DataMember]
		public virtual decimal ValorTotalGlobal { get; set; }

		[DataMember]
		public virtual decimal AreaTerreno { get; set; }

		[DataMember]
		public virtual decimal AreaEdificacao { get; set; }

		[DataMember]
		public virtual decimal AreaBenfeitorias { get; set; }

		[DataMember]
		public virtual decimal ValorMetroQuadradoTerreno { get; set; }

		[DataMember]
		public virtual decimal ValorMetroQuadradoEdificacao { get; set; }

		[DataMember]
		public virtual decimal ValorMetroQuadradoBenfeitorias { get; set; }

		[DataMember]
		public virtual decimal ProdutoTerreno { get; set; }

		[DataMember]
		public virtual decimal ProdutoEdificacao { get; set; }

		[DataMember]
		public virtual decimal ProdutoBenfeitorias { get; set; }

		[DataMember]
		public virtual decimal ValorTotalItemizada { get; set; }

		[DataMember]
		public virtual string PrecisaoFundamentacao { get; set; }

		[DataMember]
		public virtual int MetodologiaAvaliacao { get; set; }

		[DataMember]
		public virtual int DesempenhoMercado { get; set; }

		[DataMember]
		public virtual int AbsorcaoMercado { get; set; }

		[DataMember]
		public virtual int NivelOfertas { get; set; }

		[DataMember]
		public virtual int NivelDemanda { get; set; }

		[DataMember]
		public virtual string ObservacoesAvaliacao { get; set; }

		[DataMember]
		public virtual bool EstabilidadeSolidez { get; set; }

		[DataMember]
		public virtual string EstabilidadeSolidezJustificativa { get; set; }

		[DataMember]
		public virtual bool ViciosConstrucao { get; set; }

		[DataMember]
		public virtual string ViciosConstrucaoRelacao { get; set; }

		[DataMember]
		public virtual bool Habitabilidade { get; set; }

		[DataMember]
		public virtual string HabitabilidadeJustificativa { get; set; }

		[DataMember]
		public virtual int FatoresLiquidezValorImovel { get; set; }

		[DataMember]
		public virtual int AceitoComoGarantia { get; set; }

		[DataMember]
		public virtual string MatriculaRGI { get; set; }

		[DataMember]
		public virtual string Oficio { get; set; }

		[DataMember]
		public virtual string OutrosDocumentos { get; set; }

		[DataMember]
		public virtual int Conformidade { get; set; }

		[DataMember]
		public virtual string Divergencia { get; set; }

		[DataMember]
		public virtual string ObservacoesFinais { get; set; }

		[DataMember]
		public virtual string LocalEmissaoLaudo { get; set; }

		[DataMember]
		public virtual string Referencia { get; set; }

		[DataMember]
		public virtual int NumeroClosets { get; set; }

		[DataMember]
		public virtual int NumeroUnidadesPredio { get; set; }

		[DataMember]
		public virtual string Solicitante { get; set; }

		[DataMember]
		public virtual string FatoresLiquidezExplicitacao { get; set; }

		[DataMember]
		public virtual Imovel Imovel { get; set; }

		[DataMember]
		public virtual Cidade Comarca { get; set; }

		[DataMember]
		public virtual Usuario ResponsavelTecnico { get; set; }

		[DataMember]
		public virtual Usuario RepresentanteLegalEmpresa { get; set; }

		[DataMember]
		public virtual IList<ServicoPublicoComunitario> ListaServicoPublicoComunitario  { get; set; }

		[DataMember]
		public virtual IList<InfraEstruturaUrbana> ListaInfraEstruturaUrbana  { get; set; }

		#endregion Properties

		#region Object Overrides

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (GetType() != obj.GetType())
				return false;

			return LaudoID <= 0 ? false : LaudoID == ((Laudo) obj).LaudoID;
		}

		public override int GetHashCode()
		{
			if (LaudoID <= 0)
				return base.GetHashCode();

			//Use 37 as a multiplier as it is a relatively large prime number
			//which helps to avoid collisions in a hashed data structure
			return 37 * LaudoID;
		}

		#endregion Object Overrides

	}
}
