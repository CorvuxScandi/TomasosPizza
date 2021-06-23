using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tomasos_Pizzeria.Models
{
    public class MenuItemModel
    {
        public List<Matratt> Matratts { get; set; }
        public List<MatrattProdukt> MatrattsProdukts { get; set; }
        public List<MatrattTyp> Typs { get; set; }
        public List<Produkt> Produkts { get; set; }
        public BestallningMatratt BestallningMatratt { get; set; }
        public bool ShowAdminControlls { get; set; }
    }
}
