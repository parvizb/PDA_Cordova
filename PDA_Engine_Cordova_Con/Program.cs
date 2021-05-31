using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace PDA_Engine_Cordova_Con
{

    class Program
    {
      public  static string AppPath = "";
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("PDA-Engine Cordova App Builder 0.2 beta ");
                Console.WriteLine("Copyright MIT Parviz Bazgosha Bushear  Iran 2021");
                Console.WriteLine("Set PDA_Output User Varaible for Custom Output ");
                Console.WriteLine("Output Path:" + PDAL.MapPath("~\\Output"));
                AppPath = System.Environment.CurrentDirectory;

                PDAL.load();
                PDAL.BuildApp();
                Console.WriteLine("App Builded!.");
                Console.ReadKey();
            }
            catch(Exception ex)
            {

                Console.Write(ex.ToString());
                Console.ReadKey();

            }
        }
    }
}
