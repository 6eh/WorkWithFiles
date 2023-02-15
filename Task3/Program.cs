using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Task3
{
    class Program
    {
        const int MaxLifeMinutes = 30; // Время жизни файлов и папок в минутах

        class FolderInf
        {
            public long InitialSize;
            public long CurrentSize;
            public long FilesDeleted;
            public long DeletedFilesSize;            
        }

        static FolderInf FolderInformation = new();
        const string RootPath = @"/Users/ownermac/Desktop/EraseMe"; // Папка, которую будем чистить


        static void Main(string[] args)
        {
            
            int i = 0;
            while (true)
            {                
                Console.WriteLine($"{DateTime.Now}\n");
                FolderInformation.CurrentSize = CheckFolders(RootPath);
                Console.WriteLine("-------------------------");
                if (i == 0) // Если это первый проход, то назначить изначальный размер папки
                    FolderInformation.InitialSize = FolderInformation.CurrentSize;

                Console.WriteLine($"Initial Folder Size: {FolderInformation.InitialSize} byte");
                Console.WriteLine($"Files deleted: {FolderInformation.FilesDeleted}");
                Console.WriteLine($"Deleted Files Size: {FolderInformation.DeletedFilesSize} byte");
                Console.WriteLine($"Current Folder Size: {FolderInformation.CurrentSize} byte");
                
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
                i++;
                if (i > 99)
                    i = 1;
            }
        }

        static long CheckFolders(string path)
        {
            long folderSize = 0;
            // Заходим в папку
            DirectoryInfo directory = new(path);
            if (directory.Exists)
            {
                // Чистим файлы в папке
                folderSize = CheckFiles(directory.GetFiles());
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
                    Console.WriteLine($">{directory.Name} is empty");
                    try
                    {
                        TimeSpan ts = DateTime.Now - directory.LastAccessTime;
                        // Проверяем папку на время жизни
                        if (ts.Minutes >= MaxLifeMinutes)
                        {
                            Console.WriteLine($">{directory.Name} has not been used for > {MaxLifeMinutes} minutes and be deleted");
                            // Удалить папку
                            if (directory.Exists)
                                directory.Delete(true);
                            else
                                Console.WriteLine($">{directory.FullName} is not exists!");
                        }
                        else
                            Console.WriteLine($">{directory.Name} will be deleted after {(MaxLifeMinutes - ts.Minutes)} minutes");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
            }
            else
                Console.WriteLine($">{directory.Name} is not exists!");
            return folderSize;

        }

        static long CheckFiles(FileInfo[] files)
        {
            long filesSize = 0;
            foreach (var file in files)
            {
                try
                {
                    // Считаем размер файлов в папке
                    filesSize += file.Length;                    
                    TimeSpan ts = DateTime.Now - file.LastAccessTime;
                    // Если файл не использовался более заданного времени
                    if (ts.Minutes >= MaxLifeMinutes)
                    {
                        Console.WriteLine($">{file.Name} has not been used for > {MaxLifeMinutes} minutes and be deleted");
                        // Удалить файл
                        if (file.Exists)
                        {
                            FolderInformation.FilesDeleted += 1;
                            FolderInformation.DeletedFilesSize += file.Length;
                            file.Delete();
                        }
                        else
                            Console.WriteLine($">{file.FullName} is not exists!");
                    }
                    else
                        Console.WriteLine($"> {file.Name} will be deleted after {(MaxLifeMinutes - ts.Minutes)} minutes");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            return filesSize;
        }
    }
}

