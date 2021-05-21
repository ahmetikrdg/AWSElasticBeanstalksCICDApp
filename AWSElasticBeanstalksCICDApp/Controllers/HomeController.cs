using AWSElasticBeanstalksCICDApp.Business.Abstract;
using AWSElasticBeanstalksCICDApp.Entity;
using AWSElasticBeanstalksCICDApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AWSElasticBeanstalksCICDApp.Controllers
{
    public class HomeController : Controller
    {
        private IUserRegisterService _userRegisterService;
        public HomeController(IUserRegisterService userRegisterService)
        {
            _userRegisterService = userRegisterService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UserView()     
        {
            UserRegislerListViewModel userRegislerListViewModel = new UserRegislerListViewModel()
            {
                userRegisters = _userRegisterService.GetAll()
            };
            return View(userRegislerListViewModel);
        }
        [HttpGet]
        public IActionResult UserCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserCreate(UserRegisterModel userRegisterModel)
        {
            var entity = new UserRegister()
            {
                FirstName = userRegisterModel.FirstName,
                LastName = userRegisterModel.LastName,
                EMail = userRegisterModel.EMail,
                UserName=userRegisterModel.UserName,
                Password = userRegisterModel.Password
            };

            _userRegisterService.Create(entity);

            return RedirectToAction("UserView");
        }

        [HttpGet]
        public IActionResult UserEditv(int? id)
        {
            if (id==null)
            {
                return RedirectToAction("UserView");
            }
            var entity = _userRegisterService.GetById((int)id);

            if (entity==null)
            {
                return NotFound();  
            }

            var model = new UserRegisterModel()
            {
                UserId=entity.UserId,           
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                EMail = entity.EMail,
                UserName = entity.UserName,
                Password = entity.Password
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UserEdit(UserRegisterModel userRegisterModel)
        {
            var entity = _userRegisterService.GetById(userRegisterModel.UserId);

            entity.FirstName = userRegisterModel.FirstName;
            entity.LastName = userRegisterModel.LastName;
            entity.EMail = userRegisterModel.EMail;
            entity.UserName = userRegisterModel.UserName;
            entity.Password = userRegisterModel.Password;

            _userRegisterService.Update(entity);
            return RedirectToAction("UserView");
        }

        [HttpPost]
        public IActionResult UserDelete(int? id)
        {
            var entity = _userRegisterService.GetById((int)id);

            if (entity==null)
            {
                return NotFound();
            }
            _userRegisterService.Delete(entity);

            return RedirectToAction("UserView");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
