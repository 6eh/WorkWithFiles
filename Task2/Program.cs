using System;
using System.IO;

namespace Task2
{
    class Program
    {
        static long AllSize = 0;

        static void Main(string[] args)
        {
            string path = @"/Users/ownermac/Desktop/EraseMe";

            GedFolderSize(path);
            Console.WriteLine(AllSize);
            Console.ReadKey();
        }

        static void GedFolderSize(string path)
        {            
            try
            { 
                DirectoryInfo directory = new(path);
                if (directory.Exists)
                {
                    // Считаем размер файлов в папке                    
                    foreach (var file in directory.GetFiles())
                    {
                        Console.WriteLine($"{file.Name} size is {file.Length} bytes");
                        AllSize += file.Length;
                    }
                    if (directory.GetDirectories().Length > 0)
                    {
                        foreach (var folder in directory.GetDirectories())
                        {
                            // Если есть вложенная папка, заходим в неё через рекурсию
                            GedFolderSize(folder.FullName);
                        }                        
                    }
                }
                else
                    Console.WriteLine($"Folder {directory.FullName} in not exists");                
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}

