using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace cmd_project
{
    class Virtual_Disk:dirctory
    {
       
        static public string file = @"C:\Users\mm\source\repos\cmd project\cmd project\output.txt";
        public void initialize(string path)
        {
            
            if (File.Exists(path))
            {
             
                Fat_Table.read_FAT();
                
                dirctory root = new dirctory("H:\\".ToCharArray(),0x10,5,0,null);
                root.Read_Directory();
                Program.current_Directory = root;
            
            }
            else
            {
                using FileStream fs = File.Create("output.txt");

                //SuperBlock

                char[] writesupercluster = new char[1024];
                for (int i = 0; i < 1024; i++)
                {
                    writesupercluster[i] = '0';
                }
                using (StreamWriter writer = File.AppendText((file)))
                {
                    foreach (char ln in writesupercluster)
                    {
                        writer.Write(ln);
                    }
                }

                //  MetaSystem
                char[] writeMetaSystem = new char[4096];
                for (int i = 0; i < 4096; i++)
                {
                    writeMetaSystem[i] = '*';
                }
                using (StreamWriter writer = File.AppendText((file)))
                {
                    foreach (char ln in writeMetaSystem)
                    {
                        writer.Write(ln);
                    }
                }
                //content
                char[] writeContent = new char[1024 * 1019];
                for (int i = 0; i < 1024 * 1019; i++)
                {
                    writeContent[i] = '#';
                }
                using (StreamWriter writer = File.AppendText((file)))
                {
                    foreach (char ln in writeContent)
                    {
                        writer.Write(ln);
                    }
                }
                Fat_Table.prepare_FAT();


                dirctory root = new dirctory("H:\\".ToCharArray(), 0x10, 5, 0, null) ;
       

                root.Write_Directory();
                
                Fat_Table.Write_FAT();

                Program.current_Directory = root;

               
            }

        }
        static public void Write_Cluster(int Cluster_index, byte[] bytes)
        {
            using (FileStream writer = File.OpenWrite((file)))
            {
                writer.Seek(Cluster_index * 1024, SeekOrigin.Begin);
                foreach (char ln in bytes)
                {
                    writer.WriteByte((byte)ln);

                }
                writer.Flush();
                writer.Close();
            }

        }
        static public byte[] read_Cluster(int Cluster_index)
        {
            byte[] a = new byte[1024];
            using (FileStream reader = new FileStream(file, FileMode.Open, FileAccess.Read))
            {

                reader.Seek(Cluster_index * 1024, SeekOrigin.Begin);
                reader.Read(a, 0, 1024);
                return a;
            }
        }

            




    }
}
