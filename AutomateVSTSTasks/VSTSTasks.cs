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
        public void GetTeamProjects()
        {    
            //use the httpclient        
            using (var client = new HttpClient())
            {
                //set our headers
                client.BaseAddress = new Uri("https://" + Program.domainName + ".visualstudio.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Program.credentials);

                //send the request and content
                HttpResponseMessage response = client.GetAsync("_apis/projects?api-version=2.2").Result;   
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    JObject responseJSON = JObject.Parse(result);
                    int numProjects = (int)responseJSON["count"];
                    Console.WriteLine("These are the current projects:");
                    for (int i = 0; i < numProjects; i++)
                    {
                        Console.WriteLine((string)responseJSON["value"][i]["name"]);
                    }
                    Console.WriteLine("\nReturned the following status code and description: " + (int)response.StatusCode + " " + response.StatusCode);
                    Console.ReadLine();
                }
            }
        }

        public void CreateTeamProject()
        {
            //object to create a new team project
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

            //use the httpclient
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Program.credentials);

                // serialize the fields array into a json string
                var patchValue = new StringContent(JsonConvert.SerializeObject(teamProject), Encoding.UTF8, "application/json");
                var method = new HttpMethod("POST");

                var request = new HttpRequestMessage(method, "https://" + Program.domainName +".visualstudio.com/_apis/projects?api-version=2.2") { Content = patchValue };
                var response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("\nReturned the following status code and description: " + (int)response.StatusCode + " " + response.StatusCode);
                    Console.ReadLine();

                }
            }
        }

        public void CreateTeam()
        {
            //create a team object to save
            Object teamData = new
            {
                name = Program.newTeamName,
                description = Program.newTeamDescr,
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Program.credentials);

                // serialize the fields array into a json string         
                var patchValue = new StringContent(JsonConvert.SerializeObject(teamData), Encoding.UTF8, "application/json"); // mediaType needs to be application/json-patch+json for a patch call
                var method = new HttpMethod("POST");

                var request = new HttpRequestMessage(method, "https://" + Program.domainName + ".visualstudio.com/_apis/projects/" + Program.existingProject + "/teams?api-version=2.2") { Content = patchValue };
                var response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("\nReturned the following status code and description: " + (int)response.StatusCode + " " + response.StatusCode);
                    Console.ReadLine();
                }
            }
        }

        public void GetTeams()
        {
            //use the httpclient        
            using (var client = new HttpClient())
            {
                //set our headers
                client.BaseAddress = new Uri("https://" + Program.domainName + ".visualstudio.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Program.credentials);

                //send the request and content
                HttpResponseMessage response = client.GetAsync("_apis/projects/" + Program.existingProject + "/teams?api-version=2.2").Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    JObject responseJSON = JObject.Parse(result);
                    int numProjects = (int)responseJSON["count"];
                    Console.WriteLine("These are the current teams:");
                    for (int i = 0; i < numProjects; i++)
                    {
                        Console.WriteLine((string)responseJSON["value"][i]["name"]);
                    }
                    Console.WriteLine("\nReturned the following status code and description: " + (int)response.StatusCode + " " + response.StatusCode);
                    Console.ReadLine();
                }
            }
        }

        public void GetIterations()
        {
            //use the httpclient        
            using (var client = new HttpClient())
            {
                //set our headers
                client.BaseAddress = new Uri("https://" + Program.domainName + ".visualstudio.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Program.credentials);

                //send the request and content
                HttpResponseMessage response = client.GetAsync(Program.existingProject + "/" +  Program.existingTeam + "/_apis/work/teamsettings/iterations?api-version=2.2").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    JObject responseJSON = JObject.Parse(result);
                    int numProjects = (int)responseJSON["count"];
                    Console.WriteLine("These are the current iterations for team: " + Program.existingTeam);
                    for (int i = 0; i < numProjects; i++)
                    {
                        Console.WriteLine((string)responseJSON["value"][i]["name"] + 
                            " " + (string)responseJSON["value"][i]["attributes"]["startDate"] + 
                            " " + (string)responseJSON["value"][i]["attributes"]["finishDate"]);
                    }
                    Console.WriteLine("\nReturned the following status code and description: " + (int)response.StatusCode + " " + response.StatusCode);
                    Console.ReadLine();
                }
            }
        }

        // This method is not working
        public void AddIterations()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Program.credentials);

                var method = new HttpMethod("POST");

                var request = new HttpRequestMessage(method, "https://" + Program.domainName + ".visualstudio.com/" + Program.existingProject + "/" +
                    Program.existingTeam + "/_apis/work/teamsettings/iterations?api-version=2.2");
                var response = client.SendAsync(request).Result;
                Console.WriteLine("\nReturned the following status code and description: " + (int)response.StatusCode + " " + response.StatusCode);
                Console.ReadLine();
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("\nReturned the following status code and description: " + (int)response.StatusCode + " " + response.StatusCode);
                    Console.ReadLine();
                }
            }
        }
    }
}