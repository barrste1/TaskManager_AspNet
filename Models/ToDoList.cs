using System;
using System.Collections.Generic;

namespace ToDoList_08272020.Models
{
    public partial class ToDoList
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Assignment { get; set; }
        public string AssignmentDescription { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? Completed { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
