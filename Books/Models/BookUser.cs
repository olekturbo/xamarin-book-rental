using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Models
{
    public class BookUser
    {
        [ForeignKey(typeof(Book))]
        public int BookId { get; set; }
        [ForeignKey(typeof(User))]
        public int UserId { get; set; }
    }
}
