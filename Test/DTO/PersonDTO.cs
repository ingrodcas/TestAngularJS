using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.DTO
{
    public class PersonDTO
    {
        public int Id { get; set; }
        public Nullable<int> PersonTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public Nullable<System.DateTime> ModificationDate { get; set; }
        public Nullable<System.Guid> RegistrationUser { get; set; }
        public Nullable<System.Guid> ModificationUser { get; set; }
        public string Identificacion { get; set; }
        public int totalcount { get; set; }
    }
}