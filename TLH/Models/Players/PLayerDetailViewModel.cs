using System;
using System.Collections.Generic;

namespace TLH.Models.Players
{
    public class PLayerDetailViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Merit { get; set; }

        public string Comment { get; set; }

        public List<AchivementViewModel> Achives { get; set; }
    }

    public class AchivementViewModel
    {
        public string TourName { get; set; }

        public string TeamName { get; set; }

        public decimal Amount { get; set; }

        public int Place { get; set; }
    }
}
