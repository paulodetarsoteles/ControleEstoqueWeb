using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class CadastroController : Controller
    {
        // GET: Cadastro
        public ActionResult Produto()
        {
            return View();
        }

        public ActionResult MarcaProduto()
        {
            return View();
        }

        public ActionResult GrupoProduto()
        {
            return View();
        }

        public ActionResult LocalProduto()
        {
            return View();
        }

        public ActionResult Fornecedor()
        {
            return View();
        }

        public ActionResult Usuario()
        {
            return View();
        }

        public ActionResult PerfilUsuario()
        {
            return View();
        }
    }
}