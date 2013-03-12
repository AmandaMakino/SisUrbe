using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Core;
using System.Linq;
using SysCEF.Model;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Collections.Generic;
using SysCEF.Web.Models;
using SysCEF.DAO.Implementacao;

namespace SysCEF.Web.Helpers
{
    public class OpenXmlHelper
    {
        private WorkbookPart _wbPart;
        private SpreadsheetDocument _document;
        private SharedStringTablePart _sharedStringPart;
        private Worksheet _activeWorksheet;
        private Controls _controlesFolhaAtual;

        #region Public Methods
        public List<object[]> LerPlanilhaDados(Stream stream)
        {
            List<object[]> dadosPlanilha = new List<object[]>();

            using (_document = SpreadsheetDocument.Open(stream, true))
            {
                try
                {
                    _wbPart = _document.WorkbookPart;
                    _sharedStringPart = GetSharedStringPart();

                    if (SelectFirstSheet())
                    {
                        int indice = 0;

                        foreach (Row row in _activeWorksheet.Descendants<Row>())
                        {
                            indice++;

                            if (indice == 1) continue; // Pula o cabeçalho.

                            List<object> dadosLinha = new List<object>();

                            foreach (Cell celula in row.Descendants<Cell>())
                                dadosLinha.Add(GetCellValue(celula));

                            dadosPlanilha.Add(dadosLinha.ToArray());
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    _document.Close();
                }

                return dadosPlanilha;
            }
        }

        public void PreencherPlanilha(MemoryStream memoryStream, Laudo laudo, Configuracao configuracao)
        {
            using (_document = SpreadsheetDocument.Open(memoryStream, true))
            {
                try
                {
                    _wbPart = _document.WorkbookPart;
                    _sharedStringPart = GetSharedStringPart();

                    #region Laudo fl 1
                    if (SelectSheet("Laudo fl 1"))
                    {
                        #region Cabeçalho
                        PreencherCampo("L8", laudo.Solicitante);
                        PreencherCampo("W8", laudo.Referencia);
                        #endregion

                        #region Identificação
                        if (laudo.Produto != null)
                            PreencherCampo("B12", laudo.Produto.Descricao);

                        if (laudo.Linha != null)
                            PreencherCampo("W12", laudo.Linha.Descricao);

                        if (laudo.Fonte != null)
                            PreencherCampo("B15", laudo.Fonte.Descricao);

                        PreencherCampo("B18", laudo.Imovel.NomeCliente);
                        PreencherCampo("W18", laudo.Imovel.TipoLogradouro.Descricao);

                        PreencherCampo("B21", string.Format("{0}, {1}", laudo.Imovel.Endereco, laudo.Imovel.Numero));
                        PreencherCampo("W21", laudo.Imovel.Complemento);

                        PreencherCampo("B24", laudo.Imovel.Bairro);
                        PreencherCampo("W24", laudo.Imovel.Cidade.Nome.ToUpper());
                        PreencherCampo("AH24", laudo.Imovel.Cidade.Estado.Sigla);
                        #endregion

                        #region Caracterização da Região
                        SelecionarOpcao(((EnumUsosPredominantes)laudo.UsosPredominantes).ToString());

                        SelecionarServicosPublicos(laudo);

                        SelecionarInfraEstruturasUrbanas(laudo);
                        #endregion

                        #region Terreno
                        PreencherCampo("B35", EnumHelper.GetDescription((EnumFormaTerreno)laudo.FormaTerreno));
                        PreencherCampo("G35", EnumHelper.GetDescription((EnumCotaGreide)laudo.CotaGreideTerreno));
                        PreencherCampo("P35", EnumHelper.GetDescription((EnumInclinacaoTerreno)laudo.InclinacaoTerreno));
                        PreencherCampo("W35", EnumHelper.GetDescription((EnumSituacaoTerreno)laudo.SituacaoTerreno));
                        PreencherCampo("AE3", EnumHelper.GetDescription((EnumSuperficieTerreno)laudo.SuperficieTerreno));
                        PreencherCampo("B38", laudo.MedidaAreaTerreno);
                        PreencherCampo("H38", laudo.MedidaFrenteTerreno);
                        PreencherCampo("M38", laudo.MedidaFundosTerreno);
                        PreencherCampo("R38", laudo.MedidaEsquerdaTerreno);
                        PreencherCampo("X38", laudo.MedidaDireitaTerreno);
                        PreencherCampo("AC38", laudo.FracaoIdealTerreno);
                        #endregion

                        #region Edificação
                        PreencherCampo("B43", EnumHelper.GetDescription((EnumTipoEdificacao)laudo.TipoEdificacao));
                        PreencherCampo("H43", EnumHelper.GetDescription((EnumUsosPredio)laudo.UsoEdificacao));
                        PreencherCampo("O43", laudo.NumeroPavimentos);
                        PreencherCampo("U43", laudo.IdadeEdificio);
                        PreencherCampo("AB43", EnumHelper.GetDescription((EnumPosicaoEdificacao)laudo.PosicaoEdificacao));

                        PreencherCampo("B46", EnumHelper.GetDescription((EnumPadraoAcabamento)laudo.PadraoAcabamento));
                        PreencherCampo("H46", EnumHelper.GetDescription((EnumEstadoConservacao)laudo.EstadoConservacao));
                        PreencherCampo("M46", EnumHelper.GetDescription((EnumTetos)laudo.Tetos));
                        PreencherCampo("Q46", EnumHelper.GetDescription((EnumFechamentoParedes)laudo.FechamentoParedes));
                        PreencherCampo("X46", laudo.NumeroVagasCobertas);
                        PreencherCampo("AD46", laudo.NumeroVagasDescobertas);

                        PreencherCampo("G49", laudo.AreaUnidadePrivativa);
                        PreencherCampo("L49", laudo.AreaUnidadeComum);
                        PreencherCampo("Q49", laudo.AreaUnidadeTotal);

                        PreencherCampo("G50", laudo.AreaEstacionamentoPrivativa);
                        PreencherCampo("L50", laudo.AreaEstacionamentoComum);
                        PreencherCampo("Q50", laudo.AreaEstacionamentoTotal);

                        PreencherCampo("G51", laudo.AreaOutrosPrivativa);
                        PreencherCampo("L51", laudo.AreaOutrosComum);
                        PreencherCampo("Q51", laudo.AreaOutrosTotal);

                        PreencherCampo("G52", laudo.AreaTotalPrivativa);
                        PreencherCampo("L52", laudo.AreaTotalComum);
                        PreencherCampo("Q52", laudo.AreaTotalAverbada);
                        PreencherCampo("Y52", laudo.AreaTotalNaoAverbada);
                        PreencherCampo("AE52", laudo.SomatorioAreas);

                        PreencherCampo("B56", ObterDivisaoInterna(laudo));

                        PreencherCampo("B62", EnumHelper.GetDescription((EnumUsosPredio)laudo.UsoPredio));
                        PreencherCampo("N62", laudo.NumeroPavimentosPredio);
                        PreencherCampo("S62", laudo.NumeroUnidadesPredio);
                        PreencherCampo("X62", laudo.NumeroElevadoresPredio);
                        PreencherCampo("AC62", EnumHelper.GetDescription((EnumPosicaoPredio)laudo.PosicaoPredio));

                        PreencherCampo("B65", EnumHelper.GetDescription((EnumPadraoAcabamento)laudo.PadraoAcabamento));
                        PreencherCampo("G65", EnumHelper.GetDescription((EnumEstadoConservacao)laudo.EstadoConservacaoPredio));
                        PreencherCampo("M65", laudo.IdentificacaoPavimentosPredio);
                        PreencherCampo("AF65", laudo.IdadeAparentePredio);
                        #endregion

                        #region Avaliação
                        PreencherCampo("B69", laudo.ValorAvaliacao);
                        PreencherCampo("H69", laudo.ValorAvaliacaoExtenso);

                        PreencherCampo("G73", laudo.AreaGlobal);
                        PreencherCampo("Q73", laudo.AreaTerreno);
                        PreencherCampo("W73", laudo.AreaEdificacao);
                        PreencherCampo("AD73", laudo.AreaBenfeitorias);

                        PreencherCampo("G74", laudo.ValorMetroQuadradoGlobal);
                        PreencherCampo("Q74", laudo.ValorMetroQuadradoTerreno);
                        PreencherCampo("W74", laudo.ValorMetroQuadradoEdificacao);
                        PreencherCampo("AD74", laudo.ValorMetroQuadradoBenfeitorias);

                        PreencherCampo("Q75", laudo.ProdutoTerreno);
                        PreencherCampo("W75", laudo.ProdutoEdificacao);
                        PreencherCampo("AD75", laudo.ProdutoBenfeitorias);

                        PreencherCampo("G76", laudo.ValorTotalGlobal);
                        PreencherCampo("AD76", laudo.ValorTotalItemizada);

                        PreencherCampo("B80", laudo.PrecisaoFundamentacao);
                        PreencherCampo("M80", EnumHelper.GetDescription((EnumMetodologiaAvaliacao)laudo.MetodologiaAvaliacao));

                        PreencherCampo("B83", EnumHelper.GetDescription((EnumDesempenhoMercado)laudo.DesempenhoMercado));
                        PreencherCampo("J83", EnumHelper.GetDescription((EnumAbsorcaoMercado)laudo.AbsorcaoMercado));
                        PreencherCampo("T83", EnumHelper.GetDescription((EnumNivelImobiliario)laudo.NivelOfertas));
                        PreencherCampo("AD83", EnumHelper.GetDescription((EnumNivelImobiliario)laudo.NivelDemanda));
                        #endregion

                        #region Rodapé
                        if (configuracao != null)
                            PreencherCampo("G90", string.Format("{0} / {1}", configuracao.NomeEmpresa, configuracao.CNPJEmpresa));

                        if (!string.IsNullOrEmpty(laudo.LocalEmissaoLaudo))
                            PreencherCampo("B93", string.Format("{0}  /  {1}", laudo.LocalEmissaoLaudo, DateTime.Now.Date.ToString("dd/MM/yyyy")));

                        if (laudo.ResponsavelTecnico != null)
                        {
                            PreencherCampo("E96", string.Format("{0} / {1}", laudo.ResponsavelTecnico.Nome.ToUpper(), laudo.ResponsavelTecnico.CREA));
                            PreencherCampo("E97", laudo.ResponsavelTecnico.CPF);
                        }

                        if (laudo.RepresentanteLegalEmpresa != null)
                        {
                            PreencherCampo("T96", laudo.RepresentanteLegalEmpresa.Nome.ToUpper());
                            PreencherCampo("T97", laudo.RepresentanteLegalEmpresa.CPF);
                        }
                        #endregion
                    }
                    #endregion

                    #region Laudo fl 2
                    if (SelectSheet("Laudo fl 2"))
                    {
                        #region Cabeçalho
                        PreencherCampo("L6", laudo.Solicitante);
                        PreencherCampo("W6", laudo.Referencia);
                        #endregion

                        #region Informações Complementares
                        //SelecionarOpcao(laudo.EstabilidadeSolidez ? "EstSim" : "EstNao");

                        PreencherCampo("C12", laudo.EstabilidadeSolidezJustificativa);

                        //SelecionarOpcao(laudo.ViciosConstrucao ? "VicioSim" : "VicioNao");

                        PreencherCampo("C17", laudo.ViciosConstrucaoRelacao);

                        //SelecionarOpcao(laudo.Habitabilidade ? "HabitSim" : "HabitNao");

                        PreencherCampo("C22", laudo.HabitabilidadeJustificativa);

                        switch ((EnumFatoresLiquidezValorImovel)laudo.FatoresLiquidezValorImovel)
                        {
                            case EnumFatoresLiquidezValorImovel.Val:
                                //SelecionarOpcao("Val");
                                break;
                            case EnumFatoresLiquidezValorImovel.Desval:
                                //SelecionarOpcao("Desval");
                                break;
                            case EnumFatoresLiquidezValorImovel.Nenh:
                                //SelecionarOpcao("Nenh");
                                break;
                        }

                        PreencherCampo("C28", laudo.FatoresLiquidezExplicitacao);
                        #endregion

                        #region Garantia, Documentação Apresentada e Observações
                        //SelecionarOpcao(laudo.AceitoComoGarantia == 0 ? "GarSim" : "GarNao");

                        PreencherCampo("B37", laudo.MatriculaRGI);
                        PreencherCampo("I37", laudo.Oficio);
                        PreencherCampo("T37", laudo.Comarca == null ? string.Empty : laudo.Comarca.Nome);

                        PreencherCampo("B40", laudo.OutrosDocumentos);

                        //SelecionarOpcao(laudo.Conformidade == 0 ? "DocSim" : "DocNao");

                        PreencherCampo("C45", laudo.Divergencia);

                        PreencherCampo("C49", laudo.ObservacoesFinais);
                        #endregion

                        #region Rodapé
                        if (configuracao != null)
                            PreencherCampo("G69", string.Format("{0} / {1}", configuracao.NomeEmpresa, configuracao.CNPJEmpresa));

                        if (!string.IsNullOrEmpty(laudo.LocalEmissaoLaudo))
                            PreencherCampo("B72", string.Format("{0} / {1}", laudo.LocalEmissaoLaudo, DateTime.Now.Date.ToString("dd/MM/yyyy")));

                        if (laudo.ResponsavelTecnico != null)
                        {
                            PreencherCampo("E75", string.Format("{0} / {1}", laudo.ResponsavelTecnico.Nome.ToUpper(), laudo.ResponsavelTecnico.CREA));
                            PreencherCampo("E76", laudo.ResponsavelTecnico.CPF);
                        }

                        if (laudo.RepresentanteLegalEmpresa != null)
                        {
                            PreencherCampo("T75", laudo.RepresentanteLegalEmpresa.Nome.ToUpper());
                            PreencherCampo("T76", laudo.RepresentanteLegalEmpresa.CPF);
                        }
                        #endregion
                    }
                    #endregion

                    #region Laudo fl 3
                    if (SelectSheet("Laudo fl 3"))
                    {
                        #region Cabeçalho
                        PreencherCampo("L6", laudo.Solicitante);
                        PreencherCampo("W6", laudo.Referencia);
                        #endregion

                        #region Identificação
                        if (laudo.Produto != null)
                            PreencherCampo("B10", laudo.Produto.Descricao);

                        if (laudo.Linha != null)
                            PreencherCampo("W10", laudo.Linha.Descricao);

                        if (laudo.Fonte != null)
                            PreencherCampo("B13", laudo.Fonte.Descricao);

                        PreencherCampo("B16", laudo.Imovel.NomeCliente);
                        PreencherCampo("W16", laudo.Imovel.TipoLogradouro.Descricao);

                        PreencherCampo("B19", string.Format("{0}, {1}", laudo.Imovel.Endereco, laudo.Imovel.Numero));
                        PreencherCampo("W19", laudo.Imovel.Complemento);

                        PreencherCampo("B22", laudo.Imovel.Bairro);
                        PreencherCampo("W22", laudo.Imovel.Cidade.Nome);
                        PreencherCampo("AH22", laudo.Imovel.Cidade.Estado.Sigla);
                        #endregion

                        #region Rodapé
                        if (configuracao != null)
                            PreencherCampo("G34", string.Format("{0} / {1}", configuracao.NomeEmpresa, configuracao.CNPJEmpresa));

                        if (!string.IsNullOrEmpty(laudo.LocalEmissaoLaudo))
                            PreencherCampo("B37", string.Format("{0} / {1}", laudo.LocalEmissaoLaudo, DateTime.Now.Date.ToString("dd/MM/yyyy")));

                        if (laudo.ResponsavelTecnico != null)
                        {
                            PreencherCampo("E40", string.Format("{0} / {1}", laudo.ResponsavelTecnico.Nome.ToUpper(), laudo.ResponsavelTecnico.CREA));
                            PreencherCampo("E41", laudo.ResponsavelTecnico.CPF);
                        }

                        if (laudo.RepresentanteLegalEmpresa != null)
                        {
                            PreencherCampo("T40", laudo.RepresentanteLegalEmpresa.Nome.ToUpper());
                            PreencherCampo("T41", laudo.RepresentanteLegalEmpresa.CPF);
                        }
                        #endregion
                    }
                    #endregion
                }
                catch
                {
                    throw;
                }
                finally
                {
                    _document.Close();
                }
            }
        }
        
        public void PreencherCampo(string address, object value)
        {
            if (value != null)
            {
                var text = value.ToString();

                if (string.IsNullOrWhiteSpace(text))
                    return;

                if (value is bool)
                {
                    bool selected = Convert.ToBoolean(value);

                    if (selected)
                        InsertBoolean(address, "1");
                }
                else
                    InsertText(address, text);
                
                _activeWorksheet.Save();
            }
        }

        public void SelecionarOpcao(string name)
        {
            IEnumerable<AlternateContent> alternateContents = _controlesFolhaAtual.Elements<AlternateContent>();

            Control control = (from ac in alternateContents
                               from acc in ac.GetFirstChild<AlternateContentChoice>()
                               from c in acc.GetFirstChild<Control>()
                               from p in c.GetFirstChild<ControlProperties>()
                               where (c as Control).Name == name
                               select (c as Control)).SingleOrDefault();

            //AlternateContent alternateContent = _controlesFolhaAtual.Elements<AlternateContent>().ElementAt(index);

            //AlternateContentChoice alternateContentChoice = alternateContent.GetFirstChild<AlternateContentChoice>();
            //Control control = alternateContentChoice.GetFirstChild<Control>();

            ControlProperties controlProperties = control.GetFirstChild<ControlProperties>();

            PreencherCampo(controlProperties.LinkedCell, true);
        }

        public void SelecionarInfraEstruturasUrbanas(Laudo laudo)
        {
            foreach (var infra in laudo.ListaInfraEstruturaUrbana)
                SelecionarOpcao(((EnumInfraEstruturaUrbana)infra.TipoInfraEstruturaUrbana).ToString());
        }

        public void SelecionarServicosPublicos(Laudo laudo)
        {
            foreach (var servico in laudo.ListaServicoPublicoComunitario)
                SelecionarOpcao(((EnumServicoPublicoComunitario)servico.TipoServicoPublicoComunitario).ToString());
        }

        public string ObterDivisaoInterna(Laudo laudo)
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
        #endregion

        #region Private Methods
        private SharedStringTablePart GetSharedStringPart()
        {
            SharedStringTablePart sharedStringPart;

            if (_wbPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                sharedStringPart = _wbPart.GetPartsOfType<SharedStringTablePart>().First();
            else
                sharedStringPart = _wbPart.AddNewPart<SharedStringTablePart>();

            // If the part does not contain a SharedStringTable, create one.
            if (sharedStringPart.SharedStringTable == null)
                sharedStringPart.SharedStringTable = new SharedStringTable();

            return sharedStringPart;
        }

        private bool SelectSheet(string nomeAba)
        {
            Sheet aba = _wbPart.Workbook.Descendants<Sheet>().ToList().FirstOrDefault(s => s.Name == nomeAba);

            if (aba != null)
            {
                _activeWorksheet = ((WorksheetPart)(_wbPart.GetPartById(aba.Id))).Worksheet;
                _controlesFolhaAtual = _activeWorksheet.GetFirstChild<Controls>();

                return true;
            }

            return false;
        }

        private bool SelectFirstSheet()
        {
            Sheet aba = _wbPart.Workbook.Descendants<Sheet>().ToList().FirstOrDefault();

            if (aba != null)
            {
                _activeWorksheet = ((WorksheetPart)(_wbPart.GetPartById(aba.Id))).Worksheet;
                _controlesFolhaAtual = _activeWorksheet.GetFirstChild<Controls>();

                return true;
            }

            return false;
        }

        private Cell GetCell(string address)
        {
            Row linha = GetRow(address);

            if (linha == null)
                return null;

            return linha.Elements<Cell>().Where(c => string.Compare
                   (c.CellReference.Value, address, true) == 0).First();
        }

        private Row GetRow(string address)
        {
            var indice = GetRowIndex(address);

            return _activeWorksheet.GetFirstChild<SheetData>().
              Elements<Row>().Where(r => r.RowIndex == indice).First();
        }

        private UInt32 GetRowIndex(string address)
        {
            string rowPart;
            UInt32 l;
            UInt32 result = 0;

            for (int i = 0; i < address.Length; i++)
            {
                if (UInt32.TryParse(address.Substring(i, 1), out l))
                {
                    rowPart = address.Substring(i, address.Length - i);
                    if (UInt32.TryParse(rowPart, out l))
                    {
                        result = l;
                        break;
                    }
                }
            }
            return result;
        }

        private string GetCellValue(Cell celula)
        {
            if (celula.DataType != null && celula.DataType == CellValues.SharedString)
            {
                return GetText(celula.CellValue.Text);
            }

            return celula.CellValue.Text;
        }

        private int InsertSharedStringItem(string text)
        {
            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in _sharedStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            _sharedStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            _sharedStringPart.SharedStringTable.Save();

            return i;
        }
        
        private string GetText(string id)
        {
            var sharedString = _sharedStringPart.SharedStringTable.Elements<SharedStringItem>().ElementAt(Int32.Parse(id));

            return sharedString.Text.Text;
        }

        private void InsertText(string address, string text)
        {
            int index = InsertSharedStringItem(text);

            Cell cell = GetCell(address);
            cell.CellValue = new CellValue(index.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
        }

        private void InsertBoolean(string address, string value)
        {
            Cell cell = GetCell(address);
            cell.CellValue = new CellValue(value);
            cell.DataType = new EnumValue<CellValues>(CellValues.Boolean);
        }
        #endregion
    }
}