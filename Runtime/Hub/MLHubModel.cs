/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Hub {

    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using Stopwatch = System.Diagnostics.Stopwatch;

    internal sealed class MLHubModel : MLModel {
        
        #region --Client API--
        public static async Task<MLModelData> LoadModelData (string tag, string accessKey) { // INCOMPLETE
            const string API = @"https://hub.natsuite.io/graph";
            var authHeader = $"BASIC {accessKey}";
            var payloadStr = "";
            var client = new HttpClient();
            // Fetch model data
            var content = new StringContent(payloadStr, Encoding.UTF8, "application/json");
            content.Headers.Add(@"Authorization", authHeader);
            var response = await client.PostAsync(API, content);
            // Populate model data
            var modelData = ScriptableObject.CreateInstance<MLModelData>();
            modelData.hubInfo = (tag, accessKey);

            // Download model
            var modelUrl = "";
            var modelResponse = await client.GetAsync(modelUrl);
            modelData.data = await modelResponse.Content.ReadAsByteArrayAsync();
            // Dispose
            modelResponse.Dispose();
            response.Dispose();
            content.Dispose();
            client.Dispose();
            // Return
            return default;
        }
        #endregion


        #region --Operations--

        private readonly string tag;
        private readonly string accessKey;

        internal MLHubModel (MLModelData modelData) : base(modelData.data) {
            this.tag = modelData.hubInfo.tag;
            this.accessKey = modelData.hubInfo.accessKey;
        }

        private protected override IntPtr[] Predict (params IntPtr[] inputs) {
            var watch = Stopwatch.StartNew();
            var outputs = base.Predict(inputs);
            Report(watch.Elapsed.TotalMilliseconds);
            return outputs;
        }

        private void Report (double latency) { // INCOMPLETE

        }
        #endregion
    }
}