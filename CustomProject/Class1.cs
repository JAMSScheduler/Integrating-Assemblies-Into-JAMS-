using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace CustomProject
{
    public static class DllMethods
    {
        //CreateFileA appears in comboBox
        public static void CreateFileA(string fileName, string directory)
        {
            File.Create(Path.Combine(directory, fileName));
        }
        //CreateFileB appears in combobox
        public static void CreateFileB(string fileName, string directory)
        {
            File.Create(Path.Combine(directory, fileName));
        }
    }
}
