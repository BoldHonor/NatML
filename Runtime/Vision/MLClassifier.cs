/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Features.Types;
    using Internal;

    /// <summary>
    /// </summary>
    public class MLClassifier : MLModel { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// </summary>
        public readonly IReadOnlyList<string> labels;

        /// <summary>
        /// </summary>
        /// <param name="path">Path to ONNX model.</param>
        /// <param name="labels">List of labels which the classifier outputs.</param>
        public MLClassifier (string path, IReadOnlyList<string> labels) : base(path) => this.labels = labels;

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public (string label, float confidence) Classify (MLFeature input) {
            // Run inference
            var inputFeatures = new [] { ((INMLFeature)input).CreateFeature(this.inputs[0]) };
            var outputFeatures = new IntPtr[1];
            model.Predict(inputFeatures, outputFeatures);
            // Copy logits
            var output = outputFeatures[0];
            var classCount = ((MLArrayType)this.outputs[0]).shape[1];
            var logits = new float[classCount];
            Marshal.Copy(output.FeatureData(), logits, 0, logits.Length);
            output.ReleaseFeature();
            // Return max
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
        public (string label, float confidence)[] ClassifyAll (MLFeature input, int limit = 0) { // DEPLOY
            // Run inference
            var inputFeatures = new [] { ((INMLFeature)input).CreateFeature(this.inputs[0]) };
            var outputFeatures = new IntPtr[1];
            model.Predict(inputFeatures, outputFeatures);
            // Copy logits
            var output = outputFeatures[0];
            var classCount = ((MLArrayType)this.outputs[0]).shape[1];
            var logits = new float[classCount];
            Marshal.Copy(output.FeatureData(), logits, 0, logits.Length);
            output.ReleaseFeature();
            // Sort and return
            IEnumerable<(string, float)> result = logits
                .Select((l, i) => (labels[i], l))
                .OrderByDescending(c => c.l);
            if (limit > 0)
                result = result.Take(limit);
            return result.ToArray();
        }
        #endregion
    }
}