using System;
using System.Text;

namespace AutomateVSTSTasks
{
    public class Program
    {
        public static string domainName;
        public static string personalAccessToken;
        public static string credentials;
        static ConsoleKeyInfo key;

        public static string newProjectName;
        public static string newProjectDescr;

        public static string existingTeam;
        public static string existingProject;
        public static string newTeamName;
        public static string newTeamDescr;

        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the domain you'd like to connect to and press Enter:");
            Console.WriteLine("(https://{domainname}.visualstudio.com)");
            domainName = Console.ReadLine();
            Console.WriteLine("Please enter your personal access token and press Enter:");
            MaskToken();
            credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalAccessToken)));
            
            Console.WriteLine("What would you like to do? Type in the appropriate letter and press Enter.");
          
            Console.WriteLine("a. View Existing Team Projects");
            Console.WriteLine("b. Create a New Team Project");
            Console.WriteLine("c. View Existing Teams");
            Console.WriteLine("d. Create a New Team");
            Console.WriteLine("e. Get a Team's Iterations");
            Console.WriteLine("f. Add an Iteration to a Team");

            string input = Console.ReadLine();
            switch (input)
            {
                case "a":
                    Console.WriteLine("You typed 'a'");
                    TaskGetTeamProjects();
                    break;
                case "b":
                    Console.WriteLine("You typed 'b'");
                    Console.WriteLine("Please provide a name for the new project and press Enter");
                    newProjectName = Console.ReadLine();
                    Console.WriteLine("Please provide a description for the new project and press Enter");
                    newProjectDescr = Console.ReadLine();
                    TaskCreateTeamProject();
                    break;
                case "c":
                    Console.WriteLine("You typed 'c'");
                    Console.WriteLine("Please provide a name for the existing project and press Enter");
                    existingProject = Console.ReadLine();
                    TaskGetTeams();
                    break;
                case "d":
                    Console.WriteLine("You typed 'd'");
                    Console.WriteLine("Please provide a name of the project you want to create the team in");
                    existingProject = Console.ReadLine();
                    Console.WriteLine("Please provide a name for the new team and press Enter");
                    newTeamName = Console.ReadLine();
                    Console.WriteLine("Please provide a description for the new team and press Enter");
                    newTeamDescr = Console.ReadLine();
                    TaskCreateTeam();
                    break;
                case "e":
                    Console.WriteLine("You typed 'e'");
                    Console.WriteLine("Please provide a name of the project");
                    existingProject = Console.ReadLine();
                    Console.WriteLine("Please provide a name of the team");
                    existingTeam = Console.ReadLine();
                    TaskGetIterations();
                    break;
                case "f":
                    Console.WriteLine("You typed 'f'");
                    Console.WriteLine("Please provide a name of the project");
                    existingProject = Console.ReadLine();
                    Console.WriteLine("Please provide a name of the team");
                    existingTeam = Console.ReadLine();
                    TaskAddIterations();
                    break;
            }
            
        }

        static void MaskToken()
        {
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    personalAccessToken += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && personalAccessToken.Length > 0)
                    {
                        personalAccessToken = personalAccessToken.Substring(0, (personalAccessToken.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
        }

        static void TaskGetTeamProjects()
        {
            var temp = new VSTSTasks();
            temp.GetTeamProjects();
        }

        static void TaskCreateTeamProject()
        {
            var temp = new VSTSTasks();
            temp.CreateTeamProject();
        }

        static void TaskCreateTeam()
        {
            var temp = new VSTSTasks();
            temp.CreateTeam();
        }

        static void TaskGetTeams()
        {
            var temp = new VSTSTasks();
            temp.GetTeams();
        }

        static void TaskGetIterations()
        {
            var temp = new VSTSTasks();
            temp.GetIterations();
        }

        static void TaskAddIterations()
        {
            var temp = new VSTSTasks();
            temp.AddIterations();
        }
    }
}