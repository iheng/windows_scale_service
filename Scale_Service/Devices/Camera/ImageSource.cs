using System;
using System.IO;
using System.Security.Permissions;
using System.Drawing;
using System.Configuration;
public class ImageWatcher
{
    public delegate string UploadImageDelegate(string imgBase64);
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public static void Init()
    {
       
        // Create a new FileSystemWatcher and set its properties.
        FileSystemWatcher watcher = new FileSystemWatcher();
        watcher.Path = ConfigurationManager.AppSettings["path"].ToString(); 
       
        /* Watch for changes in LastAccess and LastWrite times, and
           the renaming of files or directories. */
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        // Only watch text files.
        watcher.Filter = "*.*";

        // Add event handlers.
        //watcher.Changed += new FileSystemEventHandler(OnChanged);
        watcher.Created += new FileSystemEventHandler(OnChanged);
        watcher.Deleted += new FileSystemEventHandler(OnChanged);

        // Begin watching.
        watcher.EnableRaisingEvents = true;

    }

    // Define the event handlers.
    private static void OnChanged(object source, FileSystemEventArgs e)
    {
        // Specify what is done when a file is changed, created, or deleted.
        //string dirPath = Path.GetDirectoryName(e.FullPath);
        //LoadImage(e.FullPath);
    /*
    int imgcout = FileCount(dirPath);
    if (imgcout >= 3)
    {
        DeleteFile(dirPath);

    }
    Console.WriteLine(imgcout);
    */

    }
    private static string LoadImage(UploadImageDelegate loadIMG,string path)
    {
        byte[] imageArray = File.ReadAllBytes(path);
        string base64Image = Convert.ToBase64String(imageArray);
        return loadIMG(base64Image);
    }

    private static int FileCount(string path)
    {
        System.IO.DirectoryInfo dir = new DirectoryInfo(path);
        return dir.GetFiles().Length;
    }
    private static void DeleteFile(string path)
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(path);
        foreach (FileInfo file in di.EnumerateFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.EnumerateDirectories())
        {
            dir.Delete(true);
        }

    }
}
