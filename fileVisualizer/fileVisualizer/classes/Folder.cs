using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileVisualizer.classes
{
    
    public class Folder : Container
    {
        private string name;
        private string path;
        private int level;
        private long size;
        private List<Container> children = new List<Container>();
        public Folder(string name, string path , int level) 
        {
            this.name = name;
            this.path = path;
            this.level = level;
            addChilds();
            calculateSize();
        }

        public void addChilds() {
            DirectoryInfo selectedFolderInfo = new DirectoryInfo(path);
            string[] files = Directory.GetFiles(path);
            string[] subfolders = Directory.GetDirectories(path);

            for (int i = 0; i < files.Length; i++) {
                string currentPath = files[i];
                FileInfo currentFile = new FileInfo(currentPath);
                string currentName = currentFile.Name;
                long currentSize = currentFile.Length;
                string currentExt = currentFile.Extension;

                File file = new File(currentName, currentExt, currentSize, currentPath, level + 1);

                children.Add(file);
            }

            for(int i = 0; i < subfolders.Length;i++) {
                DirectoryInfo currentFolder = new DirectoryInfo(subfolders[i]);
                
                string folderName = currentFolder.Name;
                Container folder = new Folder(folderName, subfolders[i], level +1);

                children.Add(folder);
            }
        }

        public override void print() {
            string indentChar = "-";
            string indent = string.Join("", Enumerable.Repeat(indentChar, level));
            System.Console.WriteLine(indent + name + ":" + size);
            for (int i = 0; i < children.Count; i++) { 
               Container child = children[i];
               child.print();
            }
        }
        public void setSize(long size) {
            this.size = size;
        }
        public void calculateSize() {
            DirectoryInfo selectedFolderInfo = new DirectoryInfo(path);
            long totalSize = selectedFolderInfo.EnumerateFiles().Sum(f => f.Length);
            size = totalSize;

            for (int i = 0; i < children.Count; i++) {
                if (children[i] is Folder) { 
                    ((Folder)children[i]).calculateSize();

                }
            }
        }
        public List<Container> getChildren() {
            return children;
        }
        public override string getName()
        {
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

        public int getALLChildsCount() {
            int sum = 0;
            
            for(int i = 0; i< children.Count; i++)
            {
                sum += countChilds(children[i]);
            }


            return sum;
        }
        private int countChilds(Container cont) {
            if (cont is File)
            {
                return 1;
            }
            else if (cont is Folder)
            {
                Folder currentFold = (Folder)cont;
                List<Container> childs = currentFold.getChildren();
                int sum = 1;
                for (int i = 0; i < childs.Count; i++)
                {
                    sum +=  countChilds(childs[i]);
                }
                return sum;
            }
            else return 0;
        
        }


    }

}

