using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Application.Result
{
    public class ResultSet
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
    }

    public class ResultSet<T> : ResultSet
    {
        public T Data { get; set; }
    }
}
