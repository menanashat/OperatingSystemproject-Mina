using System;
using System.Collections.Generic;
using System.Text;

namespace cmd_project
{
    class File_Directory : Directory_Entry
    {
        public dirctory parent;
        public string content;
        byte[] bytes;
        public File_Directory() { }
        public File_Directory(char[] file_Names, byte File_Attribute, int file_first_Cluster, int file_Sizes, dirctory parent, string contents) : base(file_Names, File_Attribute, file_first_Cluster, file_Sizes)
        {
            if (parent != null)
            {
                this.parent = parent;
            }
            content = contents;
            bytes = Encoding.ASCII.GetBytes(content);
        }

        public void Write_File()
        {
           
            double Num_of_Required_Blocks = System.Math.Ceiling(bytes.Length / 1024.0);

            int NUM_of_Full_size_Blocks = bytes.Length / 1024;

            int reminder = bytes.Length % 1024;

            if (Num_of_Required_Blocks <= Fat_Table.get_available_Blocks())
            {
                int Fat_Index;
                int last_Index = -1;
                if (file_first_Cluster != 0)
                {//5 root
                    Fat_Index = file_first_Cluster;
                }
                else
                {
                    //each 1024 byte    
                    Fat_Index = Fat_Table.getavailable_Block();
                    file_first_Cluster = Fat_Index;
                }
                    List<byte[]> LS = new List<byte[]>();


                byte[] b = new byte[1024];
                for (int i = 0; i < LS.Count; i++)
                {
                    for (int j = i * 1024, c = 0; c < 1024; j++, c++)
                    {
                        b[c] = bytes[j];
                    }
                    LS.Add(b);
                }

                byte[] b2 = new byte[1024];

                if (reminder > 0)
                {
                    for (int i = NUM_of_Full_size_Blocks * 1024, c = 0; c < reminder; i++, c++)
                    {
                        b2[c] = bytes[i];
                    }
                    LS.Add(b2);
                }

                for (int i = 0; i < LS.Count; i++)
                {

                    Virtual_Disk.Write_Cluster(Fat_Index, LS[i]);
                    Fat_Table.set_Next_Block(Fat_Index, -1);
                    if (last_Index != -1)
                    {
                        Fat_Table.set_Next_Block(last_Index, Fat_Index);
                    }
                    last_Index = Fat_Index;
                    Fat_Index = Fat_Table.getavailable_Block();
                }
                if (bytes.Length == 0)
                {
                    if (file_first_Cluster != 0)
                    {
                        Fat_Table.set_Next_Block(file_first_Cluster, 0);
                        file_first_Cluster = 0;


                    }
                }
                Fat_Table.Write_FAT();

            }
        }
        public void Read_File()
        {
           string c = string.Empty;
          
            if (file_first_Cluster != 0 && Fat_Table.get_Next_Bloack(file_first_Cluster) != 0)
            {
                List<byte> ls = new List<byte>();
                int Fat_index = file_first_Cluster;
                int next = Fat_Table.get_Next_Bloack(Fat_index);
                do
                {
                    byte[] a = new byte[1024];
                    a = Virtual_Disk.read_Cluster(Fat_index);
                    ls.AddRange(a);
                    Fat_index = next;
                    if (Fat_index != -1)
                    {
                        next = Fat_Table.get_Next_Bloack(Fat_index);
                    }
                } while (next != -1);
                // byte[] b = new byte[32];
               
                for (int i = 0; i < ls.Count; i++)
                {
                    if (Convert.ToChar((ls[i])) != '\0')
                    {
                        c += (Convert.ToChar((ls[i])));
                    }
                }
              content = c;
            }
        }
        //}

        public void Delete_file()
        {
            int Fat_index = file_first_Cluster;
            if (file_first_Cluster != 0)
            {
                int next = Fat_Table.get_Next_Bloack(Fat_index);
                do
                {
                    Fat_Table.set_Next_Block(Fat_index, 0);
                    Fat_index = next;
                    if (Fat_index != -1)
                    {
                        next = Fat_Table.get_Next_Bloack(Fat_index);
                    }

                } while (next != -1);
            }
            if (parent != null)
            {
                parent.Read_Directory();
                string s = string.Join("", file_Name);

                Fat_index = parent.searchDirectory(s);

                if (Fat_index != -1)
                {

                    parent.Directory_Table.RemoveAt(Fat_index);
                    parent.Write_Directory();
                }

                Fat_Table.Write_FAT();
            }

        }

    }





}
