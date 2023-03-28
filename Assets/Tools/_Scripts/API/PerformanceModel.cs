using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AI.Metaviz.HPL.Demo
{
    [DataContract]
    public class PerformanceModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceModel" /> class.
        /// </summary>
        /// <param name="overallScore">Normalized overall VPI score.</param>
        /// <param name="scoresBreakdown">Has a score for each of the metrics..</param>
        /// <param name="dailyOverall">Daily VPI, uses date and normalized value. Only returned when a date range is provided..</param>
        /// <param name="dailyBreakdown">Array with sub-arrays for each of the metric types. Only returned when a date range is provided..</param>
        public PerformanceModel(float overallScore = default(float), List<ScoreEntryType> scoresBreakdown = default(List<ScoreEntryType>), List<ScoreEntryDate> dailyOverall = default(List<ScoreEntryDate>), List<ScoreTypeArray> dailyBreakdown = default(List<ScoreTypeArray>))
        {
            this.OverallScore = overallScore;
            this.ScoresBreakdown = scoresBreakdown;
            this.DailyOverall = dailyOverall;
            this.DailyBreakdown = dailyBreakdown;
        }

        /// <summary>
        /// Normalized overall VPI score
        /// </summary>
        /// <value>Normalized overall VPI score</value>
        [DataMember(Name="overall_score", EmitDefaultValue=false)]
        public float OverallScore { get; set; }

        /// <summary>
        /// Has a score for each of the metrics.
        /// </summary>
        /// <value>Has a score for each of the metrics.</value>
        [DataMember(Name="scores_breakdown", EmitDefaultValue=false)]
        public List<ScoreEntryType> ScoresBreakdown { get; set; }

        /// <summary>
        /// Daily VPI, uses date and normalized value. Only returned when a date range is provided.
        /// </summary>
        /// <value>Daily VPI, uses date and normalized value. Only returned when a date range is provided.</value>
        [DataMember(Name="daily_overall", EmitDefaultValue=false)]
        public List<ScoreEntryDate> DailyOverall { get; set; }

        /// <summary>
        /// Array with sub-arrays for each of the metric types. Only returned when a date range is provided.
        /// </summary>
        /// <value>Array with sub-arrays for each of the metric types. Only returned when a date range is provided.</value>
        [DataMember(Name="daily_breakdown", EmitDefaultValue=false)]
        public List<ScoreTypeArray> DailyBreakdown { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PerformanceModel {\n");
            sb.Append("  OverallScore: ").Append(OverallScore).Append("\n");
            sb.Append("  ScoresBreakdown: ").Append(ScoresBreakdown).Append("\n");
            sb.Append("  DailyOverall: ").Append(DailyOverall).Append("\n");
            sb.Append("  DailyBreakdown: ").Append(DailyBreakdown).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
    }

    /// <summary>
    /// Metadata associated with a behavior model
    /// </summary>
    [DataContract]
    public class BehaviorMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorMetadata" /> class.
        /// </summary>
        /// <param name="batchId">batch_id returned if a specific ID was requested; not returned if date range requested..</param>
        /// <param name="beginDate">beginning date, returned if requested with a range..</param>
        /// <param name="endDate">ending date, returned if requested with a range..</param>
        public BehaviorMetadata(string batchId = default(string), DateTime beginDate = default(DateTime),
            DateTime endDate = default(DateTime))
        {
            this.BatchId = batchId;
            this.BeginDate = beginDate;
            this.EndDate = endDate;
        }

        /// <summary>
        /// batch_id returned if a specific ID was requested; not returned if date range requested.
        /// </summary>
        /// <value>batch_id returned if a specific ID was requested; not returned if date range requested.</value>
        [DataMember(Name = "batch_id", EmitDefaultValue = false)]
        public string BatchId { get; set; }

        /// <summary>
        /// beginning date, returned if requested with a range.
        /// </summary>
        /// <value>beginning date, returned if requested with a range.</value>
        [DataMember(Name = "begin_date", EmitDefaultValue = false)]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// ending date, returned if requested with a range.
        /// </summary>
        /// <value>ending date, returned if requested with a range.</value>
        [DataMember(Name = "end_date", EmitDefaultValue = false)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BehaviorMetadata {\n");
            sb.Append("  BatchId: ").Append(BatchId).Append("\n");
            sb.Append("  BeginDate: ").Append(BeginDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
    }


    /// <summary>
    /// PerformanceResponse
    /// </summary>
    [DataContract]
    public class PerformanceResponse
    {
        /// <summary>
        /// Type of score returned from one of the following metrics- - &#x60;accuracy&#x60;, &#x60;detection&#x60;, &#x60;endurance&#x60;, &#x60;field_of_view&#x60;, or &#x60;multi_tracking&#x60;. Can also be a descriptive string for the &#x60;overall&#x60; type of objects in a VPI response.
        /// </summary>
        /// <value>Type of score returned from one of the following metrics- - &#x60;accuracy&#x60;, &#x60;detection&#x60;, &#x60;endurance&#x60;, &#x60;field_of_view&#x60;, or &#x60;multi_tracking&#x60;. Can also be a descriptive string for the &#x60;overall&#x60; type of objects in a VPI response.</value>

        [JsonConverter(typeof(StringEnumConverter))]

        public enum PerformanceComponents
        {
            /// <summary>
            /// Enum Fov for value: fov
            /// </summary>
            [EnumMember(Value = "fov")] Fov = 1,

            /// <summary>
            /// Enum Fieldofview for value: field_of_view
            /// </summary>
            [EnumMember(Value = "field_of_view")] Fieldofview = 2,

            /// <summary>
            /// Enum Accuracy for value: accuracy
            /// </summary>
            [EnumMember(Value = "accuracy")] Accuracy = 3,

            /// <summary>
            /// Enum Multitracking for value: multi_tracking
            /// </summary>
            [EnumMember(Value = "multi_tracking")] Multitracking = 4,

            /// <summary>
            /// Enum Endurance for value: endurance
            /// </summary>
            [EnumMember(Value = "endurance")] Endurance = 5,

            /// <summary>
            /// Enum Detection for value: detection
            /// </summary>
            [EnumMember(Value = "detection")] Detection = 6

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceResponse" /> class.
        /// </summary>
        /// <param name="metadata">metadata.</param>
        /// <param name="model">model.</param>
        public PerformanceResponse(BehaviorMetadata metadata = default(BehaviorMetadata),
            PerformanceModel model = default(PerformanceModel))
        {
            this.Metadata = metadata;
            this.Model = model;
        }

        /// <summary>
        /// Gets or Sets Metadata
        /// </summary>
        [DataMember(Name = "metadata", EmitDefaultValue = false)]
        public BehaviorMetadata Metadata { get; set; }

        /// <summary>
        /// Gets or Sets Model
        /// </summary>
        [DataMember(Name = "model", EmitDefaultValue = false)]
        public PerformanceModel Model { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PerformanceResponse {\n");
            sb.Append("  Metadata: ").Append(Metadata).Append("\n");
            sb.Append("  Model: ").Append(Model).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
    }

    /// <summary>
    /// A score entry. The &#x60;score_date&#x60; property gives the date/time the given score was accurate/updated.
    /// </summary>
    [DataContract]
    public class ScoreEntryType
    {
        /// <summary>
        /// Defines ScoreType
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ScoreTypeEnum
        {
            /// <summary>
            /// Enum Fov for value: fov
            /// </summary>
            [EnumMember(Value = "fov")] Fov = 1,

            /// <summary>
            /// Enum Fieldofview for value: field_of_view
            /// </summary>
            [EnumMember(Value = "field_of_view")] Fieldofview = 2,

            /// <summary>
            /// Enum Accuracy for value: accuracy
            /// </summary>
            [EnumMember(Value = "accuracy")] Accuracy = 3,

            /// <summary>
            /// Enum Multitracking for value: multi_tracking
            /// </summary>
            [EnumMember(Value = "multi_tracking")] Multitracking = 4,

            /// <summary>
            /// Enum Endurance for value: endurance
            /// </summary>
            [EnumMember(Value = "endurance")] Endurance = 5,

            /// <summary>
            /// Enum Detection for value: detection
            /// </summary>
            [EnumMember(Value = "detection")] Detection = 6

        }

        /// <summary>
        /// Gets or Sets ScoreType
        /// </summary>
        [DataMember(Name = "score_type", EmitDefaultValue = false)]
        public ScoreTypeEnum? ScoreType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreEntryType" /> class.
        /// </summary>
        /// <param name="scoreType">scoreType.</param>
        /// <param name="scoreValue">The normalized value of the score..</param>
        public ScoreEntryType(ScoreTypeEnum? scoreType = default(ScoreTypeEnum?), float scoreValue = default(float))
        {
            this.ScoreType = scoreType;
            this.ScoreValue = scoreValue;
        }


        /// <summary>
        /// The normalized value of the score.
        /// </summary>
        /// <value>The normalized value of the score.</value>
        [DataMember(Name = "score_value", EmitDefaultValue = false)]
        public float ScoreValue { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ScoreEntryType {\n");
            sb.Append("  ScoreType: ").Append(ScoreType).Append("\n");
            sb.Append("  ScoreValue: ").Append(ScoreValue).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
    }

    /// <summary>
    /// A score entry. The &#x60;score_date&#x60; property gives the date/time the given score was accurate/updated.
    /// </summary>
    [DataContract]
    public class ScoreEntryDate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreEntryDate" /> class.
        /// </summary>
        /// <param name="scoreDate">scoreDate.</param>
        /// <param name="scoreValue">The normalized value of the score..</param>
        public ScoreEntryDate(DateTime scoreDate = default(DateTime), float scoreValue = default(float))
        {
            this.ScoreDate = scoreDate;
            this.ScoreValue = scoreValue;
        }

        /// <summary>
        /// Gets or Sets ScoreDate
        /// </summary>
        [DataMember(Name = "score_date", EmitDefaultValue = false)]
        public DateTime ScoreDate { get; set; }

        /// <summary>
        /// The normalized value of the score.
        /// </summary>
        /// <value>The normalized value of the score.</value>
        [DataMember(Name = "score_value", EmitDefaultValue = false)]
        public float ScoreValue { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ScoreEntryDate {\n");
            sb.Append("  ScoreDate: ").Append(ScoreDate).Append("\n");
            sb.Append("  ScoreValue: ").Append(ScoreValue).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
    }

    /// <summary>
    /// Object that has an array of score entries with a &#x60;score_type&#x60; tag.
    /// </summary>
    [DataContract]
    public class ScoreTypeArray
    {
        /// <summary>
        /// Defines ScoreType
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ScoreTypeEnum
        {
            /// <summary>
            /// Enum Fov for value: fov
            /// </summary>
            [EnumMember(Value = "fov")] Fov = 1,

            /// <summary>
            /// Enum Fieldofview for value: field_of_view
            /// </summary>
            [EnumMember(Value = "field_of_view")] Fieldofview = 2,

            /// <summary>
            /// Enum Accuracy for value: accuracy
            /// </summary>
            [EnumMember(Value = "accuracy")] Accuracy = 3,

            /// <summary>
            /// Enum Multitracking for value: multi_tracking
            /// </summary>
            [EnumMember(Value = "multi_tracking")] Multitracking = 4,

            /// <summary>
            /// Enum Endurance for value: endurance
            /// </summary>
            [EnumMember(Value = "endurance")] Endurance = 5,

            /// <summary>
            /// Enum Detection for value: detection
            /// </summary>
            [EnumMember(Value = "detection")] Detection = 6

        }

        /// <summary>
        /// Gets or Sets ScoreType
        /// </summary>
        [DataMember(Name = "score_type", EmitDefaultValue = false)]
        public ScoreTypeEnum? ScoreType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreTypeArray" /> class.
        /// </summary>
        /// <param name="scoreType">scoreType.</param>
        /// <param name="scores">Array of scores.</param>
        public ScoreTypeArray(ScoreTypeEnum? scoreType = default(ScoreTypeEnum?),
            List<ScoreEntryDate> scores = default(List<ScoreEntryDate>))
        {
            this.ScoreType = scoreType;
            this.Scores = scores;
        }


        /// <summary>
        /// Array of scores
        /// </summary>
        /// <value>Array of scores</value>
        [DataMember(Name = "scores", EmitDefaultValue = false)]
        public List<ScoreEntryDate> Scores { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ScoreTypeArray {\n");
            sb.Append("  ScoreType: ").Append(ScoreType).Append("\n");
            sb.Append("  Scores: ").Append(Scores).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
