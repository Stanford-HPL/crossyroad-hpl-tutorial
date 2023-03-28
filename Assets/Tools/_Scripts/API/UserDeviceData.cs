using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AI.Metaviz.HPL.Demo
{
    /// <summary>
    /// information about a the device a particular user is associate with
    /// </summary>
    [DataContract]
    public class UserDeviceData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDevice" /> class.
        /// </summary>
        /// <param name="userId">userId.</param>
        /// <param name="device">device.</param>
        public UserDeviceData(string userId = default(string), DeviceData device = default(DeviceData))
        {
            this.UserId = userId;
            this.Device = device;
        }

        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        [DataMember(Name = "user_id", EmitDefaultValue = false)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or Sets Device
        /// </summary>
        [DataMember(Name = "device", EmitDefaultValue = false)]
        public DeviceData Device { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UserDevice {\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("  Device: ").Append(Device).Append("\n");
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
    /// information about a device&#39;s display
    /// </summary>
    [DataContract]
    public class DeviceData
    {
        /// <summary>
        /// platform the device runs on
        /// </summary>
        /// <value>platform the device runs on</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PlatformEnum
        {
            /// <summary>
            /// Enum IOS for value: iOS
            /// </summary>
            [EnumMember(Value = "iOS")] IOS = 1,

            /// <summary>
            /// Enum Web for value: web
            /// </summary>
            [EnumMember(Value = "web")] Web = 2,

            /// <summary>
            /// Enum Android for value: Android
            /// </summary>
            [EnumMember(Value = "Android")] Android = 3

        }

        /// <summary>
        /// platform the device runs on
        /// </summary>
        /// <value>platform the device runs on</value>
        [DataMember(Name = "platform", EmitDefaultValue = false)]
        public PlatformEnum? Platform { get; set; }

        /// <summary>
        /// input device for moving cursor and performing clicks (default &#x3D; unknown)
        /// </summary>
        /// <value>input device for moving cursor and performing clicks (default &#x3D; unknown)</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum InputDeviceEnum
        {
            /// <summary>
            /// Enum Unknown for value: unknown
            /// </summary>
            [EnumMember(Value = "unknown")] Unknown = 1,

            /// <summary>
            /// Enum Mouse for value: mouse
            /// </summary>
            [EnumMember(Value = "mouse")] Mouse = 2,

            /// <summary>
            /// Enum Trackpad for value: trackpad
            /// </summary>
            [EnumMember(Value = "trackpad")] Trackpad = 3,

            /// <summary>
            /// Enum Trackball for value: trackball
            /// </summary>
            [EnumMember(Value = "trackball")] Trackball = 4,

            /// <summary>
            /// Enum Touchscreen for value: touchscreen
            /// </summary>
            [EnumMember(Value = "touchscreen")] Touchscreen = 5

        }

        /// <summary>
        /// input device for moving cursor and performing clicks (default &#x3D; unknown)
        /// </summary>
        /// <value>input device for moving cursor and performing clicks (default &#x3D; unknown)</value>
        [DataMember(Name = "input_device", EmitDefaultValue = false)]
        public InputDeviceEnum? InputDevice { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceData" /> class.
        /// </summary>
        /// <param name="id">unique identifier for a device model.</param>
        /// <param name="platform">platform the device runs on.</param>
        /// <param name="isVirtualReality">indicates whether using a virtual reality display.</param>
        /// <param name="viewingDistanceCm">viewing distance in centimeters (default to 32F).</param>
        /// <param name="dimCm">dimCm.</param>
        /// <param name="dimPx">dimPx.</param>
        /// <param name="pixelsPerPoint">pixels per point of the screen.</param>
        /// <param name="displayPrimaries">displayPrimaries.</param>
        /// <param name="isTouchscreen">WebSDK oriented boolean - declares whether or not the device handles touchscreen events (not necessarily the input device used by the user).</param>
        /// <param name="inputDevice">input device for moving cursor and performing clicks (default &#x3D; unknown).</param>
        /// <param name="inputEvents">inputEvents.</param>
        public DeviceData(string id = default(string), PlatformEnum? platform = default(PlatformEnum?),
            bool isVirtualReality = default(bool), float viewingDistanceCm = 32F,
            Dimensions dimCm = default(Dimensions), Dimensions dimPx = default(Dimensions),
            float pixelsPerPoint = default(float),
            CieDisplayPrimaries displayPrimaries = default(CieDisplayPrimaries), bool isTouchscreen = default(bool),
            InputDeviceEnum? inputDevice = default(InputDeviceEnum?),
            InputEventStream inputEvents = default(InputEventStream))
        {
            Id = id;
            Platform = platform;
            IsVirtualReality = isVirtualReality;
            ViewingDistanceCm = viewingDistanceCm;
            DimCm = dimCm;
            DimPx = dimPx;
            PixelsPerPoint = pixelsPerPoint;
            DisplayPrimaries = displayPrimaries;
            IsTouchscreen = isTouchscreen;
            InputDevice = inputDevice;
            InputEvents = inputEvents;
        }

        /// <summary>
        /// unique identifier for a device model
        /// </summary>
        /// <value>unique identifier for a device model</value>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }


        /// <summary>
        /// indicates whether using a virtual reality display
        /// </summary>
        /// <value>indicates whether using a virtual reality display</value>
        [DataMember(Name = "is_virtual_reality", EmitDefaultValue = false)]
        public bool IsVirtualReality { get; set; }

        /// <summary>
        /// viewing distance in centimeters
        /// </summary>
        /// <value>viewing distance in centimeters</value>
        [DataMember(Name = "viewing_distance_cm", EmitDefaultValue = false)]
        public float ViewingDistanceCm { get; set; }

        /// <summary>
        /// Gets or Sets DimCm
        /// </summary>
        [DataMember(Name = "dim_cm", EmitDefaultValue = false)]
        public Dimensions DimCm { get; set; }

        /// <summary>
        /// Gets or Sets DimPx
        /// </summary>
        [DataMember(Name = "dim_px", EmitDefaultValue = false)]
        public Dimensions DimPx { get; set; }

        /// <summary>
        /// pixels per point of the screen
        /// </summary>
        /// <value>pixels per point of the screen</value>
        [DataMember(Name = "pixels_per_point", EmitDefaultValue = false)]
        public float PixelsPerPoint { get; set; }

        /// <summary>
        /// Gets or Sets DisplayPrimaries
        /// </summary>
        [DataMember(Name = "display_primaries", EmitDefaultValue = false)]
        public CieDisplayPrimaries DisplayPrimaries { get; set; }

        /// <summary>
        /// WebSDK oriented boolean - declares whether or not the device handles touchscreen events (not necessarily the input device used by the user)
        /// </summary>
        /// <value>WebSDK oriented boolean - declares whether or not the device handles touchscreen events (not necessarily the input device used by the user)</value>
        [DataMember(Name = "is_touchscreen", EmitDefaultValue = false)]
        public bool IsTouchscreen { get; set; }


        /// <summary>
        /// Gets or Sets InputEvents
        /// </summary>
        [DataMember(Name = "input_events", EmitDefaultValue = false)]
        public InputEventStream InputEvents { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class DeviceData {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Platform: ").Append(Platform).Append("\n");
            sb.Append("  IsVirtualReality: ").Append(IsVirtualReality).Append("\n");
            sb.Append("  ViewingDistanceCm: ").Append(ViewingDistanceCm).Append("\n");
            sb.Append("  DimCm: ").Append(DimCm).Append("\n");
            sb.Append("  DimPx: ").Append(DimPx).Append("\n");
            sb.Append("  PixelsPerPoint: ").Append(PixelsPerPoint).Append("\n");
            sb.Append("  DisplayPrimaries: ").Append(DisplayPrimaries).Append("\n");
            sb.Append("  IsTouchscreen: ").Append(IsTouchscreen).Append("\n");
            sb.Append("  InputDevice: ").Append(InputDevice).Append("\n");
            sb.Append("  InputEvents: ").Append(InputEvents).Append("\n");
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
    /// Tristmulus values for the given device.
    /// </summary>
    [DataContract]
    public class CieDisplayPrimaries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CieDisplayPrimaries" /> class.
        /// </summary>
        /// <param name="red">red.</param>
        /// <param name="green">green.</param>
        /// <param name="blue">blue.</param>
        public CieDisplayPrimaries(Tristimulus red = default(Tristimulus), Tristimulus green = default(Tristimulus),
            Tristimulus blue = default(Tristimulus))
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        /// <summary>
        /// Gets or Sets Red
        /// </summary>
        [DataMember(Name = "red", EmitDefaultValue = false)]
        public Tristimulus Red { get; set; }

        /// <summary>
        /// Gets or Sets Green
        /// </summary>
        [DataMember(Name = "green", EmitDefaultValue = false)]
        public Tristimulus Green { get; set; }

        /// <summary>
        /// Gets or Sets Blue
        /// </summary>
        [DataMember(Name = "blue", EmitDefaultValue = false)]
        public Tristimulus Blue { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CieDisplayPrimaries {\n");
            sb.Append("  Red: ").Append(Red).Append("\n");
            sb.Append("  Green: ").Append(Green).Append("\n");
            sb.Append("  Blue: ").Append(Blue).Append("\n");
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
    /// A single CIE 1931 tristimulus value. Y is in cd/m²
    /// </summary>
    [DataContract]
    public class Tristimulus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tristimulus" /> class.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <param name="z">z.</param>
        public Tristimulus(float x = default(float), float y = default(float), float z = default(float))
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Gets or Sets X
        /// </summary>
        [DataMember(Name = "x", EmitDefaultValue = false)]
        public float X { get; set; }

        /// <summary>
        /// Gets or Sets Y
        /// </summary>
        [DataMember(Name = "y", EmitDefaultValue = false)]
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
            sb.Append("class Tristimulus {\n");
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
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
    }

    /// <summary>
    /// Single event
    /// </summary>
    [DataContract]
    public class InputEvent
    {
        /// <summary>
        /// Type of event recorded
        /// </summary>
        /// <value>Type of event recorded</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum EventTypeEnum
        {
            /// <summary>
            /// Enum Mousemove for value: mousemove
            /// </summary>
            [EnumMember(Value = "mousemove")] Mousemove = 1,

            /// <summary>
            /// Enum Click for value: click
            /// </summary>
            [EnumMember(Value = "click")] Click = 2,

            /// <summary>
            /// Enum Touchstart for value: touchstart
            /// </summary>
            [EnumMember(Value = "touchstart")] Touchstart = 3,

            /// <summary>
            /// Enum Touchmove for value: touchmove
            /// </summary>
            [EnumMember(Value = "touchmove")] Touchmove = 4,

            /// <summary>
            /// Enum Touchend for value: touchend
            /// </summary>
            [EnumMember(Value = "touchend")] Touchend = 5,

            /// <summary>
            /// Enum Touchcancel for value: touchcancel
            /// </summary>
            [EnumMember(Value = "touchcancel")] Touchcancel = 6,

            /// <summary>
            /// Enum Scroll for value: scroll
            /// </summary>
            [EnumMember(Value = "scroll")] Scroll = 7,

            /// <summary>
            /// Enum Wheel for value: wheel
            /// </summary>
            [EnumMember(Value = "wheel")] Wheel = 8

        }

        /// <summary>
        /// Type of event recorded
        /// </summary>
        /// <value>Type of event recorded</value>
        [DataMember(Name = "event_type", EmitDefaultValue = false)]
        public EventTypeEnum? EventType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputEvent" /> class.
        /// </summary>
        /// <param name="timestamp">timestamp.</param>
        /// <param name="eventType">Type of event recorded.</param>
        /// <param name="eventPosition">eventPosition.</param>
        public InputEvent(decimal timestamp = default(decimal), EventTypeEnum? eventType = default(EventTypeEnum?),
            Position eventPosition = default(Position))
        {
            this.Timestamp = timestamp;
            this.EventType = eventType;
            this.EventPosition = eventPosition;
        }

        /// <summary>
        /// Gets or Sets Timestamp
        /// </summary>
        [DataMember(Name = "timestamp", EmitDefaultValue = false)]
        public decimal Timestamp { get; set; }


        /// <summary>
        /// Gets or Sets EventPosition
        /// </summary>
        [DataMember(Name = "event_position", EmitDefaultValue = false)]
        public Position EventPosition { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class InputEvent {\n");
            sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
            sb.Append("  EventType: ").Append(EventType).Append("\n");
            sb.Append("  EventPosition: ").Append(EventPosition).Append("\n");
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
    /// Object holding the mouse event stream.
    /// </summary>
    [DataContract]
    public class InputEventStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputEventStream" /> class.
        /// </summary>
        /// <param name="eventStream">eventStream.</param>
        public InputEventStream(List<InputEvent> eventStream = default(List<InputEvent>))
        {
            this.EventStream = eventStream;
        }

        /// <summary>
        /// Gets or Sets EventStream
        /// </summary>
        [DataMember(Name = "event_stream", EmitDefaultValue = false)]
        public List<InputEvent> EventStream { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class InputEventStream {\n");
            sb.Append("  EventStream: ").Append(EventStream).Append("\n");
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
