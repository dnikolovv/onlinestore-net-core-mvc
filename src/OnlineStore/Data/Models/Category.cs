namespace OnlineStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Category : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must specify a category name.")]
        public string Name { get; set; }
    }
}
