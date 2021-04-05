/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Features.Types;
    using Internal;

    /// <summary>
    /// </summary>
    public sealed class MLClassifier : MLModel<(string label, float confidence)[]> {

        #region --Client API--
        /// <summary>
        /// </summary>
        public readonly string[] labels;

        /// <summary>
        /// </summary>
        /// <param name="path">Path to ONNX model.</param>
        /// <param name="labels">List of labels which the classifier outputs.</param>
        public MLClassifier (string path, string[] labels) : base(path) {
            this.labels = labels;
            // Allocate logits
            var outputShape = ((MLArrayType)this.outputs[0]).shape;
            this.classes = outputShape.Aggregate(1, (a, b) => a * b);
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public unsafe (string label, float confidence) Classify (MLFeature input) {
            // Predict
            var inputFeature = input.CreateNMLFeature(this.inputs[0]);
            var outputFeature = Predict(inputFeature)[0];
            // Find label
            var logits = (float*)outputFeature.FeatureData();
            var argMax = 0;
            for (var i = 1; i < classes; ++i)
                if (logits[i] > logits[argMax])
                    argMax = i;
            // Return
            return (labels[argMax], logits[argMax]);
        }

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public unsafe override (string label, float confidence)[] Predict (params MLFeature[] inputs) { // Slower than ::Classify
            // Predict
            var inputFeature = inputs[0].CreateNMLFeature(this.inputs[0]);
            var outputFeature = Predict(inputFeature)[0];
            // Copy logits
            var logits = (float*)outputFeature.FeatureData();
            var pairs = new (string, float l)[classes];
            for (var i = 0; i < classes; ++i)
                pairs[i] = (labels[i], logits[i]);
            // Release
            inputFeature.ReleaseFeature();
            outputFeature.ReleaseFeature();
            // Order descending
            return pairs.OrderByDescending(c => c.l).ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static async Task<string[]> LoadLabelsFromStreamingAssets (string relativePath) => File.ReadAllLines(await MLModelUtility.ModelPathFromStreamingAssets(relativePath));
        #endregion


        #region --Operations--

        private readonly int classes;
        #endregion
    }
}