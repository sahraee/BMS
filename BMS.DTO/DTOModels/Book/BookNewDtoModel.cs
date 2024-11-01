using System.ComponentModel.DataAnnotations;

namespace BMS.DTO.DTOModels.Book
{
    /// <summary>
    /// Information of book
    /// </summary>
    public class BookNewDtoModel
    {

        /// <summary>
        /// Book Title
        /// </summary>
        [Required(ErrorMessage = "Title must has value")]
        public string Title { get; set; }


        /// <summary>
        /// Book published year
        /// </summary>
        [Required(ErrorMessage = "Published must has value")]
        public int PublishedYear { get; set; }


        /// <summary>
        /// Book author
        /// </summary>
        [Required(ErrorMessage = "Author must has value")]
        public string Author { get; set; }

        /// <summary>
        /// Book genre
        /// </summary>
        [Required(ErrorMessage =  "Genre must has value")]
        public string Genre { get; set; }
    }
}
