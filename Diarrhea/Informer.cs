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

            foreach (var e in dataTable)
            {
                Console.WriteLine($"{e.Name,-16}{e.Offset,-16}{e.Size,-16}");
            }
        }
    }
}