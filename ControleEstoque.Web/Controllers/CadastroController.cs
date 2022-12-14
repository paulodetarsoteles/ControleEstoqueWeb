using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class CadastroController : Controller
    {
        private const string _senhapadrao = "1q2w3e4r";

        #region Produto

        [Authorize]
        public ActionResult Produto()
        {
            return View();
        }

        #endregion

        #region Marca Produto

        [Authorize]
        public ActionResult MarcaProduto()
        {
            return View();
        }

        #endregion

        #region Grupo de Produtos

        [Authorize]
        public ActionResult GrupoProduto()
        {
            List<GrupoProdutoModel> listaGrupoProduto = GrupoProdutoModel.RecuperarLista();

            ViewBag.PagAtual = 1; 
            ViewBag.QtdeLinhasPorPagina = 10;
            int restoPaginas = listaGrupoProduto.Count % ViewBag.QtdeLinhasPorPagina > 0 ? 1 : 0;
            ViewBag.QtdePaginas = listaGrupoProduto.Count / ViewBag.QtdeLinhasPorPagina + restoPaginas;

            return View(listaGrupoProduto);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarGrupoProduto(int id)
        {
            return Json(GrupoProdutoModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarGrupoProduto(GrupoProdutoModel model)
        {
            string resultado = "Ok";
            string idSalvo = string.Empty;
            List<string> mensagens = new List<string>();

            if (!ModelState.IsValid)
            {
                resultado = "Aviso";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList(); 
            }
            else
            {
                try
                {
                    int id = model.Salvar();

                    if (id > 0) idSalvo = id.ToString(); 
                    else resultado = "Erro"; 
                }
                catch (Exception)
                {
                    resultado = "Erro ao salvar"; 
                    throw new Exception(resultado);
                }
            }

            //Vai ser retornado aqui um objeto anônimo
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo});
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirGrupoProduto(int id)
        {
            return Json(GrupoProdutoModel.ExcluirPeloId(id));
        }

        #endregion

        #region LocalProduto

        [Authorize]
        public ActionResult LocalProduto()
        {
            return View();
        }

        #endregion

        #region Fornecedor

        [Authorize]
        public ActionResult Fornecedor()
        {
            return View();
        }

        #endregion

        #region Usuarios

        [Authorize]
        public ActionResult Usuario()
        {
            ViewBag.SenhaPadrao = _senhapadrao; 
            return View(UsuarioModel.RecuperarLista());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarUsuario(int id)
        {
            return Json(UsuarioModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarUsuario(UsuarioModel model)
        {
            string resultado = "Ok";
            string idSalvo = string.Empty;
            List<string> mensagens = new List<string>();

            if (!ModelState.IsValid)
            {
                resultado = "Aviso";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    if (model.Senha == _senhapadrao) model.Senha = ""; 

                    int id = model.Salvar();

                    if (id > 0) idSalvo = id.ToString();
                    else resultado = "Erro";
                }
                catch (Exception)
                {
                    resultado = "Erro ao salvar";
                    throw new Exception(resultado);
                }
            }

            //Vai ser retornado aqui um objeto anônimo
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioModel.ExcluirPeloId(id));
        }

        #endregion

        #region PerfilUsuario

        [Authorize]
        public ActionResult PerfilUsuario()
        {
            return View();
        }

        #endregion
    }
}
