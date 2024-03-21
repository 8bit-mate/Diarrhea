namespace Diarrhea
{
    /// <summary>
    /// Provide methods to inform user about container's data.
    /// </summary>
    public static class Informer
    {
        private const int StrLen = 16;

        /// <summary>
        /// Prints list of files packed in a container.
        /// </summary>
        /// <param name="parser">Parser instance.</param>
        public static void ListFiles(ContainerParser parser)
        {
            var dataTable = parser.Parse();

            Console.WriteLine($"{"Filename",-StrLen}{"Offset (base10)",-StrLen}{"Size (base10)",-StrLen}");

            foreach (var e in dataTable)
            {
                Console.WriteLine($"{e.Name,-StrLen}{e.Offset,-StrLen}{e.Size,-StrLen}");
            }
        }
    }
}