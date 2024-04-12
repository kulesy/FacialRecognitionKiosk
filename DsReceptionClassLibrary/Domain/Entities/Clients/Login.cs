using DsReceptionClassLibrary.Domain.Entities.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.Domain.Entities.Clients
{
    public class Login
    {
        [Display(Name = "No.")]
        public int LoginId { get; set; }
        [Display(Name = "Client(s)")]
        public string FullName { get; set; }
        [Display(Name = "Sign In Date")]
        public DateTime SignInDate { get; set; }
        public string ClientId { get; set; }
        public Client Client { get; set; }
    }
}
