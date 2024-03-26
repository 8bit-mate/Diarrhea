namespace Diarrhea;

using CommandLine;

/// <summary>
/// Defines app's commands and their options.
/// </summary>
public static class Options
{
    private const int DefNumFilesReserved = 1024;

    /// <summary>
    /// Base class for the options related to any pack/extract command.
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
    /// Base class for the options related to any extraction command.
    /// </summary>
    public class ExtractBaseOptions : ContainerReadOptions
    {
        [Option('o', "output", Required = true, HelpText = "Output directory")]
        public string OutputDir { get; set; }

        [Option(
            'p',
            "prefix",
            Required = false,
            Default = "",
            HelpText = "Adds a prefix to each file name.")]
        public string Prefix { get; set; }

        [Option(
            's',
            "suffix",
            Required = false,
            Default = "",
            HelpText = "Adds a suffix to each file name.")]
        public string Suffix { get; set; }
    }

    /// <summary>
    /// Base class for the options related to a read operation with a container.
    /// </summary>
    public class ContainerReadOptions : ContainerOperationOptions
    {
        [Option('i', "input", Required = true, HelpText = "Input *.dat file to be processed.")]
        public string InputFile { get; set; }
    }

    [Verb("extract", HelpText = "Extract individual files.")]
    public class ExtractOptions : ExtractBaseOptions
    {
        [Option('f', "f-list", Required = true, HelpText = "Files to extract")]
        public IEnumerable<string> Filenames { get; set; }
    }

    [Verb("extract-all", HelpText = "Extract all files.")]
    public class ExtractAllOptions : ExtractBaseOptions
    {
    }

    [Verb("list", HelpText = "List files on a *.dat container.")]
    public class ListOptions : ContainerReadOptions
    {
        [Option(
            'r',
            "rows",
            Required = false,
            Default = new[] { "filename", "offset", "size" },
            HelpText = "Rows to print out. The default value prints out all rows.")]
        public IEnumerable<string> Rows { get; set; }
    }

    [Verb("packdir", HelpText = "Pack a directory with files into a *.dat container.")]
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
            HelpText = "Mask to select files from the directory.")]
        public string Mask { get; set; }

        [Option(
            'r',
            "regex",
            Required = false,
            Default = "(.*)",
            HelpText = "RegEx filter to remove prefixes/suffixes from the input file names.")]
        public string RegExFiler { get; set; }
    }
}