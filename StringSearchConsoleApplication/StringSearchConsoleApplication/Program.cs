using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace StringSearchConsoleApplication
{
    class Program
    {
        public static void Main(string[] args)
        {


            Console.WriteLine("I use Windows10. This should be working on other systems too.");
            Console.WriteLine("--------------------");
            Console.WriteLine("First:");
            Console.WriteLine("   - App creates folder (TEXTFolder) and temporary WriteText3.txt file");
            Console.WriteLine("   - App inserts 150MB string inside WriteText3.txt file");
            Console.WriteLine("   - After successfull search, WriteText3.txt file is being deleted.");
            Console.WriteLine(" ");
            Console.WriteLine("Copy and paste CORRECT path to some EMPTY folder and press enter:");

            string folderName = Console.ReadLine().ToString();

            // Specifying a name for  top-level folder.
            //string folderName = @"c:\";

            // creating a string that specifies the path to a subfolder under  
            // top-level folder, adding a name for the subfolder to TESTFolder.
            string pathString = System.IO.Path.Combine(folderName, "TESTFolder");

            System.IO.Directory.CreateDirectory(pathString);


            string fileName = "WriteText3.txt";
            // Using Combine to add the file name to the path.
            pathString = System.IO.Path.Combine(pathString, fileName);


            // Verifying the path that have been constructed.

            Console.WriteLine("Path to my file: {0}\n", pathString);
            Console.WriteLine(" ");


            if (!System.IO.File.Exists(pathString))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(pathString))
                {
                    for (byte i = 0; i < 1; i++)
                    {
                        fs.WriteByte(i);
                        Console.WriteLine(".txt file created");
                    }
                }
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists. Please delete the file first", fileName);
                Console.WriteLine(" ");
                Console.WriteLine("Press any key to exit");
                System.Console.ReadKey();
                return;
            }




            // Writing string to a text file.

            string text5 = String.Concat(Enumerable.Repeat("AclassisthemostpowerfuldatatypeinC#.Likeastructure", 3000000));
            //System.IO.File.WriteAllText(@"C:\TEXTFolder\WriteText3.txt", text5);
            System.IO.File.WriteAllText(pathString, text5);

            Console.WriteLine(" Super long string inserted (3000000 characters)");
            Console.WriteLine("   ");
            Console.WriteLine(" Approx. size of WriteText3.txt file should be 150 MB = 3000000 characters");
            Console.WriteLine(" ");
            Console.WriteLine("To ensure <2sec search time and <1GB memory limit: ");
            Console.WriteLine("   - efective search should be on file less than 230MB size (=5000000 chars)");
            Console.WriteLine("   - string for searching should be more than 2 characters long");
            Console.WriteLine("   - all of this this also depends on your framework");
            Console.WriteLine(" ------------------------------------------------------------------------------ ");
            Console.WriteLine(" I have tried several string search algorithms for best performance.");
            Console.WriteLine(" I used Boyer–Moore String Search Algorithm as best choice in this case.");
            Console.WriteLine(" ");
            
            Console.WriteLine(" Insert your string to search and press enter:");

            string stringToSearch = Console.ReadLine().ToString();


            // Read the file as one string.
            //string textInFile = System.IO.File.ReadAllText(@"C:\TEXTFolder\WriteText3.txt");
            string textInFile = System.IO.File.ReadAllText(pathString);
            

            Stopwatch stopWatch = new Stopwatch();

            // SEARCH execution
            stopWatch.Start();
            int[] value = SearchStringBM(textInFile, stringToSearch);
            stopWatch.Stop();



            // returning results 
            //Console.WriteLine(string.Join(",", value));
            Console.WriteLine("search successfully ended.");
            Console.WriteLine("Number of matches found:" + value.Length.ToString());
            Console.WriteLine(" ");

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}.{1:00}",
                ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime (sec.milisec): " + elapsedTime);

            Console.WriteLine(" ");

            // to delete file
            Console.WriteLine("Press enter to delete .txt file");
            Console.WriteLine(" ");
            Console.Read();


            // DELETING .txt file
            if (System.IO.File.Exists(pathString))
            {

                try
                {
                    System.IO.File.Delete(pathString);
                    Console.WriteLine("File sucessfully deleted");
                    Console.WriteLine("Don't forget to delete folder TESTFolder");
                    Console.WriteLine(" ");

                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            Console.WriteLine("Press any key to exit");


            //Keep the console window open in debug mode
            System.Console.ReadKey();




        }



        // Boyer–Moore String Search Algorithm

        public static int[] SearchStringBM(string str, string pat)
        {
            List<int> retVal = new List<int>();
            int m = pat.Length;
            int n = str.Length;

            int[] badChar = new int[256];

            BadCharHeuristic(pat, m, ref badChar);

            int s = 0;
            while (s <= (n - m))
            {
                int j = m - 1;

                while (j >= 0 && pat[j] == str[s + j])
                    --j;

                if (j < 0)
                {
                    retVal.Add(s);
                    s += (s + m < n) ? m - badChar[str[s + m]] : 1;
                }
                else
                {
                    s += Math.Max(1, j - badChar[str[s + j]]);
                }
            }

            return retVal.ToArray();
        }

        private static void BadCharHeuristic(string str, int size, ref int[] badChar)
        {
            int i;

            for (i = 0; i < 256; i++)
                badChar[i] = -1;

            for (i = 0; i < size; i++)
                badChar[(int)str[i]] = i;
        }



    }

}
