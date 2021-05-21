using AWSElasticBeanstalksCICDApp.Data.Abstract;
using AWSElasticBeanstalksCICDApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSElasticBeanstalksCICDApp.Data.EfCore
{
    public class UserRegisterRepository:GenericRepository<UserRegister,AWSCICDAppDbContext>,IUserRegisterRepository
    {
    }
}
