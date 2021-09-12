﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Extensions;
using WebApplication1.Models;
using WebApplication1.Models.ViewModel;

namespace WebApplication1.Controllers
{
    public class PhongBanController : Controller
    {
        private QLNhanSuEntities db = new QLNhanSuEntities();

        // GET: PhongBan

        public ActionResult Index(int? page, string loaiTimKiem, string tenTimKiem)
        {

            int pageNumber = (page ?? 1);
            int pageSize = 10;
            IQueryable<PhongBan> phongBans;
            List<PhongBanViewModel> phongBanViewModels;
            try
            {
                if (loaiTimKiem == "MaPhongBan")
                {
                    if (tenTimKiem == null || tenTimKiem == "")
                    {
                        this.AddNotification("Vui lòng nhập từ khóa để tìm kiếm theo mã phòng ban!", NotificationType.WARNING);
                    }
                    phongBans = db.PhongBans.Where(x => x.MaPB.ToString().Contains(tenTimKiem.ToString()) && x.MaPB != 12).OrderBy(x => x.TenPB);
                    phongBanViewModels = phongBans.ToList().ConvertAll<PhongBanViewModel>(x => x);
                    return View("Index", phongBanViewModels.ToPagedList(pageNumber, pageSize));
                }
                else if (loaiTimKiem == "TenPhongBan")
                {
                    if (tenTimKiem == null || tenTimKiem == "")
                    {
                        this.AddNotification("Vui lòng nhập từ khóa để tìm kiếm theo tên phòng ban!", NotificationType.WARNING);
                    }
                    phongBans = db.PhongBans.Where(x => x.TenPB.Contains(tenTimKiem.ToString()) && x.MaPB != 12).OrderBy(x => x.TenPB);
                    phongBanViewModels = phongBans.ToList().ConvertAll<PhongBanViewModel>(x => x);
                    return View("Index", phongBanViewModels.ToPagedList(pageNumber, pageSize));
                }
                phongBans = db.PhongBans.Where(x => x.MaPB != 12).OrderBy(x => x.TenPB);
                phongBanViewModels = phongBans.ToList().ConvertAll<PhongBanViewModel>(x => x);
                return View("Index", phongBanViewModels.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                this.AddNotification("Có lỗi xảy ra. Vui lòng thực hiện tìm kiếm lại!", NotificationType.ERROR);
                phongBans = db.PhongBans.Where(x => x.TenPB.Contains("+-*/*-+-*/+") && x.MaPB != 12).OrderBy(x => x.TenPB);
                phongBanViewModels = phongBans.ToList().ConvertAll<PhongBanViewModel>(x => x);
                return View("Index", phongBanViewModels.ToPagedList(pageNumber, pageSize));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(List<PhongBanViewModel> phongBanViewModels)
        {
            try
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                var checkIsChecked = phongBanViewModels.Where(x => x.IsChecked == true);
                if (checkIsChecked == null)
                {
                    this.AddNotification("Vui lòng chọn phòng ban để xóa!", NotificationType.ERROR);
                    return RedirectToAction("Index");
                }
                foreach (var item in phongBanViewModels)
                {
                    if (item.IsChecked == true)
                    {
                        int maPhongBan = item.MaPB;
                        PhongBan phongBan = db.PhongBans.Where(x => x.MaPB == maPhongBan).SingleOrDefault();
                        if (phongBan != null)
                        {
                            db.PhongBans.Remove(phongBan);
                            db.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                this.AddNotification("Không thể xóa vì phòng ban này đã và đang được sử dụng!", NotificationType.ERROR);
                return RedirectToAction("Index");
            }
        }
        // GET: PhongBan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongBan phongBan = db.PhongBans.Find(id);
            if (phongBan == null)
            {
                return HttpNotFound();
            }
            PhongBanViewModel phongBanViewModel = phongBan;
            return View(phongBanViewModel);
        }

        // GET: PhongBan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhongBan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhongBanViewModel phongBanViewModel)
        {
            PhongBan phongBan;
            if (ModelState.IsValid)
            {
                phongBan = phongBanViewModel;
                db.PhongBans.Add(phongBan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phongBanViewModel);
        }

        // GET: PhongBan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongBan phongBan = db.PhongBans.Find(id);
            if (phongBan == null)
            {
                return HttpNotFound();
            }
            PhongBanViewModel phongBanViewModel = phongBan;
            return View(phongBanViewModel);
        }

        // POST: PhongBan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhongBanViewModel phongBanViewModel)
        {
            PhongBan phongBan;
            if (ModelState.IsValid)
            {
                phongBan = phongBanViewModel;
                db.Entry(phongBan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phongBanViewModel);
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
