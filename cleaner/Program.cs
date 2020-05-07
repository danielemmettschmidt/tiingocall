using System;
using System.Reflection;
using System.Security.Permissions;
using System.IO;
using cleaner_driver;

namespace cleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin");

            string dir;

            if(args.Length == 0)
            {
                Console.WriteLine("You didn't supply any arguments.");
                dir = bettergetassembly();
            }
            else
            {
                dir = args[0];
            }

            if (valdir(dir) == false)
            {
                Console.WriteLine("First argument needs to be a valid directory.");
                return;
            }
            else
            {
                Parser _parser = new Parser(dir);

                if (_parser.isstillgood == true)
                {
                    Console.WriteLine("Password:");

                    EngineQuery eq = new EngineQuery();
                    eq.server = "167.71.172.36";
                    eq.user = "root";
                    eq.password = Console.ReadLine();

                    MYSQLEngine.WriteManifest(_parser, in eq);

                    MYSQLEngine.WriteSource(_parser, in eq);

                }

            }

        }

        static string bettergetassembly()
        {
            string start = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).CodeBase);

            if (start.Contains("file:\\file:") == true)
            {
                start = start.Replace("file:\\file:", "");
            }

            if (start.Contains("file:\\") == true)
            {
                start = start.Replace("file:\\", "");
            }

            return start;
        }

        public static bool valdir(string input)
        {
            if (Directory.Exists(input))
            {
                return true;
            }
            else
            {
                Console.WriteLine("\nDirectory does not exist.\n");
                return false;
            }
        }

    }
}
