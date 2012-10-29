using System;
using DataAccess;
using SysCEF.DAO.Interface;
using SysCEF.Model;

namespace SysCEF.Web.BusinessLogic
{
    public class UploadOSHelper
    {
        #region Constantes
        // ReSharper disable InconsistentNaming
        //private const int ATIVIDADE = 9;
        private const int PRODUTO = 10;
        private const int LINHA = 11;
        private const int FONTE = 12;
        private const int REFERENCIA = 14;
        private const int CLIENTE = 15;
        private const int IDENTIFICACAO_CLIENTE = 16;
        private const int LOGRADOURO_ENDERECO = 17;
        private const int NUMERO = 18;
        private const int CEP_BAIRRO = 19;
        private const int CIDADE_UF = 20;
        //private const int PRAZO_EXECUCAO = 21;
        //private const int VALORES = 22;
        private const int SOLICITANTE = 31;
        private const int SOLICITANTE2 = 32;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Propriedades
        public IUnitOfWork UnitOfWork { get; set; }
        public ILaudoRepositorio LaudoRepository { get; set; }
        public ITipoLogradouroRepositorio TipoLogradouroRepositorio { get; set; }
        public IEstadoRepositorio EstadoRepositorio { get; set; }
        public ICidadeRepositorio CidadeRepositorio { get; set; }
        public IProdutoRepositorio ProdutoRepositorio { get; set; }
        public ILinhaRepositorio LinhaRepositorio { get; set; }
        public IFonteRepositorio FonteRepositorio { get; set; }
        #endregion

        #region Métodos Públicos
        public Laudo GerarLaudoAPartirArquivo(string fileName)
        {
            var linhasArquivo = System.IO.File.ReadAllLines(fileName);

            var referencia = ObterTexto(linhasArquivo[REFERENCIA], 30);

            var textoSolicitante = linhasArquivo[SOLICITANTE].Trim() + linhasArquivo[SOLICITANTE2].Trim();

            var laudo = LaudoRepository.ObterPorReferencia(UnitOfWork, referencia) ??
                        new Laudo
                            {
                                Referencia = referencia,
                                Solicitante = ObterSolicitante(textoSolicitante),
                                Produto = ProdutoRepositorio.Obter(UnitOfWork, ObterNumero(linhasArquivo[PRODUTO], 3)),
                                Linha = LinhaRepositorio.Obter(UnitOfWork, ObterNumero(linhasArquivo[LINHA], 3)),
                                Fonte = FonteRepositorio.Obter(UnitOfWork, ObterNumero(linhasArquivo[FONTE], 3)),
                                Status = (int) EnumStatusLaudo.Importado,
                                FormaTerreno = (int) EnumFormaTerreno.Retangular,
                                InclinacaoTerreno = (int) EnumInclinacaoTerreno.Plano,
                                SituacaoTerreno = (int) EnumSituacaoTerreno.MeioQuadra,
                                SuperficieTerreno = (int) EnumSuperficieTerreno.Seco,
                                FracaoIdealTerreno = 0.000000f,
                                AceitoComoGarantia = (int) EnumSimOuNao.Sim,
                                Conformidade = (int)EnumSimOuNao.Sim,
                                EstabilidadeSolidez = true,
                                Habitabilidade = true,
                                FatoresLiquidezValorImovel = (int) EnumFatoresLiquidezValorImovel.Nenhum
                            };

            laudo.Produto = ProdutoRepositorio.Obter(UnitOfWork, ObterNumero(linhasArquivo[PRODUTO], 3));

            laudo.Imovel = GerarImovelAPartirArquivo(linhasArquivo);

            return laudo;
        }

        public Imovel GerarImovelAPartirArquivo(string[] linhasArquivo)
        {
            var imovel = new Imovel
                       {
                           TipoLogradouro = TipoLogradouroRepositorio.ObterPorSigla(UnitOfWork, ExtrairTipoLogradouro(linhasArquivo[LOGRADOURO_ENDERECO])),
                           NomeCliente = ObterTexto(linhasArquivo[CLIENTE]),
                           IdentificacaoCliente = ObterTexto(linhasArquivo[IDENTIFICACAO_CLIENTE]),
                           Endereco = ObterEndereco(linhasArquivo[LOGRADOURO_ENDERECO]),
                           Numero = ObterNumero(linhasArquivo[NUMERO], 3),
                           Complemento = ObterTextoSegundoCampo(linhasArquivo[NUMERO]),
                           CEP = ObterTexto(linhasArquivo[CEP_BAIRRO], 10),
                           Bairro = ObterTextoSegundoCampo(linhasArquivo[CEP_BAIRRO])
                       };

            PreencherCidadeUF(linhasArquivo[CIDADE_UF], imovel);

            return imovel;
        }

        public int ObterNumero(string linha, int comprimento = 0)
        {
            var indice = linha.IndexOf(':') + 2;
            var texto = comprimento == 0 ? linha.Substring(indice).Trim() : linha.Substring(indice, comprimento).Trim();

            int codigo;
            Int32.TryParse(texto, out codigo);

            return codigo;
        }

        public string ObterTexto(string linha, int comprimento = 0)
        {
            var indice = linha.IndexOf(':') + 2;
            return comprimento == 0 ? linha.Substring(indice).Trim() : linha.Substring(indice, comprimento).Trim();
        }

        public string ExtrairTipoLogradouro(string linha)
        {
            var indice = linha.IndexOf(':') + 2;
            var tipoLogradouro = linha.Substring(indice, 3).Trim();

            var array = tipoLogradouro.Split(' ');

            return array[0];
        }

        public decimal ObterValor(string linha, int indice = 0, int comprimento = 0)
        {
            if (indice == 0)
                indice = linha.IndexOf(':') + 4; // + 4 para ignorar o 'R$'.

            var texto = comprimento == 0 ? linha.Substring(indice).Trim() : linha.Substring(indice, comprimento).Trim();

            decimal valor;
            Decimal.TryParse(texto, out valor);

            return valor;
        }
        #endregion

        #region Métodos Privados
        private string ObterSolicitante(string linha)
        {
            const string textoIndicativo = "COMPARECER NA ";

            var indice = linha.IndexOf(textoIndicativo, StringComparison.Ordinal) + textoIndicativo.Length;

            var solicitante = linha.Substring(indice).Trim();
            
            if (solicitante.IndexOf(',') > 0)
                solicitante = solicitante.Remove(solicitante.IndexOf(','));
            
            return solicitante.Replace(".", string.Empty);
        }

        private string ObterEndereco(string linha)
        {
            var siglaLogradouro = ObterTexto(linha, 3);
            var indice = linha.IndexOf(siglaLogradouro.Trim(), StringComparison.Ordinal) + 2;

            return linha.Substring(indice).Trim();
        }

        private string ObterTextoSegundoCampo(string linha)
        {
            var indice = linha.LastIndexOf(':') + 2;

            return linha.Substring(indice).Trim();
        }

        private void PreencherCidadeUF(string linha, Imovel imovel)
        {
            var cidadeEstadoTexto = ObterTexto(linha);

            var cidadeEstadoArray = cidadeEstadoTexto.Split('/');

            if (cidadeEstadoArray.Length != 2) return;

            imovel.Cidade = CidadeRepositorio.ObterPorNomeUF(UnitOfWork, cidadeEstadoArray[0].Trim(), cidadeEstadoArray[1].Trim());
            
            if (imovel.Cidade == null)
                throw new InvalidOperationException(string.Format("Não foi possível encontrar a cidade '{0}'", cidadeEstadoTexto));
        }
        #endregion
    }
}