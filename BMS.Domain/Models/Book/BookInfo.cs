using BMS.Domain.Models.Common;


namespace BMS.Domain.Models.Book
{
    public class BookInfo :  BaseModel<int>
    {

        public string Title {  get; set; }
        public string Author { get; set; }  
        public int PublishedYear { get; set; }
        public string Genre {  get; set; }


    }
}
