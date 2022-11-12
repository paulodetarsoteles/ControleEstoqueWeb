using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Usuário: ")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Senha obrigatória")]
        [Display(Name = "Senha: ")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        public bool Esqueci { get; set; }
    }
}