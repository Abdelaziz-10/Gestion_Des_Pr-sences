using System;
using System.Collections.Generic;

#nullable disable

namespace Gestion_Des_prèneces.Models
{
    public partial class Mission
    {
        public Mission()
        {
            Collaborateurs = new HashSet<Collaborateur>();
        }

        public int IdMission { get; set; }
        public string NomMission { get; set; }

        public virtual ICollection<Collaborateur> Collaborateurs { get; set; }
    }
}
