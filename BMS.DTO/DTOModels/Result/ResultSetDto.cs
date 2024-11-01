using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DTO.DTOModels.Result
{
    /// <summary>
/// Result operation
/// </summary>
    public class ResultSetDto
    {
        /// <summary>
        /// Result operation
        /// </summary>
        public bool IsSucceed { get; set; } = true;
        /// <summary>
        /// Message of operation
        /// </summary>
        public string Message { get; set; } = "";
    }

    public class ResultSetDto<T> : ResultSetDto
    {
        /// <summary>
        ///Return data
        /// </summary>
        public T Data { get; set; }
    }
}
