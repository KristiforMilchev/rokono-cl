using System;
using System.Collections.Generic;
using System.Diagnostics;
using rokono_cl.CLHandlers;
using rokono_cl.Data_Hanlders;
using RokonoDbManager.Models;

namespace rokono_cl
{
    
    class Program
    {
        private static SavedConnection SavedConnection {get; set;}
        private static string Password {get;set;}
        private static string User {get;set;}
        private static string Database { get; set; }
        private static string FilePath {get; set;}
        public static string DbContextPath {get; set;}
        private static string Ip {get; set;}
        private static bool Os {get; set;}
        public static bool IsLinux
        {
            get
            {
                int p = (int) Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }
        static void Main(string[] args)
        {
        //    args = new string[13];
        //    args[0] = "-u";
        //    args[1] = "StoriesUntold";
        //    args[2] = "-d";
        //    args[3] = "WW";
        //    args[4] = "-a";
        //    args[5] = "192.168.1.3";
        //    args[6] = "-file";
        //    args[7] = "/home/kristifor/Projects/Games/StoriesUnraveled/Server/Slaves/StoriesUnraveledServer/schema.wsd";
        //    args[8] = "-CP";
        //    args[9] = "home/kristifor/Projects/Games/StoriesUnraveled/Server/Slaves/StoriesUnraveledServer/StoriesUntoldDataLaye/DbModels";
        //    args[10] = "-password";
        //    args[11] = "Hj153426";
        //    args[12] = "-s";


     //       Select query 
            // args = new string[1];
            // args[0] = "-L";

            args = new string[5];
            args[0] = "-Connection";
            args[1] = "4";
            args[2] = "-GS";
            args[4] = "-Context";


            for(int i = 0; i < args.Length; i++)
            {
                switch(args[i])
                {
                    case "-u":
                        User = args[i+1];
                        break;
                    case "-password":
                        Password = args[i+1];
                        break;
                    case "-d":
                        Database = args[i+1];
                        break;
                    case "-file":
                        FilePath = args[i+1];
                        break;  
                    case "-a":
                        Ip = args[i+1];
                    break;
                    case "-e":
                        EditConnection();
                    break;
                    case "-r":
                        RemoveConnection();
                    break;
                    case "-s":
                        SaveDatabaseGen();
                        break;
                    case "-L":
                        GetConnections();
                        break;
                    
                    case "-CP":
                        DbContextPath = args[i+1];
                    break;
                    case "-Context":
                        GenerateDbContext();
                    break;
                    case "-Connection":
                        SavedConnection = GetConnectionById(args[i+1]);
                        System.Console.WriteLine(SavedConnection.ConnectionString);
                        break;
                    case "-GS":
                        DiagramHandlers.GenerateSchema(SavedConnection,SavedConnection.Database,SavedConnection.FilePath);
                        break;
                    case "-GF":
                        DiagramHandlers.GenerateSchema(SavedConnection,Database,FilePath);
                        break;
                    case "--Help":
                        ShowHelpMenu();
                        break;

                    
                }  
            }
            if(args.Length == 0)
                System.Console.WriteLine("Use rokono-cl --Help  *for more information*");
        }

        private static void GenerateDbContext()
        {

            
            var cmd = string.Empty;
            if(Os)
            {
                cmd = $"dotnet ef dbcontext scaffold \"Server={Ip};Database={Database};User ID={User};Password='{Password}';\"  Microsoft.EntityFrameworkCore.SqlServer -o {DbContextPath} -f";
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = cmd;
                process.StartInfo = startInfo;
                process.Start();
            }
            else
            {
                
                if(SavedConnection != null)
                {
                    DbContextPath = SavedConnection.DbContextPath;

                    cmd = $"cd {DbContextPath} & dotnet ef dbcontext scaffold \"{SavedConnection.ConnectionString}\"  Microsoft.EntityFrameworkCore.SqlServer -o Models -f";
                }
                else
                    cmd = $"cd {DbContextPath} & dotnet ef dbcontext scaffold \"Server={Ip};Database={Database};User ID={User};Password='{Password}';\"  Microsoft.EntityFrameworkCore.SqlServer -f";

                var bashResult  = Bash(cmd);
                
            }
        }
        public static string Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    
        private static void RemoveConnection()
        {
            var data = InputHandler.GetSavedConnections();
            var getCons = data == null ? new List<SavedConnection>() : data;
            if(getCons.Count == 0)
            {
                System.Console.WriteLine("Collection is empty, you can't delete an non existing row!");
            }
            
            getCons.Remove(SavedConnection);
            var rebase = new List<SavedConnection>();
            getCons.ForEach(x=>{
                var current = x;
                current.ConnectionId = current.ConnectionId -1;
                rebase.Add(current);
            });
            InputHandler.SavedConnections(getCons);

        }

        private static void EditConnection()
        {
            System.Console.WriteLine("In");
            var data = InputHandler.GetSavedConnections();
            var getCons = data == null ? new List<SavedConnection>() : data;
            
            var conStirng =$"Server={Ip};Database={Database};User ID={User};Password='{Password}';";
           
            SavedConnection.Username = User;
            SavedConnection.Password = Password;
            SavedConnection.FilePath = FilePath;
            SavedConnection.Database = Database;
            SavedConnection.Host = Ip;
            SavedConnection.ConnectionString = conStirng;
         
            getCons.Add(SavedConnection);
            InputHandler.SavedConnections(getCons);
        }

        private static void ShowHelpMenu()
        {
            System.Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            System.Console.WriteLine("Name: Rokono-Cl");
            System.Console.WriteLine("Description: Quick and easy tool to generate UML diagrams from relational databases released as an extension of plantUML extension for visual studio code");
            System.Console.WriteLine("The tool comes as is and its not supported or developed by the team behind plantUML, but it is fully integrated to work with the drawing libaray to generate database UML diagrams.");
            System.Console.WriteLine("Author: Kristifor Milchev");
            System.Console.WriteLine("Contact for support: Kristifor@rttinternational.com");
            System.Console.WriteLine("Github: //");
            System.Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            System.Console.WriteLine();
            System.Console.WriteLine();

            System.Console.WriteLine("Usage example rokono-cl [options] [commands]");
            System.Console.WriteLine("-u : Prompts for a username it should be an account that has access to view table relationships. ");
            System.Console.WriteLine("-password: requires a valid password for the current sql user");
            System.Console.WriteLine("-d: the default database that will be used to generate a *.wsd diagram ");
            System.Console.WriteLine("-file: requires a path that will point to a *.wsd file (optional) ");
            System.Console.WriteLine("-a: the endpoint of the sql server, it could be a domain or an ip and if it doesn't run on the default port please specify it with ip:port");
            System.Console.WriteLine("-L: returns a list of saved database diagrams for quick access.");
            System.Console.WriteLine("-r: removes a record from the saved connections");
            System.Console.WriteLine("-CP: Specifies a directory to save the generated edmx context. Used for generating database first approach with entity framework core, eventually it executes DbScaffold on the database. the command can be specified at the end to ensure that the project context is the same as the generated UML diagram.");
            System.Console.WriteLine("-e: edits a saved connection");
            System.Console.WriteLine("-Connection: requires specified Id after the command in order to select a connection from the quick access list. Saved quick connectison can be viewd with -L for identified use the ID column result");
            System.Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            System.Console.WriteLine("                              Please include the commnds after the supplied options                         ");
            System.Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            System.Console.WriteLine("-s: specify this command at the end of the new connection in order to save it for quick access in the future. Example rokono-cl -u User -password \"Password\" -a ip -d DatabaseName -file PathToWsdFile -s ");
            System.Console.WriteLine("-e: specify this command at the end of the new connection followed by -Connection ID in order to edit a record in the saved connections list.");
            System.Console.WriteLine("-r: specitfy this command after -Connection ID in order to remove a connection from the saved connections list.");
            System.Console.WriteLine("-GF: Uses a saved connection to generate a plantUML diagram for a specific database that is on the same server as the quick access connection with a custom filepath. Usage rokono-cl -Connection ID -d DatabaseName -file customfilePath -GF");
            System.Console.WriteLine("-GS: Uses a saved connection to generate a plantUML diagram for the default set database using the default saved filepath. Usage rokono-cl -Connection ID -GS");
            System.Console.WriteLine("-Context: runs dbscaffold on a database to generate database first update or initalization on the project directory ensuring consitancy between the generated UML diagram and database model inside the project. Important, must be -CP must be pointed to the root project folder in order to generate database context!!!");
            System.Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

        }

        private static SavedConnection GetConnectionById(string conId)
        {
            var result = InputHandler.GetSavedConnection(conId);
            if(result == null)
            {
                System.Console.WriteLine("Connection doesn't exist please try connecting to the database using the following syntax. ");
                System.Console.WriteLine("-U username -password password -d databasename -file filepath -a hostip -s to save for later use");
                System.Console.WriteLine("You can also do --help for more information");
            }
            return result;
        }

        private static void GetConnections()
        {
            var getCons = InputHandler.GetSavedConnections();
            Console.WriteLine(getCons.ToStringTable(
                new[] {"ID", "Database Name", "Host", "File Path", "Context Path"},
                a => a.ConnectionId, a => a.Database, a => a.Host, a=> a.FilePath, a=> a.DbContextPath));
        }

        private static void SaveDatabaseGen()
        {
            System.Console.WriteLine("In");
            var data = InputHandler.GetSavedConnections();
            var getCons = data == null ? new List<SavedConnection>() : data;
            var count =  getCons.Count == 0 ?  getCons.Count + 1 : getCons.Count + 1;
            var conStirng =$"Server={Ip};Database={Database};User ID={User};Password='{Password}';";
            var savedConnection = new SavedConnection{
                Username = User,
                Password = Password,
                FilePath = FilePath,
                Database = Database,
                Host = Ip,
                ConnectionString = conStirng,
                ConnectionId = count,
                DbContextPath = DbContextPath
            };

            getCons.Add(savedConnection);
           InputHandler.SavedConnections(getCons);
        }   
    }
}
