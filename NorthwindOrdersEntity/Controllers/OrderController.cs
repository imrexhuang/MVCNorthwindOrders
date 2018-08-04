using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using log4net;
using NorthwindOrdersEntity.Models;
using NorthwindOrdersEntity.ViewModels;
using X.PagedList;

//log檔輸出路徑請在Web.config設定

namespace NorthwindOrdersEntity.Controllers
{
    public class OrderController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(typeof(OrderController));

        private NorthwindEntities db = new NorthwindEntities();

        // 分頁後每頁顯示的筆數
        private const int defaultPageSize = 20;

        public OrderController()
        {
            log.Info("開始執行OrderController");
        }
        
        // GET: Order
        public ActionResult Index(int page = 1)
        {

            int currentPageIndex =  page<1　?　1　: page;
            TempData["CurrentPage"] = currentPageIndex;

            var order = (from om in db.Orders
                        join od in db.Order_Details
                        on om.OrderID equals od.OrderID
                        select new OrderPocViewModel
                        {
                            OrderID = om.OrderID,
                            CustomerID = om.CustomerID,
                            EmployeeID = om.EmployeeID,
                            ShipName = om.ShipName,
                            ShipAddress = om.ShipAddress,
                            ProductID = od.ProductID,
                            UnitPrice = od.UnitPrice,
                            Quantity = od.Quantity,
                        }).OrderBy( x => x.OrderID).ToPagedList(currentPageIndex, defaultPageSize).AsEnumerable();

            return View(order);
        }

        // GET: Order/Details/5
        public ActionResult Details(int? oid , int? pid)
        {
            if (oid == null || pid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = (from om in db.Orders
                         join od in db.Order_Details
                         on om.OrderID equals od.OrderID
                         where om.OrderID == oid && od.ProductID == pid
                         select new OrderPocViewModel
                         {
                             OrderID = om.OrderID,
                             CustomerID = om.CustomerID,
                             EmployeeID = om.EmployeeID,
                             ShipName = om.ShipName,
                             ShipAddress = om.ShipAddress,
                             ProductID = od.ProductID,
                             UnitPrice = od.UnitPrice,
                             Quantity = od.Quantity,
                         }).First();

            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,EmployeeID,ShipName,ShipAddress,ProductID,UnitPrice,Quantity")] OrderPocViewModel orderPocViewModel)
        {
            if (ModelState.IsValid)
            {
                var order = new Orders()
                {
                    OrderID = orderPocViewModel.OrderID,
                    CustomerID = orderPocViewModel.CustomerID,
                    EmployeeID = orderPocViewModel.EmployeeID,
                    ShipName = orderPocViewModel.ShipName,
                    ShipAddress = orderPocViewModel.ShipAddress,
                };


                var orderDetails = new Order_Details()
                {
                    OrderID = orderPocViewModel.OrderID,
                    ProductID = orderPocViewModel.ProductID,
                    UnitPrice = orderPocViewModel.UnitPrice,
                    Quantity = orderPocViewModel.Quantity,
                };


                try
                {
                    db.Orders.Add(order);
                    db.Order_Details.Add(orderDetails);
                    db.SaveChanges();

                    return RedirectToAction("Create");
                }
                catch (Exception ex)
                {
                    log.Error("{0} Exception:", ex);
                    ModelState.AddModelError("DBERROR", "新增訂單發生錯誤！");
                }
                
            }

            return View(orderPocViewModel);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? oid , int? pid)
        {
            if (oid == null || pid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = (from om in db.Orders
                         join od in db.Order_Details
                         on om.OrderID equals od.OrderID
                         where om.OrderID == oid && od.ProductID == pid
                         select new OrderPocViewModel
                         {
                             OrderID = om.OrderID,
                             CustomerID = om.CustomerID,
                             EmployeeID = om.EmployeeID,
                             ShipName = om.ShipName,
                             ShipAddress = om.ShipAddress,
                             ProductID = od.ProductID,
                             UnitPrice = od.UnitPrice,
                             Quantity = od.Quantity,
                         }).First();

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,EmployeeID,ShipName,ShipAddress,ProductID,UnitPrice,Quantity")] OrderPocViewModel orderPocViewModel)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    var order = db.Orders.Find(orderPocViewModel.OrderID);
                    order.CustomerID = orderPocViewModel.CustomerID;
                    order.EmployeeID = orderPocViewModel.EmployeeID;
                    order.ShipName = orderPocViewModel.ShipName;
                    order.ShipAddress = orderPocViewModel.ShipAddress;
                    db.SaveChanges();

                    var orderDetails = db.Order_Details.Find(orderPocViewModel.OrderID , orderPocViewModel.ProductID);
                    orderDetails.ProductID = orderPocViewModel.ProductID;
                    orderDetails.UnitPrice = orderPocViewModel.UnitPrice;
                    orderDetails.Quantity = orderPocViewModel.Quantity;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    log.Error("{0} Exception:", ex);
                    ModelState.AddModelError("DBERROR", "編輯訂單存檔發生錯誤！");
                }

            }
            return View(orderPocViewModel);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPocViewModel orderPocViewModel = db.OrderPocViewModels.Find(id);
            if (orderPocViewModel == null)
            {
                return HttpNotFound();
            }
            return View(orderPocViewModel);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderPocViewModel orderPocViewModel = db.OrderPocViewModels.Find(id);
            db.OrderPocViewModels.Remove(orderPocViewModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
