using OnlineBanking.Domain.Entities;
using WebUI.domain.Models;

namespace WebUI.domain.Interfaces.Services
{
   public interface ICustomerService
   {
       void Add( EnrollCustomerViewModel model, User user, ClaimsViewModel claims);
   }
}
