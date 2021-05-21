using AWSElasticBeanstalksCICDApp.Business.Abstract;
using AWSElasticBeanstalksCICDApp.Controllers;
using AWSElasticBeanstalksCICDApp.Data.Abstract;
using AWSElasticBeanstalksCICDApp.Data.EfCore;
using AWSElasticBeanstalksCICDApp.Entity;
using AWSElasticBeanstalksCICDApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AWSElasticBeanstalksCICDApp.Tests
{
    public class Testing
    {
        private readonly Mock<IUserRegisterService> _mockRepo;        
        private readonly HomeController _homeController;
        private List<UserRegister> userRegisters;
        private List<UserRegisterModel> userModelRegisters;     
        protected AWSCICDAppDbContext _AWSCICDAppDbContext { get;private set; }

      

        public Testing()
        {
            _mockRepo = new Mock<IUserRegisterService>();
            _homeController = new HomeController(_mockRepo.Object);
            userRegisters = new List<UserRegister>()
            {
                new UserRegister{UserId=1,FirstName="Ahmet",LastName="Karadað",UserName="ahmetikrdg",EMail="ahmetikrdg@outlook.com",Password="123"},
                new UserRegister{UserId=1,FirstName="Buðra",LastName="Çakýcý",UserName="bcakici",EMail="bugracakici@outlook.com",Password="321"},
                new UserRegister{UserId=1,FirstName="Veli",LastName="Vel",UserName="velvel",EMail="velele@outlook.com",Password="456"}
            };

            userModelRegisters = new List<UserRegisterModel>()
            {
                new UserRegisterModel{UserId=1,FirstName="Ahmet",LastName="Karadað",UserName="ahmetikrdg",EMail="ahmetikrdg@outlook.com",Password="123"},
                new UserRegisterModel{UserId=1,FirstName="Buðra",LastName="Çakýcý",UserName="bcakici",EMail="bugracakici@outlook.com",Password="321"}
            };
        }


        [Fact]
        public void Index_ActionExecutes_ReturnView()
        {
            var result = _homeController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ActionExecutes_ReturnView()
        {
            var result = _homeController.Privacy();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void UserView_ActionExecutes_ReturnUserView()
        {
            var result = _homeController.UserView();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void UserView_ActionExecutes_ReturnUserList()
        {
            _mockRepo.Setup(repo => repo.GetAll()).Returns(userRegisters);

            var result = _homeController.UserView();
            var viewResult = Assert.IsType<ViewResult>(result);
            var userList = Assert.IsAssignableFrom<UserRegislerListViewModel>(viewResult.Model);
            Assert.Equal<int>(3, userRegisters.Count);
        }

        [Fact]
        public void UserCreate_ActionExecutes_ReturnView()
        {
            var result = _homeController.UserCreate();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void UserCreatePost_FullyModel_ReturnRedirectToAction()
        {
            UserRegister newUserRegister = null;
                
            _mockRepo.Setup(x => x.Create(It.IsAny<UserRegister>())).Callback<UserRegister>(x => newUserRegister = x);

            var result = _homeController.UserCreate(userModelRegisters.First());
            var redirect= Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UserView", redirect.ActionName);

            _mockRepo.Verify(x => x.Create(It.IsAny<UserRegister>()), Times.Once);
        }       

        [Fact]
        public void UserEdit_IdIsNull_ReturnRedirectToIndexAction()
        {
            var result = _homeController.UserEditv(null);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UserView", redirect.ActionName);      
        }

        [Theory]
        [InlineData(2)]
        public void UserEdit_IdInValid_ReturnNotFound(int userId)
        {
            UserRegister userRegister = null;
            _mockRepo.Setup(x => x.GetById(userId)).Returns(userRegister);

            var result = _homeController.UserEditv(userId);
            var redirect = Assert.IsType<NotFoundResult>(result);
            Assert.Equal<int>(404, redirect.StatusCode);
        }

        [Fact]
        public void UserEdit_ActionExecutes_ReturnUserRegister()
        {
            var userRegister = userRegisters.First(x => x.UserId == 1);
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(userRegister);

            var result = _homeController.UserEditv(1);
            var viewResut = Assert.IsType<ViewResult>(result);
            var resultUserRegister = Assert.IsAssignableFrom<UserRegisterModel>(viewResut.Model);
            Assert.Equal(userRegister.UserId, resultUserRegister.UserId);
        }

        [Theory]
        [InlineData(1)]
        public void UserEditPost_FullyColumn_ReturnRediretToAction(int userId)
        {
            var user = userRegisters.First();
            _mockRepo.Setup(x => x.GetById(userId)).Returns(user);
            _mockRepo.Setup(x => x.Update(user));
                
            var userModel = userModelRegisters.First();
            var result = _homeController.UserEdit(userModel);

            var redirect=Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UserView", redirect.ActionName);
        }

        [Fact]
        public void UserDelete_IdIsNull_ReturnNotFound()
        {
            UserRegister userRegister = null;
            
            var result = _homeController.UserDelete(0);
            _mockRepo.Setup(x => x.GetById(0)).Returns(userRegister);

            var redirect = Assert.IsType<NotFoundResult>(result);
            _mockRepo.Verify(x => x.GetById(0), Times.Once);
        }

        [Theory]
        [InlineData(3)]
        public void UserDelete_ActionExecutes_ReturnUser(int userId)
        {
            var user = userRegisters.First();
            _mockRepo.Setup(x => x.GetById(userId)).Returns(user);

            var result = _homeController.UserDelete(userId);
            _mockRepo.Setup(x => x.Delete(user));
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UserView", redirect.ActionName);
        }

        [Fact]      
        public void AWSCICDDb_IntegrationTest_ReturnFullySuccess()
        {
            var user = new UserRegister {FirstName = "Ahmet", LastName = "Karadað", UserName = "ahmetikrdg", EMail = "ahmetikrdg@outlook.com", Password = "123" };

            using (var context = new AWSCICDAppDbContext())
            {
                context.Set<UserRegister>().Add(user);
                context.SaveChanges();
            }       

            using (var context = new AWSCICDAppDbContext())
            {
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }

            using (var context = new AWSCICDAppDbContext())
            {
                context.Set<UserRegister>().ToList();
            }

            using (var context = new AWSCICDAppDbContext())
            {
                context.Set<UserRegister>().Find(1);
            }

            using (var context = new AWSCICDAppDbContext())
            {
                context.Set<UserRegister>().Remove(user);
                context.SaveChanges();
            }
            
        }

    }
}
