/*
 * Vizzario Insights API
 *
 * This API allows you to post psychometric data from the Vizzario Insights platform.
 *
 */

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AI.Metaviz.HPL.Demo
{
    [Serializable]
    [DataContract]
    public class EventArray
    {
        [DataMember(Name = "raw_data", EmitDefaultValue = false)]
        public List<Event> Data { get; set; }

        /// <summary>
        /// array object containing raw data
        /// </summary>
        public EventArray(List<Event> eventArray)
        {
            Data = eventArray;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static EventArray FromJson(string data)
        {
            return JsonConvert.DeserializeObject<EventArray>(data);
        }
    }

    /// <summary>
    /// A single interaction.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="eventType">determines type of event:&#x60;parent&#x60; or &#x60;child&#x60;..</param>
        /// <param name="parentId">String identifying the parent node in the event timeline. &#x60;parent_id&#x60; is unique for &#x60;parent&#x60; nodes, &#x60;child&#x60; nodes can only point to one extant &#x60;parent&#x60;..</param>
        /// <param name="taskId">determines type of task..</param>
        /// <param name="userInput">indicates whether the event was a user-initiated or software-initiated..</param>
        /// <param name="shouldRespond">indicates whether the user is expected to respond to the event. Only for &#x60;parent&#x60; nodes..</param>
        /// <param name="timeOccurred">timeOccurred.</param>
        /// <param name="foreground">foreground.</param>
        /// <param name="background">background.</param>
        /// <param name="position">position.</param>
        /// <param name="dimensions">dimensions.</param>
        public Event(EventTypeEnum eventType = default(EventTypeEnum), string parentId = default(string), string taskId = default(string), bool userInput = default(bool), bool shouldRespond = default(bool), DateTime timeOccurred = default(DateTime), Color foreground = default(Color), Color background = default(Color), Position position = default(Position), Dimensions dimensions = default(Dimensions))
        {
            EventType = eventType;
            ParentId = parentId;
            TaskId = taskId;
            UserInput = userInput;
            ShouldRespond = shouldRespond;
            TimeOccurred = timeOccurred.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            Foreground = foreground;
            Background = background;
            Position = position;
            Dimensions = dimensions;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum EventTypeEnum
        {
            /// <summary>
            /// Enum Parent for value: parent
            /// </summary>
            [EnumMember(Value = "Parent")] Parent = 1,

            /// <summary>
            /// Enum Child for value: child
            /// </summary>
            [EnumMember(Value = "Child")] Child = 2
        }

        /// <summary>
        /// determines type of event:&#x60;parent&#x60; or &#x60;child&#x60;.
        /// </summary>
        /// <value>determines type of event:&#x60;parent&#x60; or &#x60;child&#x60;.</value>
        [DataMember(Name = "event_type")]
        public EventTypeEnum EventType { get; set; }

        /// <summary>
        /// String identifying the parent node in the event timeline. &#x60;parent_id&#x60; is unique for &#x60;parent&#x60; nodes, &#x60;child&#x60; nodes can only point to one extant &#x60;parent&#x60;.
        /// A parent is the first occurrence of a stimulus. A child is a subsequent occurrence of the same stimulus, and should be linked to the parent.
        /// </summary>
        /// <value>String identifying the parent node in the event timeline. &#x60;parent_id&#x60; is unique for &#x60;parent&#x60; nodes, &#x60;child&#x60; nodes can only point to one extant &#x60;parent&#x60;.</value>
        [DataMember(Name = "parent_id")]
        public string ParentId { get; set; }

        /// <summary>
        /// determines type of task.
        /// </summary>
        /// <value>determines type of task.</value>
        [DataMember(Name = "task_id")]
        public string TaskId { get; set; }

        /// <summary>
        /// indicates whether the event was a user-initiated or software-initiated.
        /// </summary>
        /// <value>indicates whether the event was a user-initiated or software-initiated.</value>
        [DataMember(Name = "user_input")]
        public bool UserInput { get; set; }

        /// <summary>
        /// indicates whether the user is expected to respond to the event. Only for &#x60;parent&#x60; nodes.
        /// </summary>
        /// <value>indicates whether the user is expected to respond to the event. Only for &#x60;parent&#x60; nodes.</value>
        [DataMember(Name = "should_respond")]
        public bool ShouldRespond { get; set; }

        /// <summary>
        /// Gets TimeOccurred
        /// </summary>
        [DataMember(Name = "time_occurred")]
        public string TimeOccurred { get; private set; }

        /// <summary>
        /// Sets TimeOccurred
        /// </summary>
        public void SetTimeOccurred(DateTime timeOccurred)
        {
            TimeOccurred = timeOccurred.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        /// <summary>
        /// Gets or Sets Foreground
        /// </summary>
        [DataMember(Name = "foreground")]
        public Color Foreground { get; set; }

        /// <summary>
        /// Gets or Sets Background
        /// </summary>
        [DataMember(Name = "background")]
        public Color Background { get; set; }

        /// <summary>
        /// Gets or Sets Position
        /// </summary>
        [DataMember(Name = "position")]
        public Position Position { get; set; }

        /// <summary>
        /// Gets or Sets Dimensions
        /// </summary>
        [DataMember(Name = "dimensions")]
        public Dimensions Dimensions { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Event {\n");
            sb.Append("  EventType: ").Append(EventType).Append("\n");
            sb.Append("  ParentId: ").Append(ParentId).Append("\n");
            sb.Append("  TaskId: ").Append(TaskId).Append("\n");
            sb.Append("  UserInput: ").Append(UserInput).Append("\n");
            sb.Append("  ShouldRespond: ").Append(ShouldRespond).Append("\n");
            sb.Append("  TimeOccurred: ").Append(TimeOccurred).Append("\n");
            sb.Append("  Foreground: ").Append(Foreground).Append("\n");
            sb.Append("  Background: ").Append(Background).Append("\n");
            sb.Append("  Position: ").Append(Position).Append("\n");
            sb.Append("  Dimensions: ").Append(Dimensions).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    [Serializable]
    [DataContract]
    // The color the stimulus. Use RGB color types if the object is a solid fill, image_id is reccomended otherwise.
    public class Color
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Color" /> class.
        /// </summary>
        /// <param name="encoding">The type of color encoding &#x60;RGB8&#x60;, &#x60;RGB16&#x60;, &#x60;RGBfloat&#x60; or &#x60;Image&#x60;; if unused, &#x60;null&#x60;..</param>
        /// <param name="r">Red channel value, if a solid fill, 0-1.</param>
        /// <param name="g">Green channel value, if a solid fill, 0-1.</param>
        /// <param name="b">Blue channel value, if a solid fill, 0-1.</param>
        /// <param name="imageId">Optional unique image ID for sprites, generated during dev onboarding. Present in &#x60;VizzarioConfig.plist&#x60; to associate with friendly name..</param>
        /// <param name="transforms">transforms.</param>
        public Color(string encoding = "null", float r = default, float g = default, float b = default,
            string imageId = null, ImageTransformations transforms = default)
        {
            this.Encoding = encoding;
            this.R = r;
            this.G = g;
            this.B = b;
            this.ImageId = imageId;
            this.Transforms = transforms;
        }

        /// <summary>
        /// The type of color encoding &#x60;RGB8&#x60;, &#x60;RGB16&#x60;, &#x60;RGBfloat&#x60; or &#x60;Image&#x60;; if unused, &#x60;null&#x60;.
        /// </summary>
        /// <value>The type of color encoding &#x60;RGB8&#x60;, &#x60;RGB16&#x60;, &#x60;RGBfloat&#x60; or &#x60;Image&#x60;; if unused, &#x60;null&#x60;.</value>
        [DataMember(Name = "encoding")]
        public string Encoding { get; set; }

        /// <summary>
        /// Red channel value, if a solid fill, from 0-1.
        /// </summary>
        /// <value>Red channel value, if a solid fill</value>
        [DataMember(Name = "r")]
        public float R { get; set; }

        /// <summary>
        /// Green channel value, if a solid fill, from 0-1.
        /// </summary>
        /// <value>Green channel value, if a solid fill</value>
        [DataMember(Name = "g")]
        public float G { get; set; }

        /// <summary>
        /// Blue channel value, if a solid fill, from 0-1.
        /// </summary>
        /// <value>Blue channel value, if a solid fill</value>
        [DataMember(Name = "b")]
        public float B { get; set; }

        /// <summary>
        /// Optional unique image ID for sprites, generated during dev onboarding. Present in &#x60;VizzarioConfig.plist&#x60; to associate with friendly name.
        /// </summary>
        /// <value>Optional unique image ID for sprites, generated during dev onboarding. Present in &#x60;VizzarioConfig.plist&#x60; to associate with friendly name.</value>
        [DataMember(Name = "image_id", EmitDefaultValue = false)]
        public string ImageId { get; set; }

        /// <summary>
        /// Gets or Sets Transforms
        /// </summary>
        [DataMember(Name = "transforms", EmitDefaultValue = false)]
        public ImageTransformations Transforms { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Color {\n");
            sb.Append("  Encoding: ").Append(Encoding).Append("\n");
            sb.Append("  R: ").Append(R).Append("\n");
            sb.Append("  G: ").Append(G).Append("\n");
            sb.Append("  B: ").Append(B).Append("\n");
            sb.Append("  ImageId: ").Append(ImageId).Append("\n");
            sb.Append("  Transforms: ").Append(Transforms).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
    
    /// <summary>
    /// unit of measure for dimensions
    /// </summary>
    /// <value>unit of measure for dimensions</value>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UnitTypeEnum
    {
        /// <summary>
        /// Enum Points for value: points
        /// </summary>
        [EnumMember(Value = "points")] Points = 1,

        /// <summary>
        /// Enum Pixels for value: pixels
        /// </summary>
        [EnumMember(Value = "pixels")] Pixels = 2,

        /// <summary>
        /// Enum Meters for value: meters
        /// </summary>
        [EnumMember(Value = "meters")] Meters = 3,

        /// <summary>
        /// Enum Centimeters for value: centimeters
        /// </summary>
        [EnumMember(Value = "centimeters")] Centimeters = 4,

        /// <summary>
        /// Enum Cm for value: cm
        /// </summary>
        [EnumMember(Value = "cm")] Cm = 5,

        /// <summary>
        /// Enum M for value: m
        /// </summary>
        [EnumMember(Value = "m")] M = 6
    }

    [Serializable]
    [DataContract]
    public class ImageTransformations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTransformations" /> class.
        /// </summary>
        /// <param name="alpha">Opacity (alpha) of object. Range of [0, 1].</param>
        /// <param name="hue">Degrees from original hue of 0°. Range of [-180°, 180°].</param>
        /// <param name="brightness">Multiplier that describes how image brightness was altered. 1 denotes original brightness. Range of [0, +inf).</param>
        /// <param name="contrast">Multiplier that describes how image contrast was altered. 1 denotes original contrast. Range of [0, +inf).</param>
        /// <param name="saturation">Multiplier that describes how image saturation was altered. 1 denotes original saturation. Range of [0, +inf).</param>
        /// <param name="blur">Number of pixels used in a gaussian blur function. Range of [0, +inf).</param>
        public ImageTransformations(float alpha = default, float hue = default, float brightness = default,
            float contrast = default, float saturation = default, float blur = default)
        {
            this.Alpha = alpha;
            this.Hue = hue;
            this.Brightness = brightness;
            this.Contrast = contrast;
            this.Saturation = saturation;
            this.Blur = blur;
        }

        /// <summary>
        /// Opacity (alpha) of object. Range of [0, 1]
        /// </summary>
        /// <value>Opacity (alpha) of object. Range of [0, 1]</value>
        [DataMember(Name = "alpha", EmitDefaultValue = false)]
        public float Alpha { get; set; }

        /// <summary>
        /// Degrees from original hue of 0°. Range of [-180°, 180°]
        /// </summary>
        /// <value>Degrees from original hue of 0°. Range of [-180°, 180°]</value>
        [DataMember(Name = "hue", EmitDefaultValue = false)]
        public float Hue { get; set; }

        /// <summary>
        /// Multiplier that describes how image brightness was altered. 1 denotes original brightness. Range of [0, +inf)
        /// </summary>
        /// <value>Multiplier that describes how image brightness was altered. 1 denotes original brightness. Range of [0, +inf)</value>
        [DataMember(Name = "brightness", EmitDefaultValue = false)]
        public float Brightness { get; set; }

        /// <summary>
        /// Multiplier that describes how image contrast was altered. 1 denotes original contrast. Range of [0, +inf)
        /// </summary>
        /// <value>Multiplier that describes how image contrast was altered. 1 denotes original contrast. Range of [0, +inf)</value>
        [DataMember(Name = "contrast", EmitDefaultValue = false)]
        public float Contrast { get; set; }

        /// <summary>
        /// Multiplier that describes how image saturation was altered. 1 denotes original saturation. Range of [0, +inf)
        /// </summary>
        /// <value>Multiplier that describes how image saturation was altered. 1 denotes original saturation. Range of [0, +inf)</value>
        [DataMember(Name = "saturation", EmitDefaultValue = false)]
        public float Saturation { get; set; }

        /// <summary>
        /// Number of pixels used in a gaussian blur function. Range of [0, +inf)
        /// </summary>
        /// <value>Number of pixels used in a gaussian blur function. Range of [0, +inf)</value>
        [DataMember(Name = "blur", EmitDefaultValue = false)]
        public float Blur { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ImageTransformations {\n");
            sb.Append("  Alpha: ").Append(Alpha).Append("\n");
            sb.Append("  Hue: ").Append(Hue).Append("\n");
            sb.Append("  Brightness: ").Append(Brightness).Append("\n");
            sb.Append("  Contrast: ").Append(Contrast).Append("\n");
            sb.Append("  Saturation: ").Append(Saturation).Append("\n");
            sb.Append("  Blur: ").Append(Blur).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    [Serializable]
    [DataContract]
    public class Dimensions
    {
        /// <summary>
        /// unit of measure for dimensions
        /// </summary>
        /// <value>unit of measure for dimensions</value>
        [DataMember(Name = "unit_type")]
        public UnitTypeEnum? UnitType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dimensions" /> class.
        /// </summary>
        /// <param name="unitType">unit of measure for dimensions.</param>
        /// <param name="width">width.</param>
        /// <param name="height">height.</param>
        /// <param name="depth">depth.</param>
        public Dimensions(UnitTypeEnum? unitType = default(UnitTypeEnum?), float width = default(float),
            float height = default(float), float depth = default(float))
        {
            this.UnitType = unitType;
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
        }


        /// <summary>
        /// Gets or Sets Width
        /// </summary>
        [DataMember(Name = "width", EmitDefaultValue = false)]
        public float Width { get; set; }

        /// <summary>
        /// Gets or Sets Height
        /// </summary>
        [DataMember(Name = "height", EmitDefaultValue = false)]
        public float Height { get; set; }

        /// <summary>
        /// Gets or Sets Depth
        /// </summary>
        [DataMember(Name = "depth", EmitDefaultValue = false)]
        public float Depth { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Dimensions {\n");
            sb.Append("  UnitType: ").Append(UnitType).Append("\n");
            sb.Append("  Width: ").Append(Width).Append("\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  Depth: ").Append(Depth).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    [Serializable]
    [DataContract]
    public class Position
    {
        /// <summary>
        /// unit of measure for position
        /// </summary>
        /// <value>unit of measure for position</value>
        [DataMember(Name = "unit_type", EmitDefaultValue = false)]
        public UnitTypeEnum? UnitType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position" /> class.
        /// </summary>
        /// <param name="unitType">unit of measure for position.</param>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <param name="z">z.</param>
        public Position(UnitTypeEnum? unitType = default(UnitTypeEnum?), float x = default(float),
            float y = default(float), float z = default(float))
        {
            this.UnitType = unitType;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Gets or Sets X
        /// </summary>
        [DataMember(Name = "x")]
        public float X { get; set; }

        /// <summary>
        /// Gets or Sets Y
        /// </summary>
        [DataMember(Name = "y")]
        public float Y { get; set; }

        /// <summary>
        /// Gets or Sets Z
        /// </summary>
        [DataMember(Name = "z", EmitDefaultValue = false)]
        public float Z { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Position {\n");
            sb.Append("  UnitType: ").Append(UnitType).Append("\n");
            sb.Append("  X: ").Append(X).Append("\n");
            sb.Append("  Y: ").Append(Y).Append("\n");
            sb.Append("  Z: ").Append(Z).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}