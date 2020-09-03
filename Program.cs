using CoryGehr.GroupMe.GetImages.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace CoryGehr.GroupMe.GetImages
{
    /// <summary>
    /// Main application class
    /// </summary>
    class Program
    {
        /// <summary>
        /// GroupMe API Base URL
        /// </summary>
        private static string _apiUrl = "https://api.groupme.com/v3";
        /// <summary>
        /// API Client object
        /// </summary>
        private static HttpClient _client;

        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        static async Task Main(string[] args)
        {
            // Get parameters
            string accessToken = String.Empty;

            Console.Write("Enter your GroupMe access token: ");

            // Hide typed characters
            while(true)
            {
                var key = Console.ReadKey(true);
                if(key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                accessToken += key.KeyChar;
            }

            Console.WriteLine("\n");

            if(!String.IsNullOrEmpty(accessToken))
            {
                // Create API client
                _client = _createApiClient(accessToken);

                // Get user groups
                ApiResponse<List<Group>> groups = await _apiRequestAsync<List<Group>>("groups?per_page=100");

                Console.WriteLine("Enter a Group ID:\n");
                foreach(Group group in groups.Response)
                {
                    Console.WriteLine(String.Format("[{0}] {1}", group.Id, group.Name));
                }
                Console.Write("\nSelection ID: ");
                string groupId = Console.ReadLine();

                // Get selected group
                Group target = groups.Response
                    .Where(g => g.Id.ToString() == groupId)
                    .FirstOrDefault();

                if(target != null)
                {
                    Console.WriteLine("Selected '{0}'", target.Name);
                    Console.WriteLine("\nDumping pictures...");
                    DirectoryInfo dumpPath = Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\groupme_dumps\\" + target.Name + "_images");

                    string beforeId = target.Messages.LastMessageId;
                    long messageCount = 0;
                    long imageCount = 0;

                    do
                    {
                        // Get max amount of messages possible in a call
                        try
                        {
                            ApiResponse<MessagesResponse> messagesResponse = await _apiRequestAsync<MessagesResponse>(
                                String.Format("groups/{0}/messages?before_id={1}&limit=100", target.Id, beforeId)
                            );

                            List<Message> messages = messagesResponse.Response.Messages;

                            if (messages != null && messages.Count > 0)
                            {
                                // Set new "before_id"
                                beforeId = messages.Last().Id;

                                for (int i = 0; i < messages.Count; i++)
                                {
                                    messageCount++;

                                    // Check for image
                                    Message message = messages[i];

                                    if (message.Attachments != null)
                                    {
                                        IEnumerable<Attachment> images = message.Attachments
                                            .Where(i => i.Type == "image");

                                        foreach (Attachment image in images)
                                        {
                                            await _downloadFileAsync(image.Url, dumpPath.FullName + "\\" + message.CreatedAt + "_");
                                            imageCount++;
                                            Thread.Sleep(500);
                                        }
                                    }

                                    // Wait between requests
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch(NotModifiedException)
                        {
                            // Process finished
                            break;
                        }
                    } while (true);

                    Console.WriteLine("{0} image(s) found in {1} message(s).", imageCount, messageCount);
                }
                else
                {
                    Console.WriteLine("ERROR! Invalid Group ID provided.");
                }
            }
            else
            {
                Console.WriteLine("ERROR! Empty access token provided.");
            }
        }

        /// <summary>
        /// Sends a request to the API and returns the response
        /// </summary>
        /// <param name="path">API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <returns>API response</returns>
        private static async Task<ApiResponse<T>> _apiRequestAsync<T>(string path)
        {
            HttpResponseMessage response = await _client.GetAsync("v3/"+path);
            Console.WriteLine("Sending request to {0}", path);
            if(response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ApiResponse<T>>(content);
            }
            else
            {
                throw new Exception("Request failed with HTTP " + response.StatusCode);
            }
        }

        /// <summary>
        /// Downloads a file from the specified URL to a local directory
        /// </summary>
        /// <param name="url">File URL</param>
        /// <param name="targetPath">Target file path</param>
        /// <returns>Path to downloaded file</returns>
        private static async Task _downloadFileAsync(string url, string targetPath)
        {
            Uri target = new Uri(url);
            HttpResponseMessage response = await _client.GetAsync(target);

            if(response.IsSuccessStatusCode)
            {
                Stream imageStream = await response.Content.ReadAsStreamAsync();
                string targetFileName = targetPath + target.PathAndQuery + ".jpg";
                targetFileName = targetFileName.Replace("/", "");
                using (FileStream file = new FileStream(targetFileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    Console.WriteLine("Writing image to {0}", targetFileName);
                    await imageStream.CopyToAsync(file);
                }
            }
            else
            {
                if(response.StatusCode == HttpStatusCode.NotModified)
                {
                    throw new NotModifiedException();
                }
                else
                {
                    throw new Exception("Request failed with HTTP " + response.StatusCode);
                }
            }
        }

        /// <summary>
        /// Creates an HTTP Client which can be used to query the GroupMe API
        /// </summary>
        /// <param name="accessToken">User access token</param>
        /// <returns>HttpClient</returns>
        private static HttpClient _createApiClient(string accessToken)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_apiUrl);
            client.DefaultRequestHeaders.Add("X-Access-Token", accessToken);
            return client;
        }
    }
}
