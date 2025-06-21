using System;
using System.Collections.Generic;

#nullable disable

namespace Gestion_Des_prèneces.Models.ModelsView
{
    public partial class DisponibilitéView
    {
        public int IdDisponibilité { get; set; }
        public DateTime? DateMiseEnDisponibilité { get; set; }
        public DateTime? DateHDebutDisponibilité { get; set; }
        public DateTime? DateHFinDisponibilité { get; set; }
        public string NomCl { get; set; }
        public string Prenom { get; set; }

        public virtual Collaborateur NumClNavigation { get; set; }
    }
}
