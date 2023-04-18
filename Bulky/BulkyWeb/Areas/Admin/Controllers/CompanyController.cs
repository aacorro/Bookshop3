
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using BulkyWeb.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            
            return View(objCompanyList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id) 
        {

            if(id == null || id ==0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u=>u.Id==id);
                return View(companyObj);
            }

        }


        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)    
        {
            if (ModelState.IsValid)
            {
                if(CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }
               
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                    return View(CompanyObj);
            }
        }

        #region Edith methods reemplaced by Upsert
        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company? companyFromDb = _unitOfWork.Company.Get(c => c.Id == id);
        //    //different ways:
        //    //Company? companyFromDb1 = _db.Categories.Find(id);
        //    //Company? companyFromDb2 = _db.Categories.Where(c => c.Id == id).FirstOrDefault();

        //    if (companyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(companyFromDb);
        //}


        //[HttpPost]
        //public IActionResult Edit(Company obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Company updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();

        //}

        #endregion

        #region Delete mothods reemplaces by API CALLS
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company? companyFromDb = _unitOfWork.Company.Get(c => c.Id == id);
        //    //different ways:
        //    //Company? companyFromDb1 = _db.Categories.Find(id);
        //    //Company? companyFromDb2 = _db.Categories.Where(c => c.Id == id).FirstOrDefault();

        //    if (companyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(companyFromDb);
        //}


        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Company? obj = _unitOfWork.Company.Get(c => c.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Company.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Company deleted successfully";
        //    return RedirectToAction("Index");

        //}
        #endregion



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyTobeDeleted = _unitOfWork.Company.Get(u=>u.Id == id);
            if(companyTobeDeleted == null)
            { 
                return Json(new { succss = false, message = "Error while deleting" }); 
            }

            _unitOfWork.Company.Remove(companyTobeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        

        #endregion
    }
}
