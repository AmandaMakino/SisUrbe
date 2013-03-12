using System;
using System.Data;
using System.Web;
using System.Web.Mvc;
using SysCEF.Common.Interface;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using SysCEF.Web.Models;
using System.IO;
using SysCEF.Web.Helpers;
using Core;

namespace SysCEF.Web.Controllers
{
    public class ConfiguracoesController : Controller
    {
        public ISysCEFWorkLifetimeManager WorkLifetimeManager { get; set; }
        public IConfiguracaoRepositorio ConfiguracaoRepositorio { get; set; }
        public IFonteRepositorio FonteRepositorio { get; set; }
        public ILinhaRepositorio LinhaRepositorio { get; set; }
        public IProdutoRepositorio ProdutoRepositorio { get; set; }

        public ActionResult Index()
        {
            var configuracao = ConfiguracaoRepositorio.Obter(WorkLifetimeManager.Value) ?? new Configuracao();

            var model = new ConfiguracoesModel
            {
                NomeEmpresa = configuracao.NomeEmpresa,
                CNPJEmpresa = configuracao.CNPJEmpresa,
                TiposImportacao = RadioButtonHelper.ParseEnumToRadioButtonList(EnumTipoImportacao.Fonte)
            };

            return PartialView(model);
        }

        public string Salvar(ConfiguracoesModel model)
        {
            string mensagem;

            WorkLifetimeManager.Value.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                var configuracao = ConfiguracaoRepositorio.Obter(WorkLifetimeManager.Value) ?? new Configuracao();

                configuracao.NomeEmpresa = model.NomeEmpresa;
                configuracao.CNPJEmpresa = model.CNPJEmpresa;

                ConfiguracaoRepositorio.Salvar(WorkLifetimeManager.Value, configuracao);
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

        public string Importar(HttpPostedFileBase fileData, FormCollection forms, string tipo)
        {
            try
            {
                var openXmlHelper = new OpenXmlHelper();
                var memoryStream = new MemoryStream();

                fileData.InputStream.CopyTo(memoryStream);

                var listaDados = openXmlHelper.LerPlanilhaDados(memoryStream);

                int codigo = 0;
                tipo = tipo.Substring(0, tipo.IndexOf('?'));

                switch (EnumHelper.ParseEnumDescription<EnumTipoImportacao>(tipo))
                {
                    case EnumTipoImportacao.Fonte:
                        FonteRepositorio.ExcluirTudo(WorkLifetimeManager.Value);

                        foreach (var item in listaDados)
                        {
                            Int32.TryParse(item[0].ToString(), out codigo);
                            FonteRepositorio.Salvar(WorkLifetimeManager.Value, 
                                new Fonte { Codigo = codigo, Descricao = item[1].ToString() });
                        }
                        break;
                    case EnumTipoImportacao.Linha:
                        LinhaRepositorio.ExcluirTudo(WorkLifetimeManager.Value);
                        foreach (var item in listaDados)
                        {
                            Int32.TryParse(item[0].ToString(), out codigo);
                            LinhaRepositorio.Salvar(WorkLifetimeManager.Value, new Linha { Codigo = codigo, Descricao = item[1].ToString() });
                        }
                        break;
                    case EnumTipoImportacao.Produto:
                        ProdutoRepositorio.ExcluirTudo(WorkLifetimeManager.Value);
                        foreach (var item in listaDados)
                        {
                            Int32.TryParse(item[0].ToString(), out codigo);
                            ProdutoRepositorio.Salvar(WorkLifetimeManager.Value, new Produto { Codigo = codigo, Descricao = item[1].ToString() });
                        }
                        break;
                }

                WorkLifetimeManager.Value.Commit();

                return "Dados importados com sucesso!";
            }
            catch
            {
                WorkLifetimeManager.Value.Rollback();
                return "Não foi possível importar os dados. Verifique a planilha e tente novamente!";
            }
        }
    }
}
