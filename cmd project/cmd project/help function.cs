using System;
using System.Collections.Generic;
using System.Text;

namespace cmd_project
{
    class help_function
    {
      public  help_function() {
            Console.WriteLine("cd      - Change the current default directory to . If the argument is not present, report the current directory. If the directory does not exist an appropriate error should be reported.");
            Console.WriteLine("cls     - Clear the screen.");
            Console.WriteLine("dir     - List the contents of directory .");
            Console.WriteLine("quit    - Quit the shell");
            Console.WriteLine("del     - Deletes one or more files.");
            Console.WriteLine("help    - Provides Help information for commands.");
            Console.WriteLine("md      - Creates a directory");
            Console.WriteLine("rd      - Removes a directory.");
            Console.WriteLine("rename  - Renames a file.");
            Console.WriteLine("copy    - Copies one or more files to another location");
            Console.WriteLine("type    - Displays the contents of a text file.");
            Console.WriteLine("import  – import text file(s) from your computer");
            Console.WriteLine("export  – export text file(s) to your computer");
        }
        public help_function(string command)
        {
            if (command == "cd") { Console.WriteLine("Change the current default directory to . If the argument is not present, report the current directory. If the directory does not exist an appropriate error should be reported."); }
            else if (command == "cls") { Console.WriteLine("Clear the screen."); }
            else if (command == "dir") { Console.WriteLine("List the contents of directory ."); }
            else if (command == "quit") { Console.WriteLine("Quit the shell"); }
            else if (command == "del") { Console.WriteLine("Deletes one or more files."); }
            else if (command == "help") { Console.WriteLine("Provides Help information for commands."); }
            else if (command == "md") { Console.WriteLine("Creates a directory"); }
            else if (command == "rd") { Console.WriteLine("Removes a directory."); }
            else if (command == "rename") { Console.WriteLine("Renames a file."); }
            else if (command == "type") { Console.WriteLine("Displays the contents of a text file."); }
            else if (command == "import") { Console.WriteLine("import text file(s) from your computer"); }
            else if (command == "export") { Console.WriteLine("export text file(s) to your computer"); } 
            else if (command == "copy") { Console.WriteLine("Copies one or more files to another location"); } 
            else { Console.WriteLine("This command is not supported by the help utility. "); }
        
        }


        }
}
