using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
namespace cmd_project
{
    class Program
    {
        public static dirctory current_Directory = new dirctory();
        public static void Main(string[] args)
        {
            Virtual_Disk D = new Virtual_Disk();
            D.initialize(@"C:\Users\mm\source\repos\cmd project\cmd project\output.txt");
            string names = String.Concat(Program.current_Directory.file_Name.Where(c => !Char.IsWhiteSpace(c)));
            string a = new string(names);
            //List<string> Date = new List<string>();
            //DateTime aDate = DateTime.Now;
            //string[] fileEntries = Directory.GetFiles(@"E:\cv");
            //List<string> fileImport = new List<string>();


            while (true)
            {
                bool isused = false;


                Console.Write(a + ">");

          

                string command = Console.ReadLine().ToLower();


                string[] splits = command.Split(" ");


                /////////
           
            


                
                ///////////


 

                
               if (splits.Length == 1)
                {

                    if (splits[0] == "help")
                    {
                        help_function help = new help_function();
                    }
                    else if (splits[0] == "quit")
                    {
                        quit_function quit = new quit_function();
                    }
                    else if (splits[0] == "cls" || splits[0] == "clr")
                    {
                        clear_function cls = new clear_function();
                    }
                    else if (splits[0] == "cd")
                    {
                        //    cd_function cd = new cd_function(ref a);
                        Console.WriteLine(Directory.GetCurrentDirectory());
                    }
                    else if (splits[0] == "md")
                    {
                        Console.WriteLine("the syntax of command is incorrect");
                    }
                    else if (splits[0] == "rd")
                    {
                        Console.WriteLine("the syntax of command is incorrect");
                    }
                    else if (splits[0] == "rename")
                    {
                        Console.WriteLine("the syntax of command is incorrect");
                    }
                    else if (splits[0] == "dir")
                    {
                        int count_num_files = 0;
                        int count_num_Directory = 0;
                        int size_of_each_file = 0;
                       // Console.WriteLine(Program.current_Directory.Directory_Table[0].File_Attribute);
                        for (int i = 0; i < Program.current_Directory.Directory_Table.Count; i++)
                        {
                            if (Program.current_Directory.Directory_Table[i].File_Attribute == 1)
                            {
                                Console.WriteLine("         " + Program.current_Directory.Directory_Table[i].file_Size + "  " + new string(Program.current_Directory.Directory_Table[i].file_Name));

                                count_num_files++;
                                size_of_each_file += Program.current_Directory.Directory_Table[i].file_Size;
                            }
                            else if (Program.current_Directory.Directory_Table[i].File_Attribute == 16)
                            {
                                Console.WriteLine("         <DIR>    " + "  " + new string(Program.current_Directory.Directory_Table[i].file_Name));
                                count_num_Directory++;
                            }
                        }
                        Console.WriteLine("                     " + count_num_files + "  File(s)     " + size_of_each_file);
                        Console.WriteLine("                     " + count_num_Directory + "  DIR(s)      " + Fat_Table.get_free_spaces() + "  bytes free");

                    }
                    else
                    {
                        Console.WriteLine("\'" + command + "\'" + " is not recognized as an internal or external command,operable program or batch file.");
                    }
                }
                else if (splits.Length == 2)
                {

                    if (splits[0] == "cd")
                    {
                        if (splits[1].Contains(":"))
                        {
                           
                            cd_function cd = new cd_function(ref a, splits[1]);
                        }
                        else
                        {
                            isused = true;
                            int index = Program.current_Directory.searchDirectory(splits[1]);
                            if (index != -1)
                            {
                                int first = Program.current_Directory.Directory_Table[index].file_first_Cluster;
                                dirctory d = new dirctory(splits[1].ToCharArray(), 0x10, first, 0, Program.current_Directory);
                                Program.current_Directory = d;

                                a = a + String.Concat(d.file_Name.Where(c => !Char.IsWhiteSpace(c)));//بتمسج المسافات
                                Program.current_Directory.Read_Directory();
                            }
                            else
                            {
                                Console.WriteLine("The system cannot find the path specified.");
                            }
                        }
                    }
                    else if (splits[0] == "rd")
                    {
                        if (splits[1].Contains(":"))
                        {

                        }
                        else
                        {
                            isused = true;
                            int index = Program.current_Directory.searchDirectory(splits[1]);
                            if (index != -1)
                            {
                               // Date.RemoveAt(index);
                                int first = Program.current_Directory.Directory_Table[index].file_first_Cluster;
                                dirctory d = new dirctory(splits[1].ToCharArray(), 0x10, first, 0, Program.current_Directory);
                                d.Delete_Directory();
                            }
                            else
                            {
                                Console.WriteLine("The system cannot find the file specified.");
                            }
                        }
                    }
                    else if (splits[0] == "type")
                    {
                        int index = Program.current_Directory.searchDirectory(splits[1]);
                        if (index != -1)
                        {

                            int first = Program.current_Directory.Directory_Table[index].file_first_Cluster;
                            int size = Program.current_Directory.Directory_Table[index].file_Size;
                         
                            string content_txt = File.ReadAllText("E:\\cv\\mina.txt");
                            File_Directory d1 = new File_Directory(splits[1].ToCharArray(), 0x01, first, size, Program.current_Directory, content_txt);
                            d1.Read_File();
                            Console.WriteLine(d1.content);
                           // Console.WriteLine(content_txt);
                        }

                        else
                        {
                            Console.WriteLine("The system cannot find the file specified.");
                        }



                    }

                    else if (splits[0] == "import")
                    {
                        if (File.Exists(splits[1]))
                        {
                            string name_txt = Path.GetFileName(splits[1]);
                            string content_txt = File.ReadAllText(splits[1]);
                            int size_txt = content_txt.Length;
                            int index = Program.current_Directory.searchDirectory(name_txt);
                            if (index == -1)
                            {
                                if (size_txt > 0)
                                {
                                    Program.current_Directory.file_first_Cluster = Fat_Table.getavailable_Block();
                                }
                                else { }
                                File_Directory d = new File_Directory(name_txt.ToCharArray(), 0x01, 0, size_txt, Program.current_Directory, content_txt);
                                d.Write_File();
                                Directory_Entry d1 = new Directory_Entry(name_txt.ToCharArray(), 0x01, 0, size_txt);
                                Program.current_Directory.Directory_Table.Add(d1);
                                Program.current_Directory.Write_Directory();
                            }

                        }
                        else
                        {

                        }

                    }

                    else if (splits[0] == "del")
                    {
                        if (splits[1].Contains(":"))
                        {

                        }
                        else
                        {
                            int index = Program.current_Directory.searchDirectory(splits[1]);
                            if (index != -1)
                            {
                                if (Program.current_Directory.Directory_Table[index].File_Attribute == 1)
                                {
                                    int first = Program.current_Directory.Directory_Table[index].file_first_Cluster;
                                    int size = Program.current_Directory.Directory_Table[index].file_Size;
                                    string content = "";
                                    File_Directory d1 = new File_Directory(splits[1].ToCharArray(), 0x01, first, size, Program.current_Directory, content);
                                    d1.Delete_file();

                                }

                            }
                            else
                            {
                                Console.WriteLine("The system cannot find the file specified.");
                            }
                        }
                    }

                    else if (splits[0] == "md")
                    {
                        if (splits[1].Contains(":"))
                        {  //  string dir = @"C:\test\Aaron";
                            if (!Directory.Exists(splits[1]))
                            {
                                Directory.CreateDirectory(splits[1]);
                            }
                        }
                        else if (!splits[1].Contains(":"))
                        {
                            isused = true;
                            if (Program.current_Directory.searchDirectory(splits[1]) == -1)
                            {
                                //Console.WriteLine(Program.current_Directory.Directory_Table[0].File_Attribute);
                                Directory_Entry d = new Directory_Entry(splits[1].ToCharArray(), 0x10, 0, 0);
                                //Console.WriteLine(Program.current_Directory.Directory_Table[0].File_Attribute);
                                Program.current_Directory.Directory_Table.Add(d);
                                //Console.WriteLine(Program.current_Directory.Directory_Table[0].File_Attribute);
                                Program.current_Directory.Write_Directory();
                                if (Program.current_Directory.parent != null)
                                {
                                    Program.current_Directory.parent.Update(Program.current_Directory.getDirector_entry());
                                    Program.current_Directory.parent.Write_Directory();
                                }
                                //DateTime aDate2 = DateTime.Now;
                             //   Date.Add(aDate2.ToString("MM / dd / yyyy  HH:mm  tt"));

                            }

                            else
                            {
                                Console.WriteLine("A subdirectory or file " + splits[1] + " is already exists.");
                            }
                        }
                    }
                    else if (splits[0] == "help")
                    {
                        isused = true;
                        help_function help = new help_function(splits[1]);
                    }


                    else if (isused == false)
                    {
                        Console.WriteLine(splits[0] + " is not recognized as an internal or external command,operable program or batch file.");
                    }
                }

                else if (splits.Length > 2)
                {

                    if (splits[0] == "rename")
                    {
                        if (splits[1].Contains(":"))
                        {
                            cd_function cd = new cd_function(ref a, splits[1]);
                        }
                        else
                        {
                            int index = Program.current_Directory.searchDirectory(splits[1]);
                            int index2 = Program.current_Directory.searchDirectory(splits[2]);

                            if (index != -1)
                            {
                                if (index2 == -1)
                                {
                                    Directory_Entry d1 = Program.current_Directory.Directory_Table[index];

                                    d1.file_Name = splits[2].ToCharArray();
                                    //  Program.current_Directory.Directory_Table.RemoveAt(index);

                                }
                                else
                                {
                                    Console.WriteLine("A subdirectory or file " + splits[2] + " is already exists.");
                                }

                            }
                            else
                            {

                                Console.WriteLine("The system cannot find the file specified.");
                            }
                        }
                    }
                    else if (splits[0] == "export")
                    {
                        int index = Program.current_Directory.searchDirectory(splits[1]);
                        if (index != -1)
                        {
                            if (System.IO.Directory.Exists(splits[2]))
                            {
                                int first = Program.current_Directory.Directory_Table[index].file_first_Cluster;
                                int size = Program.current_Directory.Directory_Table[index].file_Size;
                                string content = "";
                                File_Directory d1 = new File_Directory(splits[1].ToCharArray(), 0x01, first, size, Program.current_Directory, content);
                                d1.Read_File();
                                StreamWriter write = new StreamWriter(splits[2] + "\\" + splits[1]);
                                write.Write(d1.content);
                                write.Flush();
                                write.Close();
                            }
                            else
                            {
                                Console.WriteLine("The system cannot find the file specified.");
                            }
                        }

                        else
                        {
                            Console.WriteLine("The system cannot find the file specified.");
                        }


                    }


                    else if (splits[0] == "copy")
                    {
                        //    Console.WriteLine("Done");
                        int index = Program.current_Directory.searchDirectory(splits[1]);
                        if (index != -1)
                        {
                            int indexs = Program.current_Directory.searchDirectory(splits[2]);
                            if (System.IO.Directory.Exists(splits[2]))
                            {
                                if (Program.current_Directory.ToString() != splits[2])
                                {
                                    //cd_copy(splits[2]);
                                    //import_copy("E:\\cv\\mina.txt");
                                    
                                    char ask;
                                    
                                    Console.WriteLine("Do you want to overide (y/n)");
                                    ask = Convert.ToChar(Console.ReadLine());
                                    if (ask == 'y')
                                    {//if 
                                        int first = Program.current_Directory.Directory_Table[index].file_first_Cluster;
                                        int size = Program.current_Directory.Directory_Table[index].file_Size;
                                  

                                        Directory_Entry d1 = new Directory_Entry(splits[1].ToCharArray(), 0x01, first, size);
                                    
                                        Program.current_Directory.Directory_Table.Add(d1);

                                        Program.current_Directory.Write_Directory();
                                        
                                      

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("The system cannot find the file specified.");
                                }
                            }

                            else
                            {
                                Console.WriteLine("The system cannot find the file specified.");
                            }

                        }


                    }


                }

            }

                
            
        }
    }
}