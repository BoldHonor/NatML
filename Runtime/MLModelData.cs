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

    /// <summary>
    /// </summary>
    public sealed class MLModelData : ScriptableObject {
        
        #region --Client API--
        /// <summary>
        /// Classification labels.
        /// </summary>
        public string[] labels => classLabels;

        /// <summary>
        /// Expected image normalization when making predictions with this model.
        /// </summary>
        public (Vector3 mean, Vector3 std) normalization => (imageNormalization.mean, imageNormalization.std);
        
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public MLModel Deserialize () => new MLModel(data);

        /// <summary>
        /// </summary>
        /// <param name="path">Path to ONNX model file. This method supports laoding from StreamingAssets.</param>
        /// <returns></returns>
        public static async Task<MLModelData> FromPath (string path) { // DEPLOY
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
        /// </summary>
        /// <param name="url">URL to ONNX model file.</param>
        /// <returns></returns>
        public static async Task<MLModelData> FromURL (string url) { // DEPLOY
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
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        public static Task<MLModelData> FromMuna (string tag, string accessKey = null) { // INCOMPLETE
            return default;
        }
        #endregion
        

        #region --Operations--
        [SerializeField, HideInInspector] internal byte[] data;
        [SerializeField, HideInInspector] internal string[] classLabels;
        [SerializeField, HideInInspector] internal Normalization imageNormalization;
        [Serializable] internal struct Normalization { public Vector3 mean; public Vector3 std; }
        #endregion
    }
}