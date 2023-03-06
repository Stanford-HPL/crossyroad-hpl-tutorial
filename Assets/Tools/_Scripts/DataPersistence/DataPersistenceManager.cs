using System;
using System.IO;
using UnityEngine;

namespace _Scripts.DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")] [SerializeField]
        private string fileName;

        private GamePreferences _gamePreferences;
        private FileDataHandler _dataHandler;
        public static DataPersistenceManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            var dataPath = Path.Combine(Environment.CurrentDirectory, "Assets");
            _dataHandler = new FileDataHandler(dataPath, fileName);
            LoadGame();
        }

        

        public void NewGame()
        {
            _gamePreferences = new GamePreferences();
            SaveGame();
        }

        public void LoadGame()
        {

            _gamePreferences = _dataHandler.Load();

            if (_gamePreferences == null)
            {
                NewGame();
            }
        }

        public static GamePreferences GetPreferences()
        {
            return Instance._gamePreferences;
        }

        public void SaveGame()
        {
            /*foreach (IDataPersistence dataPersistence in _dataPersistences)
            {
                dataPersistence.SaveData(ref _gamePreferences);
            }*/
            
            _dataHandler.Save(_gamePreferences);
        }
    }
}