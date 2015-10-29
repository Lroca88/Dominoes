using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dominoes.Models
{
    public class DashboardUserView
    {
        public DashboardUserView()
        {
            this.ID = new List<int>();
            this.Name = new List<string>();
            this.TotalPoints = new List<int>();
        
        }
        public int UserProfileInfoID { get; set; }
        public string UserName { get; set; }
        public List<int> ID { get; set; }
        public List<string> Name { get; set; }
        public List<int> TotalPoints { get; set; }
    }
}