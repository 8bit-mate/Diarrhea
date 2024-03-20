namespace Diarrhea
{
    /// <summary>
    /// Provide methods to inform user about container's data.
    /// </summary>
    public class Informer
    {
        /// <summary>
        /// Prints list of files packed in a container.
        /// </summary>
        /// <param name="parser">Parser instance.</param>
        public static void ListFiles(ContainerParser parser)
        {
            var dataTable = parser.Parse();

            Console.WriteLine($"{"Filename",-16}{"Offset (base10)",-16}{"Size (base10)",-16}");

            foreach (var (fileName, offset, size) in dataTable)
            {
                Console.WriteLine($"{fileName,-16}{offset,-16}{size,-16}");
            }
        }
    }
}