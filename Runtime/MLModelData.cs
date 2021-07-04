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
    using Hub;

    /// <summary>
    /// Self-contained archive with ML model and supplemental data needed to make predictions with the model.
    /// </summary>
    public sealed class MLModelData : ScriptableObject {
        
        #region --Client API--
        /// <summary>
        /// Model classification labels.
        /// This is `null` if the model does not have any classification labels.
        /// </summary>
        public string[] labels => classLabels?.Length > 0 ? classLabels : default;

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
        public unsafe MLModel Deserialize () {
            var flags = this.flags;
            var model = new MLModel(graphData, &flags);
            if (!string.IsNullOrEmpty(session)) {
                (model as INMLReporter).onPrediction += latency => {
                    #pragma warning disable 4014
                    NMLHub.ReportPrediction(session, latency);
                    #pragma warning restore 4014
                };
            }
            return model;
        }

        /// <summary>
        /// Fetch ML model data from NatML hub.
        /// </summary>
        /// <param name="tag">Model tag.</param>
        /// <param name="accessKey">Hub access key.</param>
        /// <param name="analytics">Enable performance analytics reporting.</param>
        /// <returns>ML model data.</returns>
        public static async Task<MLModelData> FromHub (string tag, string accessKey = null, bool analytics = true) {
            var modelData = await NMLHub.LoadFromCache(tag);
            if (modelData == null) {
                modelData = await NMLHub.LoadFromHub(tag, accessKey);
                #pragma warning disable 4014
                NMLHub.SaveToCache(modelData);
                #pragma warning restore 4014
            }
            if (!analytics)
                modelData.session = null;
            return modelData;
        }

        /// <summary>
        /// Fetch ML model data from a local file.
        /// </summary>
        /// <param name="path">Path to ONNX model file.</param>
        /// <returns>ML model data.</returns>
        public static Task<MLModelData> FromFile (string path) {
            var modelData = ScriptableObject.CreateInstance<MLModelData>();
            modelData.graphData = File.ReadAllBytes(path);
            return Task.FromResult(modelData);
        }

        /// <summary>
        /// Fetch ML model data from StreamingAssets.
        /// </summary>
        /// <param name="path">Relative path to ONNX model file in `StreamingAssets` folder.</param>
        /// <returns>ML model data.</returns>
        public static async Task<MLModelData> FromStreamingAssets (string relativePath) {
            // Check for direct extraction
            var fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            if (Application.platform != RuntimePlatform.Android)
                return await FromFile(fullPath);
            // Extract from app archive
            using (var request = UnityWebRequest.Get(fullPath)) {
                request.SendWebRequest();
                while (!request.isDone)
                    await Task.Yield();
                if (request.isNetworkError || request.isHttpError)
                    throw new ArgumentException($"Failed to create MLModelData from StreamingAssets: {relativePath}");
                var modelData = ScriptableObject.CreateInstance<MLModelData>();
                modelData.graphData = request.downloadHandler.data;
                return modelData;
            }
        }
        #endregion


        #region --Operations--
        internal string tag;
        internal string session;
        [SerializeField, HideInInspector] internal byte[] graphData;
        [SerializeField, HideInInspector] internal int flags;
        [SerializeField, HideInInspector] internal string[] classLabels;
        [SerializeField, HideInInspector] internal Normalization imageNormalization;
        [SerializeField, HideInInspector] internal MLImageFeature.AspectMode imageAspectMode;

        [Serializable]
        public struct Normalization {
            [SerializeField] internal float[] mean;
            [SerializeField] internal float[] std;
            public void Deconstruct (out Vector3 outMean, out Vector3 outStd) {
                (outMean, outStd) = (Vector3.zero, Vector3.one);
                if (mean != null)
                    outMean = new Vector3(mean[0], mean[1], mean[2]);
                if (std != null)
                    outStd = new Vector3(std[0], std[1], std[2]);
            }
        }
        #endregion
    }
}