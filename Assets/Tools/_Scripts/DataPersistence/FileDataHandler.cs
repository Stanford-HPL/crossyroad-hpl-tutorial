using System.IO;
using UnityEditor;
using UnityEngine;

namespace _Scripts.DataPersistence
{
    public class FileDataHandler
    {
        private string _dataPath;
        private string _dataFileName;
        
        
        public FileDataHandler(string dataPath, string dataFileName)
        {
            _dataPath = dataPath;
            _dataFileName = dataFileName;
        }

        public GamePreferences Load()
        {
            string fullPath = Path.Combine(_dataPath, _dataFileName);
            GamePreferences loadedData = null;
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad;
                    using (FileStream steam = new FileStream(fullPath, FileMode.Open))
                    {
                        using(StreamReader reader = new StreamReader(steam))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    loadedData = JsonUtility.FromJson<GamePreferences>(dataToLoad);
                } catch (IOException e)
                {
                    EditorUtility.DisplayDialog("JSON Reading Error", e.Message + "\n The JSON file provided was invalid. A new JSON configuration will be generated.", "OK");
                }
            }

            return loadedData;
        }

        public void Save(GamePreferences preferences)
        {
            string fullPath = Path.Combine(_dataPath, _dataFileName);
            try
            {
                //create directory if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                
                //serialize C# object to json
                string json = JsonUtility.ToJson(preferences, true);
                
                //write to serialized json to file
                using (FileStream steam = new FileStream(fullPath, FileMode.Create))
                {
                    using(StreamWriter writer = new StreamWriter(steam))
                    {
                        writer.Write(json);
                    }
                }
            } catch (IOException e)
            {
                Debug.Log("Error saving file: " + e.Message);
            }
        }
    }
}