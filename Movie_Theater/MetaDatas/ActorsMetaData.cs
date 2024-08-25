using System.ComponentModel.DataAnnotations;

namespace Movie_Theater.MetaDatas
{
    internal class ActorsMetaData
    {
        [Display(Name ="演員編號")]
        public int ActorId { get; set; }

        [Display(Name = "演員名稱")]
        public string ActorName { get; set; }
    }
}