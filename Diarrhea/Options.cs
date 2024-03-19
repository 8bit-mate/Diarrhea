namespace Diarrhea
{
    using CommandLine;

    /// <summary>
    /// Defines app's commands and their options.
    /// </summary>
    public class Options
    {
        private const int DefNumFilesReserved = 1024;

        /// <summary>
        /// Base class for the options related to an IO operation with a container.
        /// </summary>
        public class ContainerOperationOptions
        {
            [Option(
                'n',
                "n-files",
                Default = DefNumFilesReserved,
                HelpText = "Container copacity: number of files allowed to pack")]
            public int NumFilesReserved { get; set; }
        }

        /// <summary>
        /// Base class for the options related to a read operation with a container.
        /// </summary>
        public class ContainerReadOptions : ContainerOperationOptions
        {
            [Option('i', "input", Required = true, HelpText = "Input *.dat file to be processed")]
            public string InputFile { get; set; }
        }

        [Verb("extract", HelpText = "Extract individual files")]
        public class ExtractOptions : ContainerReadOptions
        {
            [Option('f', "f-list", Required = true, HelpText = "Files to extract")]
            public IEnumerable<string> Filenames { get; set; }

            [Option('o', "output", Required = true, HelpText = "Output directory")]
            public string OutputDir { get; set; }
        }

        [Verb("list", HelpText = "List files on a *.dat container")]
        public class ListOptions : ContainerReadOptions
        {
        }

        [Verb("packdir", HelpText = "Pack a directory with files into a *.dat container")]
        public class PackOptions : ContainerOperationOptions
        {
            [Option('i', "input", Required = true, HelpText = "Directory with files to pack")]
            public string InputDir { get; set; }

            [Option('o', "output", Required = true, HelpText = "Output file")]
            public string OutputFile { get; set; }
        }
    }
}