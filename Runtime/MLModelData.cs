/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NatSuite.ML.Editor")]
namespace NatSuite.ML {

    using System;
    using System.IO;
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
        /// Expected image reflection mode for predictions with this model.
        /// </summary>
        public MLImageFeature.ReflectionMode reflectionMode => imageReflectionMode;
        
        /// <summary>
        /// Deserialize the model data to create an ML model that can be used for prediction.
        /// You MUST dispose the model once you are done with it.
        /// </summary>
        /// <returns>ML model.</returns>
        public MLModel Deserialize () => new MLModel(data);

        /// <summary>
        /// Fetch ML model data from a local file.
        /// </summary>
        /// <param name="path">Path to ONNX model file. This method supports laoding from StreamingAssets.</param>
        /// <returns>ML model data.</returns>
        public static async Task<MLModelData> FromPath (string path) {
            // On Android, we need to extract from StreamingAssets
            if (Application.platform == RuntimePlatform.Android && path.Contains(Application.streamingAssetsPath))
                return await FromURL(path);
            // Check
            if (!File.Exists(path))
                throw new ArgumentException($"Failed to create MLModelData from path: {path}", nameof(path));
            // Create
            var modelData = ScriptableObject.CreateInstance<MLModelData>();
            modelData.data = File.ReadAllBytes(path);
            return modelData;
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
        /// Fetch ML model data from the Muna ML marketplace.
        /// </summary>
        /// <param name="tag">Model tag.</param>
        /// <param name="accessKey">Muna access key for fetching private models.</param>
        /// <returns>ML model data.</returns>
        public static Task<MLModelData> FromMuna (string tag, string accessKey = null) { // INCOMPLETE
            return default;
        }
        #endregion
        

        #region --Operations--
        [SerializeField, HideInInspector] internal byte[] data;
        [SerializeField, HideInInspector] internal string[] classLabels;
        [SerializeField, HideInInspector] internal Normalization imageNormalization;
        [SerializeField, HideInInspector] internal MLImageFeature.AspectMode imageAspectMode;
        [SerializeField, HideInInspector] internal MLImageFeature.ReflectionMode imageReflectionMode;
        #endregion
    }
}