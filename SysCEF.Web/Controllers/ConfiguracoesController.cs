using System;
using System.Data;
using System.Web;
using System.Web.Mvc;
using SysCEF.Common.Interface;
using SysCEF.DAO.Interface;
using SysCEF.Model;

namespace SysCEF.Web.Controllers
{
    public class ConfiguracoesController : Controller
    {
        public ISysCEFWorkLifetimeManager WorkLifetimeManager { get; set; }
        public IConfiguracaoRepositorio ConfiguracaoRepositorio { get; set; }
        
        public ActionResult Index()
        {
            var configuracao = ConfiguracaoRepositorio.Obter(WorkLifetimeManager.Value) ?? new Configuracao();
            
            return PartialView(configuracao);
        }
        
        public string Salvar(Configuracao modelConfiguracao)
        {
            string mensagem;

            WorkLifetimeManager.Value.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                var configuracao = ConfiguracaoRepositorio.Obter(WorkLifetimeManager.Value) ?? new Configuracao();

                configuracao.NomeEmpresa = modelConfiguracao.NomeEmpresa;
                configuracao.CNPJEmpresa = modelConfiguracao.CNPJEmpresa;

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
    }
}
