using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassIn.Communication.Responses
{
    public class PaginatedResponseAllAttendeesJson : ResponseAllAttendeesJson
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int ItensCount { get; set; }
    }
}
