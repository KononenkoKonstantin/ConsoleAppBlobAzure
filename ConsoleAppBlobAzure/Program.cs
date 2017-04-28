using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppBlobAzure
{
    class Program
    {
        static void Main(string[] args)
        {            
            DataManager manager = new DataManager();
            Menu m = new Menu();
            InputManager im = new InputManager();
            im.PathToData = "..\\..\\data\\";
            im.PathToDownloads = "..\\..\\downloads\\";

            do
            {
                m.ShowMenu();
                switch (m.Choice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Создание контейнера\n");
                        im.InputContainerName();
                        manager.CreateContainer(im.ContainerName);
                        if (manager.IsContainerExists(im.ContainerName))
                            Console.WriteLine("Контейнер успешно создан");
                        else
                            Console.WriteLine("Ошибка! Контейнер не был создан");
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Установка общего доступа для контейнера\n");
                        im.InputContainerName();
                        if (manager.IsContainerExists(im.ContainerName))
                        {
                            manager.SetContainerPublicAccess();
                            Console.WriteLine("Для данного контейнера открыт общий доступ");
                        }
                        else
                        {
                            Console.WriteLine("Контейнер не найден");
                        }
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Добавление BLOB-объекта в хранилище Azure");
                        im.InputContainerName();
                        if (manager.IsContainerExists(im.ContainerName))
                        {
                            im.InputBlobName();                          
                            im.InputFileName();
                            manager.AddBlobObject(im.BlobName, im.PathToData, im.FileName);
                            if (manager.GetFileReference(im.ContainerName, im.BlobName))
                                Console.WriteLine("Blob-объект успешно загружен в хранилище");
                            else
                                Console.WriteLine("Ошибка записи");
                        }
                        else
                        {
                            Console.WriteLine("Контейнер не найден");
                        }
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("Перечисление BLOB-объектов в контейнере");
                        im.InputContainerName();
                        if (manager.IsContainerExists(im.ContainerName))
                        {
                            manager.ShowContainer(im.ContainerName);
                        }
                        else
                        {
                            Console.WriteLine("Контейнер не найден");
                        }
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("Скачивание Blob-объектов");
                        im.InputContainerName();
                        if (manager.IsContainerExists(im.ContainerName))
                        {
                            im.InputBlobName();                            
                            manager.DownloadBlobObject(im.ContainerName, im.BlobName, im.PathToDownloads);
                            if (File.Exists(im.PathToDownloads+im.BlobName))
                                Console.WriteLine("Файл успешно сохранен");
                            else
                                Console.WriteLine("Ошибка закачки");
                        }
                        else
                        {
                            Console.WriteLine("Контейнер не найден");
                        }
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine("Чтение текстового Blob-объекта");
                        im.InputContainerName();
                        if (manager.IsContainerExists(im.ContainerName))
                        {
                            im.InputBlobName();
                            try
                            {
                                manager.GetTextBlobObject(im.ContainerName, im.BlobName);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);                                
                            }                 
                               
                        }
                        else
                        {
                            Console.WriteLine("Контейнер не найден");
                        }
                        break;
                    case 7:
                        Console.Clear();
                        Console.WriteLine("Удаление контейнера");
                        im.InputContainerName();
                        if (manager.IsContainerExists(im.ContainerName))
                        {
                            manager.DeleteContainer(im.ContainerName);
                            Console.WriteLine("Контейнер успешно удален");
                        }
                        else
                        {
                            Console.WriteLine("Контейнер не найден");
                        }
                        break;                        
                    case 8:
                        Console.Clear();
                        Console.WriteLine("Удаление Blob-объекта");
                        im.InputContainerName();
                        if (manager.IsContainerExists(im.ContainerName))
                        {
                            im.InputFileName();
                            manager.DeleteBlobObject(im.ContainerName, im.FileName);
                            Console.WriteLine("Объект успешно удален");
                        }
                        else
                        {
                            Console.WriteLine("Контейнер не найден");
                        }
                        break;
                    case 9:
                        Console.Clear();
                        Console.WriteLine("Асинхронное перечисление BLOB-объектов");
                        im.InputContainerName();
                        if (manager.IsContainerExists(im.ContainerName))
                        {                            
                            var task = DataManager.ListBlobsSegmentedInFlatListing(im.ContainerName);
                            task.GetAwaiter();
                        }
                        else
                        {
                            Console.WriteLine("Контейнер не найден");
                        }
                        break;
                    case 10:
                        Console.Clear();
                        Console.WriteLine("Симуляция записи в расширенный Blob-объект");
                        im.InputContainerName();
                        if (manager.IsContainerExists(im.ContainerName))
                        {
                            im.InputBlobName();
                            manager.SimulateAppendBlob(im.ContainerName, im.BlobName);
                        }
                        else
                        {
                            Console.WriteLine("Контейнер не найден");
                        }
                        break;
                    case 11:
                        Console.Clear();
                        m.Finish();
                        Environment.Exit(0);
                        break;        
                    
                    default:
                        Console.WriteLine("Вы ввели некорректное значение");
                        break;
                }
            } while (m.AllowContinue());
        }
    }
}
