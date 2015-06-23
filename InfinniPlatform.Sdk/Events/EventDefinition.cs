namespace InfinniPlatform.Sdk.Events
{
    /// <summary>
    ///     Строготипизированное событие обработчика
    /// </summary>
    public sealed class EventDefinition
    {
        public EventDefinition()
        {
            Index = -1;
        }

        public string Property { get; set; }
        public object Value { get; set; }
        public EventType Action { get; set; }
        public int Index { get; set; }
    }
}