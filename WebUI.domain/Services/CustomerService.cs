using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using OnlineBanking.Domain.Entities;
using OnlineBanking.Domain.Enumerators;
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

        public void Add(EnrollCustomerViewModel model, User user, ClaimsViewModel claims)
        {
            var customer = new Customer
            {
                UserId = user.Id,
                Birthday = model.Birthday,
                Gender = model.Gender,
                Account = new Account
                {
                    AccountType = model.AccountType,
                    AccountNumber = RandomNumberGenerator.GetInt32(9998, 9999) * RandomNumberGenerator.GetInt32(99998, 99999),
                    CreatedBy = claims.Username,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Balance = model.AccountType != AccountType.Savings ? 0 : 5000
                }
            };

            _customerRepo.Add(customer);
            _unitOfWork.Commit();

        }
    }
}
