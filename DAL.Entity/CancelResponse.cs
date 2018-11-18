using DAL.Entity.Abstract;

namespace DAL.Entity
{
    public class CancelResponse : IAisResponse
    {
        /// <summary>
        /// [ID] Идентификатор задания в АИС ТПС
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// [CID] Идентификатор команды, в ответ на которую предоставляется результа отмены
        /// </summary>
        public string Cid { get; set; }

        /// <summary>
        /// [R] Признак отсутствия ошибок при попытке отмены
        /// </summary>
        public bool R { get; set; }

        /// <summary>
        /// [RM] Сообщение о результате отмены задания
        /// </summary>
        public string Rm { get; set; }

        /// <summary>
        /// [TS] Статус задания 
        /// </summary>
        public IAisResponse Ts { get; set; }
    }
}