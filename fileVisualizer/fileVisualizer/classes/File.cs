using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace fileVisualizer.classes
{
    public class File :  Container
    {
        private int level;
        private string path;
        private string name;
        private string ext;
        private long size;
        public File(string name, string ext, long size, string path, int level) 
        {       
            this.name = name;
            this.level = level;
            this.path = path;
            this.ext = ext;
            this.size = size;
           
        }

        public string getExt()
        {
            return ext;
        }
        public override void print() {
            string indentChar = "-";
            string indent = string.Join("", Enumerable.Repeat(indentChar, level));
           
            System.Console.WriteLine(indent +this.name + size);
 
        }
        public override string getName() {
            return name;
        }
        public override long getSize()
        {
            return size;
        }
        public override int getLevel()
        {
            return level;
        }
    }
}
