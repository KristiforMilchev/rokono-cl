using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using rokono_cl.Data_Hanlders;
using rokono_cl.DatabaseHandlers;
using RokonoDbManager.Models;

namespace rokono_cl.CLHandlers
{
    public class InputHandler
    {
        public static List<SavedConnection> GetSavedConnections()
        {
            if(!File.Exists("SavedConnections.txt"))
            {
                System.Console.WriteLine("Empty");
                return new List<SavedConnection>();
            }
            var fileData = File.ReadAllText("SavedConnections.txt");
            return JsonConvert.DeserializeObject<List<SavedConnection>>(fileData);   
        }

        internal static void SavedConnections(List<SavedConnection> getCons)
        {
            
            if(!File.Exists("SavedConnections.txt"))
            {
                File.Create("SavedConnections.txt").Close();
                AddConnections(getCons,"SavedConnections.txt");
            }
            else
                AddConnections(getCons,"SavedConnections.txt");
             
        }

        private static void AddConnections(List<SavedConnection> getCons, string v)
        {
            File.WriteAllText(v,JsonConvert.SerializeObject(getCons));
        }

         

        internal static SavedConnection GetSavedConnection(string conId)
        {

            if(!File.Exists("SavedConnections.txt"))
                return null;
            var fileData = File.ReadAllText("SavedConnections.txt");
            return JsonConvert.DeserializeObject<List<SavedConnection>>(fileData).FirstOrDefault(x=>x.ConnectionId == int.Parse(conId));        
        }
    }
}