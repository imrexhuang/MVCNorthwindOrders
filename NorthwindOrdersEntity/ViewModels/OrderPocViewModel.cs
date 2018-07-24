using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace NorthwindOrdersEntity.ViewModels
{
    public class OrderPocViewModel
    {

        [Key]
        [DisplayName("訂單編號")]
        public int OrderID { get; set; }

        [DisplayName("客戶編號")]
        public string CustomerID { get; set; }

        [DisplayName("員工編號")]
        public Nullable<int> EmployeeID { get; set; }

        [DisplayName("收件人姓名")]
        public string ShipName { get; set; }

        [DisplayName("收件住址")]
        public string ShipAddress { get; set; }

        [DisplayName("產品編號")]
        public int ProductID { get; set; }

        [DisplayName("單價")]
        public decimal UnitPrice { get; set; }

        [DisplayName("數量")]
        public short Quantity { get; set; }

        [DisplayName("此訂單配送業者編號")]
        public Nullable<int> ShipVia { get; set; }

    }


}