using System.Collections.Generic;
using DAL.Entity.Abstract;

namespace DAL.Entity.Status
{
    public class FillOutStatusDetail : IStatusDetail
    {
        /// <summary>
        /// [Details] Детализирующие данные
        /// </summary>
        public List<FillOutStatusDetailDetail> Details { get; set; }
    }
}