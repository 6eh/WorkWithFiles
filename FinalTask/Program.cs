using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{
    [Serializable]
    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    class Program
    {
        const string BinFilePath = @"/Users/ownermac/Downloads/Students.dat";
        const string StudentsFolder = @"/Users/ownermac/Desktop/Students";
                
        static void Main(string[] args)
        {
            ReadValues();
            
            Console.ReadKey();            
        }

        static void ReadValues()
        {
            if (File.Exists(BinFilePath))
            {
                try
                {
                    BinaryFormatter formatter = new();
                    using (var fs = new FileStream(BinFilePath, FileMode.OpenOrCreate))
                    {
                        Student[] students = (Student[])formatter.Deserialize(fs);
                        foreach (var student in students)
                        {
                            Console.WriteLine($"{student.Name}({student.Group})");
                            CreateFiles(student);
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

        }

        static void CreateFiles(Student student)
        {
            try
            {
                if (!Directory.Exists(StudentsFolder))
                    Directory.CreateDirectory(StudentsFolder);

                using (StreamWriter sw = new(StudentsFolder + $"/{student.Group}.txt", true))
                {
                    sw.WriteLine($"{student.Name}, {student.DateOfBirth}");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}

