using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaCursoOnline.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Msg = "Bem-vindo ao Novo Curso Online";
            ViewBag.Objetivo = "O Nosso maior investimento é o seu sucesso!";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Esta aplicação tem o objetivo de aprimorar os conhecimentos da equipe na tecnologia .NET MVC";

            return View();
        }

        public ActionResult Contact()
        {
            if (true)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}