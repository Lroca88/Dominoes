using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dominoes.Models
{
    public class UserProfileInfo
    {
        public UserProfileInfo()
        {
            this.Groups = new HashSet<DominoesGroup>();         // Groups the user belongs to
            this.GameSeries = new HashSet<GameSerie>();         // Game Series administered by the user
        }

        public int UserProfileInfoID { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Email{ get; set; }
        public virtual int GroupAdministered { get; set; }

        public virtual ICollection<DominoesGroup> Groups { get; set; }
        public virtual ICollection<GameSerie> GameSeries { get; set; }
    }
}