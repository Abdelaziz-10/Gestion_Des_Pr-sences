using System;
using System.Collections.Generic;

#nullable disable

namespace Gestion_Des_prèneces.Models
{
    public partial class Bulletin
    {
        public int IdBulletin { get; set; }
        public int? NbHT { get; set; }
        public DateTime? DateBulletin { get; set; }
        public int? NumCl { get; set; }

        public virtual Collaborateur NumClNavigation { get; set; }
    }
}
