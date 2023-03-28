using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AI.Metaviz.HPL.Demo
{
    /// <summary>
    /// Generic metadata to associate with a batch
    /// </summary>
    [DataContract]
    public class BatchMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchMetadata" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected BatchMetadata() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchMetadata" /> class.
        /// </summary>
        /// <param name="metadata">metadata (required).</param>
        /// <param name="updateTime">updateTime.</param>
        public BatchMetadata(Dictionary<string, Object> metadata = default(Dictionary<string, Object>),
            DateTime updateTime = default(DateTime))
        {
            // to ensure "metadata" is required (not null)
            if (metadata == null)
            {
                throw new InvalidDataException("metadata is a required property for BatchMetadata and cannot be null");
            }
            else
            {
                this.Metadata = metadata;
            }

            this.UpdateTime = updateTime;
        }

        /// <summary>
        /// Gets or Sets Metadata
        /// </summary>
        [DataMember(Name = "metadata", EmitDefaultValue = true)]
        public Dictionary<string, Object> Metadata { get; set; }

        /// <summary>
        /// Gets or Sets UpdateTime
        /// </summary>
        [DataMember(Name = "update_time", EmitDefaultValue = false)]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BatchMetadata {\n");
            sb.Append("  Metadata: ").Append(Metadata).Append("\n");
            sb.Append("  UpdateTime: ").Append(UpdateTime).Append("\n");
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

