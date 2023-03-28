using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class VPIManager : MonoBehaviour
{
    public static VPIManager Instance;
    
    public string OverallVPIScore { get; private set; }
    public string AccuracyScore { get; private set; }
    public string DetectionScore { get; private set; }
    public string EnduranceScore { get; private set; }
    public string FOVScore { get; private set; }
    public string MultitrackingScore { get; private set; }
    
    /// <summary>
    /// Makes GameManager a singleton
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// Parses through the VPI score and assigns the values to the appropriate variables
    /// </summary>
    /// <param name="vpiScore">JSON String representation of the VPI Score from the GET Behavior Performance Model endpoint</param>
    public void ParseVPIScore(string vpiScore)
    {
        var root = JsonConvert.DeserializeObject<Root>(vpiScore);
        OverallVPIScore = ((int)root.model.overall_score).ToString();
        foreach (var score in root.model.scores_breakdown)
        {
            switch (score.score_type)
            {
                case "accuracy":
                    if (score.score_value == 0) AccuracyScore = "—";
                    else AccuracyScore = ((int)score.score_value).ToString();
                    break;
                case "detection":
                    if (score.score_value == 0) DetectionScore = "—";
                    else DetectionScore = ((int)score.score_value).ToString();
                    break;
                case "endurance":
                    if (score.score_value == 0) EnduranceScore = "—";
                    else EnduranceScore = ((int)score.score_value).ToString();
                    break;
                case "field_of_view":
                    if (score.score_value == 0) FOVScore = "—";
                    else FOVScore = ((int)score.score_value).ToString();
                    break;
                case "multi_tracking":
                    if (score.score_value == 0) MultitrackingScore = "—";
                    else MultitrackingScore = ((int)score.score_value).ToString();
                    break;
            }
        }
    }
}
public class Metadata
{
    public string batch_id { get; set; }
}

public class Model
{
    public double overall_score { get; set; }
    public List<ScoresBreakdown> scores_breakdown { get; set; }
}

public class Root
{
    public Metadata metadata { get; set; }
    public Model model { get; set; }
}

public class ScoresBreakdown
{
    public string score_type { get; set; }
    public double score_value { get; set; }
}
