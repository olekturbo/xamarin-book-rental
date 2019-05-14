using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Models
{
    [Table("Books")]
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }

        [ManyToMany(typeof(BookUser))]
        public List<User> Users { get; set; }
    }
}
