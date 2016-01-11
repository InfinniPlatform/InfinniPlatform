using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.ContextTypes
{
    public sealed class ApplyResultContext : CommonContext, IApplyResultContext
    {
        public ApplyResultContext()
        {
            IsValid = true;
        }

        /// <summary>
        /// Объект, к которому приме
        /// </summary>
        public dynamic Item { get; set; }
    }
}