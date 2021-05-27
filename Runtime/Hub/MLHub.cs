/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

#define DEV_HUB

namespace NatSuite.ML.Hub {

    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using Features;
    
    internal static class MLHub {

        #region --Client API--

        public static async Task<MLModelData> LoadFromHub (string tag, string accessKey) {
            // Build payload
            var mutation = "modelData (tag: $tag, device: $device) { session graphData labels normalization { mean std } aspectMode }";
            var query = $"mutation ($tag: String!, $device: Device!) {{ {mutation} }}";
            var device = new Device {
                id = SystemInfo.deviceUniqueIdentifier,
                model = SystemInfo.deviceModel,
                os = SystemInfo.operatingSystem,
                gfx = SystemInfo.graphicsDeviceType.ToString()
            };
            var payload = JsonUtility.ToJson(new Payload {
                query = query,
                variables = new Mutation { tag = tag, device = device }
            });
            // Request
            using (var client = new HttpClient())
                using (var content = new StringContent(payload, Encoding.UTF8, "application/json")) {
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
                        // Create model data
                        var cachedData = responseDict.data.modelData;
                        using (var modelResponse = await client.GetAsync(cachedData.graphData)) {
                            var graphData = await modelResponse.Content.ReadAsByteArrayAsync();
                            var modelData = Load(tag, cachedData, graphData);
                            return modelData;
                        }
                    }
            }
        }

        public static async Task<MLModelData> LoadFromCache (string tag) {
            // Check
            var cacheName = tag.Replace('/', '_');
            var cachePath = Path.Combine(Application.persistentDataPath, "ML", $"{cacheName}.nml");
            if (!File.Exists(cachePath))
                return default;
            // Load
            var cachedData = JsonUtility.FromJson<MLCachedData>(File.ReadAllText(cachePath));
            using (var stream = new FileStream(cachedData.graphData, FileMode.Open, FileAccess.Read)) {
                var graphData = new byte[stream.Length];
                await stream.ReadAsync(graphData, 0, graphData.Length);
                var modelData = Load(tag, cachedData, graphData);            
                return modelData;
            }            
        }

        public static async void SaveToCache (MLModelData modelData) {
            // Check
            if (modelData == null)
                return;
            // Build data
            var cacheName = modelData.tag.Replace('/', '_');
            var basePath = Path.Combine(Application.persistentDataPath, "ML");
            var cachePath = Path.Combine(basePath, $"{cacheName}.nml");
            var graphPath = Path.Combine(basePath, Guid.NewGuid().ToString());
            var cachedData = new MLCachedData {
                session = modelData.session,
                graphData = graphPath,
                labels = modelData.classLabels,
                normalization = modelData.imageNormalization,
                aspectMode = modelData.imageAspectMode.ToString()
            };
            // Write
            Directory.CreateDirectory(basePath);
            using (var stream = new FileStream(graphPath, FileMode.Create, FileAccess.Write, FileShare.None))
                await stream.WriteAsync(modelData.graphData, 0, modelData.graphData.Length);
            using (var stream = new StreamWriter(cachePath))
                await stream.WriteAsync(JsonUtility.ToJson(cachedData));
        }
        #endregion


        #region --Operations--

        private const string API =
        #if !DEV_HUB
        @"http://api.natsuite.io/graph";
        #else
        @"http://localhost:8000/graph"; 
        #endif

        private static MLModelData Load (string tag, MLCachedData cachedData, byte[] graphData) {
            var modelData = ScriptableObject.CreateInstance<MLModelData>();
            modelData.tag = tag;
            modelData.session = cachedData.session;
            modelData.graphData = graphData;
            modelData.classLabels = cachedData.labels?.Length != 0 ? cachedData.labels : null;
            modelData.imageNormalization = cachedData.normalization;
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
        private struct MLCachedData {
            public string session;
            public string graphData;
            public string[] labels;
            public MLModelData.Normalization normalization;
            public string aspectMode;
        }

        [Serializable]
        private struct Device { public string id; public string model; public string os; public string gfx; }

        [Serializable]
        private struct Mutation { public string tag; public Device device; }

        [Serializable]
        private struct Payload { public string query; public Mutation variables; }

        [Serializable]
        private struct Response { public ResponseData data; public string[] errors; }
        
        [Serializable]
        private struct ResponseData { public MLCachedData modelData; }
        #endregion
    }
}