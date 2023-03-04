using ControleEstoque.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ControleEstoque.Web.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Login obrigatório")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Senha obrigatória")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        public string Nome { get; set; }

        #region Acesso a dados

        public static UsuarioModel Validar(string login, string senha)
        {
            UsuarioModel ret = null;
            SqlConnection conexao = new SqlConnection();

            try
            {
                using (conexao)
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "SELECT * FROM usuario WHERE login = @login AND senha = @senha";
                        comando.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                        comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(senha);
                        var reader = comando.ExecuteReader();

                        if (reader.Read())
                        {
                            ret = new UsuarioModel
                            {
                                Id = (int)reader["id"],
                                Login = (string)reader["login"],
                                Senha = (string)reader["senha"],
                                Nome = (string)reader["nome"]
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível acessar o banco de dados. " + e.Message);
            }
            finally
            {
                if (conexao.State == ConnectionState.Open) conexao.Close();
            }
            return ret;
        }

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
                    comando.CommandText = "SELECT COUNT(*) FROM usuario";
                    retorno = (int)comando.ExecuteScalar();
                }
                conexao.Close();
            }
            return retorno;
        }

        public static List<UsuarioModel> RecuperarLista(int pagina, int tamPagina)
        {
            var ret = new List<UsuarioModel>();
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    int posicao = (pagina - 1) * tamPagina;

                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "SELECT * FROM usuario ORDER BY nome OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY",
                        posicao > 0 ? posicao - 1 : 0, tamPagina);

                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        ret.Add(new UsuarioModel
                        {
                            Id = (int)reader["id"],
                            Login = (string)reader["login"],
                            Nome = (string)reader["nome"],
                            Senha = (string)reader["senha"]
                        });
                    }
                }
                conexao.Close();
            }
            return ret;
        }

        public static UsuarioModel RecuperarPeloId(int id)
        {
            var ret = new UsuarioModel();
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "SELECT * FROM usuario WHERE (id = @id)";
                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var reader = comando.ExecuteReader();

                    if (reader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            Id = (int)reader["id"],
                            Login = (string)reader["login"],
                            Nome = (string)reader["nome"]
                        };
                    }
                }
                conexao.Close();
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
                    if (model.Id == 0)
                    {
                        comando.CommandText = "INSERT INTO usuario " +
                                              "(login, senha, nome) " +
                                              "VALUES " +
                                              "(@login, @senha, @nome) " +
                                              "SELECT CONVERT(int, SCOPE_IDENTITY());";
                        comando.Parameters.Add("@login", SqlDbType.VarChar).Value = this.Login;
                        comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "UPDATE usuario SET login = @login, " +
                                              (!string.IsNullOrEmpty(this.Senha) ? "senha = @senha, " : "") +
                                              "nome = @nome " +
                                              "WHERE id = @id; ";
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;
                        comando.Parameters.Add("@login", SqlDbType.VarChar).Value = this.Login;
                        if (!string.IsNullOrEmpty(this.Senha)) comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        if (comando.ExecuteNonQuery() > 0) ret = this.Id;
                    }
                }
                conexao.Close();
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
                        comando.CommandText = "DELETE FROM usuario WHERE (id = @id)";
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                        ret = (comando.ExecuteNonQuery()) > 0;
                    }
                    conexao.Close();
                }
            }
            return ret;
        }

        public string RecuperarStringNomePerfis()
        {
            string ret = string.Empty;
            var conexao = new SqlConnection();
            conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
            conexao.Open();
            try
            {
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "SELECT p.nome " +
                        "FROM perfilUsuario pu, perfil p " +
                        "WHERE pu.usuarioId = @usuarioId " +
                        "AND pu.perfilId = p.id " +
                        "AND p.ativo = 1; ");
                    comando.Parameters.Add("@usuarioId", SqlDbType.Int).Value = this.Id;

                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        ret += (ret != string.Empty ? ";" : "") + (string)reader["nome"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao conectar com o banco de dados. " + ex.Message);
            }
            finally
            {
                if (conexao.State == ConnectionState.Open) conexao.Close();
            }
            return ret;
        }

        #endregion
    }
}
