using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using static ControleEstoque.Web.Models.Enums;

namespace ControleEstoque.Web.Models
{
    public class FornecedorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório!")]
        public string Nome { get; set; }

        public string RazaoSocial { get; set; }

        [Display(Name = "CPF/CNPJ")]
        public string NumDocumento { get; set; }

        [Display(Name = "Tipo de Pessoa")]
        public TipoPessoa TipoPessoa { get; set; }

        public string Telefone { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Contato { get; set; }

        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        public bool Ativo { get; set; }

        #region Acesso a dados

        public static int RecuperarQuantidade()
        {
            int retorno = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();

                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "SELECT COUNT(*) FROM fornecedor";
                    retorno = (int)comando.ExecuteScalar();
                }
                conexao.Close();
            }
            return retorno;
        }

        public static List<FornecedorModel> RecuperarLista(int pagina, int tamPagina, string filtro = "")
        {
            List<FornecedorModel> retorno = new List<FornecedorModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();

                using (var comando = new SqlCommand())
                {
                    int posicao = (pagina - 1) * tamPagina;
                    string filtroWhere = "";

                    if (!string.IsNullOrEmpty(filtro))
                        filtroWhere = string.Format("WHERE LOWER(nome) LIKE '%{0}%' ", filtro.ToLower());

                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "SELECT * " +
                        "FROM fornecedor " +
                        filtroWhere +
                        "ORDER BY nome " +
                        "OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;",
                        posicao > 0 ? posicao - 1 : 0, tamPagina);

                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        retorno.Add(new FornecedorModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            RazaoSocial = (string)reader["razaoSocial"],
                            NumDocumento = (string)reader["numDocumento"],
                            TipoPessoa = (TipoPessoa)((int)reader["tipoPessoa"]),
                            Telefone = (string)reader["telefone"],
                            Contato = (string)reader["contato"],
                            Endereco = (string)reader["endereco"],
                            Ativo = (bool)reader["ativo"]
                        });
                    }
                }
                conexao.Close();
            }
            return retorno;
        }

        public static FornecedorModel RecuperarPeloId(int id)
        {
            FornecedorModel retorno = null;
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "SELECT * FROM fornecedor WHERE (id = @id)";
                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var reader = comando.ExecuteReader();

                    if (reader.Read())
                    {
                        retorno = new FornecedorModel()
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            RazaoSocial = (string)reader["razaoSocial"],
                            NumDocumento = (string)reader["numDocumento"],
                            TipoPessoa = (TipoPessoa)((int)reader["tipoPessoa"]),
                            Telefone = (string)reader["telefone"],
                            Contato = (string)reader["contato"],
                            Endereco = (string)reader["endereco"],
                            Ativo = (bool)reader["ativo"]
                        };
                    }
                }
                conexao.Close();
            }
            return retorno;
        }

        public int Salvar()
        {
            int retorno = 0;
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
                        comando.CommandText = "INSERT INTO fornecedor " +
                                                    "(nome, razaoSocial, numDocumento, tipoPessoa, telefone, contato, endereco, ativo) " +
                                                 "VALUES " +
                                                    "(@nome, @razaoSocial, @numDocumento, @tipoPessoa, @telefone, @contato, @endereco, @ativo); " +
                                              "SELECT CONVERT(int, SCOPE_IDENTITY());";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@razaoSocial", SqlDbType.VarChar).Value = this.RazaoSocial ?? "";
                        comando.Parameters.Add("@numDocumento", SqlDbType.VarChar).Value = this.NumDocumento ?? "";
                        comando.Parameters.Add("@tipoPessoa", SqlDbType.Int).Value = this.TipoPessoa;
                        comando.Parameters.Add("@telefone", SqlDbType.VarChar).Value = this.Telefone ?? "";
                        comando.Parameters.Add("@contato", SqlDbType.VarChar).Value = this.Contato ?? "";
                        comando.Parameters.Add("@endereco", SqlDbType.VarChar).Value = this.Endereco ?? "";
                        comando.Parameters.Add("@ativo", SqlDbType.Bit).Value = (this.Ativo ? 1 : 0);

                        retorno = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "UPDATE fornecedor " +
                                              "SET " +
                                                    "nome = @nome, " +
                                                    "razaoSocial = @razaoSocial, " +
                                                    "numDocumento = @numDocumento, " +
                                                    "tipoPessoa = @tipoPessoa, " +
                                                    "telefone = @telefone, " +
                                                    "contato = @contato, " +
                                                    "endereco = @endereco, " +
                                                    "ativo = @ativo " +
                                              "WHERE " +
                                                    "id = @id;";

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@razaoSocial", SqlDbType.VarChar).Value = this.RazaoSocial ?? "";
                        comando.Parameters.Add("@numDocumento", SqlDbType.VarChar).Value = this.NumDocumento ?? "";
                        comando.Parameters.Add("@tipoPessoa", SqlDbType.Int).Value = this.TipoPessoa;
                        comando.Parameters.Add("@telefone", SqlDbType.VarChar).Value = this.Telefone ?? "";
                        comando.Parameters.Add("@contato", SqlDbType.VarChar).Value = this.Contato ?? "";
                        comando.Parameters.Add("@endereco", SqlDbType.VarChar).Value = this.Endereco ?? "";
                        comando.Parameters.Add("@ativo", SqlDbType.Bit).Value = (this.Ativo ? 1 : 0);
                        if (comando.ExecuteNonQuery() > 0) retorno = this.Id;
                    }
                }
                conexao.Close();
            }
            return retorno;
        }

        public static bool ExcluirPeloId(int id)
        {
            bool retorno = false;
            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "DELETE FROM fornecedor WHERE (id = @id)";
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                        retorno = (comando.ExecuteNonQuery()) > 0;
                    }
                    conexao.Close();
                }
            }
            return retorno;
        }

        #endregion
    }
}