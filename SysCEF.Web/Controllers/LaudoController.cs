using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Core;
using SysCEF.Common.Interface;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using SysCEF.Web.Helpers;
using SysCEF.Web.Models;
using System.Text;

namespace SysCEF.Web.Controllers
{
    public class LaudoController : Controller
    {
        #region Propriedades
        public ISysCEFWorkLifetimeManager WorkLifetimeManager { get; set; }
        public ILaudoRepositorio LaudoRepositorio { get; set; }
        public IImovelRepositorio ImovelRepositorio { get; set; }
        public ITipoLogradouroRepositorio TipoLogradouroRepositorio { get; set; }
        public IEstadoRepositorio EstadoRepositorio { get; set; }
        public ICidadeRepositorio CidadeRepositorio { get; set; }
        public IUsuarioRepositorio UsuarioRepositorio { get; set; }
        public IConfiguracaoRepositorio ConfiguracaoRepositorio { get; set; }
        public IProdutoRepositorio ProdutoRepositorio { get; set; }
        public ILinhaRepositorio LinhaRepositorio { get; set; }
        public IFonteRepositorio FonteRepositorio { get; set; }
        #endregion

        #region Actions
        public ActionResult ImportarOS()
        {
            return PartialView();
        }

        public string Importar(HttpPostedFileBase fileData, FormCollection forms)
        {
            var uploadHelper = new UploadOSHelper
            {
                UnitOfWork = WorkLifetimeManager.Value,
                LaudoRepository = LaudoRepositorio,
                TipoLogradouroRepositorio = TipoLogradouroRepositorio,
                EstadoRepositorio = EstadoRepositorio,
                CidadeRepositorio = CidadeRepositorio,
                ProdutoRepositorio = ProdutoRepositorio,
                LinhaRepositorio = LinhaRepositorio,
                FonteRepositorio = FonteRepositorio
            };

            var path = System.Web.HttpContext.Current.Request.MapPath("~/Content/uploads/");

            var fileName = Path.Combine(path, fileData.FileName);

            fileData.SaveAs(fileName); // Salva OS na pasta de Uploads do Servidor.

            try
            {
                var laudo = uploadHelper.GerarLaudoAPartirArquivo(fileName);

                LaudoRepositorio.Salvar(WorkLifetimeManager.Value, laudo);
                WorkLifetimeManager.Value.Commit();

                return "Arquivo importado com sucesso!";
            }
            catch
            {
                return "Não foi possível importar o arquivo. Verifique-o e tente novamente!";
            }
        }

        public ActionResult Lista(string status)
        {
            if (string.IsNullOrEmpty(status))
                status = EnumStatusLaudo.Importado.ToString();

            var laudos = BuscarLaudosPorStatus(status);

            return PartialView(new ListaLaudoViewModel(status, laudos, null));
        }

        public ActionResult ListaImportada()
        {
            var status = EnumStatusLaudo.Importado.ToString();
            var laudos = BuscarLaudosPorStatus(status);
            var opcoesHelper = new OpcoesHelper();

            var model = new ListaLaudoViewModel(status, laudos, null)
                {
                    ListaResponsaveisTecnicos = opcoesHelper.ObterOpcoesResponsaveisTecnicos(UsuarioRepositorio.BuscarPorPerfil(WorkLifetimeManager.Value, EnumPerfil.UsuarioComum))
                };

            return PartialView(model);
        }

        public ActionResult Index(int id)
        {
            var laudo = LaudoRepositorio.Obter(WorkLifetimeManager.Value, id);

            return PartialView(PreencherViewModel(laudo));
        }

        public JsonResult Exportar(int id)
        {
            bool sucesso;
            string mensagem;
            var nomeArquivo = string.Empty;

            try
            {
                var laudo = LaudoRepositorio.Obter(WorkLifetimeManager.Value, id);

                if (laudo == null)
                    throw new InvalidOperationException(string.Format("Laudo não encontrado (Id: {0})", id));

                nomeArquivo = ExportarLaudo(laudo);
                sucesso = true;
                mensagem = "Arquivo exportado com sucesso!";
            }
            catch (Exception exception)
            {
                sucesso = false;
                mensagem = "Não foi possível realizar a operação: " + exception.Message;
            }

            return Json(new { sucesso, mensagem, nomeArquivo });
        }
        
        public ActionResult Salvar(LaudoViewModel viewModel)
        {
            string mensagem;

            WorkLifetimeManager.Value.BeginTransaction(IsolationLevel.Serializable);
            
            try
            {
                var laudo = LaudoRepositorio.Obter(WorkLifetimeManager.Value, viewModel.LaudoId);

                AtualizarLaudo(laudo, viewModel);

                ImovelRepositorio.Salvar(WorkLifetimeManager.Value, laudo.Imovel);
                LaudoRepositorio.Salvar(WorkLifetimeManager.Value, laudo);

                WorkLifetimeManager.Value.Commit();

                mensagem = "Operação efetuada com sucesso!";
            }
            catch (Exception ex)
            {
                WorkLifetimeManager.Value.Rollback();
                mensagem = "Não foi possível efetuar alteração: " + ex.InnerException;
            }
            
            var laudos = BuscarLaudosPorStatus(viewModel.StatusLaudo);

            return viewModel.StatusLaudo == EnumStatusLaudo.Importado.ToString()
                       ? PartialView("ImportarOS")
                       : PartialView("Lista", new ListaLaudoViewModel(viewModel.StatusLaudo, laudos, mensagem));
        }

        public ActionResult AtualizarAreasEdificacao(LaudoViewModel viewModel)
        {
            return PartialView("AreasEdificacao", ObterAreasEdificacaoCalculadas(viewModel));
        }

        public string Agendar(int idLaudo, string dataVistoria, string horaVistoria, int idResponsavelTecnico)
        {
            string mensagem;

            WorkLifetimeManager.Value.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                var laudo = LaudoRepositorio.Obter(WorkLifetimeManager.Value, idLaudo);


                laudo.DataHoraVistoria = ObterDataHora(dataVistoria, horaVistoria);

                if (idResponsavelTecnico > 0)
                    laudo.ResponsavelTecnico = UsuarioRepositorio.Obter(WorkLifetimeManager.Value, idResponsavelTecnico);

                laudo.Status = (int) EnumStatusLaudo.AFazer;

                LaudoRepositorio.Salvar(WorkLifetimeManager.Value, laudo);

                WorkLifetimeManager.Value.Commit();

                mensagem = "Operação efetuada com sucesso!";
            }
            catch (Exception ex)
            {
                WorkLifetimeManager.Value.Rollback();
                mensagem = "Não foi possível efetuar alteração: " + ex.InnerException;
            }

            return mensagem;
        }
        #endregion

        #region Métodos Principais
        private LaudoViewModel PreencherViewModel(Laudo laudo)
        {
            var opcoesHelper = new OpcoesHelper(laudo);

            var viewModel = new LaudoViewModel
            {
                Laudo = laudo,
                LaudoId = laudo.LaudoID,
                StatusLaudo = ((EnumStatusLaudo)laudo.Status).ToString(),
                MetodoDefinicaoValor = RadioButtonHelper.ParseEnumToRadioButtonList((EnumMetodoDefinicaoValor)laudo.MetodoDefinicaoValor),
                DesempenhoMercado = RadioButtonHelper.ParseEnumToRadioButtonList((EnumDesempenhoMercado)laudo.DesempenhoMercado),
                AbsorcaoMercado = RadioButtonHelper.ParseEnumToRadioButtonList((EnumAbsorcaoMercado)laudo.AbsorcaoMercado),
                NumeroOfertas = RadioButtonHelper.ParseEnumToRadioButtonList((EnumNivelImobiliario)laudo.NivelOfertas),
                NivelDemanda = RadioButtonHelper.ParseEnumToRadioButtonList((EnumNivelImobiliario)laudo.NivelDemanda),
                FatoresLiquidezValorImovel = RadioButtonHelper.ParseEnumToRadioButtonList((EnumFatoresLiquidezValorImovel)laudo.FatoresLiquidezValorImovel),
                AceitoComoGarantia = RadioButtonHelper.ParseEnumToRadioButtonList((EnumSimOuNao)laudo.AceitoComoGarantia),
                Conformidade = RadioButtonHelper.ParseEnumToRadioButtonList((EnumSimOuNao)laudo.Conformidade),
                // Identificação
                ListaEstados = opcoesHelper.ObterOpcoesEstado(EstadoRepositorio.BuscarTodos(WorkLifetimeManager.Value)),
                ListaCidades = opcoesHelper.ObterOpcoesCidade(CidadeRepositorio.BuscarTodasEstado(WorkLifetimeManager.Value, laudo.Imovel.Cidade.Estado.Sigla)),
                ListaTiposLogradouro = opcoesHelper.ObterOpcoesTipoLogradouro(TipoLogradouroRepositorio.BuscarTodos(WorkLifetimeManager.Value)),
                // Caracterização da Região
                ListaServicosPublicosComunitarios = opcoesHelper.ObterOpcoesEnum<EnumServicoPublicoComunitario>(),
                ListaInfraEstruturasUrbanas = opcoesHelper.ObterOpcoesEnum<EnumInfraEstruturaUrbana>(),
                // Final
                ListaComarcas = opcoesHelper.ObterOpcoesCidade(CidadeRepositorio.BuscarTodasEstado(WorkLifetimeManager.Value, laudo.Imovel.Cidade.Estado.Sigla)),
                ListaResponsaveisTecnicos = opcoesHelper.ObterOpcoesResponsaveisTecnicos(UsuarioRepositorio.BuscarPorPerfil(WorkLifetimeManager.Value, EnumPerfil.UsuarioComum)),
                ListaRepresentantesLegais = opcoesHelper.ObterOpcoesRepresentantesLegais(UsuarioRepositorio.BuscarPorPerfil(WorkLifetimeManager.Value, EnumPerfil.Administrador)),
            };

            MarcarServicosPublicosComunitarios(viewModel.ListaServicosPublicosComunitarios, laudo.ListaServicoPublicoComunitario);
            MarcarInfraEstruturasUrbanas(viewModel.ListaInfraEstruturasUrbanas, laudo.ListaInfraEstruturaUrbana);
            
            if (laudo.DataHoraVistoria.HasValue)
            {
                viewModel.DataVistoria = laudo.DataHoraVistoria.Value.Date.ToString("dd/MM/yyyy");
                viewModel.HoraVistoria = laudo.DataHoraVistoria.Value.ToString("HH:mm");
            }

            return viewModel;
        }

        private void AtualizarLaudo(Laudo laudo, LaudoViewModel model)
        {
            #region Identificação
            if (laudo.Imovel.Cidade.Estado.EstadoID != model.Laudo.Imovel.Cidade.Estado.EstadoID)
                laudo.Imovel.Cidade.Estado = EstadoRepositorio.Obter(WorkLifetimeManager.Value, model.Laudo.Imovel.Cidade.Estado.EstadoID);
            if (laudo.Imovel.Cidade.CidadeID != model.Laudo.Imovel.Cidade.CidadeID)
                laudo.Imovel.Cidade = CidadeRepositorio.Obter(WorkLifetimeManager.Value, model.Laudo.Imovel.Cidade.CidadeID);
            if (laudo.Imovel.TipoLogradouro.TipoLogradouroID != model.Laudo.Imovel.TipoLogradouro.TipoLogradouroID)
                laudo.Imovel.TipoLogradouro = TipoLogradouroRepositorio.Obter(WorkLifetimeManager.Value, model.Laudo.Imovel.TipoLogradouro.TipoLogradouroID);

            laudo.Imovel.Endereco = model.Laudo.Imovel.Endereco;
            laudo.Imovel.Numero = model.Laudo.Imovel.Numero;
            laudo.Imovel.Complemento = model.Laudo.Imovel.Complemento;
            laudo.Produto = model.Laudo.Produto;
            laudo.Linha = model.Laudo.Linha;
            laudo.Fonte = model.Laudo.Fonte;
            laudo.Imovel.NomeCliente = model.Laudo.Imovel.NomeCliente;
            #endregion

            #region Caracterização da Região
            laudo.UsosPredominantes = model.Laudo.UsosPredominantes;

            RemoverOuAdicionarServicosPublicos(laudo, model);

            RemoverOuAdicionarInfraEstruturaUrbana(laudo, model);
            #endregion

            #region Terreno
            laudo.FormaTerreno = model.Laudo.FormaTerreno;
            laudo.CotaGreideTerreno = model.Laudo.CotaGreideTerreno;
            laudo.InclinacaoTerreno = model.Laudo.InclinacaoTerreno;
            laudo.SituacaoTerreno = model.Laudo.SituacaoTerreno;
            laudo.SuperficieTerreno = model.Laudo.SuperficieTerreno;
            laudo.MedidaAreaTerreno = model.Laudo.MedidaAreaTerreno;
            laudo.MedidaFrenteTerreno = model.Laudo.MedidaFrenteTerreno;
            laudo.MedidaFundosTerreno = model.Laudo.MedidaFundosTerreno;
            laudo.MedidaDireitaTerreno = model.Laudo.MedidaDireitaTerreno;
            laudo.MedidaEsquerdaTerreno = model.Laudo.MedidaEsquerdaTerreno;
            laudo.FracaoIdealTerreno = model.Laudo.FracaoIdealTerreno;
            #endregion

            #region Edificação
            laudo.TipoEdificacao = model.Laudo.TipoEdificacao;
            laudo.UsoEdificacao = model.Laudo.UsoEdificacao; 
            laudo.NumeroPavimentos = model.Laudo.NumeroPavimentos;
            laudo.IdadeEdificio = model.Laudo.IdadeEdificio;
            laudo.PosicaoEdificacao = model.Laudo.PosicaoEdificacao;
            laudo.PadraoAcabamento = model.Laudo.PadraoAcabamento;
            laudo.EstadoConservacao = model.Laudo.EstadoConservacao;
            laudo.Tetos = model.Laudo.Tetos;
            laudo.FechamentoParedes = model.Laudo.FechamentoParedes;
            laudo.NumeroVagasCobertas = model.Laudo.NumeroVagasCobertas;
            laudo.NumeroVagasDescobertas = model.Laudo.NumeroVagasDescobertas;
            laudo.AreaUnidadePrivativa = model.Laudo.AreaUnidadePrivativa;
            laudo.AreaUnidadeComum = model.Laudo.AreaUnidadeComum;
            laudo.AreaUnidadeTotal = model.Laudo.AreaUnidadeTotal;
            laudo.AreaEstacionamentoPrivativa = model.Laudo.AreaEstacionamentoPrivativa;
            laudo.AreaEstacionamentoComum = model.Laudo.AreaEstacionamentoComum;
            laudo.AreaEstacionamentoTotal = model.Laudo.AreaEstacionamentoTotal;
            laudo.AreaOutrosPrivativa = model.Laudo.AreaOutrosPrivativa;
            laudo.AreaOutrosComum = model.Laudo.AreaOutrosComum;
            laudo.AreaOutrosTotal = model.Laudo.AreaOutrosTotal;
            laudo.AreaTotalPrivativa = model.Laudo.AreaTotalPrivativa;
            laudo.AreaTotalComum = model.Laudo.AreaTotalComum;
            laudo.AreaTotalAverbada = model.Laudo.AreaTotalAverbada;
            laudo.AreaTotalNaoAverbada = model.Laudo.AreaTotalNaoAverbada;
            laudo.SomatorioAreas = model.Laudo.SomatorioAreas;
            laudo.NumeroQuartos = model.Laudo.NumeroQuartos;
            laudo.NumeroSalas = model.Laudo.NumeroSalas;
            laudo.NumeroCirculacao = model.Laudo.NumeroCirculacao;
            laudo.NumeroBanheiros = model.Laudo.NumeroBanheiros;
            laudo.NumeroSuites = model.Laudo.NumeroSuites;
            laudo.NumeroClosets = model.Laudo.NumeroClosets;
            laudo.NumeroCopas = model.Laudo.NumeroCopas;
            laudo.NumeroCozinhas = model.Laudo.NumeroCozinhas;
            laudo.NumeroAreasServico = model.Laudo.NumeroAreasServico;
            laudo.NumeroVarandas = model.Laudo.NumeroVarandas;
            laudo.NumeroTerracosCobertos = model.Laudo.NumeroTerracosCobertos;
            laudo.NumeroTerracosDescobertos = model.Laudo.NumeroTerracosDescobertos;
            laudo.UsoPredio = model.Laudo.UsoPredio;
            laudo.NumeroPavimentosPredio = model.Laudo.NumeroPavimentosPredio;
            laudo.NumeroUnidadesPredio = model.Laudo.NumeroUnidadesPredio;
            laudo.NumeroElevadoresPredio = model.Laudo.NumeroElevadoresPredio;
            laudo.PosicaoPredio = model.Laudo.PosicaoPredio;
            laudo.PadraoConstrutivoPredio = model.Laudo.PadraoConstrutivoPredio;
            laudo.EstadoConservacaoPredio = model.Laudo.EstadoConservacaoPredio;
            laudo.IdentificacaoPavimentosPredio = model.Laudo.IdentificacaoPavimentosPredio;
            laudo.IdadeAparentePredio = model.Laudo.IdadeAparentePredio;
            #endregion

            #region Avaliação
            laudo.ValorAvaliacao = model.Laudo.ValorAvaliacao;
            laudo.ValorAvaliacaoExtenso = model.Laudo.ValorAvaliacaoExtenso;
            laudo.MetodoDefinicaoValor = (int)model.MetodoDefinicaoValor.SelectedValue;
            laudo.AreaGlobal = model.Laudo.AreaGlobal;
            laudo.ValorMetroQuadradoGlobal = model.Laudo.ValorMetroQuadradoGlobal;
            laudo.ValorTotalGlobal = model.Laudo.ValorTotalGlobal;
            laudo.AreaTerreno = model.Laudo.AreaTerreno;
            laudo.AreaEdificacao = model.Laudo.AreaEdificacao;
            laudo.AreaBenfeitorias = model.Laudo.AreaBenfeitorias;
            laudo.ValorMetroQuadradoTerreno = model.Laudo.ValorMetroQuadradoTerreno;
            laudo.ValorMetroQuadradoEdificacao = model.Laudo.ValorMetroQuadradoEdificacao;
            laudo.ValorMetroQuadradoBenfeitorias = model.Laudo.ValorMetroQuadradoBenfeitorias;
            laudo.ProdutoTerreno = model.Laudo.ProdutoTerreno;
            laudo.ProdutoEdificacao = model.Laudo.ProdutoEdificacao;
            laudo.ProdutoBenfeitorias = model.Laudo.ProdutoBenfeitorias;
            laudo.ValorTotalItemizada = model.Laudo.ValorTotalItemizada;
            laudo.PrecisaoFundamentacao = model.Laudo.PrecisaoFundamentacao;
            laudo.MetodologiaAvaliacao = model.Laudo.MetodologiaAvaliacao;
            laudo.DesempenhoMercado = (int)model.DesempenhoMercado.SelectedValue;
            laudo.AbsorcaoMercado = (int)model.AbsorcaoMercado.SelectedValue;
            laudo.NivelOfertas = (int)model.NumeroOfertas.SelectedValue;
            laudo.NivelDemanda = (int)model.NivelDemanda.SelectedValue;
            laudo.ObservacoesAvaliacao = model.Laudo.ObservacoesAvaliacao;
            #endregion

            #region Informações Complementares
            laudo.EstabilidadeSolidez = model.Laudo.EstabilidadeSolidez;
            laudo.EstabilidadeSolidezJustificativa = model.Laudo.EstabilidadeSolidezJustificativa;
            laudo.ViciosConstrucao = model.Laudo.ViciosConstrucao;
            laudo.ViciosConstrucaoRelacao = model.Laudo.ViciosConstrucaoRelacao;
            laudo.Habitabilidade = model.Laudo.Habitabilidade;
            laudo.HabitabilidadeJustificativa = model.Laudo.HabitabilidadeJustificativa;
            laudo.FatoresLiquidezValorImovel = (int)model.FatoresLiquidezValorImovel.SelectedValue;
            laudo.FatoresLiquidezExplicitacao = model.Laudo.FatoresLiquidezExplicitacao;
            #endregion

            #region Garantia, Documentação Apresentada
            laudo.AceitoComoGarantia = (int)model.AceitoComoGarantia.SelectedValue;

            laudo.MatriculaRGI = model.Laudo.MatriculaRGI;
            laudo.Oficio = model.Laudo.Oficio;

            if (model.Laudo.Comarca == null || model.Laudo.Comarca.CidadeID == 0)
                laudo.Comarca = null;
            else if (laudo.Comarca == null || laudo.Comarca.CidadeID != model.Laudo.Comarca.CidadeID)
                laudo.Comarca = CidadeRepositorio.Obter(WorkLifetimeManager.Value, model.Laudo.Comarca.CidadeID);

            laudo.OutrosDocumentos = model.Laudo.OutrosDocumentos;
            laudo.Conformidade = (int)model.Conformidade.SelectedValue;
            laudo.Divergencia = model.Laudo.Divergencia;
            #endregion

            #region Observações Finais
            laudo.ObservacoesFinais = model.Laudo.ObservacoesFinais;
            laudo.LocalEmissaoLaudo = model.Laudo.LocalEmissaoLaudo;

            if (model.Laudo.ResponsavelTecnico == null || model.Laudo.ResponsavelTecnico.UsuarioId == 0)
                laudo.ResponsavelTecnico = null;
            else if (laudo.ResponsavelTecnico == null || laudo.ResponsavelTecnico.UsuarioId != model.Laudo.ResponsavelTecnico.UsuarioId)
                laudo.ResponsavelTecnico = UsuarioRepositorio.Obter(WorkLifetimeManager.Value, model.Laudo.ResponsavelTecnico.UsuarioId);

            if (model.Laudo.RepresentanteLegalEmpresa == null || model.Laudo.RepresentanteLegalEmpresa.UsuarioId == 0)
                laudo.RepresentanteLegalEmpresa = null;
            else if (laudo.RepresentanteLegalEmpresa == null || laudo.RepresentanteLegalEmpresa.UsuarioId != model.Laudo.RepresentanteLegalEmpresa.UsuarioId)
                laudo.RepresentanteLegalEmpresa = UsuarioRepositorio.Obter(WorkLifetimeManager.Value, model.Laudo.RepresentanteLegalEmpresa.UsuarioId);
            #endregion
            
            if (laudo.Status == (int) EnumStatusLaudo.AFazer)
                VerificarStatusLaudo(laudo);
        }

        private void VerificarStatusLaudo(Laudo laudo)
        {
            if (laudo.ListaServicoPublicoComunitario != null && laudo.ListaServicoPublicoComunitario.Any() &&
                laudo.ListaInfraEstruturaUrbana != null && laudo.ListaInfraEstruturaUrbana.Any() &&
                laudo.MedidaAreaTerreno > 0 &&
                laudo.FracaoIdealTerreno > 0 &&
                laudo.SomatorioAreas > 0)
                laudo.Status = (int)EnumStatusLaudo.EmAndamento;
        }

        private string ExportarLaudo(Laudo laudo)
        {
            //var excelWriter = new SysCEFExcelWriter(caminhoPastaServidor, laudo.Referencia);
            //excelWriter.PreencherPlanilha(laudo);
            //excelWriter.SalvarFecharArquivo();

            var nomeArquivo = string.Format("UPredio_{0}.xlsx", laudo.Referencia.Replace("/", ""));

            WorkLifetimeManager.Value.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                var configuracao = ConfiguracaoRepositorio.Obter(WorkLifetimeManager.Value);
                var caminhoTemplate = Path.Combine(Server.MapPath("~/Content/uploads/"), "Source.xlsx");
                var caminhoArquivo = Path.Combine(Server.MapPath("~/Content/uploads/"), nomeArquivo);
                
                System.IO.File.Copy(caminhoTemplate, caminhoArquivo, true);

                var openXmlHelper = new OpenXmlHelper();
                openXmlHelper.PreencherPlanilha(caminhoArquivo, laudo, configuracao);
                                
                laudo = LaudoRepositorio.Obter(WorkLifetimeManager.Value, laudo.LaudoID);
                laudo.Status = (int)EnumStatusLaudo.Concluido;

                LaudoRepositorio.Salvar(WorkLifetimeManager.Value, laudo);

                WorkLifetimeManager.Value.Commit();
            }
            catch
            {
                WorkLifetimeManager.Value.Rollback();
            }

            return nomeArquivo;
        }

        private void PrepararResponse(string nomeArquivo)
        {
            Response.ClearHeaders();
            Response.Buffer = false;
            
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Connection", "Keep-Alive");
            Response.AddHeader("Content-Disposition", String.Format("attachment;  filename={0}", nomeArquivo));
            Response.ContentEncoding = Encoding.UTF8;
        }

        private LaudoViewModel ObterAreasEdificacaoCalculadas(LaudoViewModel model)
        {
            var viewModel = new LaudoViewModel { Laudo = new Laudo() };

            viewModel.Laudo.AreaUnidadePrivativa = model.Laudo.AreaUnidadePrivativa;
            viewModel.Laudo.AreaUnidadeComum = model.Laudo.AreaUnidadeComum;
            viewModel.Laudo.AreaUnidadeTotal = model.Laudo.AreaUnidadePrivativa + model.Laudo.AreaUnidadeComum;

            viewModel.Laudo.AreaEstacionamentoPrivativa = model.Laudo.AreaEstacionamentoPrivativa;
            viewModel.Laudo.AreaEstacionamentoComum = model.Laudo.AreaEstacionamentoComum;
            viewModel.Laudo.AreaEstacionamentoTotal = model.Laudo.AreaEstacionamentoPrivativa + model.Laudo.AreaEstacionamentoComum;

            viewModel.Laudo.AreaOutrosPrivativa = model.Laudo.AreaOutrosPrivativa;
            viewModel.Laudo.AreaOutrosComum = model.Laudo.AreaOutrosComum;
            viewModel.Laudo.AreaOutrosTotal = model.Laudo.AreaOutrosPrivativa + model.Laudo.AreaOutrosComum;

            viewModel.Laudo.AreaTotalPrivativa = model.Laudo.AreaUnidadePrivativa + model.Laudo.AreaEstacionamentoPrivativa + model.Laudo.AreaOutrosPrivativa;
            viewModel.Laudo.AreaTotalComum = model.Laudo.AreaUnidadeComum + model.Laudo.AreaEstacionamentoComum + model.Laudo.AreaOutrosComum;
            viewModel.Laudo.AreaTotalAverbada = viewModel.Laudo.AreaTotalPrivativa + viewModel.Laudo.AreaTotalComum;

            viewModel.Laudo.AreaTotalNaoAverbada = model.Laudo.AreaTotalNaoAverbada;
            viewModel.Laudo.SomatorioAreas = viewModel.Laudo.AreaTotalAverbada + viewModel.Laudo.AreaTotalNaoAverbada;
            
            return viewModel;
        }
        #endregion

        #region Métodos Adicionais
        private IEnumerable<LaudoModel> BuscarLaudosPorStatus(string status)
        {
            var statusLaudo = EnumHelper.ParseEnumValueDescription<EnumStatusLaudo>(status);

            var laudos = LaudoRepositorio.BuscarLaudosPorStatus(WorkLifetimeManager.Value, statusLaudo).ToList();

            return (from laudo in laudos
                    select new LaudoModel
                    {
                        LaudoId = laudo.LaudoID,
                        Produto = laudo.Produto != null ? laudo.Produto.Descricao : string.Empty,
                        Linha = laudo.Linha != null ? laudo.Linha.Descricao : string.Empty,
                        Fonte = laudo.Fonte != null ? laudo.Fonte.Descricao : string.Empty,
                        NomeCliente = laudo.Imovel.NomeCliente,
                        SiglaLogradouro = laudo.Imovel.TipoLogradouro.Sigla,
                        Endereco = laudo.Imovel.Endereco,
                        Numero = laudo.Imovel.Numero,
                        Complemento = laudo.Imovel.Complemento,
                        Bairro = laudo.Imovel.Bairro,
                        Cidade = laudo.Imovel.Cidade.Nome.ToUpper(),
                        UF = laudo.Imovel.Cidade.Estado.Sigla,
                        Status = EnumHelper.GetDescription(statusLaudo),
                        DataHoraVistoria = laudo.DataHoraVistoria.HasValue ? laudo.DataHoraVistoria.Value.ToString(CultureInfo.CurrentCulture) : string.Empty,
                        ResponsavelTecnico = laudo.ResponsavelTecnico != null ? laudo.ResponsavelTecnico.Nome : string.Empty
                    }
                   ).ToList();
        }

        private void MarcarServicosPublicosComunitarios(List<SelectListItem> opcoes, IList<ServicoPublicoComunitario> servicosPublicosComunitarios)
        {
            if (opcoes == null || servicosPublicosComunitarios == null || !servicosPublicosComunitarios.Any()) return;

            foreach (var servico in servicosPublicosComunitarios)
            {
                var opcao = opcoes.FirstOrDefault(o => o.Value == ((EnumServicoPublicoComunitario) servico.TipoServicoPublicoComunitario).ToString());

                if (opcao != null)
                    opcao.Selected = true;
            }
        }

        private void MarcarInfraEstruturasUrbanas(List<SelectListItem> opcoes, IList<InfraEstruturaUrbana> infraEstruturasUrbanas)
        {
            if (opcoes == null || infraEstruturasUrbanas == null || !infraEstruturasUrbanas.Any()) return;

            foreach (var infra in infraEstruturasUrbanas)
            {
                var opcao = opcoes.FirstOrDefault(o => o.Value == ((EnumInfraEstruturaUrbana)infra.TipoInfraEstruturaUrbana).ToString());

                if (opcao != null)
                    opcao.Selected = true;
            }
        }

        private void RemoverOuAdicionarServicosPublicos(Laudo laudo, LaudoViewModel model)
        {
            var listaServicosLaudo = laudo.ListaServicoPublicoComunitario.ToList();
            var listaServicosSelecionados = ObterListaOpcoesAPartirString(model.ServicosSelecionados);
            
            var listaServicosARemover = new List<ServicoPublicoComunitario>(
                from servico in listaServicosLaudo
                where listaServicosSelecionados.All(s => s.Value != ((EnumServicoPublicoComunitario)servico.TipoServicoPublicoComunitario).ToString())
                select servico).ToList();

            var listaServicosAAdicionar = (from servico in listaServicosSelecionados
                                           where listaServicosLaudo.All(s => ((EnumServicoPublicoComunitario) s.TipoServicoPublicoComunitario).ToString() != servico.Value)
                                           select servico).ToList();

            foreach (var servico in listaServicosARemover)
                laudo.ListaServicoPublicoComunitario.Remove(servico);

            foreach (var servico in listaServicosAAdicionar)
            {
                var valorEnum = EnumHelper.ParseEnumValueDescription<EnumServicoPublicoComunitario>(servico.Value);
                laudo.ListaServicoPublicoComunitario.Add(new ServicoPublicoComunitario
                                                             {
                                                                 Descricao = EnumHelper.GetDescription(valorEnum),
                                                                 TipoServicoPublicoComunitario = (int) valorEnum,
                                                                 Laudo = laudo
                                                             });
            }
        }

        private void RemoverOuAdicionarInfraEstruturaUrbana(Laudo laudo, LaudoViewModel model)
        {
            var listaInfrasLaudo = laudo.ListaInfraEstruturaUrbana.ToList();
            var listaInfras = ObterListaOpcoesAPartirString(model.InfrasSelecionadas);

            var listaInfrasARemover = new List<InfraEstruturaUrbana>(
                    from infra in listaInfrasLaudo
                    where listaInfras.All(i => i.Value != ((EnumInfraEstruturaUrbana)infra.TipoInfraEstruturaUrbana).ToString())
                    select infra
                );
                
            var listaInfrasAAdicionar = (from infra in listaInfras
                                        where listaInfrasLaudo.All(i => ((EnumInfraEstruturaUrbana) i.TipoInfraEstruturaUrbana).ToString() != infra.Value)
                                        select infra).ToList();

            foreach (var infra in listaInfrasARemover)
                laudo.ListaInfraEstruturaUrbana.Remove(infra);

            foreach (var infra in listaInfrasAAdicionar)
            {
                var valorEnum = EnumHelper.ParseEnumValueDescription<EnumInfraEstruturaUrbana>(infra.Value);

                laudo.ListaInfraEstruturaUrbana.Add(new InfraEstruturaUrbana
                {
                    Descricao = EnumHelper.GetDescription(valorEnum),
                    TipoInfraEstruturaUrbana = (int)valorEnum,
                    Laudo = laudo
                });
            }
        }

        private DateTime? ObterDataHora(string data, string horario)
        {
            DateTime resultado;

            if (DateTime.TryParse(data, out resultado))
            {
                if (!string.IsNullOrEmpty(horario))
                {
                    var horaArray = horario.Split(':');
                    if (horaArray.Length == 2)
                    {
                        double horas;
                        if (Double.TryParse(horaArray[0], out horas))
                        {
                            resultado = resultado.AddHours(horas);
                            double minutos;

                            if (Double.TryParse(horaArray[1], out minutos))
                                resultado = resultado.AddMinutes(minutos);
                        }
                    }
                }

                return resultado;
            }

            return null;
        }

        private List<SelectListItem> ObterListaOpcoesAPartirString(string listaOpcoesView)
        {
            var listaItens = new List<SelectListItem>();

            if (!string.IsNullOrWhiteSpace(listaOpcoesView))
            {
                var opcoes = listaOpcoesView.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var opcao in opcoes)
                {
                    var parOpcao = opcao.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    var valor = parOpcao[0];
                    bool selecionada;
                    Boolean.TryParse(parOpcao[1], out selecionada);

                    if (selecionada)
                        listaItens.Add(new SelectListItem { Value = valor, Selected = true });
                }
            }

            return listaItens;
        }
        #endregion
    }
}
