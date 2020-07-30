using System.Collections.Generic;
using System.IO;
using System.Text;
using rokono_cl.DatabaseHandlers;
using RokonoDbManager.Models;

namespace rokono_cl.Data_Hanlders
{
    public class DiagramHandlers
    {
        public static void GenerateSchema(SavedConnection parameter, string dbName, string dbFilePath)
        {
            
            var res = new List<UmlBindingData>();
            using(var context = new DbManager($"Server={parameter.Host};Database={dbName};User ID={parameter.Username};Password='{parameter.Password}';"))
            {
                var tableData = new UmlBindingData();

                var outboundData = new List<OutboundTable>();
                var tables = context.GetTables();
                tables.ForEach(x=>{
                    outboundData.Add(context.GetTableData(x));
                });
                tableData.Tables = outboundData;
                tableData.Connections = context.GetDbUmlData();
                res.Add(tableData);
                
            }
            var generatedPlantUML =  GenerateDatabaseRelations(res,dbFilePath);
        }

        public static string GenerateDatabaseRelations(List<UmlBindingData> umlData,string fileName)
        {

            if(!string.IsNullOrEmpty(fileName))
            {
                if(!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                    GenerateSchema(umlData,fileName);
                }
                else
                    GenerateSchema(umlData,fileName);
            }

            return GetPlantUMLText(umlData); 
        }

        private static void GenerateSchema(List<UmlBindingData> umlData, string fileName)
        {     
            using (StreamWriter StreamWriter = new StreamWriter(fileName))
            { 
                umlData.ForEach(x=>{
                    x.Tables.ForEach(y=>{
                        StreamWriter.WriteLine($"class '{y.Shape.Name}'");
                        StreamWriter.WriteLine("{");
                        y.Shape.Attribute.ForEach(z=>{


                            StreamWriter.WriteLine($"    '{z.Name} {z.Column}'");
                        });

                        StreamWriter.WriteLine("}");
                    });
                    x.Connections.ForEach(y => {
                        StreamWriter.WriteLine($"'{y.TableName}' \"1\" *-- \"many\" '{y.ConnectionName}'");   
                    });
                });
               
            }
        }

        private static string GetPlantUMLText(List<UmlBindingData> umlData)
        {
            var res = new StringBuilder();
            umlData.ForEach(x=>{
                x.Tables.ForEach(y=>{
                    res.AppendLine($"class '{y.Shape.Name}'");
                    res.AppendLine("{");

                    y.Shape.Attribute.ForEach(z=>{

                        res.AppendLine($"    '{z.Name} {z.Column}'");

                    });
                    res.AppendLine("}");
                });
                x.Connections.ForEach(y => {
                    res.AppendLine($"'{y.TableName}' \"1\" *-- \"many\" '{y.ConnectionName}'");   
                });
            });
            return res.ToString();
        }
    }
}