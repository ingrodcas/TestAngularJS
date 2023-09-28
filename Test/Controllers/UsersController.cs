using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.DTO;
using Test.Enums;
using Test.Models;

namespace Test.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Authenticate(string UserName, string Password)
        {
            DBEntities db = new DBEntities();
            var user = (from u in db.Users
                        join p in db.Person
                        on u.PersonId equals p.Id
                        select new
                        {
                            Id = u.Id,
                            PersonId = u.PersonId,
                            UserName = u.UserName,
                            Password = u.Password,
                            IsTemporal = u.IsTemporal,
                            IsBlock = u.IsBlock,
                            Active = u.Active,
                            RegistrationDate = u.RegistrationDate,
                            ModificationDate = u.ModificationDate,
                            RegistrationUser = u.RegistrationUser,
                            ModificationUser = u.ModificationUser,
                            Person = new PersonDTO()
                            {
                                Id = p.Id,
                                PersonTypeId = p.PersonTypeId,
                                FirstName = p.FirstName,
                                LastName = p.LastName,
                                Active = p.Active,
                                RegistrationDate = p.RegistrationDate,
                                ModificationDate = p.ModificationDate,
                                RegistrationUser = p.RegistrationUser,
                                ModificationUser = p.ModificationUser,
                            },
                        }).Where(x => x.UserName == UserName
                        && x.Password == Password).FirstOrDefault();
            //if (user.Person.PersonTypeId == (int)PersonType_Enums.ADMINISTRADOR)
            //{
            //    return RedirectToAction("Index", "Person", new { pageIndex = 1, pageSize = 5 });
            //}
            //else
            //{
            //    return RedirectToAction("Index", "ResultsDocuments");
            //}
            return Json(user, JsonRequestBehavior.AllowGet);
        }
    }
}