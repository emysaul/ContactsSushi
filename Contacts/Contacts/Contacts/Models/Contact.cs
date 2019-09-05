using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Contacts.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public ImageSource Image { get; set; }
        public string Type { get; set; }
    }
}
