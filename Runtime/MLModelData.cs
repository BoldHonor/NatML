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
        /// Load classification labels from a plain text file.
        /// </summary>
        /// <param name="path">Path to labels file. This method supports loading from StreamingAssets.</param>
        /// <returns>Array of class labels read from file.</returns>
        public static async Task<string[]> ReadLabels (string path) { // DEPLOY
            // On Android, we need to extract from StreamingAssets
            if (Application.platform == RuntimePlatform.Android && path.Contains(Application.streamingAssetsPath)) {
                using (var request = UnityWebRequest.Get(path)) {
                    // Download from APK/AAB
                    request.SendWebRequest();
                    while (!request.isDone)
                        await Task.Yield();
                    if (request.isNetworkError || request.isHttpError)
                        throw new ArgumentException($"Failed to read labels from StreamingAssets path: {path}", nameof(path));
                    // Read
                    return request.downloadHandler.text.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            // Check
            if (!File.Exists(path))
                throw new ArgumentException($"Failed to create MLModelData from path: {path}", nameof(path));
            // Read
            var labels = File.ReadAllLines(path);
            return labels;
        }

        /// <summary>
        /// </summary>
        /// <param name="textAsset"></param>
        /// <returns></returns>
        public static Task<string[]> ReadLabels (TextAsset textAsset) {
            var labels = textAsset.text.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return Task.FromResult(labels);
        }
        #endregion
        

        #region --Operations--
        [SerializeField, HideInInspector] internal byte[] data;
        #endregion
    }
}