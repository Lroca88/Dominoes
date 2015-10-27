using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dominoes.Models
{
    public class GameSerie
    {
        public GameSerie()
        {
            this.Games = new HashSet<Game>();
        }

        public int GameSerieID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Notes { get; set; }
        public virtual byte GameWinner { get; set; }
        public virtual byte PollonaValue { get; set; }
        public virtual byte ViajeroValue { get; set; }
        public int UserProfileInfoID { get; set; }

        public virtual UserProfileInfo UserProfileInfo { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}