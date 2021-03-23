/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using System.IO;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// </summary>
    public static class MLModelUtility {

        #region --Client API--
        /// <summary>
        /// Get the path to a model in StreamingAssets.
        /// </summary>
        /// <param name="relativePath">Model path relative to streaming assets.</param>
        /// <returns>Model path.</returns>
        public static async Task<string> ModelPathFromStreamingAssets (string relativePath) {
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
        /// <param name="model"></param>
        /// <returns></returns>
        public static MLDispatcher<MLFeature[]> CreateDispatcher (MLModel model) => CreateDispatcher((MLFeature[] inputs) => model.Predict(inputs));

        /// <summary>
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public static MLDispatcher<T> CreateDispatcher<T> (Func<MLFeature, T> runner) => CreateDispatcher((MLFeature[] inputs) => runner(inputs[0]));

        /// <summary>
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public static MLDispatcher<T> CreateDispatcher<T> (Func<MLFeature[], T> runner) => new MLDispatcher<T>(runner);
        #endregion
    }
}