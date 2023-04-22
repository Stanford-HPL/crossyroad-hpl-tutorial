using System;
using System.Collections;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR;

namespace AI.Metaviz.HPL.Demo
{
    public class MetavizAPIManager : MonoBehaviour
    {
        public static MetavizAPIManager Instance;
        
        public readonly TargetDistractorTask RawEventList = new();

        private const string BaseURL = "https://insights.platform.hpl.stanford.edu";
        private string _batchID = "use GetBatchID to obtain a new unique one";
        private string _clientDevice;
        
        public string ApiKey { get; set; }

        /// <summary>
        /// Enables the MetavizAPIManager to persist across scenes as a singleton.
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
        /// Gets the client device's unique ID and stores it in the client_device variable.
        /// Gets the current batch ID and stores it in the batch_id variable.
        /// </summary>
        private void Start()
        {
            GetClientDevice();
            StartCoroutine(BeginSession());
        }
        
        /// <summary>
        /// Sends a POST request to the server's /psychometric/measures endpoint to calculate psychometric measures from the user's raw psychometric response data.
        /// </summary>
        /// <param name="eventToPost">The event array to post to the server.</param>
        /// <param name="callback">An optional callback function that will receive the response from the server as a string.</param>
        public void BeginPostPsychometrics(EventArray eventToPost, Action<string> callback = null)
        {
            StartCoroutine(PostPsychometrics(eventToPost, callback));
        }
        
        /// <summary>
        /// Begins a coroutine to send a GET request to the server's /behaviors/performance endpoint to retrieve performance model measures calculated from the user's raw psychometric response data.
        /// </summary>
        /// <param name="callback">An optional callback function that will receive the response from the server as a string.</param>
        public void BeginGetPerformanceModel(Action<string> callback = null)
        {
            StartCoroutine(GetPerformanceModel(callback));
        }
        
        /// <summary>
        /// Sends a POST request to the server's /metadata/batches/{batch_id} endpoint to post the questionnaire to the server.
        /// </summary>
        /// <param name="batchMetadataToPost">The questionnaire metadata to post to the server.</param>
        public void BeginPostBatchMetadata(BatchMetadata batchMetadataToPost)
        {
            StartCoroutine(PostBatchMetadata(batchMetadataToPost));
        }
        
        /// <summary>
        /// Sends a GET request to the server's /metadata/batches/{batch_id} endpoint to receive the questionnaire on the server.
        /// </summary>
        /// <param name="callback">An optional callback function that will receive the response from the server as a string.</param>
        public void BeginGetBatchMetadata(Action<string> callback = null)
        {
            StartCoroutine(GetBatchMetadata(callback));
        }
        
        /// <summary>
        /// Reads the access token from the auth.json file and returns it.
        /// The auth.json file should contain the access token on the first line.
        /// It should be located in the .metaviz folder in the user's home directory.
        /// The .metaviz folder should be created if it does not exist.
        /// The user's home directory is ~/ on Linux and Mac and C:\Users\{username} on Windows.
        /// </summary>
        /// <returns> The access token or null if the file does not exist. </returns>
        private string GetAccessToken()
        {
            var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var authPath = $"{userPath}/.metaviz/auth.json";
            if (File.Exists(authPath))
            {
                return File.ReadAllText(authPath).Trim();
            }

            Debug.LogError("API Key is null and auth.json does not exist. Please check https://github.com/srcnalt/OpenAI-Unity#saving-your-credentials except use the filepath of .metaviz/authPath.json instead.");

            return null;
        }
        
        /// <summary>
        /// When a new session begins, obtain the batch ID and post the device data to the server's /device endpoint.
        /// </summary>
        private IEnumerator BeginSession()
        {
            yield return StartCoroutine(GetBatchID());
            DeviceData deviceData = GetUserDeviceData();
            StartCoroutine(PostDevice(deviceData));
        }
        
        /// <summary>
        /// Posts Device Data to the server's /device endpoint via a POST request using a UnityWebRequest object.
        /// The result of the post is passed to an optional callback function.
        /// </summary>
        /// <param name="deviceDataToPost">The DeviceData object to be posted to the endpoint.</param>
        /// <param name="callback">An optional callback function that will receive the PostResult object returned from the server.</param>
        /// <returns>An IEnumerator object to be used in a Coroutine.</returns>
        private IEnumerator PostDevice(DeviceData deviceDataToPost, Action<string> callback = null)
        {
            using UnityWebRequest postDeviceRequest = CreateRequest(BaseURL + "/device",
                RequestType.Post, deviceDataToPost);
            AttachHeader(postDeviceRequest, "batch_id", _batchID);
            AttachHeader(postDeviceRequest, "client_device", _clientDevice);
            yield return postDeviceRequest.SendWebRequest();
            AssertRequestSuccessful(postDeviceRequest, RequestType.Post);
            callback?.Invoke(postDeviceRequest.downloadHandler.text);
        }
        
        /// <summary>
        /// Posts BatchMetadata to the server's /metadata/batches/{batch_id} endpoint via a POST request using a UnityWebRequest object.
        /// The result of the post is passed to an optional callback function.
        /// </summary>
        /// <param name="batchMetadataToPost">The BatchMetadataToPost object to be posted to the endpoint.</param>
        /// <returns>An IEnumerator object to be used in a Coroutine.</returns>
        public IEnumerator PostBatchMetadata(BatchMetadata batchMetadataToPost)
        {
            using UnityWebRequest postBatchMetadataRequest = CreateRequest(BaseURL + "/metadata/batches/" + _batchID,
                RequestType.Post, batchMetadataToPost);
            yield return postBatchMetadataRequest.SendWebRequest();
            
            AssertRequestSuccessful(postBatchMetadataRequest, RequestType.Post);
        }
        
        /// <summary>
        /// Sends a GET request to the server's /metadata/batches/{batch_id} endpoint to retrieve the metadata for the current batch.
        /// </summary>
        /// <param name="callback">An optional callback function that will receive the response from the server as a string.</param>
        /// <returns>An IEnumerator object to be used in a Coroutine.</returns>
        private IEnumerator GetBatchMetadata(Action<string> callback = null)
        {
            using UnityWebRequest getRequest = CreateRequest(BaseURL + "/metadata/batches/" + _batchID);
            yield return getRequest.SendWebRequest();
            AssertRequestSuccessful(getRequest, RequestType.Get);
            callback?.Invoke(getRequest.downloadHandler.text);
        }

        
        /// <summary>
        /// Sends a GET request to the "/session" endpoint to retrieve the batch ID.
        /// </summary>
        /// <returns>An IEnumerator object that sends the GET request and retrieves the batch ID.</returns>
        private IEnumerator GetBatchID()
        {
            using UnityWebRequest getSessionRequest = CreateRequest(BaseURL + "/session");
            AttachHeader(getSessionRequest, "client_device", _clientDevice);
            yield return getSessionRequest.SendWebRequest();
            var deserializedGetRequestData = JsonUtility.FromJson<SessionData>(getSessionRequest.downloadHandler.text);
            _batchID = deserializedGetRequestData.batch_id;
        }

        /// <summary>
        /// Sends a GET request to the server's /behaviors/performance endpoint to retrieve performance model calculated from the user's raw psychometric response data.
        /// </summary>
        /// <param name="callback">An optional callback function that will receive the response from the server as a string.</param>
        /// <returns>An IEnumerator object to be used in a Coroutine.</returns>
        private IEnumerator GetPerformanceModel(Action<string> callback = null)
        {
            using UnityWebRequest getRequest = CreateRequest(BaseURL + "/behaviors/performance");
            AttachHeader(getRequest, "batch_id", _batchID);
            yield return getRequest.SendWebRequest();
            AssertRequestSuccessful(getRequest, RequestType.Get);
            callback?.Invoke(getRequest.downloadHandler.text);
        }

        /// <summary>
        /// Posts raw stimuli-response psychometric data to the server's /psychometrics/stimuli-response endpoint via a POST request using a UnityWebRequest object.
        /// The result of the post is passed to an optional callback function.
        /// </summary>
        /// <param name="eventToPost">The Event object to be posted to the endpoint.</param>
        /// <param name="callback">An optional callback function that will receive the PostResult object returned from the server.</param>
        /// <returns>An IEnumerator object to be used in a Coroutine.</returns>
        private IEnumerator PostPsychometrics(EventArray eventToPost, Action<string> callback = null)
        {
            using UnityWebRequest postRequest = CreateRequest(BaseURL + "/psychometrics/stimuli-response",
                RequestType.Post, eventToPost);
            AttachHeader(postRequest, "batch_id", _batchID);
            yield return postRequest.SendWebRequest();
            AssertRequestSuccessful(postRequest, RequestType.Post);
            callback?.Invoke(postRequest.downloadHandler.text);
        }

        /// <summary>
        /// Creates a UnityWebRequest object with the specified HTTP method, endpoint, and data (if provided).
        /// </summary>
        /// <param name="path">The endpoint URL to send the request to.</param>
        /// <param name="type">The HTTP method to use (default is GET).</param>
        /// <param name="data">The data to send in the request body (if applicable).</param>
        /// <returns>A UnityWebRequest object configured with the specified parameters.</returns>
        private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.Get, object data = null)
        {
            var request = new UnityWebRequest(path, type.ToString());

            if (data != null)
            {
                var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, Formatting.Indented));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            AttachHeader(request, "Content-Type", "application/json");
            AttachHeader(request, "Authorization", "Bearer " + (GetAccessToken() != null ? GetAccessToken() : ApiKey));
            return request;
        }

        /// <summary>
        /// Attaches a header to a UnityWebRequest object.
        /// </summary>
        /// <param name="request"> The UnityWebRequest object to which the header will be attached. </param>
        /// <param name="key"> The key of the header to be attached. </param>
        /// <param name="value"> The value of the header to be attached. </param>
        private void AttachHeader(UnityWebRequest request, string key, string value)
        {
            request.SetRequestHeader(key, value);
        }

        /// <summary>
        /// Gets the device type of the client.
        /// This is used to determine which endpoint to use for the /session GET request.
        /// This is also used as a header for the POST request to the /session endpoint.
        /// </summary>
        private void GetClientDevice()
        {
            _clientDevice = SystemInfo.deviceType switch
            {
                DeviceType.Console => "Console",
                DeviceType.Desktop => "Desktop",
                DeviceType.Handheld => "Handheld",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Gets the device platform (Web, Android, IOS). This is used for the POST to the /session GET request.
        /// </summary>
        /// <returns>Device Platform through the DeviceData.PlatformEnum</returns>
        private DeviceData.PlatformEnum GetDevicePlatform()
        {
            return Application.platform switch
            {
                RuntimePlatform.Android => DeviceData.PlatformEnum.Android,
                RuntimePlatform.IPhonePlayer => DeviceData.PlatformEnum.IOS,
                _ => DeviceData.PlatformEnum.Web
            };
        }

        /// <summary>
        /// Creates the DeviceData object for the POST request to the /session endpoint. 
        /// </summary>
        /// <returns>Device Data</returns>
        private DeviceData GetUserDeviceData()
        {
            const float convertInchToCm = 2.54F;

            return new DeviceData(
                id: SystemInfo.deviceUniqueIdentifier,
                platform: GetDevicePlatform(),
                isVirtualReality: XRSettings.isDeviceActive,
                dimCm: new Dimensions(UnitTypeEnum.Centimeters,
                    Screen.width / Screen.dpi * convertInchToCm, Screen.height/Screen.dpi * convertInchToCm),
                dimPx: new Dimensions(UnitTypeEnum.Pixels, Screen.width, Screen.height),
                pixelsPerPoint: 1F
            );
        }

        /// <summary>
        /// Asserts whether the request has failed or successful. If failed, then the error is logged in the console.
        /// </summary>
        /// <param name="request">The request that is being sent</param>
        /// <param name="type">Determines the type of the rest bia the RequestType Enum</param>
        private void AssertRequestSuccessful(UnityWebRequest request, RequestType type)
        {
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                // log error
				Debug.LogError(request.result);
            }
        }
    }
    
    public enum RequestType
    {
        Get = 0,
        Post = 1
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class SessionData
    {
        // Ensure no getter / setter
        // Uppercase / lowercase has to match exactly
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnassignedField.Global
        public string batch_id;
    }
}