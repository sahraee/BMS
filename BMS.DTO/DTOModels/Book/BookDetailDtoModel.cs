
namespace BMS.DTO.DTOModels.Book
{
    /// <summary>
    /// Information of a book
    /// </summary>
    public class BookDetailDtoModel
    {
        /// <summary>
        /// Book identity
        /// </summary>
         public int BookId { get; set; }
        /// <summary>
        /// Book title
        /// </summary>
         public string Title { get; set; }
        /// <summary>
        /// Book author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Book published year
        /// </summary>
        public int PublishedYear { get; set; }

        /// <summary>
        /// Book genre
        /// </summary>
        public string Genre { get; set; }



    }
}
