using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Souq.DataAccessLayer;
using Souq.Tables;
using Souq.ViewsModels;

namespace Souq.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _repoProduct;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        IWebHostEnvironment en;
        private readonly IMapper mapper;

        public ProductController(IRepository<Product> repository, UserManager<IdentityUser> userManager,
                                        SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment _en , IMapper mapper)
        {
            _repoProduct = repository;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            en = _en;
            this.mapper = mapper;
        }
        public static string GenaricPath()
        {
            Random rnd = new Random();
            var x = "Souq"+ DateTime.Now.Year.ToString() +
                DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
            return x;
        }
        [Authorize(Roles = "SellerRole")]
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult createproduct(ProductViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    string path = en.WebRootPath + "/assets/";
                    string fileName = GenaricPath();
                    string fileExtention = System.IO.Path.GetExtension(model.productImg.FileName);
                    using (var stream = new System.IO.FileStream(path + fileName + fileExtention, System.IO.FileMode.CreateNew))
                    {
                        if(fileExtention==".png" || fileExtention == ".jpg" || fileExtention == ".jpeg" || fileExtention == ".PNG" || fileExtention == ".JPG" || fileExtention == ".JPEG"  )
                        {
                            model.productImg.CopyTo(stream);
                            stream.Dispose();
                        }
                        else
                        {
                            ViewBag.imgError = " تاكد من امتداد الصورة ";
                            return View("addproduct",model);
                        }
                       
                    }
                    var product = new Product()
                    {
                        productName = model.productName,
                        productBrand = model.productBrand,
                        productPrice = model.productPrice,
                        ProductDiskUsage = model.ProductDiskUsage,
                        ProductRamUsage = model.ProductRamUsage,
                        quantity = model.quantity,
                        Description=model.Description,
                        category = (Product.Category)model.category,
                        productImgUrl = path + fileName + fileExtention,
                        AppUserId = _userManager.GetUserId(User)
                    };
                    _repoProduct.Insert(product);

                    return RedirectToAction("index", "home");
                }

                return View("addproduct", model);
            }
            catch (Exception e)
            {

                return Content(e.Message);
            }
          
        }


        [HttpGet]
        public IActionResult products()
        {
            var AllProducts= _repoProduct.GetAll();
            return View(AllProducts);
        }

        [Authorize(Roles = "SellerRole")]
        [HttpGet]
        public IActionResult MyProducts()
        {
            var UserId = _userManager.GetUserId(User);
            var MyProducts = _repoProduct.Find(P => P.AppUserId == UserId);
            return View(MyProducts);
        }

        public IActionResult DeleteProduct(int id)
        {
            var product = _repoProduct.GetById(id);
            _repoProduct.Delete(product);
            return RedirectToAction("MyProducts", "product");
        }

        public IActionResult EditeProduct(int id)
        {
            var product = _repoProduct.GetById(id);
            var UserId =_userManager.GetUserId(User);
            var productUserId = product.AppUserId;
            if(UserId==productUserId)
            {
                ProductViewModel mdl = mapper.Map<ProductViewModel>(product);
                return View(mdl);
            }
            else
            {
                return RedirectToAction("index", "home");
            }
         
        }

      [HttpPost]
      public IActionResult editeproduct(ProductViewModel model)
      {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = _repoProduct.GetById(model.Id);


                    product.productName = model.productName;
                    product.productBrand = model.productBrand;
                    product.productPrice = model.productPrice;
                    product.ProductDiskUsage = model.ProductDiskUsage;
                    product.ProductRamUsage = model.ProductRamUsage;
                    product.quantity = model.quantity;
                    product.Description = model.Description;
                    product.category = (Product.Category)model.category;                    
                    product.AppUserId  = _userManager.GetUserId(User);
                    if (model.productImg != null)
                    {
                        string path = en.WebRootPath + "/assets/";
                        string fileName = GenaricPath();
                        string fileExtention = System.IO.Path.GetExtension(model.productImg.FileName);
                        using (var stream = new System.IO.FileStream(path + fileName + fileExtention, System.IO.FileMode.CreateNew))
                        {

                            if (fileExtention == ".png" || fileExtention == ".jpg" || fileExtention == ".jpeg" || fileExtention == ".JPEG" || fileExtention == ".JPG" || fileExtention == ".PNG")
                            {
                                model.productImg.CopyTo(stream);
                                stream.Dispose();
                            }
                            else
                            {
                                ViewBag.imgError = " تاكد من امتداد الصورة ";
                                return View("addproduct", model);
                            }

                            product.productImgUrl = path + fileName + fileExtention;

                        }
                    }
                
                    _repoProduct.Update(product);

                    return RedirectToAction("index", "home");
                }

                return View("editeproduct", model);
            }
            catch (Exception e)
            {

                return Content(e.Message);
            }

        }

      [HttpGet]
      public IActionResult Product(int id)
      {
            var product = _repoProduct.GetById(id);
            return View(product);
      }


    }
}
