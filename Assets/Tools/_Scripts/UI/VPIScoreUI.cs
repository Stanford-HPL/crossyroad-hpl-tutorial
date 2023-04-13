using System;
using AI.Metaviz.HPL.Demo;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
// using Slider = UnityEngine.UIElements.Slider;

namespace UI
{
    public class VPIScoreUI : MonoBehaviour
    {
        public enum ScoreType
        {
            Overall,
            Accuracy,
            Multitracking,
            Endurance,
            FOV,
            Detection
        }
        public ScoreType scoreType;
        
        private const int MAX_SCORE = 175;
        
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField]private TMP_Text _labelText;
        
        private string _score;
        private string _label;

        private void Awake()
        {
            GetVPIScore();
        }

        private void GetVPIScore()
        {
            MetavizAPIManager.Instance.RawEventList.GetVpiScore(vpiScoreCallback: vpiScore =>
            {
                VPIManager.Instance.ParseVPIScore(vpiScore);
                UpdateUI();
            });
        }

        /// <summary>
        /// Gets the score from the VPIManager and updates the UI
        /// </summary>
        private void UpdateUI()
        {
            GetScore();
            if (scoreType == ScoreType.Overall)
            {
                _scoreText.text = "Your overall VPI " + _label + " : " + _score;
            }
            else
            {
                UpdateSlider();
                _scoreText.text = _score;
                _labelText.text = _label;
            }
        }

        /// <summary>
        /// Gets the score from the VPIManager
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void GetScore()
        {
            {
                switch (scoreType)
                {
                    case ScoreType.Overall:
                        _score = VPIManager.Instance.OverallVPIScore;
                        break;
                    case ScoreType.Accuracy:                        
                        _score = VPIManager.Instance.AccuracyScore;
                        break;
                    case ScoreType.Multitracking:
                        _score= VPIManager.Instance.MultitrackingScore;
                        break;
                    case ScoreType.Endurance:
                        _score = VPIManager.Instance.EnduranceScore;
                        break;
                    case ScoreType.FOV:
                        _score = VPIManager.Instance.FOVScore;
                        break;
                    case ScoreType.Detection:
                        _score =VPIManager.Instance.DetectionScore;
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
                GetLabel();
            }
        }

        /// <summary>
        /// Obtains the labels based on the score
        /// </summary>
        private void GetLabel()
        {
            if (_score == "—")
            {
                _label = "needs more play";
                return;
            }
            var score = Single.Parse(_score);
            if (score < 73) _label = "is low";
            else if (score < 91) _label = "is ok";
            else if (score < 110) _label = "is good";
            else if (score < 128) _label = "is great";
            else if (score >= 128) _label = "is marvelous";
        }

        /// <summary>
        /// For those that user the slider, then update the slider accordingly
        /// </summary>
        private void UpdateSlider()
        {
            var slider = gameObject.GetComponent<Slider>();
            if (_score != "—") slider.value = Single.Parse(_score) / MAX_SCORE;
        }
    }
}
