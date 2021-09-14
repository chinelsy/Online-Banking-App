using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineBanking.Domain.Entities;
using OnlineBanking.Domain.Interfaces.Repositories;
using OnlineBanking.Domain.UnitOfWork;
using WebUI.domain.Interfaces.Services;
using WebUI.domain.Models;

namespace WebUI.domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepo;

        public CustomerService(IUnitOfWork unitOfWork, ICustomerRepository customerRepo)
        {
            _unitOfWork = unitOfWork;
            _customerRepo = customerRepo;
        }

        public void Add(CustomerViewModel model)
        {
            var customer = new Customer
            {
               
                Birthday = model.Birthday,
                Gender = model.Gender,

                Account = new Account
                {
                    AccountType = model.AccountType
                }
            };

            _customerRepo.Add(customer);
            _unitOfWork.Commit();

        }
    }
}
