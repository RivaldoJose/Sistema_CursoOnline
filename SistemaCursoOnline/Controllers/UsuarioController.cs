using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaCursoOnline.Context;
using SistemaCursoOnline.Models;

namespace SistemaCursoOnline.Controllers
{
    public class UsuarioController : Controller
    {
        private Context.Context db = new Context.Context();

        bool detalhesComuns = false;

        // GET: Usuario
        public ActionResult Index()
        {
            return View(db.tbUsuario.ToList());
        }

        public JsonResult Valida_CPF(string cpf)
        {
            Usuario c = db.tbUsuario.FirstOrDefault(s => s.CPF == cpf);
            bool encontrouCPF = false;

            try
            {
                if (ValidacaoInternaCPF(cpf))
                {
                    /* if (c == default)
                     {
                         encontrouCPF = true;
                         return Json(encontrouCPF, JsonRequestBehavior.AllowGet);
                     }
                     //return Json(RedirectToAction("CreateCPF", "Usuario", new { usuario = c }));
                     return Json(String.Format("O cpf {0} já existe no sistema", cpf), JsonRequestBehavior.AllowGet);*/
                    return Json(true, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(encontrouCPF, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                return Json(encontrouCPF, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreateCPF(Usuario usuario)
        {
            List<Turma> listaTurma = db.tbTurma.Where(s => s.Ativo == true).ToList();

            ViewBag.ListaTurma = listaTurma;
            return View();
        }

        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Remove caracteres não numéricos
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemovePontos(string text)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[^0-9]");
            string ret = reg.Replace(text, string.Empty);
            return ret;
        }

        /// <summary>
        /// Valida se um cpf é válido
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool ValidacaoInternaCPF(string cpf)
        {
            //Remove formatação do número, ex: "123.456.789-01" vira: "12345678901"
            cpf = RemovePontos(cpf);

            if (cpf.Length > 11)
                return false;

            while (cpf.Length != 11)
                cpf = '0' + cpf;

            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;

            if (igual || cpf == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(cpf[i].ToString());

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.DetalhesComuns = detalhesComuns;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.tbUsuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            List<Turma> turmas = db.tbTurma.ToList();
            ViewBag.ListaTurma = turmas;
            return View(usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            List<Turma> listaTurma = db.tbTurma.Where(x => x.Ativo == true).ToList();

            ViewBag.ListaTurma = listaTurma;
            return View();
        }

        // POST: Usuario/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUser,Nome,CPF,DataNascimento,IdTurma")] Usuario usuario)
        {

            Usuario userCpf = db.tbUsuario.FirstOrDefault(x => x.CPF == usuario.CPF);
            List<Turma> listaTurma = db.tbTurma.Where(x => x.Ativo == true).ToList();
            ViewBag.ListaTurma = listaTurma;

            if (ModelState.IsValid)
            {
                if (userCpf != default)
                {
                    usuario = userCpf;
                    return View("Create", userCpf);//RedirectToAction("Details", "Usuario", new { id = userCpf.IdUser});
                }
                //return View(usuario);
                db.tbUsuario.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userCpf);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.tbUsuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            List<Turma> turmas = db.tbTurma.ToList();
            ViewBag.ListaTurma = turmas;
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUser,Nome,CPF,DataNascimento,IdTurma")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.tbUsuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            List<Turma> turmas = db.tbTurma.ToList();
            ViewBag.ListaTurma = turmas;
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.tbUsuario.Find(id);
            db.tbUsuario.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
