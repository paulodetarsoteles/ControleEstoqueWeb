using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.Web.Models
{
    public class AlterarSenhaViewModel
    {
        [Display(Name = "Senha Atual")]
        [Required(ErrorMessage = "Informa a senha atual")]
        public string SenhaAtual { get; set; }

        [Display(Name = "Nova senha")]
        [Required(ErrorMessage = "Informe a nova senha")]
        public string NovaSenha { get; set; }

        [Display(Name = "Confirme a nova senha")]
        [Required(ErrorMessage = "Precisa confirmar a nova senha")]
        [Compare("NovaSenha", ErrorMessage = "A nova senha está diferente da confirmação")]
        public string ConfirmacaoNovaSenha { get; set; }
    }
}
