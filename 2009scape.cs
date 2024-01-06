using System;
using System.Diagnostics;
using System.IO;
using System.Net;

class Program
{
    static void Main()
    {
        // Define the URLs and file paths
        string jarUrl = "https://gitlab.com/2009scape/rt4-client/-/jobs/5406815814/artifacts/raw/client/build/libs/rt4-client.jar";
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
            // Download rt4-client.jar
            Console.WriteLine("Downloading rt4-client.jar...");
            using (var client = new WebClient())
            {
                client.DownloadFile(jarUrl, jarPath);
            }
            Console.WriteLine("Download complete.");

            // Write to config.json
            Console.WriteLine("Writing to config.json...");
            File.WriteAllText(configPath, configContent);
            Console.WriteLine("Write complete.");

            // Check if Java is installed
            Console.WriteLine("Checking for Java installation...");
            if (!IsJavaInstalled())
            {
                // Download Java
                Console.WriteLine("Java not found. Downloading Java installer...");
                using (var client = new WebClient())
                {
                    client.DownloadFile(javaUrl, javaInstaller);
                }
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
