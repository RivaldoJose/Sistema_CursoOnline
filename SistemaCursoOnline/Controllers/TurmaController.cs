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
    public class TurmaController : Controller
    {
        private Context.Context db = new Context.Context();

        // GET: Turma
        public ActionResult Index()
        {
            List<Turma> listaTurmas = db.tbTurma.OrderBy(t => t.DescricaoTurma).ToList();
            List<Curso> listaCursos = db.tbCurso.ToList();
            ViewBag.ListaCursos = listaCursos;

            return View(listaTurmas);
        }

        // GET: Turma/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turma turma = db.tbTurma.Find(id);
            if (turma == null)
            {
                return HttpNotFound();
            }
            return View(turma);
        }

        // GET: Turma/Create
        public ActionResult Create()
        {
            Turma turma = new Turma();
            turma.Ativo = true;
            ViewBag.ListaTurno = new List<string>() { "Matutino", "Vespertino", "Noturno" };

            List<Curso> listaCurso = db.tbCurso.ToList();
            ViewBag.ListaCurso = listaCurso;
            return View(turma);
        }

        // POST: Turma/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTurma,IdCurso,DescricaoTurma,QtdeAlunos,Turno,Ativo")] Turma turma)
        {
            if (ModelState.IsValid)
            {
                db.tbTurma.Add(turma);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<Curso> listaCurso = db.tbCurso.ToList();
            ViewBag.ListaCurso = listaCurso;
            return View(turma);
        }

        // GET: Turma/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turma turma = db.tbTurma.Find(id);
            if (turma == null)
            {
                return HttpNotFound();
            }
            List<Curso> listaCurso = db.tbCurso.ToList();
            ViewBag.ListaCurso = listaCurso;
            return View(turma);
        }

        // POST: Turma/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTurma,IdCurso,DescricaoTurma,QtdeAlunos,Turno,Ativo")] Turma turma)
        {
            List<Curso> listaCurso = db.tbCurso.ToList();
            List<Usuario> listaUsuarios = db.tbUsuario.Where(x => x.IdTurma == turma.IdTurma).ToList();

            if (ModelState.IsValid)
            {
                if (listaUsuarios.Count <= turma.QtdeAlunos)
                {
                    db.Entry(turma).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Erro! A quantidade de alunos já matriculados nesta turma é superior " +
                    "à quantidade de vagas que você deseja alterar");
            }
            ViewBag.ListaCurso = listaCurso;
            return View(turma);
        }

        // GET: Turma/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turma turma = db.tbTurma.Find(id);
            if (turma == null)
            {
                return HttpNotFound();
            }
            return View(turma);
        }

        // POST: Turma/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Turma turma = db.tbTurma.Find(id);
            db.tbTurma.Remove(turma);
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
