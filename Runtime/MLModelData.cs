/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NatSuite.ML.Editor")]
namespace NatSuite.ML {

    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Networking;
    using Features;

    /// <summary>
    /// </summary>
    public sealed class MLModelData : ScriptableObject {
        
        #region --Client API--
        /// <summary>
        /// </summary>
        [Serializable]
        public struct Normalization { // We need to support null checks, but Unity's serialization won't allow us prosper
            [SerializeField] internal Vector3 mean;
            [SerializeField] internal Vector3 std;
            [SerializeField] internal bool valid;
            public void Deconstruct (out Vector3 mean, out Vector3 std) => (mean, std) = (this.mean, this.std);
            public static implicit operator bool (Normalization n) => n.valid;
        }

        /// <summary>
        /// Model classification labels.
        /// This is `null` if the model does not have any classification data available.
        /// </summary>
        public string[] labels => classLabels;

        /// <summary>
        /// Expected image feature normalization for predictions with this model.
        /// </summary>
        public Normalization normalization => imageNormalization;

        /// <summary>
        /// Expected image aspect mode for predictions with this model.
        /// </summary>
        public MLImageFeature.AspectMode aspectMode => imageAspectMode;
        
        /// <summary>
        /// Deserialize the model data to create an ML model that can be used for prediction.
        /// You MUST dispose the model once you are done with it.
        /// </summary>
        /// <returns>ML model.</returns>
        public MLModel Deserialize () => new MLModel(data);

        /// <summary>
        /// Fetch ML model data from a local file.
        /// </summary>
        /// <param name="path">Path to ONNX model file.</param>
        /// <returns>ML model data.</returns>
        public static Task<MLModelData> FromPath (string path) {
            var modelData = ScriptableObject.CreateInstance<MLModelData>();
            modelData.data = File.ReadAllBytes(path);
            return Task.FromResult(modelData);
        }

        /// <summary>
        /// Fetch ML model data from StreamingAssets.
        /// </summary>
        /// <param name="path">Relative path to ONNX model file in `StreamingAssets` folder.</param>
        /// <returns>ML model data.</returns>
        public static async Task<MLModelData> FromStreamingAssets (string relativePath) {
            var fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            if (Application.platform == RuntimePlatform.Android)
                return await FromURL(fullPath);
            else
                return await FromPath(fullPath);                
        }

        /// <summary>
        /// Fetch ML model data from a remote URL.
        /// </summary>
        /// <param name="url">URL to ONNX model file.</param>
        /// <returns>ML model data.</returns>
        public static async Task<MLModelData> FromURL (string url) {
            using (var request = UnityWebRequest.Get(url)) {
                // Download from APK/AAB
                request.SendWebRequest();
                while (!request.isDone)
                    await Task.Yield();
                if (request.isNetworkError || request.isHttpError)
                    throw new ArgumentException($"Failed to create MLModelData from URL: {url}", nameof(url));
                // Create
                var modelData = ScriptableObject.CreateInstance<MLModelData>();
                modelData.data = request.downloadHandler.data;
                return modelData;
            }
        }

        /// <summary>
        /// Fetch ML model data from NatML hub.
        /// </summary>
        /// <param name="tag">Model tag.</param>
        /// <param name="accessKey">Hub access key.</param>
        /// <returns>ML model data.</returns>
        public static async Task<MLModelData> FromHub (string tag, string accessKey) { // INCOMPLETE
            // Check if cached
            var cachePath = Path.Combine(Application.persistentDataPath, "ML", $"{tag.Replace('/', '_')}.mldata");
            if (File.Exists(cachePath)) {
                var cachedData = JsonUtility.FromJson<MLCachedData>(File.ReadAllText(cachePath));
                var modelData = ScriptableObject.CreateInstance<MLModelData>();
                modelData.data = File.ReadAllBytes(cachedData.data);
                modelData.classLabels = cachedData.labels.Length != 0 ? cachedData.labels : null;
                modelData.imageNormalization = cachedData.normalization;
                modelData.imageAspectMode = cachedData.aspectMode;
                return modelData;
            }
            // Fetch from Hub

            // Cache locally
            return default;
        }
        #endregion


        #region --Operations--
        [SerializeField, HideInInspector] internal byte[] data;
        [SerializeField, HideInInspector] internal string[] classLabels;
        [SerializeField, HideInInspector] internal Normalization imageNormalization;
        [SerializeField, HideInInspector] internal MLImageFeature.AspectMode imageAspectMode;

        [Serializable]
        private struct MLCachedData {
            public string data;
            public string[] labels;
            public Normalization normalization;
            public MLImageFeature.AspectMode aspectMode;
        }
        #endregion
    }
}