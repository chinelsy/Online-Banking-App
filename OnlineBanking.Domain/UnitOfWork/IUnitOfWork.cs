﻿using System;
using System.Collections.Generic;
using System.Text;
using OnlineBanking.Domain.Interfaces.Repositories;
using OnlineBanking.Domain.Entities;
using System.Threading.Tasks;

namespace OnlineBanking.Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Account> Accounts { get; }
        IRepository<User> Users { get; }
        IRepository<Customer> Customers { get; }
        Task<int> CommitAsync();
        int Commit();
    }
}
