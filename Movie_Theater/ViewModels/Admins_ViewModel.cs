using Movie_Theater.Models;

namespace Movie_Theater.ViewModels
{
    public class Admins_ViewModel
    {
        public string Account { get; set; }

        public int Role_Id { get; set; }

        public string AdminName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime CreateAt { get; set; }

        public string AdminPassword { get; set; }
    }
}
