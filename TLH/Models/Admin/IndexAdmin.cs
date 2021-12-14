using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TLH.Models.Admin
{
    public class IndexAdmin
    {
        public Guid Id { get; set; }
    }

    public class ACCreateViewModel
    { 
        public bool IsPrizeActive { get; set; }

        public int NumberOfDays { get; set; }

        public int NumberOfMatchesPerDay { get; set; }

        public string Name { get; set; }

        public int TeamCount { get; set; }
    }
}
