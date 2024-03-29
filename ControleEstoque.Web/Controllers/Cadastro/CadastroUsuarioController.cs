﻿using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    [Authorize(Roles = "Gerente, Administrativo")]
    public class CadastroUsuarioController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 10;
        private const string _senhapadrao = "1q2w3e4r";

        public ActionResult Index()
        {
            ViewBag.SenhaPadrao = _senhapadrao;
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 20, 30, 40 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = UsuarioModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = UsuarioModel.RecuperarQuantidade();
            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;

            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UsuarioPagina(int pagina, int tamPag)
        {
            var lista = UsuarioModel.RecuperarLista(pagina, tamPag);

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarUsuario(int id)
        {
            return Json(UsuarioModel.RecuperarPeloId(id));
        }

        [HttpPost]
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gerente")]
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioModel.ExcluirPeloId(id));
        }
    }
}
