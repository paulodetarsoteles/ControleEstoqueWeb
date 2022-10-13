using System.Data.SqlClient;

namespace ControleEstoque.Web.Models
{
    public class UsuarioModel
    {
        public static bool Validar(string login, string senha)
        {
            var ret = false; 
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = @"Password=12345678;User ID=sa;Database=ControleEstoqueDb;Data Source=DESKTOP-JVF2A0R\SQLEXPRESS";
                conexao.Open(); 
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("SELECT COUNT(*) FROM usuario WHERE login='{0}' AND senha='{1}'", login, senha);
                    ret = ((int)comando.ExecuteScalar() > 0); 
                }
            }
            return ret; 
        }
    }
}
