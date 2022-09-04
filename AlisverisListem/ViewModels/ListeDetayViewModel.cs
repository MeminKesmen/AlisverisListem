namespace AlisverisListem.ViewModels
{
    public class ListeDetayViewModel
    {
        public int ListeId { get; set; }
        public string ListeAd { get; set; }
        public bool AktifMi { get; set; }
        public List<ListeElemanViewModel> ListeElemanlar { get; set; }

    }
}
