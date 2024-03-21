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
        /// Base class for the options related to a pack/extract operation on a container.
        /// </summary>
        public class ContainerOperationOptions
        {
            [Option(
                'n',
                "n-files",
                Default = DefNumFilesReserved,
                HelpText = "Container capacity: number of files allowed to pack.")]
            public int NumFilesReserved { get; set; }
        }

        /// <summary>
        /// Base class for the options related to a read operation with a container.
        /// </summary>
        public class ContainerReadOptions : ContainerOperationOptions
        {
            [Option('i', "input", Required = true, HelpText = "Input *.dat file to be processed.")]
            public string InputFile { get; set; }
        }

        [Verb("extract-all", HelpText = "Extract all files.")]
        public class ExtractAllOptions : ContainerReadOptions
        {
            [Option('o', "output", Required = true, HelpText = "Output directory.")]
            public string OutputDir { get; set; }

            [Option(
                's',
                "suffix",
                Required = false,
                Default = "",
                HelpText = "Adds a suffix to each filename.")]
            public string Suffix { get; set; }

        }

        [Verb("list", HelpText = "List files on a .dat container.")]
        public class ListOptions : ContainerReadOptions
        {
        }

        [Verb("packdir", HelpText = "Pack a directory with files into a .dat container.")]
        public class PackOptions : ContainerOperationOptions
        {
            [Option('i', "input", Required = true, HelpText = "Directory with files to pack.")]
            public string InputDir { get; set; }

            [Option('o', "output", Required = true, HelpText = "Output file.")]
            public string OutputFile { get; set; }

            [Option(
                'm',
                "mask",
                Required = false,
                Default = "*.*",
                HelpText = "Mask to select files from the dir. Default: \"*.*\".")]
            public string Mask { get; set; }
        }
    }
}