Dim WebClient
Set WebClient = CreateObject("Microsoft.XMLHTTP")
Dim WShell
Set WShell = CreateObject("WScript.Shell")

' Function to download a file from a URL
Sub DownloadFile(URL, SaveAs)
    WebClient.Open "GET", URL, False
    WebClient.Send
    If WebClient.Status = 200 Then
        Dim Stream
        Set Stream = CreateObject("ADODB.Stream")
        Stream.Open
        Stream.Type = 1 'Binary
        Stream.Write WebClient.ResponseBody
        Stream.Position = 0
        Stream.SaveToFile SaveAs, 2 'Overwrite if file exists
        Stream.Close
    End If
End Sub

' Download rt4-client.jar
Call DownloadFile("https://gitlab.com/2009scape/rt4-client/-/jobs/5406815814/artifacts/raw/client/build/libs/rt4-client.jar", "rt4-client.jar")

' Write content to config.json
Dim FileSystem, TextFile
Set FileSystem = CreateObject("Scripting.FileSystemObject")
Set TextFile = FileSystem.CreateTextFile("config.json", True)
TextFile.WriteLine("{")
TextFile.WriteLine("""ip_management"": ""play.2009scape.org"",")
TextFile.WriteLine("""ip_address"": ""play.2009scape.org"",")
TextFile.WriteLine("""world"": 1,")
TextFile.WriteLine("""server_port"": 43594,")
TextFile.WriteLine("""wl_port"": 43595,")
TextFile.WriteLine("""js5_port"": 43595,")
TextFile.WriteLine("""ui_scale"": 1,")
TextFile.WriteLine("""fps"": 0")
TextFile.WriteLine("}")
TextFile.Close

' Check if Java is installed
If Not FileSystem.FileExists("C:\Program Files\Java\jre1.8.0_332\bin\java.exe") Then
    ' Java is not installed, download and run the installer
    Call DownloadFile("https://github.com/ojdkbuild/ojdkbuild/releases/download/java-1.8.0-openjdk-1.8.0.332-1.b09-x86/java-1.8.0-openjdk-1.8.0.332-1.b09.ojdkbuild.windows.x86.msi", "java_installer.msi")
    WShell.Run "msiexec /i java_installer.msi /qn", 1, True
End If

' Run the rt4-client.jar with Java
WShell.Run """C:\Program Files\Java\jre1.8.0_332\bin\java.exe"" -jar rt4-client.jar", 1, False
