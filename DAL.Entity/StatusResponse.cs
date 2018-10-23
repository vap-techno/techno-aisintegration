using DAL.Entity.Abstract;

namespace DAL.Entity
{
    public class StatusResponse : IAisResponse
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
        /// [SC] Код статуса задания в АСН
        /// </summary>
        public int Sc { get; set; }

        /// <summary>
        /// [RM] Сообщение о результате выполнения задания
        /// </summary>
        public string Rm { get; set; }

        /// <summary>
        /// [TS] Статус задания 
        /// </summary>
        public IStatusDetail Ts { get; set; }
    }
}