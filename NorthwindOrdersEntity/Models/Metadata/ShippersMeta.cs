using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NorthwindOrdersEntity.Models.Metadata
{
    [MetadataType(typeof(Shippers))]
    public partial class Shippers
    {
        public class ShippersMeta
        {
            [DisplayName("貨運業者編號")]
            public int ShipperID { get; set; }

            [DisplayName("公司名稱")]
            public string CompanyName { get; set; }

            [DisplayName("電話")]
            public string Phone { get; set; }
        }
    }

}