namespace AsnDataGrids.Lib
{
    public struct OpcExchangeTags
    {
        /// <summary>
        /// Идентификатор задания АИС ТПС
        /// </summary>
        public string AisId { get; set; }

        /// <summary>
        /// Идентфикатор секции
        /// </summary>
        public string SecId { get; set; }

        /// <summary>
        /// Порядковый номер поста от 1 до ..
        /// </summary>
        public string PostNumber { get; set; }

        /// <summary>
        /// Тип операции 1 - Налив в АЦ, 2 - Налив КМХ, 3 - Слив из АЦ
        /// </summary>
        public string CmdType { get; set; }

        // TODO: Вставить валидацию
    }
}