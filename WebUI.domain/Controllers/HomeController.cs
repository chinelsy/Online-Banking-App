using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineBanking.Domain.Entities;
using OnlineBanking.Domain.Repositories;
using OnlineBanking.Domain.UnitOfWork;
using WebUI.domain.Interfaces.Services;
using WebUI.domain.Models;

namespace WebUI.domain.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService _customerService;

        public HomeController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        //private readonly IUnitOfWork unitOfWork;

        //public HomeController(IUnitOfWork _unitOfWork)
        //{
        //    unitOfWork = _unitOfWork;
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        
        public IActionResult Register(CustomerViewModel customerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
               
                _customerService.Add(customerModel);
                return RedirectToAction("Index");
                               
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }



    }
}