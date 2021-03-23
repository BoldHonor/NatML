/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Features.Types;
    using Internal;

    /// <summary>
    /// </summary>
    public sealed class MLClassifier : MLModel {

        #region --Client API--
        /// <summary>
        /// </summary>
        public readonly string[] labels;

        /// <summary>
        /// </summary>
        /// <param name="path">Path to ONNX model.</param>
        /// <param name="labels">List of labels which the classifier outputs.</param>
        public MLClassifier (string path, string[] labels) : base(path) => this.labels = labels;

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public (string label, float confidence) Classify (MLFeature input) {
            var logits = Predict(input);
            return logits
                .AsParallel()   // CHECK // Benchmark this
                .Select((l, i) => (labels[i], l))
                .Aggregate((a, b) => a.l > b.l ? a : b);
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public (string label, float confidence)[] ClassifyAll (MLFeature input, int limit = 0) {
            var logits = Predict(input);
            IEnumerable<(string, float)> result = logits
                .Select((l, i) => (labels[i], l))
                .OrderByDescending(c => c.l);
            if (limit > 0)
                result = result.Take(limit);
            return result.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static async Task<string[]> LoadLabelsFromStreamingAssets (string relativePath) => File.ReadAllLines(await MLModelUtility.ModelPathFromStreamingAssets(relativePath));
        #endregion


        #region --Operations--

        private float[] Predict (MLFeature input) {
            // Copy logits
            var inputFeature = input.CreateNMLFeature(this.inputs[0]);
            var outputFeature = Predict(inputFeature)[0];
            var classCount = ((MLArrayType)this.outputs[0]).shape[1];
            var logits = new float[classCount];
            Marshal.Copy(outputFeature.FeatureData(), logits, 0, logits.Length);
            outputFeature.ReleaseFeature();
            // Return
            return logits;
        }
        #endregion
    }
}