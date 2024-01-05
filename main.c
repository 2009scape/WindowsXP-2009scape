#include <stdio.h>
#include <stdlib.h>

void downloadFile(const char *url, const char *filepath);
int isJavaInstalled();
void installJava();
void writeConfigFile();
void runJar();

int main() {
    // Download rt4-client.jar
    downloadFile("https://gitlab.com/2009scape/rt4-client/-/jobs/5406815814/artifacts/raw/client/build/libs/rt4-client.jar", "rt4-client.jar");

    // Write config.json
    writeConfigFile();

    // Check if Java is installed
    if (!isJavaInstalled()) {
        // Download and install Java
        installJava();
    }

    // Run rt4-client.jar with Java
    runJar();

    return 0;
}

void downloadFile(const char *url, const char *filepath) {
    char command[1024];
    // Using Windows 'certutil' command to download files. This is not standard and might not work in all environments.
    sprintf(command, "certutil -urlcache -split -f \"%s\" \"%s\"", url, filepath);
    system(command);
}

int isJavaInstalled() {
    // A very naive way of checking if Java is installed by trying to run 'java -version' and checking the return value.
    return system("java -version") == 0;
}

void installJava() {
    // Download and run the Java installer
    downloadFile("https://github.com/ojdkbuild/ojdkbuild/releases/download/java-1.8.0-openjdk-1.8.0.332-1.b09-x86/java-1.8.0-openjdk-1.8.0.332-1.b09.ojdkbuild.windows.x86.msi", "java_installer.msi");
    system("msiexec /i java_installer.msi /qn");
}

void writeConfigFile() {
    FILE *file = fopen("config.json", "w");
    if (file != NULL) {
        fputs("{\n  \"ip_management\": \"play.2009scape.org\",\n  \"ip_address\": \"play.2009scape.org\",\n  \"world\": 1,\n  \"server_port\": 43594,\n  \"wl_port\": 43595,\n  \"js5_port\": 43595,\n  \"ui_scale\": 1,\n  \"fps\": 0\n}", file);
        fclose(file);
    }
}

void runJar() {
    system("java -jar rt4-client.jar");
}
