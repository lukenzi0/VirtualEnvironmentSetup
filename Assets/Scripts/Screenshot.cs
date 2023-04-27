using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public string filepath = @"L:\Project\Assets"; //Filepath to where you want to store your images
    private void Start()
    {   
        // Delete all previous local storage of images to avoid confliction
       if (File.Exists(filepath + @"\query.png"))
        {
            File.Delete(filepath + @"\query.png");
        }
       else if(File.Exists(filepath + @"\reference.png"))
        {
            File.Delete(filepath + @"\reference.png");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(CoroutineScreenshot(filepath + @"\query.png"));
            UnityEngine.Debug.Log("Query screenshot has been taken");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(CoroutineScreenshot(filepath + @"\reference.png"));
            UnityEngine.Debug.Log("Reference screenshot has been taken");
        }
    }

    //This method takes a screenshot by drawing the frame fully before taking the screenshot
    //Building a 2D texture from the pixels with the borders determined by the screen size
    //Encoding the 2D texture to PNG
    private IEnumerator CoroutineScreenshot(string fileToSave)
    {
        yield return new WaitForEndOfFrame();

        //This gets the screen width and height regardless of resolution
        int width = Screen.width;
        int height = Screen.height;

        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false); //Builds empty texture 
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0); //Uses the rectangle the size of the screen to say where to get the image pixels from
        screenshotTexture.Apply();

        byte[] byteArray = screenshotTexture.EncodeToPNG();
        File.WriteAllBytes(fileToSave, byteArray); //Saves the png locally 
        saveToGoogleDrive(Path.GetFileName(fileToSave)); //Calls the method to upload the local file to google drive
    }
    //Starts a new python process, takes the script and filename to used as arguments for the python file which will upload the file
    private static void saveToGoogleDrive(string fileToSave)
    {
        var psi = new ProcessStartInfo();
        psi.FileName = @"C:\Users\luken\AppData\Local\Programs\Python\Python311\python.exe"; //The file location of the python application
        var script = @"L:\Project\Assets\GoogleDrive_Upload.py"; //The file location of the python file for uploading to google drive
        var file = fileToSave;

        psi.Arguments = $"\"{script}\" \"{file}\""; //Where the arguments are populated
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;
        psi.RedirectStandardError = true;

        var errors = "";

        using (var process = Process.Start(psi)) 
        {
            errors = process.StandardError.ReadToEnd();
        }

        UnityEngine.Debug.Log(errors);
    }
}
