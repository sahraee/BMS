using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DTO.DTOModels.Book
{/// <summary>
/// New book informion
/// </summary>
    public class BookEditDtoModel : BookNewDtoModel
    {
        /// <summary>
        /// Book identity
        /// </summary>
        [Required(ErrorMessage = "BookId must has value")]
        public int BookId { get; set; }


    }
}
