using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTQK7.Models;

namespace TTQK7.Controllers
{
    public class TuNgayDenNgayController : Controller
    {
        // GET: TuNgayDenNgay
        TTQK7Entities db = new TTQK7Entities();
        public ActionResult Left(string TuNgay, string DenNgay, string Donvi)
        {
            TuNgayDenNgay obj = new TuNgayDenNgay();
            try
            {
                obj.TuNgay = DateTime.ParseExact(TuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }catch
            {
                obj.TuNgay = DateTime.MinValue  ;
            }
            try
            {
                obj.DenNgay = DateTime.ParseExact(DenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                obj.DenNgay = DateTime.MaxValue ;
            }            
            
            int m_donvi =-1;
            if (Donvi != "")
            {
                try
                {
                    m_donvi = Convert.ToInt32(Donvi);
                }
                catch
                {
                    m_donvi = -1;
                }
            }
            var dsBaoCao = db.BaoCaoNgay.Where(x => x.NgayBaoCao == null ? false : obj.TuNgay.Year <= x.NgayBaoCao.Value.Year
            && obj.DenNgay.Year >= x.NgayBaoCao.Value.Year && obj.TuNgay.Month <= x.NgayBaoCao.Value.Month
            && obj.DenNgay.Month >= x.NgayBaoCao.Value.Month && obj.TuNgay.Day <= x.NgayBaoCao.Value.Day
            && obj.DenNgay.Day >= x.NgayBaoCao.Value.Day).ToList();
            if (Request.IsAuthenticated)
            {

                string username = User.Identity.GetUserName();
                tblUser user = db.tblUser.Where(x => x.UserName == username).FirstOrDefault();
                if (user.UserName == "admin")
                {

                }
                int idUser = user.idUser;
                int donViID = 0;
                try
                {
                    donViID = user.idDonVi.Value;
                    if (donViID == 23)//là quân khu 7 thì sẽ cho xem báo cáo của admin
                    {
                        dsBaoCao = db.BaoCaoNgay.Where(x => x.NgayBaoCao >= obj.TuNgay && x.NgayBaoCao <= obj.DenNgay && x.DonViID == m_donvi).OrderByDescending(x => x.NgayBaoCao).ToList();
                    }
                    else
                    {
                        //kiểm tra xem có báo cáo ngày chưa?
                        dsBaoCao = db.BaoCaoNgay.Where(x => x.DonViID == user.idDonVi && x.NgayBaoCao >= obj.TuNgay && x.NgayBaoCao <= obj.DenNgay).OrderByDescending(x => x.NgayBaoCao).ToList();
                    }
                }
                catch (Exception)
                {
                    //kiểm tra xem có báo cáo ngày chưa?
                    //lọc báo cáo theo đơn vị
                    if (m_donvi ==-1)
                    {
                        dsBaoCao = db.BaoCaoNgay.Where(x => x.NgayBaoCao >= obj.TuNgay && x.NgayBaoCao <= obj.DenNgay).OrderByDescending(x => x.NgayBaoCao).OrderBy(x => x.DonViID).ToList();

                    }
                    else
                    dsBaoCao = db.BaoCaoNgay.Where(x => x.NgayBaoCao >= obj.TuNgay && x.NgayBaoCao <= obj.DenNgay && x.DonViID==m_donvi).OrderByDescending(x => x.NgayBaoCao).ToList();
                    //Nếu không có thì là admin => hiển thị toàn bộ danh sách báo cáo
                }

            }
            return PartialView(dsBaoCao);
          
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: TuNgayDenNgay/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TuNgayDenNgay/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TuNgayDenNgay/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TuNgayDenNgay/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TuNgayDenNgay/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TuNgayDenNgay/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TuNgayDenNgay/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
