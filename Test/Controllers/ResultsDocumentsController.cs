using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.DTO;
using Test.Models;

namespace Test.Controllers
{
    public class ResultsDocumentsController : Controller
    {
        // GET: ResultsDocuments
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetResult(int pageIndex, int pageSize)
        {
            int personId = 9;
            DBEntities db = new DBEntities();
            var result = (from p in db.ResultsDocuments
                          let count = db.ResultsDocuments.Count(x => x.PersonId == personId)
                          select new ResultsDocumentsDTO
                          {
                              Id = p.Id,
                              PersonId = p.PersonId,
                              Procedure = p.Procedure,
                              Description = p.Description,
                              Documents = p.Documents,
                              Active = p.Active,
                              RegistrationDate = p.RegistrationDate,
                              ModificationDate = p.ModificationDate,
                              RegistrationUser = p.RegistrationUser,
                              ModificationUser = p.ModificationUser,
                              totalcount = count
                          }).Where(x => x.PersonId == personId)
                           .ToList();

            if (pageIndex < 1)
                pageIndex = 1;

            int recsCount = result.Count();
            var pager = new Pager(recsCount, pageIndex, pageSize);
            int recSkip = (pageIndex - 1) * pageSize;
            var data = result.Skip(recSkip).Take(pager.PageSize).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}