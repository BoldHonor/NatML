/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using System.IO;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// </summary>
    public static class MLModel {

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="modelPath">Model path.</param>
        /// <returns></returns>
        public static T Create<T> (string modelPath) where T : IMLModel {
            return default;
        }

        /// <summary>
        /// Get the path to a model in StreamingAssets.
        /// </summary>
        /// <param name="relativePath">Model path relative to streaming assets.</param>
        /// <returns></returns>
        public static async Task<string> GetModelPath (string relativePath) {
            var absolutePath = Path.Combine(Application.streamingAssetsPath, relativePath);
            var persistentPath = Path.Combine(Application.persistentDataPath, relativePath);
            switch (Application.platform) {
                case RuntimePlatform.Android:
                    // Check persistent storage
                    if (File.Exists(persistentPath))
                        return persistentPath;
                    // Download from APK/AAB
                    var request = UnityWebRequest.Get(absolutePath);
                    request.SendWebRequest();
                    while (!request.isDone)
                        await Task.Delay(10);
                    // Copy to persistent storage
                    new FileInfo(persistentPath).Directory.Create();
                    File.WriteAllBytes(persistentPath, request.downloadHandler.data);
                    return persistentPath;
                default:
                    return absolutePath;
            }
        }
        #endregion
    }
}