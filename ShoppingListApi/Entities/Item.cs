using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingListApi.Entities
{
    [Table("Items")]
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}