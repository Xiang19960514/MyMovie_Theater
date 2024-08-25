using Movie_Theater.Models;

namespace Movie_Theater.ViewModels
{
    public class Shows_ViewModel
    {
        public int Auditorium_Id { get; set; }

        public int ShowId { get; set; }

        public string MovieName { get; set; }

        public int Runtime { get; set; }

        public DateTime ShowDateTime { get; set; }

        public string ShowDate { get; set; }

    }
}
