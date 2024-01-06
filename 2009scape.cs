using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

class Program
{
    static void Main()
    {
        Console.WriteLine("Start...");
        // Define the file paths
        string jarPath = "rt4-client.jar";
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
            // Extract resources
            ExtractResource("Namespace.rt4-client.jar", jarPath);
            ExtractResource("Namespace.java_installer.msi", javaInstaller);

            // Write to config.json
            Console.WriteLine("Writing to config.json...");
            File.WriteAllText(configPath, configContent);
            Console.WriteLine("Write complete.");

            // Check if Java is installed
            Console.WriteLine("Checking for Java installation...");
            if (!IsJavaInstalled())
            {
                Console.WriteLine("Java not found. Installing Java...");
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

    static void ExtractResource(string resourceName, string filePath)
    {
        if (!File.Exists(filePath))
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                stream.CopyTo(fileStream);
            }
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
