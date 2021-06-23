﻿using System.Collections.Generic;

namespace Tomasos_Pizzeria.Models.ViewModels
{
    public class AdminViewModel
    {
        public NewMatrattModel NewMatratt { get; set; }
        public List<Produkt> ExcistingProdukter { get; set; }
        public List<MatrattTyp> Typer { get; set; }
        public Produkt Produkt { get; set; }
        public MatrattTyp Typ { get; set; }
    }
    public class NewMatrattModel
    {
        public string MatrattNamn { get; set; }
        public string Beskrivning { get; set; }
        public int Pris { get; set; }
        public int MatrattTyp { get; set; }
        public List<int> ProduktList { get; set; }
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
