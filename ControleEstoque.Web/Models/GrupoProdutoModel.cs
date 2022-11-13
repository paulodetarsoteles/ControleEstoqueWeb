using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;

namespace ControleEstoque.Web.Models
{
    public class GrupoProdutoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do grupo é obrigatório!")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public static List<GrupoProdutoModel> RecuperarLista()
        {
            var ret = new List<GrupoProdutoModel>();
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "SELECT * FROM grupo_produto ORDER BY nome";
                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        ret.Add(new GrupoProdutoModel()
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Ativo = (bool)reader["ativo"]
                        });
                    }
                }
            }
            return ret;
        }

        public static GrupoProdutoModel RecuperarPeloId(int id)
        {
            GrupoProdutoModel ret = null;
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("SELECT * FROM grupo_produto WHERE (id = {0})", id);
                    var reader = comando.ExecuteReader();

                    if (reader.Read())
                    {
                        ret = new GrupoProdutoModel()
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Ativo = (bool)reader["ativo"]
                        };
                    }
                }
            }
            return ret;
        }

        public int Salvar()
        {
            int ret = 0;
            var model = RecuperarPeloId(this.Id);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    if (model == null)
                    {
                        comando.CommandText = string.Format
                        (
                            "INSERT INTO grupo_produto (nome, ativo) VALUES ('{0}', {1}); SELECT CONVERT(int, SCOPE_IDENTITY());", 
                            this.Nome, this.Ativo ? 1 : 0
                        );
                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = string.Format
                        (
                            "UPDATE grupo_produto SET nome = '{1}', ativo = {2} WHERE id = {0};", 
                            this.Id, this.Nome, this.Ativo ? 1 : 0
                        );
                        if(comando.ExecuteNonQuery() > 0) ret = this.Id;
                    }
                }
            }
            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            bool ret = false;
            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = string.Format("DELETE FROM grupo_produto WHERE (id = {0})", id);
                        ret = (comando.ExecuteNonQuery()) > 0;
                    }
                }
            }
            return ret;
        }
    }
}
