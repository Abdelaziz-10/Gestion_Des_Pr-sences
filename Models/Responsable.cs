using System;
using System.Collections.Generic;

#nullable disable

namespace Gestion_Des_prèneces.Models
{
    public partial class Responsable
    {
        public Responsable()
        {
            Respocollaborateurs = new HashSet<Respocollaborateur>();
        }

        public int NumRe { get; set; }
        public string NomRe { get; set; }
        public string PrenomRe { get; set; }
        public string Adresse { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }

        public virtual ICollection<Respocollaborateur> Respocollaborateurs { get; set; }
    }
}
