using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tomasos_Pizzeria.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Matratt> Matratts { get; set; }
        public List<BestallningMatratt> Cart { get; set; }
        public BestallningMatratt BM { get; set; }
    }
}