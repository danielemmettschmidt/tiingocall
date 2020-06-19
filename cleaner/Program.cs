using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.IO;
using System.Threading;
using cleaner_driver;
using System.Linq;

namespace cleaner
{
    class Program
    {
        static string GetPassword()
        {
            string password = "";
            Console.WriteLine("Enter password:\n");

            ConsoleKeyInfo nextKey = Console.ReadKey(true);

            while (nextKey.Key != ConsoleKey.Enter)
            {
                if (nextKey.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = backspace(password);
                        // erase the last * as well
                        Console.Write(nextKey.KeyChar);
                        Console.Write(" ");
                        Console.Write(nextKey.KeyChar);
                    }
                }
                else
                {
                    password = password + nextKey.KeyChar;
                    Console.Write("*");
                }
                nextKey = Console.ReadKey(true);
            }

            return password;
        }

        private static string backspace(string input)
        {
            int ii = 0;

            string ret = "";

            foreach(char c in input)
            {
                if (ii < input.Length - 1)
                {
                    ret = ret + c;
                }

                ii++;
            }

            return ret;
        }
            

        static void Main(string[] args)
        {
            string dir;

            if(args.Length == 0)
            {
                Console.WriteLine("No arguments were supplied.\n");
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
                EngineQuery eq = new EngineQuery();
                eq.server = "167.71.172.36";
                eq.user = "root";

                bool goodpw = false;

                int pwii = 0;

                while(goodpw == false)
                {
                    if(pwii != 0)
                    {
                        if (pwii > 5 || eq.password.Equals("x"))
                        {
                            string close = "C";

                            if (eq.password != "x")
                            {
                                close = "Five or more bad passwords, c";
                            }

                            Console.WriteLine("\n" + close + "losing now.");
                            Console.ReadLine();
                            return;
                        }

                        Console.WriteLine("\nBad Password. Enter 'x' to exit.");
                    }

                    eq.password = GetPassword();

                    goodpw = MYSQLEngine.CheckPW(eq);

                    pwii++;
                }                

                Parser _parser = new Parser(dir, eq);

                _parser.Initialize();

                if (_parser.isstillgood == true)
                {
                    MYSQLEngine.WriteManifest(_parser);

                    MYSQLEngine.WriteSource(_parser);

                    Console.WriteLine("");

                    for (int ii = 0; ii < 3; ii++)
                    {
                        Thread.Sleep(1000);

                        Console.WriteLine("" + (ii + 1) + "...");
                    }

                    Console.WriteLine("");

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
