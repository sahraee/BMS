using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain
{
    public static class Constants
    {

        public static class Messages
        {
            public const string BookIdType = "Type of bookId must be positive integer";
            public const string BookNotFound = "There is no book with this Id";
            public const string DuplicateData = "There is another book with this information";

            public const string BookNotDeleted = "Book information was not deleted";
            public const string BookNotEdited = "The book information was not edited";
            public const string BookNotInserted = "The book information was not inserted";


        }
    }
}