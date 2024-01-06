using System;
using System.Diagnostics;
using System.IO;
using System.Net;

class RT4ClientHandler
{
    static void Main()
    {
        string jarUrl = "https://gitlab.com/2009scape/rt4-client/-/jobs/5406815814/artifacts/raw/client/build/libs/rt4-client.jar";
        string jarPath = "rt4-client.jar";
        string configContent = @"{
  ""ip_management"": ""play.2009scape.org"",
  ""ip_address"": ""play.2009scape.org"",
  ""world"": 1,
  ""server_port"": 43594,
  ""wl_port"": 43595,
  ""js5_port"": 43595,
  ""ui_scale"": 1,
  ""fps"": 0
}";
        string configPath = "config.json";
        string javaDownloadUrl = "https://github.com/ojdkbuild/ojdkbuild/releases/download/java-1.8.0-openjdk-1.8.0.332-1.b09-x86/java-1.8.0-openjdk-1.8.0.332-1.b09.ojdkbuild.windows.x86.msi";
        string javaInstallPath = "java_installer.msi";

        try
        {
            Console.WriteLine("Starting the RT4 Client Handler.");

            // Download rt4-client.jar
            Console.WriteLine($"Downloading rt4-client.jar from {jarUrl}");
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(jarUrl, jarPath);
            }
            Console.WriteLine("rt4-client.jar downloaded successfully.");

            // Write config.json
            Console.WriteLine("Writing config.json file.");
            File.WriteAllText(configPath, configContent);
            Console.WriteLine("config.json written successfully.");

            // Check for Java installation
            Console.WriteLine("Checking for Java installation.");
            if (!IsJavaInstalled())
            {
                Console.WriteLine("Java is not installed. Downloading installer.");
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(javaDownloadUrl, javaInstallPath);
                }
                Console.WriteLine("Java installer downloaded. Installing Java.");
                Process javaInstallProcess = Process.Start(javaInstallPath);
                javaInstallProcess.WaitForExit();
                Console.WriteLine("Java installed successfully.");
            }
            else
            {
                Console.WriteLine("Java is already installed.");
            }

            // Run rt4-client.jar with Java
            Console.WriteLine("Running rt4-client.jar with Java.");
            Process javaProcess = Process.Start("java", "-jar " + jarPath);
            javaProcess.WaitForExit();
            Console.WriteLine("rt4-client.jar execution finished.");

            Console.WriteLine("RT4 Client Handler completed successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }

    static bool IsJavaInstalled()
    {
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "java";
            process.StartInfo.Arguments = "-version";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            string output = process.StandardError.ReadToEnd();
            process.WaitForExit();
            return output.Contains("version");
        }
        catch
        {
            return false;
        }
    }
}
