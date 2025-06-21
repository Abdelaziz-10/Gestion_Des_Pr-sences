using System;
using System.Collections.Generic;

#nullable disable

namespace Gestion_Des_prèneces.Models
{
    public partial class Collaborateur
    {
        public Collaborateur()
        {
            Absences = new HashSet<Absence>();
            Bulletins = new HashSet<Bulletin>();
            Congés = new HashSet<Congé>();
            Disponibilités = new HashSet<Disponibilité>();
            Respocollaborateurs = new HashSet<Respocollaborateur>();
        }

        public int NumCl { get; set; }
        public string NomCl { get; set; }
        public string PrenomCl { get; set; }
        public string Adresse { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public int? IdMission { get; set; }

        public virtual Mission IdMissionNavigation { get; set; }
        public virtual ICollection<Absence> Absences { get; set; }
        public virtual ICollection<Bulletin> Bulletins { get; set; }
        public virtual ICollection<Congé> Congés { get; set; }
        public virtual ICollection<Disponibilité> Disponibilités { get; set; }
        public virtual ICollection<Respocollaborateur> Respocollaborateurs { get; set; }
    }
}
