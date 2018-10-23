using System.Collections.Generic;
using DAL.Entity.Abstract;

namespace DAL.Entity.Status
{
    public class FillInStatusDetail : IStatusDetail
    {
        /// <summary>
        /// [Details] Детализирующие данные
        /// </summary>
        public List<FillInStatusDetailDetail> Details { get; set; }
    }
}