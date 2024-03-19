using CommandLine;
using System.Reflection;

namespace Diarrhea
{
    internal partial class Program
    {
        static void Main(string[] args)
        {

            {
                Parser.Default.ParseArguments<Options.ExtractOptions, Options.ListOptions, Options.PackOptions>(args)
                              .WithParsed<Options.ExtractOptions>(options => RunExtract(options))
                              .WithParsed<Options.ListOptions>(options => RunListFiles(options))
                              .WithParsed<Options.PackOptions>(options => RunPackDir(options))
                              .WithNotParsed(errors => HandleParseError(errors));
            }


            static void RunExtract(Options.ExtractOptions opts)
            {
                Console.WriteLine(opts.NumFilesReserved);
            }

            static void RunListFiles(Options.ListOptions opts)
            {
            }

            static void RunPackDir(Options.PackOptions opts)
            {
            }

            static void HandleParseError(IEnumerable<Error> errs)
            {
                //handle errors
            }
        }
    }
}
