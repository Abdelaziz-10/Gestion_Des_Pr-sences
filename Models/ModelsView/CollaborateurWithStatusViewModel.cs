namespace Gestion_Des_prèneces.Models.ModelsView
{
    public class CollaborateurWithStatusViewModel
    {
        public int NumCl { get; set; }
        public string NomCl { get; set; }
        public string PrenomCl { get; set; }
        public string Adresse { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }

        public bool IsAssigned { get; set; }
    }
}
