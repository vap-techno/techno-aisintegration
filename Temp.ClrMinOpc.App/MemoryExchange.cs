using System;
using System.IO.MemoryMappedFiles;
using System.IO;

namespace Temp.ClrMinOpc.App
{
    public class MemoryExchange
    {
        private const string cmdFile = "Cmd.json";
        private const long fileSize = 4096;
        private const string respFile = "Response.json";


        /// <summary>
        /// Пишет команду в shared-память
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool WriteCmd(string json)
        {
            try
            {
                MemoryMappedFile sharedMemory = MemoryMappedFile.CreateOrOpen(cmdFile, fileSize);
                //Создаем объект для записи в разделяемый участок памяти
                using (MemoryMappedViewAccessor acessor = sharedMemory.CreateViewAccessor())
                {
                    //запись в разделяемую память
                    //запись размера с нулевого байта в разделяемой памяти
                    acessor.WriteArray<char>(0,json.ToCharArray(),0,json.Length);
               }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public string ReadResponse()
        {
            try
            {
                string json;

                MemoryMappedFile sharedMemory = MemoryMappedFile.CreateOrOpen(respFile, fileSize);
                //Создаем объект для записи в разделяемый участок памяти
                using (var stream = sharedMemory.CreateViewStream())
                {
                    using (BinaryReader binReader = new BinaryReader(stream))
                    {
                        var jsonBytes = binReader.ReadBytes((int)stream.Length);
                        json = System.Text.Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
                    }
                }

                WriteCmd("");

                return json;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }

    }
}