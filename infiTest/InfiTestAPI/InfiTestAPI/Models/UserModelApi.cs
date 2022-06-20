using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiTestAPI.Models
{
    public class UserModelApi
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
        public bool edited { get; set; }
        public bool edited_via_api { get; set; }
    }
}
