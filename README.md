# rokono-cl
Sql server to plantUML generating tool, allowing quick access to SQL relation database diagrams inside visual studio code and other editors that support plantUML/wsd diagrams. 


-----------------------------------------------------------------------------------------------------------
Name: Rokono-Cl
Description: Quick and easy tool to generate UML diagrams from relational databases released as an extension of plantUML extension for visual studio code
The tool comes as is and its not supported or developed by the team behind plantUML, but it is fully integrated to work with the drawing libaray to generate database UML diagrams.
Author: Kristifor Milchev
Contact for support: Kristifor@rttinternational.com

-----------------------------------------------------------------------------------------------------------

Usage example rokono-cl [options] [commands]
-u : Prompts for a username it should be an account that has access to view table relationships.
-password: requires a valid password for the current sql user
-d: the default database that will be used to generate a *.wsd diagram 
-file: requires a path that will point to a *.wsd file (optional) 
-a: the endpoint of the sql server, it could be a domain or an ip and if it doesn't run on the default port please specify it with ip:port
-L: returns a list of saved database diagrams for quick access.
-r: removes a record from the saved connections
-e: edits a saved connection
-Connection: requires specified Id after the command in order to select a connection from the quick access list. Saved quick connectison can be viewd with -L for identified use the ID column result

-----------------------------------------------------------------------------------------------------------
                              Please include the commnds after the supplied options                         
-----------------------------------------------------------------------------------------------------------
-s: specify this command at the end of the new connection in order to save it for quick access in the future. Example rokono-cl -u User -password \"Password\" -a ip -d DatabaseName -file PathToWsdFile -s 
-e: specify this command at the end of the new connection followed by -Connection ID in order to edit a record in the saved connections list.
-r: specitfy this command after -Connection ID in order to remove a connection from the saved connections list.
-GF: Uses a saved connection to generate a plantUML diagram for a specific database that is on the same server as the quick access connection with a custom filepath. Usage rokono-cl -Connection ID -d DatabaseName -file customfilePath -GF
-GS: Uses a saved connection to generate a plantUML diagram for the default set database using the default saved filepath. Usage rokono-cl -Connection ID -GS
-Context: runs dbscaffold on a database to generate database first update or initalization on the project directory ensuring consitancy between the generated UML diagram and database model inside the project. Important, must be -CP must be pointed to the root project folder in order to generate database context!!!"

-----------------------------------------------------------------------------------------------------------
                                                 Usage Example 
-----------------------------------------------------------------------------------------------------------
/rokono-cl -u "UserName" -password "UserPassword" -a "SQL Server IP" -d "DatabseName" -file "FilePath" -s 
-s Just saves the connection for later use its optional
If all the information you've entered is valid you will now be presented with a file inside the folder that you've specified

It should look something like this

----------------------------------------------------------------------------------------------------------
class BlogPost
{
    Id int |  | NO
    Heading nvarchar |  | NO
    FirstParagraph nvarchar |  | YES
    SecondParagrahp nvarchar |  | YES
    ThirdParagraph nvarchar |  | YES
    FourthParagraph nvarchar |  | YES
    FifthParagraph nvarchar |  | YES
    SixthParagraph nvarchar |  | YES
    InnerImage nvarchar |  | YES
    BannerLocation nvarchar |  | YES
    DateOfPost datetime |  | YES
    InnerHeaing nvarchar |  | YES
}
class AssociatedBlogPosts
{
    Id int |  | NO
    BlogId int |  | YES
    PostId int |  | YES
    CategoryId int |  | YES
    IsFeatured int |  | YES
    IsPrioritized int |  | YES
}
AssociatedBlogPosts "1" *-- "many" Blogs

-----------------------------------------------------------------------------------------------------------

Next step open the folder where the wsd is generated with visual studio code.
Important, in order to get plantUML extension in visual studio code or other editor that supports it you must have installed 
Graphviz and java sdk.
once you are ready to test just open the file in visual studio code and press alt+d if you have graphviz and java installed you should now have a relationship diagram inside of your favorite editor.


//Build steps, if you want to complite the code yourself just go to the root directory and depending on your operating system you can use

//dotnet publish -c Release -r linux-64
dotnet publish -c Release




