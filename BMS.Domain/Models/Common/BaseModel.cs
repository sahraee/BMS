using System;
using System.ComponentModel.DataAnnotations;

namespace BMS.Domain.Models.Common
{
    /// <summary>
    /// Base class for models
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseModel<TKey>
    {
        [Key]

        public TKey Id { get; set; }
        public DateTime RegDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime? RemoveDate { get; set; }
    }

   
}
