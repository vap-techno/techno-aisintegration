using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AisJson.Lib.DTO.Abstract;

namespace AisJson.Lib.DTO.Status
{
    public class EmptyStatusDetailDto: IStatusDetailDto
    {
        public object[] Details { get; set; }

        public EmptyStatusDetailDto()
        {
            Details = new object[] { };
        }

    }
}
