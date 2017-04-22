using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutomateVSTSTasks
{
    class VSTSTasks
    {
        // Helper method to set headers
        private void SetHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Program.credentials);
        }

        // Returns the names and IDs of existing projects
        public void GetTeamProjects()
        {    
            using (var client = new HttpClient())
            {
                // Set headers
                client.BaseAddress = new Uri("https://" + Program.domainName + ".visualstudio.com");
                SetHeaders(client);

                // Send request
                HttpResponseMessage response = client.GetAsync("_apis/projects?api-version=2.2").Result;   
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    JObject responseJSON = JObject.Parse(result);
                    int numProjects = (int)responseJSON["count"];
                    Console.WriteLine("\nCurrent projects (Name, ID):");
                    for (int i = 0; i < numProjects; i++)
                    {
                        Console.WriteLine((string)responseJSON["value"][i]["name"] + "\t" + (string)responseJSON["value"][i]["id"]);
                    }
                    Console.WriteLine("\nResponse: " + (int)response.StatusCode + " " + response.StatusCode + ". Press Enter to continue, type 'quit' to exit.");
                }
            }
        }

        // Creates a new project
        public void CreateTeamProject()
        {
            // Object to create a new team project
            Object teamProject = new
            {
                name = Program.newProjectName,
                description = Program.newProjectDescr,
                capabilities = new
                {
                    versioncontrol = new
                    {
                        sourceControlType = "Git"
                    },
                    processTemplate = new
                    {
                        // Agile template type 
                        // List here: https://www.visualstudio.com/en-us/docs/integrate/api/tfs/processes#getalistofprocesses
                        templateTypeId = "adcc42ab-9882-485e-a3ed-7678f01f66bc"
                    }
                }
            };

            using (var client = new HttpClient())
            {
                // Set headers
                SetHeaders(client);

                // Serialize into JSON
                var patchValue = new StringContent(JsonConvert.SerializeObject(teamProject), Encoding.UTF8, "application/json");

                // Send request
                var method = new HttpMethod("POST");
                var request = new HttpRequestMessage(method, "https://" + Program.domainName +".visualstudio.com/_apis/projects?api-version=2.2") { Content = patchValue };
                var response = client.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("\nResponse: " + (int)response.StatusCode + " " + response.StatusCode + ". Press Enter to continue, type 'quit' to exit.");
                }
            }
        }

        // Returns a list of current teams
        public void GetTeams()
        {
            using (var client = new HttpClient())
            {
                // Set headers
                client.BaseAddress = new Uri("https://" + Program.domainName + ".visualstudio.com");
                SetHeaders(client);

                // Send request
                HttpResponseMessage response = client.GetAsync("_apis/projects/" + Program.existingProject + "/teams?api-version=2.2").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    JObject responseJSON = JObject.Parse(result);
                    int numProjects = (int)responseJSON["count"];
                    Console.WriteLine("Current teams:");
                    for (int i = 0; i < numProjects; i++)
                    {
                        Console.WriteLine((string)responseJSON["value"][i]["name"]);
                    }
                    Console.WriteLine("\nResponse: " + (int)response.StatusCode + " " + response.StatusCode + ". Press Enter to continue, type 'quit' to exit.");
                }
            }
        }
        
        // Creates a new team
        public void CreateTeam()
        {
            // Object to create a new team
            Object teamData = new
            {
                name = Program.newTeamName,
                description = Program.newTeamDescr,
            };

            using (var client = new HttpClient())
            {
                // Set headers
                SetHeaders(client);

                // Serialize into JSON      
                var patchValue = new StringContent(JsonConvert.SerializeObject(teamData), Encoding.UTF8, "application/json");

                // Send request
                var method = new HttpMethod("POST");
                var request = new HttpRequestMessage(method, "https://" + Program.domainName + ".visualstudio.com/_apis/projects/" + Program.existingProject + 
                    "/teams?api-version=2.2") { Content = patchValue };
                var response = client.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("\nResponse: " + (int)response.StatusCode + " " + response.StatusCode + ". Press Enter to continue, type 'quit' to exit.");
                }
            }
        }
        
        // Returns a list of iterations for a team
        public void GetIterations()
        {
            using (var client = new HttpClient())
            {
                // Set headers
                client.BaseAddress = new Uri("https://" + Program.domainName + ".visualstudio.com");
                SetHeaders(client);

                // Send the request
                HttpResponseMessage response = client.GetAsync(Program.existingProject + "/" +  Program.existingTeam + "/_apis/work/teamsettings/iterations?api-version=2.2").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    JObject responseJSON = JObject.Parse(result);
                    int numProjects = (int)responseJSON["count"];
                    Console.WriteLine("Current iterations for team: " + Program.existingTeam + "(Name, Start Date, Finish Date, ID)");
                    for (int i = 0; i < numProjects; i++)
                    {
                        Console.WriteLine((string)responseJSON["value"][i]["name"] + 
                            "\t" + (string)responseJSON["value"][i]["attributes"]["startDate"] + 
                            "\t " + (string)responseJSON["value"][i]["attributes"]["finishDate"] +
                            "\t " + (string)responseJSON["value"][i]["id"]);
                }
                    Console.WriteLine("\nResponse: " + (int)response.StatusCode + " " + response.StatusCode + ". Press Enter to continue, type 'quit' to exit.");
                }
            }
        }

        // ATTN: This method is not working
        // Adds an iteration to a team
        public void AddIteration()
        {
            // Object to create a new iteration
            Object iteration = new
            {
                id = "TestIteration",
            };

            using (var client = new HttpClient())
            {
                SetHeaders(client);

                // Serialize into JSON
                var patchValue = new StringContent(JsonConvert.SerializeObject(iteration), Encoding.UTF8, "application/json");

                // Send request
                var method = new HttpMethod("POST");
                var request = new HttpRequestMessage(method, "https://" + Program.domainName + ".visualstudio.com/" + Program.existingProject + "/" +
                    Program.existingTeam + "/_apis/work/teamsettings/iterations?api-version=2.2") { Content = patchValue };
                var response = client.SendAsync(request).Result;
                Console.WriteLine("\nResponse: " + (int)response.StatusCode + " " + response.StatusCode);
                Console.ReadLine();
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("\nResponse: " + (int)response.StatusCode + " " + response.StatusCode + ". Press Enter to continue, type 'quit' to exit.");
                }
            }
        }

        // Returns a list of repositories for a project
        public void GetRepositories()
        {
             using (var client = new HttpClient())
             {
                // Set headers
                client.BaseAddress = new Uri("https://" + Program.domainName + ".visualstudio.com");
                SetHeaders(client);

                // Send the request
                HttpResponseMessage response = client.GetAsync(Program.existingProject + "/_apis/git/repositories?api-version=2.2").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    JObject responseJSON = JObject.Parse(result);
                    int numRepos = (int)responseJSON["count"];
                    Console.WriteLine("Current repositories for project: " + Program.existingProject);
                    for (int i = 0; i < numRepos; i++)
                    {
                        Console.WriteLine((string)responseJSON["value"][i]["name"]);                          
                    }
                    Console.WriteLine("\nResponse: " + (int)response.StatusCode + " " + response.StatusCode + ". Press Enter to continue, type 'quit' to exit.");
                }
             }
        }

        // Adds a repository to a project
        public void AddRepository()
        {
            // Object to create a new repository
            Object repoData = new
            {
                name = Program.newRepo,
                project = new
                {
                    // Does not appear to work with project name
                    id = GetProjectID(Program.existingProject),
                }
            };

            using (var client = new HttpClient())
            {
                SetHeaders(client);

                // Serialize into JSON
                var patchValue = new StringContent(JsonConvert.SerializeObject(repoData), Encoding.UTF8, "application/json");

                // Send request
                var method = new HttpMethod("POST");
                var request = new HttpRequestMessage(method, "https://" + Program.domainName + ".visualstudio.com/_apis/git/repositories?api-version=2.2") { Content = patchValue };
                var response = client.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("\nResponse: " + (int)response.StatusCode + " " + response.StatusCode + ". Press Enter to continue, type 'quit' to exit.");
                }
            }
        }

        // Helper method for AddRepository()
        // Takes in a project name and returns the project ID
        private string GetProjectID(string name)
        {
            using (var client = new HttpClient())
            {
                // Set headers
                client.BaseAddress = new Uri("https://" + Program.domainName + ".visualstudio.com");
                SetHeaders(client);

                // Send request
                HttpResponseMessage response = client.GetAsync("_apis/projects?api-version=2.2").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    JObject responseJSON = JObject.Parse(result);
                    int numProjects = (int)responseJSON["count"];
                    for (int i = 0; i < numProjects; i++)
                    {
                        if (StringComparer.OrdinalIgnoreCase.Equals((string)responseJSON["value"][i]["name"], Program.existingProject))
                        {
                            return (string)responseJSON["value"][i]["id"];
                        }
                    }
                }
            }
            return null;
        }
    }
}