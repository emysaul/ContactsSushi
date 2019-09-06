using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Contacts.Models
{
    public class Contact : Entity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string EmailHome { get; set; }
        public string Note { get; set; }
        public string ImagePath { get; set; }
        public string Type { get; set; }
    }
}
