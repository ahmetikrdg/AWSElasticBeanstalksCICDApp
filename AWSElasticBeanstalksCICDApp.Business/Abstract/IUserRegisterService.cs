using AWSElasticBeanstalksCICDApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSElasticBeanstalksCICDApp.Business.Abstract
{
    public interface IUserRegisterService
    {
        UserRegister GetById(int id);
        List<UserRegister> GetAll();
        void Create(UserRegister Entity);
        void Update(UserRegister Entity);
        void Delete(UserRegister Entity);
    }
}
