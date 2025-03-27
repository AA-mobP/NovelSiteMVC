using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovelSiteMVC.Models
{
    public class TodoModel
    {
        public int Id { get; set; }

        [Required, MaxLength(500)]
        public string Task { get; set; } = null!;

        [ValidateNever, DataType(DataType.DateTime)]
        public DateTime AddingDate { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime? DueDate { get; set; } = null;

        public StatusType Status { get; set; }
        
        public bool isDeleted { get; set; } = false;

        public override bool Equals(object? obj)
        {
            TodoModel? todoObj = obj as TodoModel;

            if (todoObj is null)
                return false;

            if (todoObj.Task == this.Task
                && todoObj.Status == Status
                && todoObj.DueDate == DueDate
                && todoObj.AddingDate == AddingDate)
                return true;

            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => Task;
    }

    public enum StatusType
    {
        Active,
        HasDueDate,
        Completed,
        Passed
    }
}
