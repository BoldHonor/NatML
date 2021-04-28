/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System.IO;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Networking;
    using Internal;
    using Predictors;

    /// <summary>
    /// Common utilities for working with MLModels.
    /// </summary>
    public static class MLModelUtility {

        #region --Client API--
        /// <summary>
        /// Get an absolute path for a file in StreamingAssets.
        /// </summary>
        /// <param name="relativePath">Model path relative to streaming assets.</param>
        /// <returns>Model path.</returns>
        public static async Task<string> StreamingAssetsToAbsolute (string relativePath) {
            // Get absolute path
            var absolutePath = Path.Combine(Application.streamingAssetsPath, relativePath);
            var persistentPath = Path.Combine(Application.persistentDataPath, relativePath);
            if (Application.platform != RuntimePlatform.Android)
                return absolutePath;
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
        }

        /// <summary>
        /// Create an async predictor from a predictor.
        /// This typically results in significant performance improvements as predictions are run on a worker thread.
        /// </summary>
        /// <param name="predictor">Backing predictor to create an async predictor with.</param>
        /// <returns>Async predictor which runs predictions on a worker thread.</returns>
        public static MLAsyncPredictor<TOutput> ToAsync<TOutput> (this IMLPredictor<TOutput> predictor) => new MLAsyncPredictor<TOutput>(predictor);
        #endregion
    }
}