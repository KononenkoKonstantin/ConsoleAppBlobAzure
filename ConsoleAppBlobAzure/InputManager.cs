using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppBlobAzure
{
    public class InputManager
    {
        public string ContainerName { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }
        public string PathToData { get; set; }
        public string PathToDownloads { get; set; }

        public InputManager()
        {
                
        }

        public void InputContainerName()
        {
            Console.Write("Введите имя контейнера: ");
            ContainerName = Console.ReadLine().ToString();
        }

        public void InputBlobName()
        {
            Console.Write("Введите имя Blob-объекта: ");
            BlobName = Console.ReadLine();
        }

        public void InputFileName()
        {
            Console.Write("Введите имя файла: ");
            FileName = Console.ReadLine();
        }
        
    }

    
}
