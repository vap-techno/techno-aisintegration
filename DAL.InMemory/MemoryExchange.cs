using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace DAL.InMemory
{
    public struct MmContent
    {
        public long Ticks;
        public int Size;
        public string Content;
    }


    public class MemoryExchange : IDisposable
    {
        private const string CMD_FILENAME = "Cmd.json";
        private const long FILE_SIZE = 16386;
        private const string RESPONSE_FILENAME = "Response.json";

        private readonly string _fileName;

        private readonly MemoryMappedFile _sharedMemory;
        private readonly MemoryMappedViewAccessor _readAccessor;
        private readonly MemoryMappedViewAccessor _writeAccessor;

        public MemoryExchange(MemoryFileName mFile)
        {
            if (mFile == MemoryFileName.Cmd)
            {
                _fileName = CMD_FILENAME;
            }
            else if (mFile == MemoryFileName.Response)
            {
                _fileName = RESPONSE_FILENAME;
            }

            _sharedMemory = MemoryMappedFile.CreateOrOpen(_fileName, FILE_SIZE,MemoryMappedFileAccess.ReadWrite);
            _readAccessor = _sharedMemory.CreateViewAccessor(0, FILE_SIZE, MemoryMappedFileAccess.Read);
            _writeAccessor = _sharedMemory.CreateViewAccessor(0, FILE_SIZE, MemoryMappedFileAccess.ReadWrite);

        }

        /// <summary>
        /// Записать значение в память
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool Write(string content)
        {
            try
            {
                //запись в разделяемую память
                //запись размера с нулевого байта в разделяемой памяти

                // TODO: Нужен логгер
                Console.WriteLine($"В файл inMemory {_fileName} записываем текст длинной {content.Length}");
                Console.WriteLine(content.ToCharArray());

                // Кодируем строку в байтовый массив
                var buffer = Encoding.UTF8.GetBytes(content);

                // Очищаем перед записью память
                _writeAccessor.Flush();

                // Записываем в первые 2 байта размер буфера
                _writeAccessor.Write(0, (int)buffer.Length);

                // Записываем в следующие 8 байт время в тиках
                _writeAccessor.Write(4, DateTime.Now.Ticks);

                // Записываем контент
                _writeAccessor.WriteArray(12, buffer, 0, buffer.Length);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }
        }

        /// <summary>
        /// Прочитать значение из памяти
        /// </summary>
        /// <returns></returns>
        public MmContent Read()
        {
            try
            {
                MmContent resp;

                // Размер контента находится в первых 4х байтах
                resp.Size = _writeAccessor.ReadInt32(0);

                // Метка времени находится в последующих 8 байтах
                resp.Ticks = _writeAccessor.ReadInt64(4);

                // TODO: Нужен логгер
                //Console.WriteLine(
                //    $"В буфере {_fileName} находится {resp.Size} байт, Метка времени {resp.Ticks} ");

                // Считываем байтовый массив контента
                byte[] buffer = new byte[resp.Size];
                _writeAccessor.ReadArray(12, buffer, 0, buffer.Length);

                // Декодируем в строку
                resp.Content = Encoding.UTF8.GetString(buffer);

                // TODO: Нужен логгер
                //Console.WriteLine(resp.Content);

                return resp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
                return new MmContent();
            }
        }

        public void Dispose()
        {
            _sharedMemory?.Dispose();
            _readAccessor?.Dispose();
            _writeAccessor?.Dispose();
        }
    }
}