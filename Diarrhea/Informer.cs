using System.Collections.ObjectModel;

namespace Diarrhea
{
    /// <summary>
    /// Provide methods to inform user about container's data.
    /// </summary>
    public static class Informer
    {
        private const int FileNameLen = 32;
        private const int IntLen = 16;

        /// <summary>
        /// Prints the list of files packed in a container.
        /// </summary>
        /// <param name="parser">Parser instance.</param>
        public static void ListFiles(ContainerParser parser, string[] keys)
        {
            var dataTable = parser.Parse();

            var attributeNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "filename", "Filename".PadRight(FileNameLen) },
                        { "offset", "Offset (base10)".PadRight(IntLen) },
                        { "size", "Size(base10)".PadRight(IntLen) },
                    };

            PrintLine(attributeNames, keys);

            foreach (var e in dataTable)
            {
                var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "filename", e.Name.PadRight(FileNameLen) },
                        { "offset", e.Offset.ToString().PadRight(IntLen) },
                        { "size", e.Size.ToString().PadRight(IntLen) },
                    };

                PrintLine(attributes, keys);
            }
        }

        private static void PrintLine(Dictionary<string, string> items, string[] keys)
        {
            foreach (var key in keys)
            {
                string itemToPrint;

                if (items.TryGetValue(key, out itemToPrint))
                {
                    Console.Write(itemToPrint);
                }
                else
                {
                    Console.WriteLine();
                    throw new ArgumentException($"Row key '{key}' is not found.");
                }
            }

            Console.WriteLine();
        }
    }
}