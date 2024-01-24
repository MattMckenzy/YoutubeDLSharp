
namespace YoutubeDLSharp
{
    /// <summary>
    /// Encapsulates the output of a yt-dlp download operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// Creates a new instance of class RunResult.
    /// </remarks>
    public class RunResult<T>(bool success, string[] error, T result)
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool Success { get; } = success;
        /// <summary>
        /// The accumulated error output.
        /// </summary>
        public string[] ErrorOutput { get; } = error;
        /// <summary>
        /// The output data.
        /// </summary>
        public T Data { get; } = result;
    }
}
