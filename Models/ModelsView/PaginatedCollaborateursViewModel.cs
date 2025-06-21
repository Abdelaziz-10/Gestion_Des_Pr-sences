using System.Collections.Generic;

namespace Gestion_Des_prèneces.Models.ModelsView
{
    public class PaginatedCollaborateursViewModel
    {
        public IEnumerable<CollaborateursModelView> Collaborateurs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<int> AssignedIds { get; set; }
    }

}
