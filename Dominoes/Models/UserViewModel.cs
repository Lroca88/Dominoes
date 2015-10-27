using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dominoes.Models
{
    public class UserViewModel
    {
        public int DominoesGroupID { get; set; }
        public int[] UserIDs { get; set; }
        public MultiSelectList UsersSelect { get; set; }
    }
}