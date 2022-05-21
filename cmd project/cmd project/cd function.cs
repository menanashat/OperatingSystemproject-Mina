using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace cmd_project
{
    class cd_function
    {
        public cd_function(ref string cd)
        {
            string str = cd.Remove(cd.LastIndexOf('\\'));
            cd = str;
        }
        public  cd_function(ref string cd, string newpath)
        {
            //string str = cd.Remove(cd.LastIndexOf('\\'));
            Virtual_Disk.initialize(@"C:\Users\mm\source\repos\cmd project\cmd project\output.txt");
            string names = String.Concat(Program.current_Directory.file_Name.Where(c => !Char.IsWhiteSpace(c)));
            cd = new string(names);


        }
        public void cd_function2(ref string cd, string newpath)
        {
            //string str = cd.Remove(cd.LastIndexOf('\\'));
            int index = Program.current_Directory.searchDirectory(newpath);
            if (index != -1)
            {
                int first = Program.current_Directory.Directory_Table[index].file_first_Cluster;
                dirctory d = new dirctory(newpath.ToCharArray(), 0x10, first, 0, Program.current_Directory);
                Program.current_Directory = d;

                cd = cd + String.Concat(d.file_Name.Where(c => !Char.IsWhiteSpace(c)));//بتمسج المسافات
               Program.current_Directory.Read_Directory();
            }
            else
            {
                Console.WriteLine("The system cannot find the path specified.");
            }

        }
        public cd_function()
        {

        }


    }
}
