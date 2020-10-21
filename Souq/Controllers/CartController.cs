using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Souq.DataAccessLayer;
using Souq.Tables;

namespace Souq.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository<Product> _repoProduct;
        private readonly IRepository<Cart> _repoCart;
        private readonly IRepository<CartDetailes> _CartDetailes;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(IRepository<Product> repository, UserManager<IdentityUser> userManager,
                                        SignInManager<IdentityUser> signInManager , IRepository<CartDetailes> CartDetailes , IRepository<Cart> repoCart)
        {
            _repoProduct = repository;
            _userManager = userManager;
            _signInManager = signInManager;
            _CartDetailes = CartDetailes;
            _repoCart = repoCart;

        
        }
        
        public IActionResult Index()
        {
            return View();
        }
       
        public IActionResult AddToUserCart(int id , int count )
        {
            if(_repoCart.Find(c=>c.AppUserId==_userManager.GetUserId(User)).FirstOrDefault()==null)
            {
                var cart = new Cart()
                {
                    AppUserId = _userManager.GetUserId(User)
                };
                _repoCart.Insert(cart);
                var CartDetailes = new CartDetailes()
                {
                    CartId=cart.Id,
                    ProductId=id,
                    productcount=count
                };
                _CartDetailes.Insert(CartDetailes);
            }
            else
            {
                //var cart = _repoCart.Find(c => c.AppUserId == _userManager.GetUserId(User)).FirstOrDefault();
             var product=   _repoCart.GetAll().Include(x => x.CartDetailes).ThenInclude(x=>x.Product).FirstOrDefault(c => c.AppUserId == _userManager.GetUserId(User))?.
                    CartDetailes?.FirstOrDefault(x=>x.ProductId==id);
                if (product != null)
                { 
                   var sum = product.productcount + count;
                    var TotalCountOfProduct = product.Product.quantity;


                    if (sum>TotalCountOfProduct)
                    {
                        var MaximumNumberThatCanBeAdded = TotalCountOfProduct - product.productcount;
                        product.productcount += MaximumNumberThatCanBeAdded;
                    }
                    else
                    {
                        product.productcount += count;
                    }
                    _CartDetailes.Update(product);
                }
                else
                {
                    var CartDetailes = new CartDetailes()
                    {
                        CartId = _repoCart.Find(x => x.AppUserId == _userManager.GetUserId(User)).FirstOrDefault().Id,
                        ProductId = id,
                        productcount = count
                    };
                    _CartDetailes.Insert(CartDetailes);
                }

            }

            return Json("Succes");

        }

        public IActionResult MyCart(int id)
        {
            if(_repoCart.GetById(id)?.AppUserId==_userManager.GetUserId(User))
            {
                // var listOfProduct = _CartDetailes.GetAll().Include(x => x.Product).Where(x => x.CartId == id).Select(x=>new xx {Product= x.Product,productcount= x.productcount});
                var listOfProduct = _CartDetailes.GetAll().Include(x => x.Product).Where(x => x.CartId == id).ToList();

                return View(listOfProduct);
            }
            else
            {
                return NotFound();
            }
            
        }
    //    public class xx
    //    {
    //        public Product Product { get; set; }
    //        public int productcount { get; set; }
    //    }
    }
}
