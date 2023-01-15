﻿using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ControleEstoque.Web.Controllers
{
    public class ContaController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid) return View(login);

            var usuario = UsuarioModel.Validar(login.Usuario, login.Senha);

            if (usuario != null)
            {
                FormsAuthentication.SetAuthCookie(usuario.Nome, login.Esqueci);
                if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                else return RedirectToAction("Index", "Home");
            }

            else ModelState.AddModelError("", "Login Inválido!"); 
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
