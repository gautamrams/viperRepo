using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ClassLibrary1
{
    public class Class1
    {

        [DllImport("cDll.dll")]
        public static extern void writeToFile(int num);

        [DllImport("cDll.dll")]
        public static extern int getFileDate();

        [DllImport("cDll.dll")]
        public static extern void addDb();

        [DllImport("cDll.dll")]
        public static extern int startApp();

        [DllImport("cDll.dll")]
        public static extern void updateDb();

        [DllImport("cDll.dll")]
        public static extern int getInfo();

        public void executeDll(int myId)
        {
            if (getInfo()!=0)
            {
                if (getFileDate() == 0)
                {
                    addDb();
                }
                else if (startApp() != 0)
                    updateDb();
                    writeToFile(myId);
            }
        }
    }
}
