# Ensure PowerShell can use Net.WebClient for downloading files
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# Function to download a file
Function Download-File([string]$url, [string]$path) {
    $webClient = New-Object System.Net.WebClient
    $webClient.DownloadFile($url, $path)
}

# File URLs and paths
$jarUrl = "https://gitlab.com/2009scape/rt4-client/-/jobs/5406815814/artifacts/raw/client/build/libs/rt4-client.jar"
$jarPath = "rt4-client.jar"
$javaInstallerUrl = "https://github.com/ojdkbuild/ojdkbuild/releases/download/java-1.8.0-openjdk-1.8.0.332-1.b09-x86/java-1.8.0-openjdk-1.8.0.332-1.b09.ojdkbuild.windows.x86.msi"
$javaInstallerPath = "java_installer.msi"

# Download rt4-client.jar
Download-File -url $jarUrl -path $jarPath

# Create config.json with the provided content
$configContent = @"
{
  "ip_management": "play.2009scape.org",
  "ip_address": "play.2009scape.org",
  "world": 1,
  "server_port": 43594,
  "wl_port": 43595,
  "js5_port": 43595,
  "ui_scale": 1,
  "fps": 0
}
"@
$configPath = "config.json"
$configContent | Out-File -FilePath $configPath

# Check if Java is installed
$javaVersion = (java -version 2>&1)
if($javaVersion -like "*'java' is not recognized*") {
    # Java is not installed, download and install Java
    Write-Host "Java is not installed. Downloading and installing Java..."
    Download-File -url $javaInstallerUrl -path $javaInstallerPath
    Start-Process -FilePath $javaInstallerPath -Wait
} else {
    Write-Host "Java is already installed."
}

# Run rt4-client.jar with Java
java -jar $jarPath
