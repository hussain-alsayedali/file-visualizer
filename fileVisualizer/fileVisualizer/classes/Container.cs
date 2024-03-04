using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileVisualizer.classes
{
    public abstract class Container
    {

        public Container()
        {

        }



        public abstract void print();
        public abstract string getName();
        public abstract long getSize();
        public abstract int getLevel();
    }
}
