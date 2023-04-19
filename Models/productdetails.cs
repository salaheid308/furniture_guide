using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class productcolor
    {
        public int id { get; set; }

        public string colorname { get; set; }

        public int categoryid { get; set; }
        public virtual category category { get; set; }

        public int gallryid { get; set; }

        public virtual gallries gallry { get; set; }

        public virtual ICollection<product> products { get; set; }
    }

    public class productcolor2
    {
        public int id { get; set; }

        public string color2name { get; set; }

        public int categoryid { get; set; }
        public virtual category category { get; set; }

        public int gallryid { get; set; }

        public virtual gallries gallry { get; set; }

        public virtual ICollection<product> products { get; set; }
    }

    public class productprice
    {
        public int id { get; set; }

        public int price { get; set; }

        public int categoryid { get; set; }
        public virtual category category { get; set; }

        public int gallryid { get; set; }

        public virtual gallries gallry { get; set; }

        public virtual ICollection<product> products { get; set; }
    }

    

    public class productsize
    {
        public int id { get; set; }

        public string size { get; set; }

        public int categoryid { get; set; }
        public virtual category category { get; set; }

        public int gallryid { get; set; }

        public virtual gallries gallry { get; set; }

        public virtual ICollection<product> products { get; set; }
    }

}