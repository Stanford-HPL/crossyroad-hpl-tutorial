using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace AI.Metaviz.HPL.Demo
{
    public class MetavizAPIManager : MonoBehaviour
    {
        public static MetavizAPIManager Instance;
        
        private const string baseURL = "https://insights.platform.hpl.stanford.edu";
        private const string batch_metadata_GET_path = "/metadata/batches/";

        private string client_device;
        private string batch_id = "use GetBatchID to obtain a new unique one";
        private object batch_metadata;

        private const string access_token =
            "eyJraWQiOiJJN0l2YmNqT2w2SWdzTU5EQlE5cUpJV3NIQlwvNlB6MUhcL0RhZzQyZTFSUXc9IiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJ0bWlmcGVxMDkyN3AybDB0MGZnaWVpYzFzIiwidG9rZW5fdXNlIjoiYWNjZXNzIiwic2NvcGUiOiJodHRwczpcL1wvcGxhdGZvcm0udml6emFyaW8uY29tXC9iZWhhdmlvcnMucmVhZCBodHRwczpcL1wvcGxhdGZvcm0udml6emFyaW8uY29tXC9yZWNvbW1lbmRhdGlvbi5yZWFkIGh0dHBzOlwvXC9wbGF0Zm9ybS52aXp6YXJpby5jb21cL2Jpb21ldHJpY3Mud3JpdGUgaHR0cHM6XC9cL3BsYXRmb3JtLnZpenphcmlvLmNvbVwvcHN5Y2hvbWV0cmljcy53cml0ZSBodHRwczpcL1wvcGxhdGZvcm0udml6emFyaW8uY29tXC9iaW9tZXRyaWNzLnJlYWQgaHR0cHM6XC9cL3BsYXRmb3JtLnZpenphcmlvLmNvbVwvbWV0YWRhdGEud3JpdGUgaHR0cHM6XC9cL3BsYXRmb3JtLnZpenphcmlvLmNvbVwvbWV0YWRhdGEucmVhZCBodHRwczpcL1wvcGxhdGZvcm0udml6emFyaW8uY29tXC9wc3ljaG9tZXRyaWNzLnJlYWQiLCJhdXRoX3RpbWUiOjE2NzgxNDQyMDAsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy13ZXN0LTIuYW1hem9uYXdzLmNvbVwvdXMtd2VzdC0yX2pzYnRQR3JKdSIsImV4cCI6MTY3ODE0NzgwMCwiaWF0IjoxNjc4MTQ0MjAwLCJ2ZXJzaW9uIjoyLCJqdGkiOiI0MTE0OGEzZi03ZGRlLTRiNGYtYTc4Ni1lZmRiNDkzMzIyNjgiLCJjbGllbnRfaWQiOiJ0bWlmcGVxMDkyN3AybDB0MGZnaWVpYzFzIn0.ds-zMPjQv8_faCEycqcMOr4Ih5OLIZQxe8oKduvRpGt8U_u2yuCEp8s9drRcXYGJ-j0NML0FeQvDulz39DHnn8k_9TtrSpNTA2YXMCnzTOxI6gy2Ox2D5ykHrBS1cMpTV5HZji9TXM1cwaxEg9cjrWzGClHWZYXAWFjBDgJ4vsZoRADh6n75OF2qhK9kIerEMU0H1vTq480EOmEKo9Q_uoWXtZGgdu8jHZEZkIAsgwRtyJbDrGk7p3BUKcs4ZpOd38IhiJwun0lTolToagxsnfc7FUjL1dMDCstp4RHRZY7QG6Gt8zhHnD4KWqK3xqKOt5gEuRECj4qAryNKK87vJw";

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
        /// <param name="eventToPost">The Event object to be sent to the server.</param>
        /// <param name="callback">An optional callback function that will receive the response from the server as a string.</param>
        public void BeginPostPsychometrics(Event eventToPost, Action<string> callback = null)
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
        private IEnumerator GetPsychometricMeasures(Action<string> callback = null) // TODO: implement psychometricmeasures class
        {
            using UnityWebRequest getRequest = CreateRequest(baseURL + "/psychometrics/measures");
            AttachHeader(getRequest, "batch_id",batch_id);
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
        private IEnumerator PostPsychometrics(Event eventToPost, Action<string> callback = null)
        {
            using UnityWebRequest postRequest = CreateRequest(baseURL + "/psychometrics/stimuli-response", RequestType.POST, eventToPost);
            AttachHeader(postRequest, "batch_id",batch_id);
            yield return postRequest.SendWebRequest();
            PrintRequestFailOrSuccess(postRequest, RequestType.POST);
            print("posted " + eventToPost.ToJson());
            callback?.Invoke(postRequest.downloadHandler.text);
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
            AttachHeader(request, "Authorization","Bearer " + access_token);
            return request;
        }

        private IEnumerator GetBatchID()
        {
            using UnityWebRequest getSessionRequest = CreateRequest(baseURL + "/session");
            AttachHeader(getSessionRequest, "client_device",client_device);
            yield return getSessionRequest.SendWebRequest();
            var deserializedGetRequestData = JsonUtility.FromJson<GetData>(getSessionRequest.downloadHandler.text);
            batch_id = deserializedGetRequestData.batch_id;
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
            
            var deserializedGetRequestData = JsonUtility.FromJson<GetData>(getBatchMetadataRequest.downloadHandler.text);
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
            var dataToPost = new PostMetadata();  // TODO: OBTAIN THE DATA WE NEED TO POST
            using UnityWebRequest postRequest = CreateRequest(baseURL + batch_metadata_GET_path + batch_id, RequestType.POST, dataToPost);
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

        /// <summary>
        /// Prints the result of a request to the console. 
        /// </summary>
        /// <param name="request"> The UnityWebRequest object whose result will be printed. </param>
        /// <param name="type"> The type of request that was made. </param>
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

    /// <summary>
    /// The type of request to be made. 
    /// </summary>
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
