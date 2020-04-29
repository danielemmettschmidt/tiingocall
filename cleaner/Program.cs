using System;
using System.Reflection;
using System.IO;

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
            }
            else
            {
                cleaner _cleaner = new cleaner(dir);
            }


        }

        static string bettergetassembly()
        {
            string start = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).CodeBase);
            string build = "";
            string ret = "";

            if (start.Contains("file:\\file:") == true)
            {
                start = start.Replace("file:\\file:", "");
            }

            if (start.Contains("file:\\") == true)
            {
                start = start.Replace("file:\\", "");
            }

            //foreach (char c in start)
            //{
            //    if(c == '\\')
            //    {
            //        ret = ret + build + c;
            //        build = "";

            //    }
            //    else
            //    {
            //        build = build + c;
            //    }
            //}

            ret = start; // either this or the comment block

            return ret;
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
