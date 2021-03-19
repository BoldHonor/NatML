/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Hub {

    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// </summary>
    public static class MLUtility {

        #region --Client API--
        /// <summary>
        /// Get the path to a model in StreamingAssets.
        /// </summary>
        /// <param name="relativePath">Model path relative to streaming assets.</param>
        /// <returns>Model path.</returns>
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
                        await Task.Yield();
                    // Copy to persistent storage
                    new FileInfo(persistentPath).Directory.Create();
                    File.WriteAllBytes(persistentPath, request.downloadHandler.data);
                    return persistentPath;
                default:
                    return absolutePath;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="relativePath"></param>
        public static async Task<string[]> ReadLabels (string relativePath) => File.ReadAllLines(await GetModelPath(relativePath));
        #endregion
    }
}