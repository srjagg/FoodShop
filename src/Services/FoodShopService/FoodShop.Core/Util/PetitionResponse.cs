namespace FoodShop.Core.Util
{
    /// <summary>
    /// Representa la respuesta de una petición.
    /// </summary>
    /// <typeparam name="T">El tipo de resultado de la petición.</typeparam>
    public class PetitionResponse<T>
    {
        /// <summary>
        /// Indica si la petición fue exitosa.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// El mensaje asociado con la petición.
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// El módulo o componente que emitió la petición.
        /// </summary>
        public string? Module { get; set; }
        /// <summary>
        /// La URL asociada con la petición (si corresponde).
        /// </summary>
        public string? URL { get; set; }
        /// <summary>
        /// El resultado de la petición.
        /// </summary>
        public T? Result { get; set; }

        public PetitionResponse() {}

        public PetitionResponse(bool success, string message, string module, string url, T result)
        {
            Success = success;
            Message = message;
            Module = module;
            URL = url;
            Result = result;
        }
    }
}
