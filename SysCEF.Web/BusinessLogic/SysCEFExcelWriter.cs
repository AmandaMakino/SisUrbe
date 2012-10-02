using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Core;
using SysCEF.Model;
using Microsoft.Office.Interop.Excel;

namespace SysCEF.Web.BusinessLogic
{
    public class SysCEFExcelWriter
    {
        public string NomeArquivo { get; set; }
        private Application AplicacaoExcel { get; set; }
        private Workbook Workbook { get; set; }
        private Worksheet WorksheetAtual { get; set; }
        private string CaminhoWorkbook { get; set; }
        private Configuracao Configuracao { get; set; }

        public SysCEFExcelWriter(string caminhoPastaServidor, string referenciaLaudo, Configuracao configuracao)
        {
            Configuracao = configuracao;

            // Define o nome e o caminho do arquivo.
            NomeArquivo = string.Format("UPredio_{0}.xls", referenciaLaudo.Replace("/", "_"));
            CaminhoWorkbook = Path.Combine(caminhoPastaServidor, NomeArquivo);
            
            CriarCopia(Path.Combine(caminhoPastaServidor, "Template.xls"));
        }

        private void CriarCopia(string nomeArquivoOriginal)
        {
            if (!File.Exists(nomeArquivoOriginal))
                throw new InvalidOperationException("Não foi possível encontrar o template nesse caminho: " + nomeArquivoOriginal);
            
            File.Copy(nomeArquivoOriginal, CaminhoWorkbook, true);
        }

        public void PreencherCampo(string celulaInicial, string celulaFinal, string dado, string formato = "")
        {
            var worksheetRange = WorksheetAtual.Range[celulaInicial, celulaFinal];

            worksheetRange.Value = dado ?? string.Empty;
            
            if (!string.IsNullOrEmpty(formato))
                worksheetRange.NumberFormat = formato;
        }

        public bool TentarSelecionarWorksheet(string worksheetName)
        {
            var sucesso = false;

            if (!string.IsNullOrEmpty(worksheetName))
            {
                foreach (Worksheet ws in Workbook.Sheets)
                {
                    if (ws.Name == worksheetName)
                    {
                        WorksheetAtual = ws;
                        sucesso = true;
                    }
                }
            }

            return sucesso;
        }
        
        public void SalvarFecharArquivo()
        {
            Workbook.Save();
            AplicacaoExcel.ActiveWorkbook.Close(false, Missing.Value, Missing.Value);
            AplicacaoExcel.Quit();
        }

        public void ExcluirArquivo()
        {
            File.Delete(CaminhoWorkbook);
        }
        
        public void PreencherCampos(Laudo laudo)
        {
            AplicacaoExcel = new Application { Visible = false };

            // Abre a cópia
            Workbook = AplicacaoExcel.Workbooks.Open(CaminhoWorkbook, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            try
            {
                #region Laudo fl 1
                if (TentarSelecionarWorksheet("Laudo fl 1"))
                {
                    #region Cabeçalho
                    PreencherCampo("L8", "V8", laudo.Solicitante);
                    PreencherCampo("W8", "AF8", laudo.Referencia);
                    #endregion

                    #region Identificação
                    PreencherCampo("B12", "U12", EnumHelper.GetDescription((EnumProduto)laudo.Produto));
                    PreencherCampo("W12", "AI12", EnumHelper.GetDescription((EnumLinha)laudo.Linha));

                    PreencherCampo("B15", "U15", EnumHelper.GetDescription((EnumFonte)laudo.Fonte));

                    PreencherCampo("B18", "U18", laudo.Imovel.NomeCliente);
                    PreencherCampo("W18", "AI18", laudo.Imovel.TipoLogradouro.Descricao);

                    PreencherCampo("B21", "U21", string.Format("{0}, {1}", laudo.Imovel.Endereco, laudo.Imovel.Numero));
                    PreencherCampo("W21", "AI21", laudo.Imovel.Complemento);

                    PreencherCampo("B24", "L24", laudo.Imovel.Bairro);
                    PreencherCampo("W24", "AG24", laudo.Imovel.Cidade.Nome);
                    PreencherCampo("AH24", "AI24", laudo.Imovel.Cidade.Estado.Sigla);
                    #endregion

                    #region Caracterização da Região
                    #region Usos Predominantes
                    var usoPredominante = ((EnumUsosPredominantes)laudo.UsosPredominantes).ToString();

                    var objeto = (OLEObject)WorksheetAtual.OLEObjects(usoPredominante);
                    if (objeto != null)
                        objeto.Object.Value = 1;
                    #endregion
                    
                    SelecionarServicosPublicos(laudo);

                    SelecionarInfraEstruturasUrbanas(laudo);
                    #endregion

                    #region Terreno
                    PreencherCampo("B35", "F35", EnumHelper.GetDescription((EnumFormaTerreno)laudo.FormaTerreno));
                    PreencherCampo("G35", "O35", EnumHelper.GetDescription((EnumCotaGreide)laudo.CotaGreideTerreno));
                    PreencherCampo("P35", "U35", EnumHelper.GetDescription((EnumInclinacaoTerreno)laudo.InclinacaoTerreno));
                    PreencherCampo("W35", "AD35", EnumHelper.GetDescription((EnumSituacaoTerreno)laudo.SituacaoTerreno));
                    PreencherCampo("AE35", "AI35", EnumHelper.GetDescription((EnumSuperficieTerreno)laudo.SuperficieTerreno));

                    PreencherCampo("B38", "F38", laudo.MedidaAreaTerreno.ToString(new CultureInfo("pt-BR")));
                    PreencherCampo("H38", "K38", laudo.MedidaFrenteTerreno.ToString(new CultureInfo("pt-BR")));
                    PreencherCampo("M38", "P38", laudo.MedidaFundosTerreno.ToString(new CultureInfo("pt-BR")));
                    PreencherCampo("R38", "U38", laudo.MedidaEsquerdaTerreno.ToString(new CultureInfo("pt-BR")));
                    PreencherCampo("X38", "AA38", laudo.MedidaDireitaTerreno.ToString(new CultureInfo("pt-BR")));
                    PreencherCampo("AC38", "AI38", laudo.FracaoIdealTerreno.ToString(new CultureInfo("pt-BR")));
                    #endregion

                    #region Edificação
                    PreencherCampo("B43", "G43", EnumHelper.GetDescription((EnumTipoEdificacao)laudo.TipoEdificacao));
                    PreencherCampo("H43", "N43", EnumHelper.GetDescription((EnumUsosPredio)laudo.UsoEdificacao));
                    PreencherCampo("O43", "T43", laudo.NumeroPavimentos.ToString(CultureInfo.InvariantCulture));
                    PreencherCampo("U43", "AA43", laudo.IdadeEdificio.ToString(CultureInfo.InvariantCulture));
                    PreencherCampo("AB43", "AI43", EnumHelper.GetDescription((EnumPosicaoEdificacao)laudo.PosicaoEdificacao));

                    PreencherCampo("B46", "G46", EnumHelper.GetDescription((EnumPadraoAcabamento)laudo.PadraoAcabamento));
                    PreencherCampo("H46", "L46", EnumHelper.GetDescription((EnumEstadoConservacao)laudo.EstadoConservacao));
                    PreencherCampo("M46", "P46", EnumHelper.GetDescription((EnumTetos)laudo.Tetos));
                    PreencherCampo("Q46", "W46", EnumHelper.GetDescription((EnumFechamentoParedes)laudo.FechamentoParedes));
                    PreencherCampo("X46", "Y46", laudo.NumeroVagasCobertas.ToString(CultureInfo.InvariantCulture));
                    PreencherCampo("AD46", "AE46", laudo.NumeroVagasDescobertas.ToString(CultureInfo.InvariantCulture));

                    PreencherCampo("G49", "J49", laudo.AreaUnidadePrivativa.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("L49", "O49", laudo.AreaUnidadeComum.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("Q49", "T49", laudo.AreaUnidadeTotal.ToString(CultureInfo.CurrentUICulture));

                    PreencherCampo("G50", "J50", laudo.AreaEstacionamentoPrivativa.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("L50", "O50", laudo.AreaEstacionamentoComum.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("Q50", "T50", laudo.AreaEstacionamentoTotal.ToString(CultureInfo.CurrentUICulture));

                    PreencherCampo("G51", "J51", laudo.AreaOutrosPrivativa.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("L51", "O51", laudo.AreaOutrosComum.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("Q51", "T51", laudo.AreaOutrosTotal.ToString(CultureInfo.CurrentUICulture));

                    PreencherCampo("G52", "J52", laudo.AreaTotalPrivativa.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("L52", "O52", laudo.AreaTotalComum.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("Q52", "T52", laudo.AreaTotalAverbada.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("Y52", "AB52", laudo.AreaTotalNaoAverbada.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("AE52", "AH52", laudo.SomatorioAreas.ToString(CultureInfo.CurrentUICulture));

                    PreencherCampo("B56", "AI58", ObterDivisaoInterna(laudo));

                    PreencherCampo("B62", "M62", EnumHelper.GetDescription((EnumUsosPredio)laudo.UsoPredio));
                    PreencherCampo("N62", "R62", laudo.NumeroPavimentosPredio.ToString(CultureInfo.InvariantCulture));
                    PreencherCampo("S62", "W62", laudo.NumeroUnidadesPredio.ToString(CultureInfo.InvariantCulture));
                    PreencherCampo("X62", "AB62", laudo.NumeroElevadoresPredio.ToString(CultureInfo.InvariantCulture));
                    PreencherCampo("AC62", "AI62", EnumHelper.GetDescription((EnumPosicaoPredio)laudo.PosicaoPredio));

                    PreencherCampo("B65", "F65", EnumHelper.GetDescription((EnumPadraoAcabamento)laudo.PadraoAcabamento));
                    PreencherCampo("G65", "L65", EnumHelper.GetDescription((EnumEstadoConservacao)laudo.EstadoConservacaoPredio));
                    PreencherCampo("M65", "AE65", laudo.IdentificacaoPavimentosPredio);
                    PreencherCampo("AF65", "AI65", laudo.IdadeAparentePredio.ToString(CultureInfo.InvariantCulture));
                    #endregion

                    #region Avaliação
                    PreencherCampo("B69", "G69", String.Format(CultureInfo.CurrentUICulture, "{0:C}", laudo.ValorAvaliacao));
                    PreencherCampo("H69", "AI69", laudo.ValorAvaliacaoExtenso);

                    PreencherCampo("G73", "K73", laudo.AreaGlobal.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("Q73", "T73", laudo.AreaTerreno.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("W73", "Z73", laudo.AreaEdificacao.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("AD73", "AH73", laudo.AreaBenfeitorias.ToString(CultureInfo.CurrentUICulture));

                    PreencherCampo("G74", "K74", laudo.ValorMetroQuadradoGlobal.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("Q74", "T74", laudo.ValorMetroQuadradoTerreno.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("W74", "Z74", laudo.ValorMetroQuadradoEdificacao.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("AD74", "AH74", laudo.ValorMetroQuadradoBenfeitorias.ToString(CultureInfo.CurrentUICulture));

                    PreencherCampo("Q75", "T75", laudo.ProdutoTerreno.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("W75", "Z75", laudo.ProdutoEdificacao.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("AD75", "AH75", laudo.ProdutoBenfeitorias.ToString(CultureInfo.CurrentUICulture));

                    PreencherCampo("G76", "K76", laudo.ValorTotalGlobal.ToString(CultureInfo.CurrentUICulture));
                    PreencherCampo("AD76", "AH76", laudo.ValorTotalItemizada.ToString(CultureInfo.CurrentUICulture));

                    PreencherCampo("B80", "L80", laudo.PrecisaoFundamentacao);
                    PreencherCampo("M80", "AI80", EnumHelper.GetDescription((EnumMetodologiaAvaliacao)laudo.MetodologiaAvaliacao));

                    PreencherCampo("B83", "I83", EnumHelper.GetDescription((EnumDesempenhoMercado)laudo.DesempenhoMercado));
                    PreencherCampo("J83", "S83", EnumHelper.GetDescription((EnumAbsorcaoMercado)laudo.AbsorcaoMercado));
                    PreencherCampo("T83", "AC83", EnumHelper.GetDescription((EnumNivelImobiliario)laudo.NivelOfertas));
                    PreencherCampo("AD83", "AI83", EnumHelper.GetDescription((EnumNivelImobiliario)laudo.NivelDemanda));
                    #endregion

                    #region Rodapé
                    if (Configuracao != null)
                        PreencherCampo("G90", "AI90", string.Format("{0} / {1}", Configuracao.NomeEmpresa, Configuracao.CNPJEmpresa));

                    if (!string.IsNullOrEmpty(laudo.LocalEmissaoLaudo))
                        PreencherCampo("B93", "AC93", string.Format("{0}  /  {1}", laudo.LocalEmissaoLaudo, DateTime.Now.Date.ToString("dd/MM/yyyy")));

                    if (laudo.ResponsavelTecnico != null)
                    {
                        PreencherCampo("E96", "Q96", string.Format("{0} / {1}", laudo.ResponsavelTecnico.Nome.ToUpper(), laudo.ResponsavelTecnico.CREA));
                        PreencherCampo("E97", "Q97", laudo.ResponsavelTecnico.CPF);
                    }

                    if (laudo.RepresentanteLegalEmpresa != null)
                    {
                        PreencherCampo("T96", "AC96", laudo.RepresentanteLegalEmpresa.Nome.ToUpper());
                        PreencherCampo("T97", "AC97", laudo.RepresentanteLegalEmpresa.CPF);
                    }
                    #endregion
                }
                #endregion

                #region Laudo fl 2
                if (TentarSelecionarWorksheet("Laudo fl 2"))
                {
                    #region Cabeçalho
                    PreencherCampo("L6", "V6", laudo.Solicitante);
                    PreencherCampo("W6", "AF6", laudo.Referencia);
                    #endregion

                    #region Informações Complementares
                    SelecionarOpcao(laudo.EstabilidadeSolidez ? "EstSim" : "EstNao");

                    PreencherCampo("C12", "AH12", laudo.EstabilidadeSolidezJustificativa);

                    SelecionarOpcao(laudo.ViciosConstrucao ? "VicioSim" : "VicioNao");

                    PreencherCampo("C17", "AH17", laudo.ViciosConstrucaoRelacao);

                    SelecionarOpcao(laudo.Habitabilidade ? "HabitSim" : "HabitNao");

                    PreencherCampo("C22", "AH22", laudo.HabitabilidadeJustificativa);

                    switch ((EnumFatoresLiquidezValorImovel)laudo.FatoresLiquidezValorImovel)
                    {
                        case EnumFatoresLiquidezValorImovel.Valorizantes:
                            SelecionarOpcao("Val");
                            break;
                        case EnumFatoresLiquidezValorImovel.Desvalorizantes:
                            SelecionarOpcao("Desval");
                            break;
                        case EnumFatoresLiquidezValorImovel.Nenhum:
                            SelecionarOpcao("Nenh");
                            break;
                    }

                    PreencherCampo("C28", "AH28", laudo.FatoresLiquidezExplicitacao);
                    #endregion

                    #region Garantia, Documentação Apresentada e Observações
                    SelecionarOpcao(laudo.AceitoComoGarantia == 0 ? "GarSim" : "GarNao");

                    PreencherCampo("B37", "H37", laudo.MatriculaRGI);
                    PreencherCampo("I37", "S37", laudo.Oficio);
                    PreencherCampo("T37", "AH37", laudo.Comarca == null ? string.Empty : laudo.Comarca.Nome);

                    PreencherCampo("B40", "AH40", laudo.OutrosDocumentos);

                    SelecionarOpcao(laudo.Conformidade == 0 ? "DocSim" : "DocNao");

                    PreencherCampo("C45", "AH45", laudo.Divergencia);

                    PreencherCampo("C49", "AH59", laudo.ObservacoesFinais);
                    #endregion

                    #region Rodapé
                    if (Configuracao != null)
                        PreencherCampo("G69", "AI69", string.Format("{0} / {1}", Configuracao.NomeEmpresa, Configuracao.CNPJEmpresa));

                    if (!string.IsNullOrEmpty(laudo.LocalEmissaoLaudo))
                        PreencherCampo("B72", "AC72", string.Format("{0} / {1}", laudo.LocalEmissaoLaudo, DateTime.Now.Date.ToString("dd/MM/yyyy")));

                    if (laudo.ResponsavelTecnico != null)
                    {
                        PreencherCampo("E75", "Q75", string.Format("{0} / {1}", laudo.ResponsavelTecnico.Nome.ToUpper(), laudo.ResponsavelTecnico.CREA));
                        PreencherCampo("E76", "Q76", laudo.ResponsavelTecnico.CPF);
                    }

                    if (laudo.RepresentanteLegalEmpresa != null)
                    {
                        PreencherCampo("T75", "AC75", laudo.RepresentanteLegalEmpresa.Nome.ToUpper());
                        PreencherCampo("T76", "AC76", laudo.RepresentanteLegalEmpresa.CPF);                        
                    }
                    #endregion
                }
                #endregion

                #region Laudo fl 3
                if (TentarSelecionarWorksheet("Laudo fl 3"))
                {
                    #region Cabeçalho
                    PreencherCampo("L6", "V6", laudo.Solicitante);
                    PreencherCampo("W6", "AF6", laudo.Referencia);
                    #endregion

                    #region Identificação
                    PreencherCampo("B10", "U10", EnumHelper.GetDescription((EnumProduto)laudo.Produto));
                    PreencherCampo("W10", "AI10", EnumHelper.GetDescription((EnumLinha)laudo.Linha));

                    PreencherCampo("B13", "U13", EnumHelper.GetDescription((EnumFonte)laudo.Fonte));

                    PreencherCampo("B16", "U16", laudo.Imovel.NomeCliente);
                    PreencherCampo("W16", "AI16", laudo.Imovel.TipoLogradouro.Descricao);

                    PreencherCampo("B19", "U19", string.Format("{0}, {1}", laudo.Imovel.Endereco, laudo.Imovel.Numero));
                    PreencherCampo("W19", "AI19", laudo.Imovel.Complemento);

                    PreencherCampo("B22", "L22", laudo.Imovel.Bairro);
                    PreencherCampo("W22", "AG22", laudo.Imovel.Cidade.Nome);
                    PreencherCampo("AH22", "AI22", laudo.Imovel.Cidade.Estado.Sigla);
                    #endregion

                    #region Rodapé
                    if (Configuracao != null)
                        PreencherCampo("G34", "AI34", string.Format("{0} / {1}", Configuracao.NomeEmpresa, Configuracao.CNPJEmpresa));

                    if (!string.IsNullOrEmpty(laudo.LocalEmissaoLaudo))
                        PreencherCampo("B37", "AC37", string.Format("{0} / {1}", laudo.LocalEmissaoLaudo, DateTime.Now.Date.ToString("dd/MM/yyyy")));

                    if (laudo.ResponsavelTecnico != null)
                    {
                        PreencherCampo("E40", "Q40", string.Format("{0} / {1}", laudo.ResponsavelTecnico.Nome.ToUpper(), laudo.ResponsavelTecnico.CREA));
                        PreencherCampo("E41", "Q41", laudo.ResponsavelTecnico.CPF);
                    }

                    if (laudo.RepresentanteLegalEmpresa != null)
                    {
                        PreencherCampo("T40", "AC40", laudo.RepresentanteLegalEmpresa.Nome.ToUpper());
                        PreencherCampo("T41", "AC41", laudo.RepresentanteLegalEmpresa.CPF);
                    }
                    #endregion
                }
                #endregion
            }
            catch
            {
                AplicacaoExcel.ActiveWorkbook.Close(false, Missing.Value, Missing.Value);
                AplicacaoExcel.Quit();
                ExcluirArquivo();
                throw;
            }
        }

        private void SelecionarOpcao(string opcao)
        {
            var objeto = (OLEObject)WorksheetAtual.OLEObjects(opcao);

            if (objeto != null)
                objeto.Object.Value = 1;
        }

        private void SelecionarInfraEstruturasUrbanas(Laudo laudo)
        {
            foreach (var infra in laudo.ListaInfraEstruturaUrbana)
            {
                var objeto = (OLEObject)WorksheetAtual.OLEObjects(((EnumInfraEstruturaUrbana)infra.TipoInfraEstruturaUrbana).ToString());

                if (objeto != null)
                    objeto.Object.Value = 1;
            }           
        }

        private void SelecionarServicosPublicos(Laudo laudo)
        {
            foreach (var servico in laudo.ListaServicoPublicoComunitario)
            {
                var objeto = (OLEObject)WorksheetAtual.OLEObjects(((EnumServicoPublicoComunitario)servico.TipoServicoPublicoComunitario).ToString());

                if (objeto != null)
                    objeto.Object.Value = 1;
            } 
        }

        private string ObterDivisaoInterna(Laudo laudo)
        {
            var divisaoInterna = new StringBuilder();

            if (laudo.NumeroQuartos > 0)
                divisaoInterna.Append(laudo.NumeroQuartos).Append(laudo.NumeroQuartos > 1 ? " QUARTOS" : " QUARTO").Append("; ");

            if (laudo.NumeroSalas > 0)
                divisaoInterna.Append(laudo.NumeroSalas).Append(laudo.NumeroSalas > 1 ? " SALAS" : " SALA").Append("; ");

            if (laudo.NumeroCirculacao > 0)
                divisaoInterna.Append(laudo.NumeroCirculacao).Append(laudo.NumeroCirculacao > 1 ? " CIRCULAÇÕES" : " CIRCULAÇÃO").Append("; ");

            if (laudo.NumeroBanheiros > 0)
                divisaoInterna.Append(laudo.NumeroBanheiros).Append(laudo.NumeroBanheiros > 1 ? " BANHEIROS" : " BANHEIRO").Append("; ");
            
            if (laudo.NumeroSuites > 0)
                divisaoInterna.Append(laudo.NumeroSuites).Append(laudo.NumeroSuites > 1 ? " SUÍTES" : " SUÍTE").Append("; ");

            if (laudo.NumeroClosets > 0)
                divisaoInterna.Append(laudo.NumeroClosets).Append(laudo.NumeroClosets > 1 ? " CLOSETS" : " CLOSET").Append("; ");

            if (laudo.NumeroCopas > 0)
                divisaoInterna.Append(laudo.NumeroCopas).Append(laudo.NumeroCopas > 1 ? " COPAS" : " COPA").Append("; ");

            if (laudo.NumeroCozinhas > 0)
                divisaoInterna.Append(laudo.NumeroCozinhas).Append(laudo.NumeroCozinhas > 1 ? " COZINHAS" : " COZINHA").Append("; ");

            if (laudo.NumeroAreasServico > 0)
                divisaoInterna.Append(laudo.NumeroAreasServico).Append(laudo.NumeroAreasServico > 1 ? " ÁREAS DE SERVIÇO" : " ÁREA DE SERVIÇO").Append("; ");

            if (laudo.NumeroVarandas > 0)
                divisaoInterna.Append(laudo.NumeroVarandas).Append(laudo.NumeroVarandas > 1 ? " VARANDAS" : " VARANDA").Append("; ");
            
            if (laudo.NumeroTerracosCobertos > 0)
                divisaoInterna.Append(laudo.NumeroTerracosCobertos).Append(laudo.NumeroTerracosCobertos > 1 ? " TERRAÇOS COBERTOS" : " TERRAÇO COBERTO").Append("; ");
            
            if (laudo.NumeroTerracosDescobertos > 0)
                divisaoInterna.Append(laudo.NumeroTerracosDescobertos).Append(laudo.NumeroTerracosDescobertos > 1 ? " TERRAÇOS DESCOBERTOS" : " TERRAÇO DESCOBERTO").Append("; ");

            return divisaoInterna.ToString(0, divisaoInterna.Length - 2); // Ignora os dois últimos caracteres da string para que não acabe em "; "
        }
    }
}