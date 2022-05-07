using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace cmd_project
{
    class Fat_Table : Virtual_Disk
    {
        static public int[] FAT = new int[1024];
        static public int co2 = 10;
        static public void prepare_FAT()
        {
            for (int i = 0; i < 1024; i++)
            {
                if (i <= 4)
                {
                    FAT[i] = -1;
                }
                else
                {
                    FAT[i] = 0;
                }

            }

        }
        
        public static byte[] b = new byte[4096];
        static public void Write_FAT()
        {
           //int 
            System.Buffer.BlockCopy(FAT, 0, b, 0, b.Length);//convert from int to byte
            using (FileStream fs = new FileStream(@"C:\Users\mm\source\repos\cmd project\cmd project\output.txt",FileMode.OpenOrCreate, FileAccess.Write))      
            {
                fs.Seek(1024, SeekOrigin.Begin); 
                fs.Write(b, 0, b.Length);
            }
        }

        static public void read_FAT()
        {
            Fat_Table.prepare_FAT();
            FileStream fs = new FileStream(@"C:\Users\mm\source\repos\cmd project\cmd project\output.txt", FileMode.Open);
            byte[] c = new byte[4096];
            fs.Seek(1024, SeekOrigin.Begin);
            fs.Read(c, 0, c.Length);
            Buffer.BlockCopy(c, 0, FAT, 0, c.Length);
            fs.Close();

        }
        static public int getavailable_Block()
        {
            int i = 0;
            for (i = 0; i < 1024; i++)
            {
                if (FAT[i] == 0)
                {
                    break;
                }

            }
            return i;

        }

        static public void set_Next_Block(int index, int next)
        {
            FAT[index] = next;
        }


        static public int get_Next_Bloack(int index)
        {
            return FAT[index];
        }

        static public int get_available_Blocks()
        {
            int counter = 0;
            for (int i = 0; i < 1024; i++)
            {
                if (FAT[i] == 0)
                {
                    counter++;
                }
            }   
            return counter;
        }
        static public int get_free_spaces() {
            int co = get_available_Blocks()-co2;
            co2++;
            return co * 1024;
        }







    }

}