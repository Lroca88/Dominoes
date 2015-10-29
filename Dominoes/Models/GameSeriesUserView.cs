using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dominoes.Models
{
    public class GameSeriesUserView
    {
        public GameSeriesUserView()
        {
            this.GameSerieID = new List<int>();
            this.GameSerieName = new List<string>();
            this.TotalPoints = new List<int>();
        
        }
        public int UserProfileInfoID { get; set; }
        public string UserName { get; set; }
        public List<int> GameSerieID { get; set; }
        public List<string> GameSerieName { get; set; }
        public List<int> TotalPoints { get; set; }
    }
}