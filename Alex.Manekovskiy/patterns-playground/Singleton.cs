using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patterns_playground
{
    public class Singleton
    {
        private static readonly Singleton instance;
        public static Singleton Instance { get { return instance; } }

        private int doCounter = 0;

        static Singleton()
        {
            instance = new Singleton();
        }

        private Singleton()
        { }

        public void Do()
        {
            doCounter++;
            Console.WriteLine("Singleton.Do:: doCounter = {0}", doCounter);
        }
    }
}
