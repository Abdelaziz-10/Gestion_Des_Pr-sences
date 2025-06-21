using System;
using System.Collections.Generic;

#nullable disable

namespace Gestion_Des_prèneces.Models
{
    public partial class Respocollaborateur
    {
        public int NumRe { get; set; }
        public int NumCl { get; set; }

        public virtual Collaborateur NumClNavigation { get; set; }
        public virtual Responsable NumReNavigation { get; set; }
    }
}
