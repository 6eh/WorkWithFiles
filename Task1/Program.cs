using System;
using System.IO;

namespace Task1
{
    class Program
    {
        const int MaxLifeMinutes = 30; // Время жизни файлов и папок в минутах
        const string RootPath = @"/Users/ownermac/Desktop/EraseMe"; // Папка, которую будем чистить

        static void Main(string[] args)
        {           
            while (true)
            {               
                Console.WriteLine($"{DateTime.Now}\n");
                CheckFolders(RootPath);
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
            }
        }

        static void CheckFolders(string path)
        {
            // Заходим в папку
            DirectoryInfo directory = new(path);
            if (directory.Exists)
            {
                // Чистим файлы в папке
                CheckFiles(directory.GetFiles());
                // Проверяем вложенные папки
                if (directory.GetDirectories().Length > 0) // Если в папке есть вложенные папки
                {
                    foreach (var folder in directory.GetDirectories())
                    {
                        //Console.WriteLine($"Folder {path} contains {folder.Name}");
                        // Если есть вложенная папка, заходим в неё через рекурсию
                        CheckFolders(folder.FullName);
                    }                  
                }
                // Если нет вложенных файлов и папок и это не сама папка, которую чистим
                else if (directory.GetFiles().Length == 0 && directory.FullName != RootPath)
                {
                    Console.WriteLine($"{directory.Name} is empty");
                    try
                    {                        
                        TimeSpan ts = DateTime.Now - directory.LastAccessTime;
                        // Проверяем папку на время жизни
                        if (ts.Minutes >= MaxLifeMinutes)
                        {
                            Console.WriteLine($"{directory.Name} has not been used for > {MaxLifeMinutes} minutes and be deleted");
                            // Удалить папку
                            if (directory.Exists)
                                directory.Delete(true);
                            else
                                Console.WriteLine($"{directory.FullName} is not exists!");
                        }
                        else
                            Console.WriteLine($"{directory.Name}({directory.Parent}) will be deleted after {(MaxLifeMinutes - ts.Minutes)} minutes");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }                
            }
            else
                Console.WriteLine($"{directory.Name} is not exists!");

        }

        static void CheckFiles(FileInfo[] files)
        {            
            foreach (var file in files)
            {
                try
                {
                    TimeSpan ts = DateTime.Now - file.LastAccessTime;
                    // Если файл не использовался более заданного времени
                    if (ts.Minutes >= MaxLifeMinutes)
                    {
                        Console.WriteLine($"{file.Name} has not been used for > {MaxLifeMinutes} minutes and be deleted");
                        // Удалить файл
                        if (file.Exists)
                            file.Delete();
                        else
                            Console.WriteLine($"{file.FullName} is not exists!");
                    }
                    else
                        Console.WriteLine($"{file.Name} will be deleted after {(MaxLifeMinutes-ts.Minutes)} minutes");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        
    }
}

