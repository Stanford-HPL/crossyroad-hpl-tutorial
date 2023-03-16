using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace AI.Metaviz.HPL.Demo
{
    public class MetavizAPIManager : MonoBehaviour
    {
        public static MetavizAPIManager Instance;
        public TargetDistractorTask RawEventList = new();

        private const string baseURL = "https://insights.platform.hpl.stanford.edu";
        private const string batch_metadata_GET_path = "/metadata/batches/";

        private string client_device;
        private string batch_id = "use GetBatchID to obtain a new unique one";
        private object batch_metadata;
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
            StartCoroutine(GetBatchID());
        }


        /// <summary>
        /// Sends a GET request to the server's /metadata/batches endpoint to retrieve the batch metadata for the current batch ID.
        /// </summary>
        /// <param name="eventToPost">The event array to post to the server.</param>
        /// <param name="callback">An optional callback function that will receive the response from the server as a string.</param>
        public void BeginPostPsychometrics(EventArray eventToPost, Action<string> callback = null)
        {
            StartCoroutine(PostPsychometrics(eventToPost, callback));
        }


        /// <summary>
        /// Begins a coroutine to send a GET request to the server's /psychometrics/measures endpoint to retrieve psychometric measures calculated from the user's raw psychometric response data.
        /// </summary>
        /// <param name="callback">An optional callback function that will receive the response from the server as a string.</param>
        public void BeginGetPsychometricMeasures(Action<string> callback = null)
        {
            StartCoroutine(GetPsychometricMeasures(callback));
        }

        /// <summary>
        /// Sends a GET request to the server's /psychometrics/measures endpoint to retrieve psychometric measures calculated from the user's raw psychometric response data.
        /// </summary>
        /// <param name="callback">An optional callback function that will receive the response from the server as a string.</param>
        /// <returns>An IEnumerator object to be used in a Coroutine.</returns>
        private IEnumerator GetPsychometricMeasures(Action<string> callback = null)
        {
            using UnityWebRequest getRequest = CreateRequest(baseURL + "/psychometrics/measures");
            AttachHeader(getRequest, "batch_id", batch_id);
            yield return getRequest.SendWebRequest();
            PrintRequestFailOrSuccess(getRequest, RequestType.GET);
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
            using UnityWebRequest postRequest = CreateRequest(baseURL + "/psychometrics/stimuli-response",
                RequestType.POST, eventToPost);
            AttachHeader(postRequest, "batch_id", batch_id);
            yield return postRequest.SendWebRequest();
            PrintRequestFailOrSuccess(postRequest, RequestType.POST);
            print(eventToPost.ToJson());
            callback?.Invoke(postRequest.downloadHandler.text);
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

            Debug.LogError(
                "API Key is null and auth.json does not exist. Please check https://github.com/srcnalt/OpenAI-Unity#saving-your-credentials except use the filepath of .metaviz/authPath.json instead.");

            return null;
        }

        /// <summary>
        /// Creates a UnityWebRequest object with the specified HTTP method, endpoint, and data (if provided).
        /// </summary>
        /// <param name="path">The endpoint URL to send the request to.</param>
        /// <param name="type">The HTTP method to use (default is GET).</param>
        /// <param name="data">The data to send in the request body (if applicable).</param>
        /// <returns>A UnityWebRequest object configured with the specified parameters.</returns>
        private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
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
        /// Sends a GET request to the "/session" endpoint to retrieve the batch ID.
        /// </summary>
        /// <returns>An IEnumerator object that sends the GET request and retrieves the batch ID.</returns>
        private IEnumerator GetBatchID()
        {
            using UnityWebRequest getSessionRequest = CreateRequest(baseURL + "/session");
            AttachHeader(getSessionRequest, "client_device", client_device);
            yield return getSessionRequest.SendWebRequest();
            var deserializedGetRequestData = JsonUtility.FromJson<GetData>(getSessionRequest.downloadHandler.text);
            batch_id = deserializedGetRequestData.batch_id;
            print("The access token is: " + GetAccessToken());
            print("The Batch ID is: " + batch_id);
        }

        public IEnumerator BeginMetaDataRequestsCoroutine()
        {
            yield return StartCoroutine(PostBatchMetadata());
            StartCoroutine(GetBatchMetadata());
        }

        private IEnumerator GetBatchMetadata()
        {
            using UnityWebRequest getBatchMetadataRequest = CreateRequest(baseURL + batch_metadata_GET_path + batch_id);
            yield return getBatchMetadataRequest.SendWebRequest();

            PrintRequestFailOrSuccess(getBatchMetadataRequest, RequestType.GET);

            var deserializedGetRequestData =
                JsonUtility.FromJson<GetData>(getBatchMetadataRequest.downloadHandler.text);
            batch_metadata = deserializedGetRequestData.batch_metadata;

            // print out batch_metadata
            if (batch_metadata != null) print("The Batch Metadata is: " + batch_metadata);
            else print("The Batch Metadata is null");
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(batch_metadata))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(batch_metadata);
                Console.WriteLine("{0}={1}", name, value);
            }
        }

        private IEnumerator PostBatchMetadata()
        {
            var dataToPost = new PostMetadata(); // TODO: OBTAIN THE DATA WE NEED TO POST
            using UnityWebRequest postRequest = CreateRequest(baseURL + batch_metadata_GET_path + batch_id,
                RequestType.POST, dataToPost);
            yield return postRequest.SendWebRequest();

            PrintRequestFailOrSuccess(postRequest, RequestType.POST);

            var deserializedPostData = JsonUtility.FromJson<PostResult>(postRequest.downloadHandler.text);
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
        /// Gets the device type of the client. This is used to determine which endpoint to use for the /session GET request.
        /// </summary>
        private void GetClientDevice()
        {
            switch (SystemInfo.deviceType)
            {
                case DeviceType.Console:
                    client_device = "Console";
                    break;
                case DeviceType.Desktop:
                    client_device = "Desktop";
                    break;
                case DeviceType.Handheld:
                    client_device = "Handheld";
                    break;
                default:
                    client_device = "Unknown";
                    break;
            }
        }

        private void PrintRequestFailOrSuccess(UnityWebRequest request, RequestType type)
        {
            if ((request.result == UnityWebRequest.Result.ConnectionError) ||
                (request.result == UnityWebRequest.Result.ProtocolError) ||
                (request.result == UnityWebRequest.Result.DataProcessingError))
            {
                print(type + "  Request Failed: " + request.result);
            }
            else
            {
                print(type + " Request Successful: ");
            }

            // print what is returned in body
            print(request.downloadHandler.text);
            print("");
        }
    }


    public enum RequestType
    {
        GET = 0,
        POST = 1
    }

    public class GetData
    {
        // Ensure no getter / setter
        // Typecase has to match exactly
        public string batch_id;
        public object batch_metadata;
        public string success;
    }

    [Serializable]
    public class PostMetadata // TODO: DUMMY CLASS 
    {
        public object metadata = null;
        public string update_time = "2002-10-22T09:39:36.694Z";
    }

    public class PostResult
    {
        public string success { get; set; }

        public override string ToString()
        {
            return "success: " + success;
        }
    }
}