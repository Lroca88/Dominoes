using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dominoes.Models
{
    public class DominoesGroup
    {

        public DominoesGroup() 
        {
            this.Users = new HashSet<UserProfileInfo>();    
        } 

        public int DominoesGroupID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Admin { get; set; }

        public virtual ICollection<UserProfileInfo> Users { get; set; }
    }
}