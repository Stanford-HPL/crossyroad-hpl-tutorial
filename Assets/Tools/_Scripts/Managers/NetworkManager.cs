using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

namespace COM.Metaviz.HPL.Demo
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager Instance;
        
        private const string baseURL = "https://insights.platform.hpl.stanford.edu";
        private const string session_enpoint = "/session";
        private const string psychometric_measures_endpoint = "/psychometrics/measures";
        private const string psychometric_stimuli_response_endpoint = "/psychometrics/stimuli-response";

        private string client_device;
        private string batch_id = "use GetBatchID to obtain a new unique one";

        private const string access_token =
            "eyJraWQiOiJJN0l2YmNqT2w2SWdzTU5EQlE5cUpJV3NIQlwvNlB6MUhcL0RhZzQyZTFSUXc9IiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJ0bWlmcGVxMDkyN3AybDB0MGZnaWVpYzFzIiwidG9rZW5fdXNlIjoiYWNjZXNzIiwic2NvcGUiOiJodHRwczpcL1wvcGxhdGZvcm0udml6emFyaW8uY29tXC9iZWhhdmlvcnMucmVhZCBodHRwczpcL1wvcGxhdGZvcm0udml6emFyaW8uY29tXC9yZWNvbW1lbmRhdGlvbi5yZWFkIGh0dHBzOlwvXC9wbGF0Zm9ybS52aXp6YXJpby5jb21cL2Jpb21ldHJpY3Mud3JpdGUgaHR0cHM6XC9cL3BsYXRmb3JtLnZpenphcmlvLmNvbVwvcHN5Y2hvbWV0cmljcy53cml0ZSBodHRwczpcL1wvcGxhdGZvcm0udml6emFyaW8uY29tXC9iaW9tZXRyaWNzLnJlYWQgaHR0cHM6XC9cL3BsYXRmb3JtLnZpenphcmlvLmNvbVwvbWV0YWRhdGEud3JpdGUgaHR0cHM6XC9cL3BsYXRmb3JtLnZpenphcmlvLmNvbVwvbWV0YWRhdGEucmVhZCBodHRwczpcL1wvcGxhdGZvcm0udml6emFyaW8uY29tXC9wc3ljaG9tZXRyaWNzLnJlYWQiLCJhdXRoX3RpbWUiOjE2Nzc1MzkxMzAsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy13ZXN0LTIuYW1hem9uYXdzLmNvbVwvdXMtd2VzdC0yX2pzYnRQR3JKdSIsImV4cCI6MTY3NzU0MjczMCwiaWF0IjoxNjc3NTM5MTMwLCJ2ZXJzaW9uIjoyLCJqdGkiOiJiZmZjNzc4Ny00ZmM1LTQ2NDEtOGRhYS1hMzQ3N2FmMzYzNDQiLCJjbGllbnRfaWQiOiJ0bWlmcGVxMDkyN3AybDB0MGZnaWVpYzFzIn0.tkmDWt6Ly6-ffD8gQkzOdt4PCf-LdzbbUqkwWUh_muJhjUdWdxkY9bJewScOZ2lQ_zjG6Z2oTh2GGYxZO8wSgBhuzqKLo6qimtP9J_h7_NmAF8u3jvIJx0aM5KG4PdoQsXY9u8DLYgUSxewCJ9IPgJr7FvkCUUYyfIeJ3G6dOfdDGx8E86Zcz-9q-h1ddtZWNyalp40UkZGGNNHZMxHfpL5lcVzerSdekguGlzPo4TGljnlHKPc-0gBSAVvFaQCAKyNTARiFK-BMdVgSGf1tTg53E00jfSsKMEoudGynE9QW7tVeZNGhdsx1klHOjMDJBu92v2hiBtWqofpTeUBKag";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);
            
            GetClientDevice();
            StartCoroutine(GetBatchID());
        }

        public void BeginRequests()
        {
            GetData psychometricGetData = new GetData();
            StartCoroutine(MakeGetRequest(baseURL + psychometric_measures_endpoint));
            StartCoroutine(MakePostRequest());
        }

        private IEnumerator MakeGetRequest(string requestPath)
        {
            var getRequest = CreateRequest(requestPath);
            yield return getRequest.SendWebRequest();
            PrintRequestFailOrSuccess(getRequest, RequestType.GET);
            var deserializedGetData = JsonUtility.FromJson<GetData>(getRequest.downloadHandler.text);
            // print(deserializedGetData.ToString());
        }

        private IEnumerator MakePostRequest()
        {
            var dataToPost = new PostData();  // TODO: OBTAIN THE DATA WE NEED TO POST
            var postRequest = CreateRequest(baseURL + psychometric_stimuli_response_endpoint, RequestType.POST, dataToPost);
            yield return postRequest.SendWebRequest();
            PrintRequestFailOrSuccess(postRequest, RequestType.POST);
            var deserializedPostData = JsonUtility.FromJson<PostResult>(postRequest.downloadHandler.text);
            // print(deserializedPostData.ToString());
        }

        // Upload and Download Handlers are what we are referencing when grabbing results of the request
        private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
        {
            var request = new UnityWebRequest(path, type.ToString());

            if (data != null)
            {
                var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            AttachHeader(request, "Content-Type", "application/json");
            AttachHeader(request, "Authorization","Bearer " + access_token);
            AttachHeader(request, "client_device",client_device);
            AttachHeader(request, "batch_id",batch_id);
            return request;
        }

        private IEnumerator GetBatchID()
        {
            UnityWebRequest getSessionRequest = CreateRequest(baseURL + session_enpoint);
            yield return getSessionRequest.SendWebRequest();
            var deserializedGetRequestData = JsonUtility.FromJson<GetData>(getSessionRequest.downloadHandler.text);
            batch_id = deserializedGetRequestData.batch_id;
            print(batch_id);
        }

        private void AttachHeader(UnityWebRequest request, string key, string value)
        {
            request.SetRequestHeader(key, value);
        }

        private void GetClientDevice()
        {
            //Check if the device running this is a console
            if (SystemInfo.deviceType == DeviceType.Console)
            {
                //Change the text of the label
                client_device = "Console";
            }

            //Check if the device running this is a desktop
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                client_device = "Desktop";
            }

            //Check if the device running this is a handheld
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                client_device = "Handheld";
            }

            //Check if the device running this is unknown
            if (SystemInfo.deviceType == DeviceType.Unknown)
            {
                client_device = "Unknown";
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
        public string success;
    }

    [Serializable]
    public class PostData
    {
        public string event_type = "";
        public string parent_id = "";
        public string task_id = "";
        public bool user_input = false;
        public bool should_respond = false;
        public string time_occurred = "";  // <date-time>
        public object foreground = null;
        public object background = null;
        public object position = null;
        public object dimensions = null;
    }

    public class PostResult
    {
        public string success { get; set; }
    }
}
