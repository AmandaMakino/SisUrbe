using System.Collections.Generic;
using System.Web.Mvc;
using Core;
using SysCEF.Model;

namespace SysCEF.Web.Models
{
    public class ConfiguracoesModel
    {
        public string NomeEmpresa { get; set; }
        public string CNPJEmpresa { get; set; }
        public string MensagemUpload { get; set; }
        public RadioButtonList<EnumTipoImportacao> TiposImportacao { get; set; }
    }
}