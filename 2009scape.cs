using System;
using System.Diagnostics;
using System.IO;
using System.Net;

class Program
{
    static void Main()
    {
        string jarUrl = "https://gitlab.com/2009scape/rt4-client/-/jobs/5406815814/artifacts/raw/client/build/libs/rt4-client.jar";
        string jarPath = "rt4-client.jar";
        string javaInstallerUrl = "https://github.com/ojdkbuild/ojdkbuild/releases/download/java-1.8.0-openjdk-1.8.0.332-1.b09-x86/java-1.8.0-openjdk-1.8.0.332-1.b09.ojdkbuild.windows.x86.msi";
        string javaInstallerPath = "java_installer.msi";

        // Download rt4-client.jar
        DownloadFile(jarUrl, jarPath);

        // Write content to config.json
        string jsonContent = @"{
  ""ip_management"": ""play.2009scape.org"",
  ""ip_address"": ""play.2009scape.org"",
  ""world"": 1,
  ""server_port"": 43594,
  ""wl_port"": 43595,
  ""js5_port"": 43595,
  ""ui_scale"": 1,
  ""fps"": 0
}";
        File.WriteAllText("config.json", jsonContent);

        // Check if Java is installed
        if (!IsJavaInstalled())
        {
            // Download Java installer
            DownloadFile(javaInstallerUrl, javaInstallerPath);

            // Run Java installer
            Process javaInstallProcess = Process.Start(javaInstallerPath);
            javaInstallProcess.WaitForExit();
        }

        // Run rt4-client.jar with Java
        Process.Start("java", "-jar " + jarPath);
    }

    static void DownloadFile(string url, string outputPath)
    {
        using (WebClient client = new WebClient())
        {
            client.DownloadFile(url, outputPath);
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
            Process proc = Process.Start(psi);
            string output = proc.StandardError.ReadToEnd();
            proc.WaitForExit();
            return output.Contains("version");
        }
        catch
        {
            return false;
        }
    }
}
