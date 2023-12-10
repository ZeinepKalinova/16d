using System;
using System.IO;

class FileManager
{
    private static string logFilePath = "LogFile.txt";

    static void Main()
    {
        Console.WriteLine("Простой файловый менеджер");

        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Просмотр содержимого директории");
            Console.WriteLine("2. Создание файла/директории");
            Console.WriteLine("3. Удаление файла/директории");
            Console.WriteLine("4. Копирование файла/директории");
            Console.WriteLine("5. Перемещение файла/директории");
            Console.WriteLine("6. Чтение из файла");
            Console.WriteLine("7. Запись в файл");
            Console.WriteLine("0. Выход");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewDirectoryContents();
                    break;
                case "2":
                    CreateFileOrDirectory();
                    break;
                case "3":
                    DeleteFileOrDirectory();
                    break;
                case "4":
                    CopyFileOrDirectory();
                    break;
                case "5":
                    MoveFileOrDirectory();
                    break;
                case "6":
                    ReadFromFile();
                    break;
                case "7":
                    WriteToFile();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    private static void ViewDirectoryContents()
    {
        Console.Write("Введите путь к директории: ");
        string path = Console.ReadLine();

        try
        {
            string[] files = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);

            Console.WriteLine("\nФайлы в директории:");
            foreach (var file in files)
            {
                Console.WriteLine(Path.GetFileName(file));
            }

            Console.WriteLine("\nПоддиректории:");
            foreach (var directory in directories)
            {
                Console.WriteLine(Path.GetFileName(directory));
            }

            LogAction($"Просмотр содержимого директории: {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            LogAction($"Ошибка при просмотре содержимого директории: {path}, {ex.Message}");
        }
    }

    private static void CreateFileOrDirectory()
    {
        Console.Write("Введите путь к директории: ");
        string directoryPath = Console.ReadLine();

        Console.Write("Введите имя файла/директории: ");
        string name = Console.ReadLine();

        Console.Write("Выберите тип (файл - F, директория - D): ");
        string type = Console.ReadLine().ToUpper();

        try
        {
            if (type == "F")
            {
                File.Create(Path.Combine(directoryPath, name));
                Console.WriteLine($"Файл {name} успешно создан.");
                LogAction($"Создание файла: {Path.Combine(directoryPath, name)}");
            }
            else if (type == "D")
            {
                Directory.CreateDirectory(Path.Combine(directoryPath, name));
                Console.WriteLine($"Директория {name} успешно создана.");
                LogAction($"Создание директории: {Path.Combine(directoryPath, name)}");
            }
            else
            {
                Console.WriteLine("Некорректный выбор типа. Попробуйте снова.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            LogAction($"Ошибка при создании файла/директории: {Path.Combine(directoryPath, name)}, {ex.Message}");
        }
    }

    private static void DeleteFileOrDirectory()
    {
        Console.Write("Введите путь к файлу/директории для удаления: ");
        string path = Console.ReadLine();

        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine($"Файл {path} успешно удален.");
                LogAction($"Удаление файла: {path}");
            }
            else if (Directory.Exists(path))
            {
                Directory.Delete(path);
                Console.WriteLine($"Директория {path} успешно удалена.");
                LogAction($"Удаление директории: {path}");
            }
            else
            {
                Console.WriteLine("Указанный файл/директория не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            LogAction($"Ошибка при удалении файла/директории: {path}, {ex.Message}");
        }
    }

    private static void CopyFileOrDirectory()
    {
        Console.Write("Введите путь к исходному файлу/директории: ");
        string sourcePath = Console.ReadLine();

        Console.Write("Введите путь к целевому каталогу: ");
        string targetDirectory = Console.ReadLine();

        try
        {
            if (File.Exists(sourcePath))
            {
                string destinationFile = Path.Combine(targetDirectory, Path.GetFileName(sourcePath));
                File.Copy(sourcePath, destinationFile);
                Console.WriteLine($"Файл {sourcePath} успешно скопирован в {destinationFile}");
                LogAction($"Копирование файла: {sourcePath} в {destinationFile}");
            }
            else if (Directory.Exists(sourcePath))
            {
                string destinationDirectory = Path.Combine(targetDirectory, Path.GetFileName(sourcePath));
                CopyDirectory(sourcePath, destinationDirectory);
                Console.WriteLine($"Директория {sourcePath} успешно скопирована в {destinationDirectory}");
                LogAction($"Копирование директории: {sourcePath} в {destinationDirectory}");
            }
            else
            {
                Console.WriteLine("Указанный файл/директория не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            LogAction($"Ошибка при копировании файла/директории: {sourcePath}, {ex.Message}");
        }
    }

    private static void MoveFileOrDirectory()
    {
        Console.Write("Введите путь к исходному файлу/директории: ");
        string sourcePath = Console.ReadLine();

        Console.Write("Введите путь к целевому каталогу: ");
        string targetDirectory = Console.ReadLine();

        try
        {
            if (File.Exists(sourcePath))
            {
                string destinationFile = Path.Combine(targetDirectory, Path.GetFileName(sourcePath));
                File.Move(sourcePath, destinationFile);
                Console.WriteLine($"Файл {sourcePath} успешно перемещен в {destinationFile}");
                LogAction($"Перемещение файла: {sourcePath} в {destinationFile}");
            }
            else if (Directory.Exists(sourcePath))
            {
                string destinationDirectory = Path.Combine(targetDirectory, Path.GetFileName(sourcePath));
                Directory.Move(sourcePath, destinationDirectory);
                Console.WriteLine($"Директория {sourcePath} успешно перемещена в {destinationDirectory}");
                LogAction($"Перемещение директории: {sourcePath} в {destinationDirectory}");
            }
            else
            {
                Console.WriteLine("Указанный файл/директория не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            LogAction($"Ошибка при перемещении файла/директории: {sourcePath}, {ex.Message}");
        }
    }

    private static void ReadFromFile()
    {
        Console.Write("Введите путь к файлу для чтения: ");
        string filePath = Console.ReadLine();

        try
        {
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                Console.WriteLine($"Содержимое файла {filePath}:\n{content}");
                LogAction($"Чтение из файла: {filePath}");
            }
            else
            {
                Console.WriteLine("Указанный файл не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            LogAction($"Ошибка при чтении из файла: {filePath}, {ex.Message}");
        }
    }

    private static void WriteToFile()
    {
        Console.Write("Введите путь к файлу для записи: ");
        string filePath = Console.ReadLine();

        Console.WriteLine("Введите текст для записи в файл:");
        string content = Console.ReadLine();

        try
        {
            File.WriteAllText(filePath, content);
            Console.WriteLine($"Текст успешно записан в файл {filePath}");
            LogAction($"Запись в файл: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            LogAction($"Ошибка при записи в файл: {filePath}, {ex.Message}");
        }
    }

    private static void LogAction(string action)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {action}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи в лог: {ex.Message}");
        }
    }

    private static void CopyDirectory(string sourceDirectory, string targetDirectory)
    {
        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        string[] files = Directory.GetFiles(sourceDirectory);
        foreach (var file in files)
        {
            string destinationFile = Path.Combine(targetDirectory, Path.GetFileName(file));
            File.Copy(file, destinationFile);
        }

        string[] directories = Directory.GetDirectories(sourceDirectory);
        foreach (var directory in directories)
        {
            string destinationDirectory = Path.Combine(targetDirectory, Path.GetFileName(directory));
            CopyDirectory(directory, destinationDirectory);
        }
    }
}
