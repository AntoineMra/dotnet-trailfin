using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trailfin.Application.Models.Users
{
    public class UpdateNameRequestDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string Pseudo { get; set; }
    }
}
