using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppBlobAzure
{
    public class Menu
    {
        int choice;
        string ans;
        public int Choice { get { return choice; } }

        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("==============================================");
            Console.WriteLine("1 - Создать контейнер                         ");
            Console.WriteLine("2 - Установить общий доступ для контейнера    ");
            Console.WriteLine("3 - Добавить BLOB-объект в хранилище Azure    ");
            Console.WriteLine("4 - Перечисление BLOB-объектов в контейнере   ");
            Console.WriteLine("5 - Скачать BLOB-объект                       ");
            Console.WriteLine("6 - Прочитать текстовый BLOB-объект           ");
            Console.WriteLine("7 - Удалить контейнер                         ");
            Console.WriteLine("8 - Удалить BLOB-объект                       ");
            Console.WriteLine("9 - Загрузить список BLOB-объектов асинхронно ");
            Console.WriteLine("10- Симуляция записи в расширенный Blob-объект");            
            Console.WriteLine("11- Выход                                     ");
            Console.WriteLine("==============================================");
            Console.Write("Ваш выбор: ");
            try
            {
                choice = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0}", ex.Message);
                Console.ResetColor();
            }
        }

        public void Finish()
        {
            Console.Clear();
            Console.WriteLine("=============================================");
            Console.WriteLine("          Работа программы завершена         ");
            Console.WriteLine("=============================================");
            Console.WriteLine("\n");
        }

        public bool AllowContinue()
        {
            Console.Write("Продолжить? (y/n): ");
            try
            {
                ans = Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0}", ex.Message);
                Console.ResetColor();
            }
            return (ans == "y");
        }
    }
}

