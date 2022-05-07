using System;
using System.Collections.Generic;
using System.Text;

namespace cmd_project
{
    class cd_function
    {
     public   cd_function(ref string cd) {
            string str = cd.Remove(cd.LastIndexOf('\\'));
            cd = str;
        }
        public   cd_function(ref string cd,string newpath) {
            //string str = cd.Remove(cd.LastIndexOf('\\'));
            
            cd = newpath.ToUpper();
        }
        public cd_function() {
           
        }


    }
}
