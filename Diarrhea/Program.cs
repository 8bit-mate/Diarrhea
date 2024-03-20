namespace Diarrhea
{
    using System;
    using CommandLine;

    /// <summary>
    /// Reads and executes commands from the CLI.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            {
                Parser.Default.ParseArguments<Options.ExtractAllOptions, Options.ListOptions, Options.PackOptions>(args)
                              .WithParsed<Options.ExtractAllOptions>(options => RunExtractAll(options))
                              .WithParsed<Options.ListOptions>(options => RunListFiles(options))
                              .WithParsed<Options.PackOptions>(options => RunPackDir(options))
                              .WithNotParsed(errors => HandleParseError(errors));
            }

            static void RunExtractAll(Options.ExtractAllOptions opts)
            {
                ContainerParser parser = new (opts.InputFile, opts.NumFilesReserved);
                Extracter.ExtractAll(parser, opts.OutputDir, opts.Suffix);
            }

            static void RunListFiles(Options.ListOptions opts)
            {
                ContainerParser parser = new (opts.InputFile, opts.NumFilesReserved);
                Informer.ListFiles(parser);
            }

            static void RunPackDir(Options.PackOptions opts)
            {
            }

            static void HandleParseError(IEnumerable<Error> errs)
            {
                // handle parsing errors
            }
        }
    }
}
