using System;
using System.Collections.Generic;

#nullable disable

namespace Tomasos_Pizzeria.Models
{
    public partial class Matratt
    {
        public Matratt()
        {
            BestallningMatratts = new HashSet<BestallningMatratt>();
            MatrattProdukts = new HashSet<MatrattProdukt>();
        }

        public int MatrattId { get; set; }
        public string MatrattNamn { get; set; }
        public string Beskrivning { get; set; }
        public int Pris { get; set; }
        public int MatrattTyp { get; set; }

        public virtual MatrattTyp MatrattTypNavigation { get; set; }
        public virtual ICollection<BestallningMatratt> BestallningMatratts { get; set; }
        public virtual ICollection<MatrattProdukt> MatrattProdukts { get; set; }
    }
}
