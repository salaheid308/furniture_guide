using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class driverprice
    {
        public int id { get; set; }

        public int cityid { get; set; }
        public virtual city city { get; set; }

        
        public int driverid { get; set; }
        public virtual drivers driver { get; set; }

        [Required]
        [DataType(DataType.Currency )]
        public int price { get; set; }
    }
}