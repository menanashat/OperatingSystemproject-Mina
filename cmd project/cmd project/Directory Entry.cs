using System;
using System.Collections.Generic;
using System.Text;

namespace cmd_project
{
    public class Directory_Entry
    {

        public char[] file_Name = new char[11];
        public byte File_Attribute;//0x0 file    0x01 floder     0x01     0x10
        public char[] File_empty = new char[12];
        public int file_first_Cluster;
        public int file_Size ;

        public Directory_Entry(char[] file_Names, byte File_Attribute, int file_first_Cluster,int file_Sizes)
        {
            // File_Attribute if . file 
            if (file_Names.Length < 11)
            { 
                for (int i = 0; i < file_Names.Length; i++)
                {
                    file_Name[i] = file_Names[i];
                }
                for (int i = file_Names.Length; i < 11; i++)
                {
                    if (i == file_Names.Length)
                    {

                        file_Name[i] = '\0';
                    }
                    else
                    {
                        file_Name[i] = ' ';
                    }
                }
            }
            else
            {
                for (int i = 0; i < 11; i++)
                {
                    file_Name[i] = file_Names[i];
                }
            }
            //this.file_Name = file_Name;
            this.File_Attribute = File_Attribute;
            this.file_first_Cluster = file_first_Cluster;
            this.file_Size = file_Sizes;
        }
        public Directory_Entry() { }

        public byte[] getbytes()
        {
            byte[] b = new byte[32];
            for (int i = 0; i <= 10; i++)
            {
                b[i] = Convert.ToByte(file_Name[i]);
            }

            b[11] = Convert.ToByte(File_Attribute);


            for (int i = 12, j = 0; i < 24; i++, j++)
            {
                b[i] = Convert.ToByte(File_empty[j]);
            }


            for (int i = 24; i < 28; i++)
            {
                b[i] = Convert.ToByte(file_first_Cluster);
            }
            byte[] t = new byte[4];
            t=BitConverter.GetBytes(file_Size);

            for (int i = 28, j = 0; i < 32; i++, j++)
            {
                b[i] = t[j];
            }
           
            return b;

        }
        public Directory_Entry getDirector_entry(byte[] b2)
        {
            Directory_Entry gh = new Directory_Entry();

            for (int i = 0; i <= 10; i++)
            {
                gh.file_Name[i] = Convert.ToChar(b2[i]);

            }

           gh. File_Attribute = b2[11];

            for (int i = 12; i < 24; i++)
            {
                gh.File_empty[i-12] ='0';
            }
            byte[] b = new byte[4];
            for (int i = 24; i < 28; i++)
            {
                b[i-24]=b2[i];

            }
            gh.file_first_Cluster = BitConverter.ToInt32(b);
            for (int i = 28; i < 32; i++)
            {
                b[i - 28] = b2[i];
            }

            gh.file_Size = BitConverter.ToInt32(b);
            return gh;
        }
        public Directory_Entry getDirector_entry()
        {
            Directory_Entry gh = new Directory_Entry();

            gh.file_Name =this.file_Name;
            gh.File_Attribute = this.File_Attribute;
            gh.File_empty = this.File_empty;
            gh.file_first_Cluster = this.file_first_Cluster;
            gh.file_Size = this.file_Size;
            return gh;
        }












    }
}
