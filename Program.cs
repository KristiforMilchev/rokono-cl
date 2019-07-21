using System;
using System.Collections.Generic;
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
        private static string Ip {get; set;}
        static void Main(string[] args)
        {
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
                    case "-s":
                        SaveDatabaseGen();
                        break;
                    case "-L":
                        GetConnections();
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
            System.Console.WriteLine("-Connection: requires specified Id after the command in order to select a connection from the quick access list. Saved quick connectison can be viewd with -L for identified use the ID column result");
            System.Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            System.Console.WriteLine("                              Please include the commnds after the supplied options                         ");
            System.Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            System.Console.WriteLine("-s: specify this command at the end of the new connection in order to save it for quick access in the future. Example rokono-cl -u User -password \"Password\" -a ip -d DatabaseName -file PathToWsdFile -s ");
            System.Console.WriteLine("-GF: Uses a saved connection to generate a plantUML diagram for a specific database that is on the same server as the quick access connection with a custom filepath. Usage rokono-cl -Connection ID -d DatabaseName -file customfilePath -GF");
            System.Console.WriteLine("-GS: Uses a saved connection to generate a plantUML diagram for the default set database using the default saved filepath. Usage rokono-cl -Connection ID -GS");
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
                new[] {"ID", "Database Name", "Host", "File Path"},
                a => a.ConnectionId, a => a.Database, a => a.Host, a=> a.FilePath));
        }

        private static void SaveDatabaseGen()
        {
            System.Console.WriteLine("In");
            var data = InputHandler.GetSavedConnections();
            var getCons = data == null ? new List<SavedConnection>() : data;
            var count =  getCons.Count == 0 ?  getCons.Count + 1 : 1;
            var conStirng =$"Server={Ip};Database={Database};User ID={User};Password='{Password}';";
            var savedConnection = new SavedConnection{
                Username = User,
                Password = Password,
                FilePath = FilePath,
                Database = Database,
                Host = Ip,
                ConnectionString = conStirng,
                ConnectionId = count
            };
            getCons.Add(savedConnection);
           InputHandler.SavedConnections(getCons);
        }   
    }
}

