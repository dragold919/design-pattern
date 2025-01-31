﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModel;
using PagedList;
using PagedList.Mvc;
using WebApplication1.Extensions;
using System.Net;
using Rotativa;
namespace WebApplication1.Controllers
{
    public class ThongKeLuongController : Controller
    {
        QLNhanSuEntities db = new QLNhanSuEntities();
        // GET: ThongKeLuong

        public void TruyenPara(int? month, int? year, string MaPB)
        {
            ViewBag.month = month;
            ViewBag.year = year;
            ViewBag.PB = MaPB;
        }
        public ActionResult TheoPhongBan(int? page, int? month, int? year, string MaPB)
        {
            TempData["page"] = page;
            TempData["month"] = month;
            TempData["year"] = year;
            TempData["MaPB"] = MaPB;
            ViewBag.MaPB = new SelectList(db.PhongBans.OrderByDescending(x => x.TenPB), "MaPB", "TenPB", "12");
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            IQueryable<LuongThang> luongThangs;
            List<LuongThangViewModel> luongThangViewModels;
            string tenPhongBan = "";
            try
            {
                if (MaPB == "12")
                {

                    if (year != null && month != null)
                    {
                        TruyenPara(month, year, MaPB);
                        this.AddNotification("Kết quả tìm kiếm theo tháng: " + month + ", năm: " + year + ", phòng ban: Tất cả.", NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.ThangNam.Month == month).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else if (year != null && month == null)
                    {
                        TruyenPara(month, year, MaPB);
                        this.AddNotification("Kết quả tìm kiếm theo năm: " + year + ", phòng ban: Tất cả.", NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else if (year == null && month != null)
                    {
                        TruyenPara(month, year, MaPB);
                        this.AddNotification("Kết quả tìm kiếm theo tháng: " + month + ", phòng ban: Tất cả.", NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else
                    {
                        TruyenPara(month, year, MaPB);
                        this.AddNotification("Kết quả tìm kiếm theo phòng ban: Tất cả.", NotificationType.INFO);
                        luongThangs = db.LuongThangs.OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels.ToPagedList(pageNumber, pageSize));
                }
                else if (MaPB != null && MaPB != "")
                {
                    tenPhongBan = db.PhongBans.Where(x => x.MaPB.ToString() == MaPB).Select(x => x.TenPB).Single();
                    if (year != null && month != null)
                    {
                        TruyenPara(month, year, MaPB);
                        this.AddNotification("Kết quả tìm kiếm theo tháng: " + month + ", năm: " + year + ", phòng ban: " + tenPhongBan + ".", NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.ThangNam.Month == month && x.LuongCoBan.NhanVien.PhongBan.MaPB.ToString().Equals(MaPB.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else if (year != null && month == null)
                    {
                        TruyenPara(month, year, MaPB);
                        this.AddNotification("Kết quả tìm kiếm theo năm: " + year + ", phòng ban: " + tenPhongBan + ".", NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.LuongCoBan.NhanVien.PhongBan.MaPB.ToString().Equals(MaPB.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else if (year == null && month != null)
                    {
                        TruyenPara(month, year, MaPB);
                        this.AddNotification("Kết quả tìm kiếm theo tháng: " + month + ", phòng ban: " + tenPhongBan + ".", NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month && x.LuongCoBan.NhanVien.PhongBan.MaPB.ToString().Equals(MaPB.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else
                    {
                        TruyenPara(month, year, MaPB);
                        this.AddNotification("Kết quả tìm kiếm theo phòng ban: " + tenPhongBan + ".", NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.LuongCoBan.NhanVien.PhongBan.MaPB.ToString().Equals(MaPB.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels.ToPagedList(pageNumber, pageSize));
                }
                TruyenPara(month, year, MaPB);

                luongThangs = db.LuongThangs.OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                return View(luongThangViewModels.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                this.AddNotification("Có lỗi xảy ra. Vui lòng thực hiện lại!", NotificationType.ERROR);
                luongThangs = db.LuongThangs.OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                return View(luongThangViewModels.ToPagedList(pageNumber, pageSize));
            }
        }


        public void TruyenPara2(int? month, int? year, string loaiTimKiem, string tenTimKiem)
        {
            ViewBag.month = month;
            ViewBag.year = year;
            ViewBag.loaiTimKiem = loaiTimKiem;
            ViewBag.tenTimKiem = tenTimKiem;
        }

        [HttpGet]
        public ActionResult TheoNhanVien(int? page, int? month, int? year, string loaiTimKiem, string tenTimKiem)
        {
            TempData["page"] = page;
            TempData["month"] = month;
            TempData["year"] = year;
            TempData["loaiTimKiem"] = loaiTimKiem;
            TempData["tenTimKiem"] = tenTimKiem;
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            IQueryable<LuongThang> luongThangs;
            List<LuongThangViewModel> luongThangViewModels;
            try
            {
                if (loaiTimKiem == "MaNhanVien")
                {
                    if (year != null && month != null)
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        this.AddNotification("Kết quả tìm kiếm theo tháng: " + month + ", năm: " + year + ", loại tìm kiếm: Mã nhân viên, từ khóa tìm kiếm: " + tenTimKiem, NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.ThangNam.Month == month && x.LuongCoBan.MaNhanVien.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else if (year != null && month == null)
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        this.AddNotification("Kết quả tìm kiếm theo năm: " + year + ", loại tìm kiếm: Mã nhân viên, từ khóa tìm kiếm: " + tenTimKiem, NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.LuongCoBan.MaNhanVien.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else if (year == null && month != null)
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        this.AddNotification("Kết quả tìm kiếm theo tháng: " + month + ", loại tìm kiếm: Mã nhân viên, từ khóa tìm kiếm: " + tenTimKiem, NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month && x.LuongCoBan.MaNhanVien.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        this.AddNotification("Kết quả tìm kiếm theo loại tìm kiếm: Mã nhân viên, từ khóa tìm kiếm: " + tenTimKiem, NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.LuongCoBan.MaNhanVien.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels.ToPagedList(pageNumber, pageSize));

                }
                else if (loaiTimKiem == "TenNhanVien")
                {
                    if (year != null && month != null)
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        this.AddNotification("Kết quả tìm kiếm theo tháng: " + month + ", năm: " + year + ", loại tìm kiếm: Tên nhân viên, từ khóa tìm kiếm: " + tenTimKiem, NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.ThangNam.Month == month && x.LuongCoBan.NhanVien.HoTen.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else if (year != null && month == null)
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        this.AddNotification("Kết quả tìm kiếm theo năm: " + year + ", loại tìm kiếm: Tên nhân viên, từ khóa tìm kiếm: " + tenTimKiem, NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.LuongCoBan.NhanVien.HoTen.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else if (year == null && month != null)
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        this.AddNotification("Kết quả tìm kiếm theo tháng: " + month + ", loại tìm kiếm: Tên nhân viên, từ khóa tìm kiếm: " + tenTimKiem, NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month && x.LuongCoBan.NhanVien.HoTen.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        this.AddNotification("Kết quả tìm kiếm theo loại tìm kiếm: Tên nhân viên, từ khóa tìm kiếm: " + tenTimKiem, NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.LuongCoBan.NhanVien.HoTen.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    if (year != null && month != null)
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        this.AddNotification("Kết quả tìm kiếm theo tháng: " + month + ", năm: " + year, NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.ThangNam.Month == month).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else if (year != null && month == null)
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        this.AddNotification("Kết quả tìm kiếm theo năm: " + year, NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else if (year == null && month != null)
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        this.AddNotification("Kết quả tìm kiếm theo tháng: " + month, NotificationType.INFO);
                        luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month).OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    else
                    {
                        TruyenPara2(month, year, loaiTimKiem, tenTimKiem);
                        luongThangs = db.LuongThangs.OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                    }
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels.ToPagedList(pageNumber, pageSize));
                }
            }
            catch
            {
                this.AddNotification("Không tìm thấy từ khóa yêu cầu. Vui lòng thực hiện tìm kiếm lại!", NotificationType.ERROR);
                luongThangs = db.LuongThangs.OrderBy(x => x.LuongCoBan.NhanVien.HoTen).ThenByDescending(x => x.ThangNam);
                luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                return View(luongThangViewModels.ToPagedList(pageNumber, pageSize));
            }

        }

        public ActionResult Details(int? id)
        {
            TempData.Keep();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LuongThang luongThang = db.LuongThangs.Find(id);
            if (luongThang == null)
            {
                return HttpNotFound();
            }
            TempData["MaNV"] = luongThang.LuongCoBan.MaNhanVien;
            LuongThangViewModel luongThangViewModel = luongThang;
            return View(luongThangViewModel);
        }

        public ActionResult Details_DSLuongTheoNhanVien(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LuongThang luongThang = db.LuongThangs.Find(id);
            if (luongThang == null)
            {
                return HttpNotFound();
            }
            TempData["MaNV"] = luongThang.LuongCoBan.MaNhanVien;
            LuongThangViewModel luongThangViewModel = luongThang;
            return View(luongThangViewModel);
        }

        #region in ra chi tiết lương tháng của nhân viên
        public ActionResult Print(int? id, string maNV, int month, int year)
        {
            return new ActionAsPdf("InBangLuongThang", new { id = id }) { FileName = "Luong_MaNV_" + maNV + "_Thang_" + month + "_Nam_" + year + ".pdf" };
        }
        public ActionResult InBangLuongThang(int? id)
        {
            LuongThang luongThang = db.LuongThangs.Find(id);
            LuongThangViewModel luongThangViewModel = luongThang;
            return View(luongThangViewModel);
        }
        #endregion in ra chi tiết lương tháng của nhân viên>>>>>>>>>>>>>>>>>

        #region in ra danh sách lương tháng của nhiều nhân viên (theo phòng ban)
        public ActionResult PrintLuongTheoPhongBan(int? month, int? year, int? MaPB)
        {
            if (MaPB == 12)
            {
                return new ActionAsPdf("DSLuongTheoPhongBan", new { month, year, MaPB }) { FileName = "LuongTheoPhongBan_Thang_" + month + "_Nam_" + year + "_PhongBan_TatCa.pdf" };
            }
            else
            {
                var phongBan = db.PhongBans.Where(x => x.MaPB == MaPB).SingleOrDefault();
                string tenPB = "";
                if (phongBan != null)
                    tenPB = phongBan.TenPB.ToString();
                return new ActionAsPdf("DSLuongTheoPhongBan", new { month, year, MaPB }) { FileName = "LuongTheoPhongBan_Thang_" + month + "_Nam_" + year + "_PhongBan_" + tenPB + ".pdf" };
            }
        }

        public void TruyenViewBagPhongBan(int? month, int? year, int? MaPB)
        {
            ViewBag.month = month;
            ViewBag.year1 = year;
            if (MaPB == 12)
            {
                ViewBag.TenPB = "Tất cả";
            }
            else
            {
                var phongBan = db.PhongBans.Where(x => x.MaPB == MaPB).SingleOrDefault();
                if (phongBan != null)
                    ViewBag.TenPB = phongBan.TenPB.ToString();
            }
        }
        public ActionResult DSLuongTheoPhongBan(int? month, int? year, int? MaPB)
        {
            IQueryable<LuongThang> luongThangs;
            List<LuongThangViewModel> luongThangViewModels;
            if (MaPB == 12)
            {
                if (year != null && month != null)
                {
                    TruyenViewBagPhongBan(month, year, MaPB);


                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month && x.ThangNam.Year == year).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else if (year != null && month == null)
                {
                    TruyenViewBagPhongBan(month, year, MaPB);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else if (year == null && month != null)
                {
                    TruyenViewBagPhongBan(month, year, MaPB);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                luongThangs = db.LuongThangs.OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                return View(luongThangViewModels);

            }
            else if (MaPB != 12)
            {
                if (year != null && month != null)
                {
                    TruyenViewBagPhongBan(month, year, MaPB);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month && x.ThangNam.Year == year && x.LuongCoBan.NhanVien.PhongBan.MaPB.ToString().Equals(MaPB.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else if (year != null && month == null)
                {
                    TruyenViewBagPhongBan(month, year, MaPB);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.LuongCoBan.NhanVien.PhongBan.MaPB.ToString().Equals(MaPB.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else if (year == null && month != null)
                {
                    TruyenViewBagPhongBan(month, year, MaPB);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month && x.LuongCoBan.NhanVien.PhongBan.MaPB.ToString().Equals(MaPB.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                luongThangs = db.LuongThangs.Where(x => x.LuongCoBan.NhanVien.PhongBan.MaPB.ToString().Equals(MaPB.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                return View(luongThangViewModels);
            }
            luongThangs = db.LuongThangs.OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
            luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
            return View(luongThangViewModels);
        }
        #endregion in ra danh sách lương tháng của nhiều nhân viên (theo phòng ban)


        #region in ra danh sách lương tháng của nhiều nhân viên (theo nhân viên)
        public void TruyenViewBagNhanVien(int? month, int? year, string loaiTimKiem, string tenTimKiem)
        {
            ViewBag.month = month;
            ViewBag.year = year;
            ViewBag.loaiTimKiem = loaiTimKiem;
            ViewBag.tenTimKiem = tenTimKiem;

        }
        public ActionResult PrintLuongTheoNhanVien(int? month, int? year, string loaiTimKiem, string tenTimKiem)
        {
            return new ActionAsPdf("DSLuongTheoNhanVien", new { month, year, loaiTimKiem, tenTimKiem }) { FileName = "LuongTheoNhanVien_Thang_" + month + "_Nam_" + year + "_LoaiTimKiem_" + loaiTimKiem + "_TenTimKiem_" + tenTimKiem + ".pdf" };
        }

        public ActionResult DSLuongTheoNhanVien(int? month, int? year, string loaiTimKiem, string tenTimKiem)
        {
            IQueryable<LuongThang> luongThangs;
            List<LuongThangViewModel> luongThangViewModels;
            if (loaiTimKiem == "MaNhanVien")
            {
                if (year != null && month != null)
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.ThangNam.Month == month && x.LuongCoBan.MaNhanVien.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else if (year != null && month == null)
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.LuongCoBan.MaNhanVien.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else if (year == null && month != null)
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month && x.LuongCoBan.MaNhanVien.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.Where(x => x.LuongCoBan.MaNhanVien.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
            }
            else if (loaiTimKiem == "TenNhanVien")
            {
                if (year != null && month != null)
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.ThangNam.Month == month && x.LuongCoBan.NhanVien.HoTen.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else if (year != null && month == null)
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.LuongCoBan.NhanVien.HoTen.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else if (year == null && month != null)
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month && x.LuongCoBan.NhanVien.HoTen.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.Where(x => x.LuongCoBan.NhanVien.HoTen.ToString().Contains(tenTimKiem.ToString())).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
            }
            else
            {
                if (year != null && month != null)
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year && x.ThangNam.Month == month).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else if (year != null && month == null)
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Year == year).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else if (year == null && month != null)
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.Where(x => x.ThangNam.Month == month).OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
                else
                {
                    TruyenViewBagNhanVien(month, year, loaiTimKiem, tenTimKiem);

                    luongThangs = db.LuongThangs.OrderBy(x => x.LuongCoBan.NhanVien.HoTen);
                    luongThangViewModels = luongThangs.ToList().ConvertAll<LuongThangViewModel>(x => x);
                    return View(luongThangViewModels);
                }
            }
        }
        #endregion in ra danh sách lương tháng của nhiều nhân viên (theo nhân viên)
    }
}