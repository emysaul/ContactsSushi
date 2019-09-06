using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Contacts.Models
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
    }
}
