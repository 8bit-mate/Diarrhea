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
                try
                {
                    HandleInput(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            static void HandleInput(string[] args)
            {
                Parser.Default.ParseArguments<Options.ExtractOptions, Options.ExtractAllOptions, Options.ListOptions, Options.PackOptions>(args)
                              .WithParsed<Options.ExtractOptions>(options => RunExtractList(options))
                              .WithParsed<Options.ExtractAllOptions>(options => RunExtractAll(options))
                              .WithParsed<Options.ListOptions>(options => RunListFiles(options))
                              .WithParsed<Options.PackOptions>(options => RunPackDir(options))
                              .WithNotParsed(errors => HandleParseError(errors));
            }

            static void RunExtractAll(Options.ExtractAllOptions opts)
            {
                ContainerParser parser = new (opts.InputFile, opts.NumFilesReserved);
                string[] nameWrapper = [opts.Prefix, opts.Suffix];
                Extracter.ExtractAll(parser, opts.OutputDir, nameWrapper);
            }

            static void RunExtractList(Options.ExtractOptions opts)
            {
                ContainerParser parser = new (opts.InputFile, opts.NumFilesReserved);
                string[] nameWrapper = [opts.Prefix, opts.Suffix];
                Extracter.ExtractList(parser, opts.Filenames.ToList(), opts.OutputDir, nameWrapper);
            }

            static void RunListFiles(Options.ListOptions opts)
            {
                ContainerParser parser = new (opts.InputFile, opts.NumFilesReserved);
                Informer.ListFiles(parser);
            }

            static void RunPackDir(Options.PackOptions opts)
            {
                Packer packer = new Packer(opts.NumFilesReserved, opts.RegExFiler);

                packer.PackDir(opts.InputDir, opts.OutputFile, opts.Mask);
            }

            static void HandleParseError(IEnumerable<Error> errs)
            {
                // handle parsing errors
            }
        }
    }
}
