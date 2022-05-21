using System;
using System.Collections.Generic;
using System.Text;

namespace cmd_project
{
    public class dirctory : Directory_Entry
    {    //directoryEntry  list
        public dirctory parent;
        public List<Directory_Entry> Directory_Table;

        public dirctory(char[] file_Name, byte File_Attribute, int file_first_Cluster, int file_sizes, dirctory parent) : base(file_Name, File_Attribute, file_first_Cluster, file_sizes)
        {

            if (parent != null)
            {
                this.parent = parent;
            }
            Directory_Table = new List<Directory_Entry>();
        }
        public dirctory()
        {

        }

        //big array



        public void Write_Directory()
        {
              byte[] Direcctory_Entry_Byte = new byte[32];
            byte[] Directory_table_bytes = new byte[32 * Directory_Table.Count];

            for (int i = 0; i < Directory_Table.Count; i++)
            {
                Direcctory_Entry_Byte = Directory_Table[i].getbytes();
                for (int j = i * 32, c = 0; c < 32; c++, j++)
                {
                    Directory_table_bytes[j] = Direcctory_Entry_Byte[c];
                }
            }

            double Num_of_Required_Blocks = System.Math.Ceiling(Directory_table_bytes.Length / 1024.0);

            int NUM_of_Full_size_Blocks = Directory_table_bytes.Length / 1024;

            int reminder = Directory_table_bytes.Length % 1024;

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
                    Fat_Index = Fat_Table.getavailable_Block();
                    file_first_Cluster = Fat_Index;
                }
                    List<byte[]> LS = new List<byte[]>();
                   
                    byte[] b = new byte[1024];
                    for (int i = 0; i < LS.Count; i++)
                    {
                        for (int j = i * 1024, c = 0; c < 1024; j++, c++)
                        {
                            b[c] = Directory_table_bytes[j];
                        }
                        LS.Add(b);
                    }

                    byte[] b2 = new byte[1024];
              
                if (reminder > 0)
                {
                    for (int i = NUM_of_Full_size_Blocks * 1024, c = 0; c < reminder; i++, c++)
                    {
                        b2[c] = Directory_table_bytes[i];
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
                if (Directory_Table.Count == 0)
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
        public void Read_Directory()
        {
             Directory_Table=new List<Directory_Entry>();
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
                byte[] b = new byte[32];
               
                for (int i = 0; i < ls.Count; i++)
                {
                    b[i % 32] = ls[i];
                    if ((i + 1) % 32 == 0) {
                        Directory_Entry d = this.getDirector_entry(b);
                        if (d.file_Name[0] != '\0') {
                            Directory_Table.Add(d);
                        }
                    }
                }
            }
        }
        public int searchDirectory(string name)
        {
            Read_Directory();
            if (name.Length < 11)
            {
                name += "\0";
                for (int i = name.Length + 1; i < 12; i++)
                {
                    name += " ";
                }
            }
            else
            {
                name = name.Substring(0, 11);
            }
            
                for (int i = 0; i < Directory_Table.Count; i++)
                {
                    string n = new string(Directory_Table[i].file_Name);
                    if (n == name)
                    {
                        return i;
                    }
                }
            
            return -1;

        }
        public void Update(Directory_Entry d)
        {
            Read_Directory();
            string name = new string(d.file_Name);
            int index = searchDirectory(Convert.ToString(name));
            if (index != -1)
            {
                Directory_Table.RemoveAt(index);
            }
            Directory_Table.Insert(index, d);
            Write_Directory();
        }
        public void Delete_Directory()
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

