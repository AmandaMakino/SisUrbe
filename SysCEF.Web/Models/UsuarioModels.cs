using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Core;
using SysCEF.Model;

namespace SysCEF.Web.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "É obrigatório informar um nome.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "É obrigatório informar um e-mail.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        public int Perfil { get; set; }

        [Required(ErrorMessage = "É obrigatório informar uma senha.")]
        [StringLength(100, ErrorMessage = "A nova senha deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "É obrigatório confirmar a senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha de Confirmação")]
        [Compare("Senha", ErrorMessage = "A senha de confirmação difere da nova senha.")]
        public string Confirmacao { get; set; }
        
        public string CPF { get; set; }
        
        public string CREA { get; set; }
    }

    public class UsuarioViewModel
    {
        public UsuarioModel Usuario { get; set; }
        public RadioButtonList<EnumPerfil> OpcoesPerfil { get; set; }
    }

    public class ListaUsuarioViewModel
    {
        public IEnumerable<Usuario> Usuarios { get; set; }
        public string Mensagem { get; set; }
    }
}
