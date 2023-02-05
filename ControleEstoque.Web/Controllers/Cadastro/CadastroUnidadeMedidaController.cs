using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers.Cadastro
{
    public class CadastroUnidadeMedidaController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 10;

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 20, 30, 40 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = UnidadeMedidaModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = UnidadeMedidaModel.RecuperarQuantidade();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;

            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult UnidadeMedidaPagina(int pagina, int tamPag)
        {
            var lista = UnidadeMedidaModel.RecuperarLista(pagina, tamPag);

            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarUnidadeMedida(int id)
        {
            return Json(UnidadeMedidaModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize(Roles = "Gerente, Administrativo")]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarUnidadeMedida(UnidadeMedidaModel model)
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
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [Authorize(Roles = "Gerente")]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirUnidadeMedida(int id)
        {
            return Json(UnidadeMedidaModel.ExcluirPeloId(id));
        }
    }
}