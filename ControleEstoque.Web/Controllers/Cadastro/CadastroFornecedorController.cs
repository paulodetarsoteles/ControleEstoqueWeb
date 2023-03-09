using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers.Cadastro
{
    [Authorize(Roles = "Gerente,Administrativo,Operador")]
    public class CadastroFornecedorController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 10;

        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 20, 30, 40 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = FornecedorModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = FornecedorModel.RecuperarQuantidade();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult FornecedorPagina(int pagina, int tamPag, string filtro)
        {
            var lista = FornecedorModel.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarFornecedor(int id)
        {
            return Json(FornecedorModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarFornecedor(FornecedorModel model)
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
                    model.NumDocumento = String.Join("", System.Text.RegularExpressions.Regex.Split(model.NumDocumento, @"[^\d]"));
                    model.Telefone = String.Join("", System.Text.RegularExpressions.Regex.Split(model.Telefone, @"[^\d]")); 

                    int id = model.Salvar();

                    if (id > 0) 
                        idSalvo = id.ToString();
                    else 
                        resultado = "Erro";
                }
                catch (Exception)
                {
                    resultado = "Erro ao salvar";
                    throw new Exception(resultado);
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gerente")]
        public JsonResult ExcluirFornecedor(int id)
        {
            return Json(FornecedorModel.ExcluirPeloId(id));
        }
    }
}