using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class OperacaoController : Controller
    {
        // GET: Operacao
        public ActionResult Entrada()
        {
            return View();
        }

        public ActionResult Saida()
        {
            return View();
        }

        public ActionResult Perda()
        {
            return View();
        }

        public ActionResult Inventario()
        {
            return View();
        }
    }
}