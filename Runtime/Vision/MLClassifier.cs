/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Features.Types;
    using Internal;

    /// <summary>
    /// </summary>
    public class MLClassifier : MLModel {

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
        #endregion


        #region --Operations--

        private float[] Predict (MLFeature input) {
            // Copy logits
            var output = NativePredict(input)[0];
            var classCount = ((MLArrayType)this.outputs[0]).shape[1];
            var logits = new float[classCount];
            Marshal.Copy(output.FeatureData(), logits, 0, logits.Length);
            output.ReleaseFeature();
            // Return
            return logits;
        }
        #endregion
    }
}