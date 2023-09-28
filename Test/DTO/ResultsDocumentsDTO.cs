using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.DTO
{
    public class ResultsDocumentsDTO
    {
        public int Id { get; set; }
        public Nullable<int> PersonId { get; set; }
        public string Description { get; set; }
        public string Procedure { get; set; }
        
        public byte[] Documents { get; set; }
        public string sDocuments { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public Nullable<System.DateTime> ModificationDate { get; set; }
        public Nullable<System.Guid> RegistrationUser { get; set; }
        public Nullable<System.Guid> ModificationUser { get; set; }
        public int totalcount { get; set; }
    }
}