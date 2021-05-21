using AWSElasticBeanstalksCICDApp.Business.Abstract;
using AWSElasticBeanstalksCICDApp.Data.Abstract;
using AWSElasticBeanstalksCICDApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSElasticBeanstalksCICDApp.Business.Concrate
{
    public class UserRegisterManager : IUserRegisterService
    {
        private IUserRegisterRepository _userRegisterRepository;
        public UserRegisterManager(IUserRegisterRepository userRegisterRepository)
        {
            _userRegisterRepository = userRegisterRepository;
        }
            
        public void Create(UserRegister Entity)
        {
            _userRegisterRepository.Create(Entity);
        }

        public void Delete(UserRegister Entity)
        {

            _userRegisterRepository.Delete(Entity);
        }

        public List<UserRegister> GetAll()
        {
            return _userRegisterRepository.GetAll();
        }

        public UserRegister GetById(int id)
        {
            return _userRegisterRepository.GetById(id);
        }

        public void Update(UserRegister Entity)
        {
            _userRegisterRepository.Update(Entity);
        }
    }
}
