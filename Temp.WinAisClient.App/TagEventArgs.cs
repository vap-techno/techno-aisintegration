﻿using System;
using Hylasoft.Opc.Common;

namespace Temp.WinAisClient.App
{
    /// <summary>
    /// Аргументы события по изменению значения тэга
    /// </summary>
    /// <typeparam name="T"> Тип данных тэга </typeparam>
    public class TagEventArgs<T> : EventArgs
    {
        public TagEventArgs(string name, ReadEvent<T> tag)
        {
            Tag = tag;
            Name = name;
        }

        public string Name { get; }
        public ReadEvent<T> Tag { get; }

    }
}