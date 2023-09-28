using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using Test.DTO;
using Test.Enums;
using Test.Models;

namespace Test.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPersons(int pageIndex, int pageSize)
        {
            DBEntities db = new DBEntities();
            var persons = (from p in db.Person
                           let count = db.Person.Count(x => x.PersonTypeId == (int)PersonType_Enums.PASIENTE)
                           select new PersonDTO
                           {
                               Id = p.Id,
                               PersonTypeId = p.PersonTypeId,
                               FirstName = p.FirstName,
                               LastName = p.LastName,
                               Active = p.Active,
                               Age = p.Age,
                               Phone = p.Phone,
                               Email = p.Email,
                               RegistrationDate = p.RegistrationDate,
                               ModificationDate = p.ModificationDate,
                               RegistrationUser = p.RegistrationUser,
                               ModificationUser = p.ModificationUser,
                               totalcount = count
                           }).Where(x => x.PersonTypeId == (int)PersonType_Enums.PASIENTE)
                           .ToList();

            if (pageIndex < 1)
                pageIndex = 1;

            int recsCount = persons.Count();
            var pager = new Pager(recsCount, pageIndex, pageSize);
            int recSkip = (pageIndex - 1) * pageSize;
            var data = persons.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            this.ViewBag.CurrentPage = pageIndex;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public string InsertPerson(Person person)
        {
            using (DBEntities db = new DBEntities())
            {
                person.PersonTypeId = (int)PersonType_Enums.PASIENTE;
                person.Active = true;
                person.RegistrationDate = DateTime.Now;
                person.RegistrationUser = Guid.NewGuid();
                db.Person.Add(person);
                db.SaveChanges();
                InsertUser(person.Id, person.Identificacion);
                return "Registro Guardado Satisfactoriamente.";

            }
        }
        public string UpdatePerson(Person person)
        {
            using (DBEntities db = new DBEntities())
            {
                var record = db.Person.Where(x => x.Id == person.Id).FirstOrDefault();
                record.PersonTypeId = (int)PersonType_Enums.PASIENTE;
                record.FirstName = person.FirstName;
                record.LastName = person.LastName;
                record.Age = person.Age;
                record.Phone = person.Phone;
                record.Email = person.Email;
                record.Active = person.Active;
                record.ModificationDate = DateTime.Now;
                record.ModificationUser = Guid.NewGuid();
                db.SaveChanges();
                return "Registro Actualizado Satisfactoriamente.";
            }
        }
        public string UploadFiles(object obj)
        {
            var sPeronid = Request.Headers["X-Person-Id"];
            var fileName = Request.Headers["X-File-Name"];
            var sProcedure = Request.Headers["X-Procedure-Id"];
            int? peronid = Convert.ToInt32(sPeronid.ToString());
            Byte[] image = null;
            Stream fs = Request.InputStream;
            BinaryReader br = new BinaryReader(fs);
            image = br.ReadBytes((Int32)fs.Length);

            using (DBEntities db = new DBEntities())
            {
                var record = db.ResultsDocuments.Where(x => x.PersonId.Value == peronid).FirstOrDefault();
                if (record == null)
                {
                    ResultsDocuments resultsDocuments = new ResultsDocuments();
                    resultsDocuments.PersonId = Convert.ToInt32(peronid);
                    resultsDocuments.Description = fileName;
                    resultsDocuments.Documents = image;
                    resultsDocuments.Procedure = sProcedure;
                    resultsDocuments.Active = true;
                    resultsDocuments.RegistrationDate = DateTime.Now;
                    resultsDocuments.RegistrationUser = Guid.NewGuid();
                    db.ResultsDocuments.Add(resultsDocuments);
                    db.SaveChanges();
                }
                else
                {
                    record.Description = fileName;
                    record.Documents = image;
                    record.Procedure = sProcedure;
                    record.Active = true;
                    record.ModificationDate = DateTime.Now;
                    record.ModificationUser = Guid.NewGuid();
                    db.SaveChanges();
                }
                return "Registro Guardado Satisfactoriamente.";
            }
            //var length = Request.ContentLength;
            //var bytes = new byte[length];
            //Request.InputStream.Read(bytes, 0, length);



            //var fileName = Request.Headers["X-File-Name"];
            //var fileSize = Request.Headers["X-File-Size"];
            //var fileType = Request.Headers["X-File-Type"];
            //var peronid = Request.Headers["X-Person-Id"];

            //var saveToFileLoc = "D:\\Images\\" + fileName;
            //var fileStream = new FileStream(saveToFileLoc, 
            //                     FileMode.Create, 
            //                     FileAccess.ReadWrite);
            //fileStream.Write(bytes, 0, length);
            //fileStream.Close();

            //return string.Format("{0} bytes uploaded", bytes.Length);
        }
        public void InsertUser(int PersonId, string desc)
        {
            using (DBEntities db = new DBEntities())
            {
                Users users = new Users();
                users.PersonId = PersonId;
                users.UserName = desc;
                users.Password = desc;
                users.IsTemporal = false;
                users.IsBlock = false;
                users.Active = true;
                users.RegistrationDate = DateTime.Now;
                users.RegistrationUser = Guid.NewGuid();
                db.Users.Add(users);
                db.SaveChanges();
            }
        }
        public ActionResult Image(String nombre)
        {
            String keys = CloudConfigurationManager.GetSetting("ConexionBlobs");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("imagenes");
            if (nombre != null)
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(nombre);
                blockBlob.DeleteIfExists();
            }
            List<Images> imglist = new List<Images>();
            foreach (IListBlobItem item in container.ListBlobs(null, true))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    Images img = new Images();
                    img.Name = blob.Name;
                    img.UrlImage = blob.Uri.AbsoluteUri;
                    imglist.Add(img);
                }
            }
            return View(imglist);
        }
        [HttpPost]
        public ActionResult Image()
        {
            var file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                String keys = CloudConfigurationManager.GetSetting("ConexionBlobs");
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("imagenes");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);
                blockBlob.UploadFromStream(file.InputStream);
            }
            return RedirectToAction("Index");
        }

    }
}