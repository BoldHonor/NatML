/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Hub {

    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using Features;
    
    public static class MLHub {

        #region --Client API--

        public static async Task<MLModelData> LoadFromHub (string tag, string accessKey) {
            // Build payload
            var mutation = "modelData (tag: $tag, device: $device) { data labels normalization { mean std } aspectMode }";
            var query = $"mutation ($tag: String!, $device: DeviceInfo!) {{ {mutation} }}";
            var payload = new Payload {
                query = query,
                variables = new Mutation {
                    tag = tag,
                    device = new DeviceInfo {
                        id = SystemInfo.deviceUniqueIdentifier,
                        model = SystemInfo.deviceModel,
                        os = SystemInfo.operatingSystem,
                        gfx = SystemInfo.graphicsDeviceType.ToString()
                    }
                }
            };
            var payloadStr = JsonUtility.ToJson(payload);
            // Request
            const string API = @"http://localhost:8000/graph"; //@"https://hub.natsuite.io/graph";
            using (var client = new HttpClient())
                using (var content = new StringContent(payloadStr, Encoding.UTF8, "application/json")) {
                    // Fetch model data
                    content.Headers.TryAddWithoutValidation(@"Authorization", accessKey);
                    using (var response = await client.PostAsync(API, content)) {
                        var responseStr = await response.Content.ReadAsStringAsync();
                        var responseDict = JsonUtility.FromJson<Response>(responseStr);
                        // Check
                        if (responseDict.errors != null) {
                            Debug.LogError($"Failed to load model from Hub: {tag}");
                            return default;
                        }
                        // Populate model data
                        var cachedData = responseDict.data.modelData;
                        using (var modelResponse = await client.GetAsync(cachedData.data)) {
                            var graphData = await modelResponse.Content.ReadAsByteArrayAsync();
                            var modelData = Load(cachedData, graphData);
                            return modelData;
                        }
                    }
            }
        }

        public static MLModelData LoadFromCache (string tag) {
            // Check
            var cacheName = tag.Replace('/', '_');
            var cachePath = Path.Combine(Application.persistentDataPath, "ML", $"{cacheName}.nml");
            if (!File.Exists(cachePath))
                return null;
            // Load
            var cachedData = JsonUtility.FromJson<MLCachedData>(File.ReadAllText(cachePath));
            var graphData = File.ReadAllBytes(cachedData.data);
            var modelData = Load(cachedData, graphData);            
            return modelData;
        }

        public static void SaveToCache (MLModelData modelData) { // INCOMPLETE
            
        }
        #endregion


        #region --Operations--

        private static MLModelData Load (MLCachedData cachedData, byte[] graphData) {
            var (mean, std) = (cachedData.normalization.mean, cachedData.normalization.std);
            var modelData = ScriptableObject.CreateInstance<MLModelData>();
            modelData.data = graphData;
            modelData.classLabels = cachedData.labels?.Length != 0 ? cachedData.labels : null;
            modelData.imageNormalization = new MLModelData.Normalization {
                mean = mean?.Length > 0 ? new Vector3(mean[0], mean[1], mean[2]) : Vector3.zero,
                std = std?.Length > 0 ? new Vector3(std[0], std[1], std[2]) : Vector3.one
            };
            modelData.imageAspectMode = GetAspectMode(cachedData.aspectMode);
            return modelData;
        }

        private static MLImageFeature.AspectMode GetAspectMode (string mode) {
            switch (mode) {
                case "SCALE_TO_FIT":
                case "ScaleToFit": goto default;
                case "ASPECT_FILL":
                case "AspectFill": return MLImageFeature.AspectMode.AspectFill;
                case "ASPECT_FIT":
                case "AspectFit": return MLImageFeature.AspectMode.AspectFit;
                default: return MLImageFeature.AspectMode.ScaleToFit;
            }
        }

        [Serializable]
        private struct MLCachedData { // INCOMPLETE // Add id
            public string data;
            public string[] labels;
            public Normalization normalization;
            public string aspectMode;
        }

        [Serializable]
        private struct DeviceInfo { public string id; public string model; public string os; public string gfx; }

        [Serializable]
        private struct Mutation { public string tag; public DeviceInfo device; }

        [Serializable]
        private struct Payload { public string query; public Mutation variables; }

        [Serializable]
        private struct Response { public ResponseData data; public string[] errors; }
        
        [Serializable]
        private struct ResponseData { public MLCachedData modelData; }

        [Serializable]
        private struct Normalization { public float[] mean; public float[] std; }
        #endregion
    }
}