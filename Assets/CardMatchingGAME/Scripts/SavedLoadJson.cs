using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavedLoadJson : MonoBehaviour
{
  public static void SaveToJsonFile(string jsonData, string filepath, string fileName)
  {
    // Combine the persistent data path with the file name to get the full file path
    string filePath = Path.Combine(Application.persistentDataPath + filepath, fileName);
    string directoryPath = Path.GetDirectoryName(filePath);

    // Check if the directory exists, if not, create it
    if (!Directory.Exists(directoryPath))
    {
      Directory.CreateDirectory(directoryPath);
    }

    // Write the JSON string to the file
    File.WriteAllText(filePath, jsonData);

    // Optionally, log the file path for debugging purposes
    Debug.Log("Data saved to: " + filePath);
  }

  public static string LoadFromJsonFile(string filepath, string fileName)
  {
    // Combine the persistent data path with the file name to get the full file path
    string filePath = Path.Combine(Application.persistentDataPath + filepath, fileName);

    // Check if the file exists
    if (File.Exists(filePath))
    {
      // Optionally, log the file path for debugging purposes
      Debug.Log("Data loaded from: " + filePath);

      // Read the JSON string from the file
      string json = File.ReadAllText(filePath);
      return json;
    }
    else
    {
      Debug.LogWarning("File not found: " + filePath);
      return null;
    }
  }

  public static List<string> GetFilesNameSavedPath(string path)
  {
    List<string> filesname = new List<string>();

    string targetpath = Application.persistentDataPath + path;

    if (Directory.Exists(targetpath))
    {
      // Get all files in the directory
      string[] files = Directory.GetFiles(targetpath);

      // Add each file name to the list
      foreach (string file in files)
      {
        // Optionally, you can get just the file name without the full path
        string fileName = Path.GetFileName(file);
        filesname.Add(fileName);
      }
    }

    return filesname;
  }

}
