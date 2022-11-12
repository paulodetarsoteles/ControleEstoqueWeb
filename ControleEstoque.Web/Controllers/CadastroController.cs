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
        private static List<GrupoProdutoModel> _listaGrupoProduto = new List<GrupoProdutoModel>
        {
            new GrupoProdutoModel() {Id = 1, Nome = "Livros", Ativo = true},
            new GrupoProdutoModel() {Id = 2, Nome = "Instrumentos", Ativo = true},
            new GrupoProdutoModel() {Id = 3, Nome = "Camisas", Ativo = false}
        };

        [Authorize]
        public ActionResult Produto()
        {
            return View();
        }

        [Authorize]
        public ActionResult MarcaProduto()
        {
            return View();
        }

        [Authorize]
        public ActionResult GrupoProduto()
        {
            return View(_listaGrupoProduto);
        }

        [HttpPost]
        [Authorize]
        public ActionResult RecuperarGrupoProduto(int id)
        {
            return Json(_listaGrupoProduto.Find(x => x.Id == id));
        }

        [HttpPost]
        [Authorize]
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
                    var registroBD = _listaGrupoProduto.Find(x => x.Id == model.Id);

                    if (registroBD == null)
                    {
                        registroBD = model;
                        registroBD.Id = _listaGrupoProduto.Max(x => x.Id) + 1;
                        _listaGrupoProduto.Add(registroBD);
                    }
                    else
                    {
                        registroBD.Nome = model.Nome;
                        registroBD.Ativo = model.Ativo;
                    }
                }
                catch (Exception)
                {
                    resultado = "Erro"; 
                    throw new Exception(resultado);
                }
            }

            //Vai ser retornado aqui um objeto anônimo
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo});
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExcluirGrupoProduto(int id)
        {
            var ret = false;
            var registroBD = _listaGrupoProduto.Find(x => x.Id == id);
            if (registroBD != null)
            {
                _listaGrupoProduto.Remove(registroBD);
                ret = true;
            }
            return Json(ret);
        }

        [Authorize]
        public ActionResult LocalProduto()
        {
            return View();
        }

        [Authorize]
        public ActionResult Fornecedor()
        {
            return View();
        }

        [Authorize]
        public ActionResult Usuario()
        {
            return View();
        }

        [Authorize]
        public ActionResult PerfilUsuario()
        {
            return View();
        }
    }
}