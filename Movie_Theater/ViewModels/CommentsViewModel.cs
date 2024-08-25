using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.ViewModels
{
    public class CommentsViewModel
    {
        [Display(Name ="評論編號")]
        public int CommentsId { get; set; }

        [Display(Name = "電影編號")]
        public int Movie_Id { get; set; }

        [Display(Name = "用戶編號")]
        public int User_Id { get; set; }

        [Display(Name = "評論訊息")]
        public string CommentMessage { get; set; }

        //Users 
        [Display(Name = "會員名稱")]
        public string UserName { get; set; }

        //Movies
        [Display(Name = "電影名稱")]
        public string MovieName { get; set; }
    }
}
