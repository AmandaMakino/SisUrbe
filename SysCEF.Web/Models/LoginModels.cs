using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SysCEF.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "É obrigatório informar o E-mail.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "É obrigatório informar a Senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        public bool DadosInvalidos { get; set; }
    }

    public class AlterarSenhaModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Atual senha")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "É obrigatório informar uma nova senha.")]
        [StringLength(100, ErrorMessage = "A nova senha deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "É obrigatório confirmar a nova senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha de Confirmação")]
        [Compare("NovaSenha", ErrorMessage = "A senha de confirmação difere da nova senha.")]
        public string Confirmacao { get; set; }

        public string MensagemErro { get; set; }
    }
}
