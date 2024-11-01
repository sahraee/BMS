
using System.ComponentModel.DataAnnotations;


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
