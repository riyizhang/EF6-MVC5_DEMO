using System.Web.Mvc;
using System.Linq;
using System.Net;
using System.Data.Entity;

namespace WebApplication1.Controllers
{
    public class zryController : Controller
    {
        StudentContainer db = new StudentContainer();

        // GET: zry
        public ActionResult Index(string name = "")
        {
            var students = from m in db.StudentInfoSet select m;
            ViewBag.name = new SelectList(students.Select(o => o.Name).Distinct());
            if (!string.IsNullOrEmpty(name))
            {
                students = students.Where(o => o.Name == name);
            }
            
            return View(students);
        }


        public ActionResult Details(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentInfo model = db.StudentInfoSet.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
           
           return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, Age, Name, Sex")] StudentInfo model)
        {
            if (ModelState.IsValid)
            {
                db.StudentInfoSet.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentInfo model = db.StudentInfoSet.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Age, Name, Sex")] StudentInfo model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentInfo model = db.StudentInfoSet.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            StudentInfo model = db.StudentInfoSet.Find(id);
            db.StudentInfoSet.Remove(model);
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