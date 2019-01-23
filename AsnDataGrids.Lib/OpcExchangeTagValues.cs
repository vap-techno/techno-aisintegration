namespace AsnDataGrids.Lib
{
    public struct OpcExchangeTagValues
    {
        /// <summary>
        /// Идентификатор задания АИС ТПС
        /// </summary>
        public long AisId { get; set; }

        /// <summary>
        /// Идентфикатор секции
        /// </summary>
        public long SecId { get; set; }

        /// <summary>
        /// Порядковый номер поста от 1 до ..
        /// </summary>
        public long PostNumber { get; set; }

        /// <summary>
        /// Тип операции 1 - Налив в АЦ, 2 - Налив КМХ, 3 - Слив из АЦ
        /// </summary>
        public uint CmdType { get; set; }

        /// <summary>
        /// Строка команды
        /// </summary>
        public string CmdStr { get; set; }
    }
}