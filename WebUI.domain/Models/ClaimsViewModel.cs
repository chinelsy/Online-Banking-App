using System.Collections.Generic;

namespace WebUI.domain.Models
{
    public class ClaimsViewModel
    {
        public string Username { get; set; }

        public string UserId { get; set; }
        public IEnumerable<string> Roles { get; set; }


    }
}