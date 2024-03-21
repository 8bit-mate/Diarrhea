namespace Diarrhea
{
    using CommandLine;

    /// <summary>
    /// Entry point. Reads and executes commands from the CLI.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            {
                HandleInput(args);
                try
                {
                    //HandleInput(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            static void HandleInput(string[] args)
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
                Packer.PackDir(opts.InputDir, opts.OutputFile, opts.Mask, opts.NumFilesReserved);
            }

            static void HandleParseError(IEnumerable<Error> errs)
            {
                // handle parsing errors
            }
        }
    }
}
