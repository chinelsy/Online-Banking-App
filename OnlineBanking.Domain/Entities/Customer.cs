﻿using System;
using System.Collections.Generic;
using System.Text;
using OnlineBanking.Domain.Interfaces;

namespace OnlineBanking.Domain.Entities
{
  public  class Customer : IEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}
