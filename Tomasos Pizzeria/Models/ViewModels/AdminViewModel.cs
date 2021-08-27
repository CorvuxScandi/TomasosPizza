using System.Collections.Generic;

namespace Tomasos_Pizzeria.Models.ViewModels
{
    public class AdminViewModel
    {
        public NewMatrattModel NewMatratt { get; set; }
        public NewMatrattTyp NewTyp { get; set; }
        public NewProduct NewProduct { get; set; }
        public List<Produkt> Produkter { get; set; }
        public List<MatrattTyp> Typer { get; set; }
    }

    public class NewMatrattModel
    {
        public string MatrattNamn { get; set; }
        public string Beskrivning { get; set; }
        public int Pris { get; set; }
        public int MatrattTyp { get; set; }
        public IEnumerable<int> ProduktList { get; set; }
    }

    public class NewMatrattTyp
    {
        public string Beskrivning { get; set; }
    }

    public class NewProduct
    {
        public string ProduktNamn { get; set; }
    }
}