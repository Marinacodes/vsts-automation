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

        public static string newRepo;

        // Main console app interface
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the domain you'd like to connect to and press Enter:");
            Console.WriteLine("(https://{domainname}.visualstudio.com)");
            domainName = Console.ReadLine();
            Console.WriteLine("Please enter your personal access token and press Enter:");
            MaskToken();
            credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalAccessToken)));
            do
            {
                Console.WriteLine("What would you like to do? Type in the appropriate letter and press Enter.");
                Console.WriteLine("\n*** Projects ***");
                Console.WriteLine("a. View Existing Team Projects");
                Console.WriteLine("b. Create a New Team Project");
                Console.WriteLine("\n*** Teams ***");
                Console.WriteLine("c. View Existing Teams");
                Console.WriteLine("d. Create a New Team");
                Console.WriteLine("\n*** Iterations ***");
                Console.WriteLine("e. Get a Team's Iterations");
                Console.WriteLine("f. Add an Iteration to a Team (IN PROGRESS)");
                Console.WriteLine("\n*** Git ***");
                Console.WriteLine("g. Get a Project's Repositories");
                Console.WriteLine("h. Add a Repository to a Project");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "a":
                        Console.WriteLine("You selected 'a. View Existing Team Projects'");
                        var a = CreateNewTask();
                        a.GetTeamProjects();
                        break;
                    case "b":
                        Console.WriteLine("You selected 'b. Create a New Team Project'");
                        Console.WriteLine("Type in a name for the new project");
                        newProjectName = Console.ReadLine();
                        Console.WriteLine("Type in a description for the new project");
                        newProjectDescr = Console.ReadLine();
                        var b = CreateNewTask();
                        b.CreateTeamProject();
                        break;
                    case "c":
                        Console.WriteLine("You selected 'c. View Existing Teams");
                        Console.WriteLine("Type in the project's name");
                        existingProject = Console.ReadLine();
                        var c = CreateNewTask();
                        c.GetTeams();
                        break;
                    case "d":
                        Console.WriteLine("You selected 'd. Create a New Team");
                        Console.WriteLine("Type in the name of the project you want to create the team in");
                        existingProject = Console.ReadLine();
                        Console.WriteLine("Type in a name for the new team");
                        newTeamName = Console.ReadLine();
                        Console.WriteLine("Type in a description for the new team");
                        newTeamDescr = Console.ReadLine();
                        var d = CreateNewTask();
                        d.CreateTeam();
                        break;
                    case "e":
                        Console.WriteLine("You selected 'e.Get a Team's Iterations'");
                        Console.WriteLine("Type in the name of the project");
                        existingProject = Console.ReadLine();
                        Console.WriteLine("Type in the name of the team");
                        existingTeam = Console.ReadLine();
                        var e = CreateNewTask();
                        e.GetIterations();
                        break;
                    case "f":
                        Console.WriteLine("You selected 'f. Add an Iteration to a Team'");
                        Console.WriteLine("Type in the name of the project");
                        existingProject = Console.ReadLine();
                        Console.WriteLine("Type in the name of the team");
                        existingTeam = Console.ReadLine();
                        var f = CreateNewTask();
                        f.AddIteration();
                        break;
                    case "g":
                        Console.WriteLine("You selected 'g. Get a Project's Repositories'");
                        Console.WriteLine("Type in the name of the project");
                        existingProject = Console.ReadLine();
                        var g = CreateNewTask();
                        g.GetRepositories();
                        break;
                    case "h":
                        Console.WriteLine("You selected 'h. Add a Repository to a Project'");
                        Console.WriteLine("Type in the name of the project");
                        existingProject = Console.ReadLine();
                        Console.WriteLine("Type in a name for the new repository");
                        newRepo = Console.ReadLine();
                        var h = CreateNewTask();
                        h.AddRepository();
                        break;
                }
            } while (!StringComparer.OrdinalIgnoreCase.Equals(Console.ReadLine(), "quit"));
        }

        // Masks the token from appearing on the console
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

        // Creates a new task
        static VSTSTasks CreateNewTask()
        {
            VSTSTasks newTask = new VSTSTasks();
            return newTask;
        }
    }
}