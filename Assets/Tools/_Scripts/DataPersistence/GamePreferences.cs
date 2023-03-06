using System;
using System.Collections.Generic;
using UI;

namespace _Scripts.DataPersistence
{
    [Serializable]
    public class GamePreferences
    {
        [Serializable]
        public class Game1
        {
            public float targetShapeProbability = 0.8f;
            public float targetEyeProbability = 0.8f;
            public float targetColorProbability = 0.8f;
            public float targetSizeProbability = 0.8f;
            
            public float targetModelEarlySpawnTime = 10f;
            public float targetModelLateSpawnTime = 10f;
            
            public float earlySpawnTime = 0.5f;
            public float lateSpawnTime = 3f;

            public int spawnPoints = 10;
        }
        
        public Game1 game1 = new();

        [Serializable]
        public class Game2
        {
            public int aliensToSpawn = 10;
            public int timeBetweenSpawns = 1;
            public int spawnGridVertical = 10;
            public int spawnGridHorizontal = 10;
        }

        public Game2 game2 = new();

        public QuestionOptions[] questionaire = {
            new()
            {
                Question = "Hours of game play per day",
                AnswerChoices = new List<string>{"0-2 hours", "2-5 hours", "5-9 hours", "12+ hours"}
            },
            new()
            {
                Question = "Hours of total screen time per day",
                AnswerChoices = new List<string>{"0-2 hours", "2-5 hours", "5-9 hours", "12+ hours"}
            },
            new()
            {
                Question = "Hours of sleep per night",
                AnswerChoices = new List<string>{"0-1 hours", "1-2 hours", "2-3 hours", "3-4 hours", "4-5 hours", "5-6 hours", "6-7 hours", "7-8 hours", "8-9 hours", "9-10 hours", "10-11 hours", "11-12 hours", "12+ hours"}
            },
            new()
            {
                Question = "Physical Activity per week",
                AnswerChoices = new List<string>{"0-1 hours", "1-2 hours", "2-3 hours", "3-4 hours", "4+ hours"}
            },
            new()
            {
                Question = "How much water do you drink per day?",
                AnswerChoices = new List<string>{"1 cup", "2 cups", "3 cups", "4 cups", "5 cups", "6 cups", "7 cups", "8+ cups"}
            },
        };
    }
}