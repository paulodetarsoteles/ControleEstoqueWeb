using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.Web.Models
{
    public class GrupoProdutoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Nome do grupo é obrigatório!")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }
    }
}