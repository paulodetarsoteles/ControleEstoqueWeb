using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class OperacaoController : Controller
    {
        [Authorize]
        public ActionResult Entrada()
        {
            return View();
        }

        [Authorize]
        public ActionResult Saida()
        {
            return View();
        }

        [Authorize]
        public ActionResult Perda()
        {
            return View();
        }

        [Authorize]
        public ActionResult Inventario()
        {
            return View();
        }
    }
}