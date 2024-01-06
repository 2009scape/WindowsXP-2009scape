using System;
using System.Diagnostics;
using System.IO;
using System.Net;

class Program
{
    static void Main()
    {
        Console.WriteLine("Start...");
        // Define the URLs and file paths
        string jarUrl = "https://fastupload.io/en/mroEanQsfSNn/YECFt9GEg0ijRGw/wgWGq6qrDm4oy/rt4-client%281%29.jar";
        string jarPath = "rt4-client.jar";
        string javaUrl = "https://github.com/ojdkbuild/ojdkbuild/releases/download/java-1.8.0-openjdk-1.8.0.332-1.b09-x86/java-1.8.0-openjdk-1.8.0.332-1.b09.ojdkbuild.windows.x86.msi";
        string javaInstaller = "java_installer.msi";
        string configPath = "config.json";
        string configContent = @"
{
  ""ip_management"": ""play.2009scape.org"",
  ""ip_address"": ""play.2009scape.org"",
  ""world"": 1,
  ""server_port"": 43594,
  ""wl_port"": 43595,
  ""js5_port"": 43595,
  ""ui_scale"": 1,
  ""fps"": 0
}";

        try
        {
            // Download rt4-client.jar using WebClient
            Console.WriteLine("Downloading rt4-client.jar...");
            DownloadFileWithWebClient(jarUrl, jarPath);

            // Write to config.json
            Console.WriteLine("Writing to config.json...");
            File.WriteAllText(configPath, configContent);
            Console.WriteLine("Write complete.");

            // Check if Java is installed
            Console.WriteLine("Checking for Java installation...");
            if (!IsJavaInstalled())
            {
                // Download Java using HttpWebRequest
                Console.WriteLine("Java not found. Downloading Java installer...");
                DownloadFileWithHttpWebRequest(javaUrl, javaInstaller);
                Console.WriteLine("Download complete. Installing Java...");
                Process.Start(javaInstaller).WaitForExit();
            }
            else
            {
                Console.WriteLine("Java is already installed.");
            }

            // Run rt4-client.jar with Java
            Console.WriteLine("Running rt4-client.jar with Java...");
            Process.Start("java", "-jar " + jarPath).WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            Console.ReadLine();
        }
    }

    static void DownloadFileWithWebClient(string fileUrl, string filePath)
    {
        using (var client = new WebClient())
        {
            client.DownloadFile(fileUrl, filePath);
        }
    }

    static void DownloadFileWithHttpWebRequest(string fileUrl, string filePath)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fileUrl);
        request.Method = "GET";

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream responseStream = response.GetResponseStream())
        using (FileStream fileStream = File.Create(filePath))
        {
            responseStream.CopyTo(fileStream);
        }
    }

    static bool IsJavaInstalled()
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "java",
                Arguments = "-version",
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process pr = Process.Start(psi))
            {
                using (StreamReader reader = pr.StandardError)
                {
                    string output = reader.ReadToEnd();
                    return output.Contains("version");
                }
            }
        }
        catch
        {
            return false;
        }
    }
}
