﻿using ControleEstoque.Web.Helpers;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ControleEstoque.Web.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }

        #region Validar Login do Usuário
        public static UsuarioModel Validar(string login, string senha)
        {
            UsuarioModel ret = null; 
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open(); 
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "SELECT * FROM usuario WHERE login=@login AND senha=@senha";
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
            return ret; 
        }
        #endregion
    }
}
