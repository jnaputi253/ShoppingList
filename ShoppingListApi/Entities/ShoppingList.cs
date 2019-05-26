using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingListApi.Entities
{
    [Table("ShoppingLists")]
    public class ShoppingList
    {
        [Key]
        public int ShoppingListId { get; set; }
        
        [Required]
        [StringLength(64, MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastUpdated { get; set; }
    }
}
