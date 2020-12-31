using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisitedCities.Models;

namespace VisitedCities.Controllers
{
    public class CityController : Controller
    {
        // GET: City
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAll()
        {
            return View(GetAllCity());
        }

        IEnumerable<City> GetAllCity()
        {
            using (DBModel db = new DBModel())
            {
                return db.Cities.ToList<City>();
            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            City city = new City();
            if (id != 0)
            {
                using (DBModel db = new DBModel())
                {
                    city = db.Cities.Where(x => x.CityId == id).FirstOrDefault<City>();
                }
            }
            return View(city);
        }

        [HttpPost]
        public ActionResult AddOrEdit(City city)
        {
            try
            {
                using (DBModel db = new DBModel())
                {
                    if (city.CityId == 0)
                    {
                        db.Cities.Add(city);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(city).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllCity()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (DBModel db = new DBModel())
                {
                    City city = db.Cities.Where(x => x.CityId == id).FirstOrDefault<City>();
                    db.Cities.Remove(city);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllCity()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}