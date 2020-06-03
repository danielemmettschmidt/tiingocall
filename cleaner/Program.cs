using System;
using System.Reflection;
using System.Security.Permissions;
using System.IO;
using System.Threading;
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
                dir = ImprovedGetAssembly();
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
                Console.WriteLine("Password:");

                EngineQuery eq = new EngineQuery();
                eq.server = "167.71.172.36";
                eq.user = "root";
                eq.password = Console.ReadLine();

                Parser _parser = new Parser(dir, eq);

                if (_parser.isstillgood == true)
                {
                    MYSQLEngine.WriteManifest(_parser);

                    MYSQLEngine.WriteSource(_parser);
                                        
                    for(int ii = 0; ii < 3; ii++)
                    {
                        Thread.Sleep(1000);

                        Console.WriteLine("" + (ii + 1));
                    }

                    _parser.ComputeBuyList();

                }

                Console.ReadLine();

            }

        }

        static string ImprovedGetAssembly()
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
