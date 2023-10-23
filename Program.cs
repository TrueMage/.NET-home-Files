using System.Diagnostics.Metrics;
using System.Reflection;

namespace home_filesearch
{
    internal class Program
    {
        public static void CountFiles(string path, Dictionary<string, int> extensions)
        {
            string[] files = Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {
                string key = Path.GetExtension(files[i]);
                if(extensions.ContainsKey(key)) extensions[key]++;
                else
                {
                    extensions.Add(key, 1);
                }
            }
        }

        public static int CountDirectories(string path)
        {
            return Directory.GetDirectories(path).Length;
        }

        public static void Count(string path, Dictionary<string, int> extensions)
        {
            string[] directories = Directory.GetDirectories(path);

            foreach (var dir in directories)
            {
                //Console.WriteLine(path);
                CountFiles(path, extensions);
                if (CountDirectories(path) > 0) Count(dir, extensions);
                /*{
                    try // Есть папки, для которых нужны права, и этот try catch просто позволяет нам их пропустить
                    {
                        
                    }
                    catch (Exception e)
                    {
                    }
                }*/
            }
        }

        static void Main(string[] args)
        {
            Dictionary<string, int> extensions = new Dictionary<string, int>();

            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                var path = drive.Name;
                try
                {
                    Count(path, extensions);
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }
            }
            // https://stackoverflow.com/questions/289/how-do-you-sort-a-dictionary-by-value
            var sortedExtensions = from entry in extensions orderby entry.Value descending select entry;

            int count = 1;
            Console.WriteLine("+----+--------------------+-------+");
            Console.WriteLine("|{0,3} | {1,-18} | {2,5} | {3,10} | {4,17} | {5,2} |", "№", "расширение", "кол-во", "общий объём в Б", "% от общего количества", "% от общего объёма");
            Console.WriteLine("+----+--------------------+-------+");

            foreach (var elem in sortedExtensions)
            {
                if (String.IsNullOrEmpty(elem.Key)) continue; 
                Console.WriteLine("|{2,3:G} | {1,-18} | {0,5:G} |", elem.Value, elem.Key, count++);
                if(count == 51) break;
            }
        }
    }
}