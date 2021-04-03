/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
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
        public MLClassifier (string path, string[] labels) : base(path) => this.labels = labels;

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public (string label, float confidence) Classify (MLFeature input) => Predict(input).First();

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public override (string label, float confidence)[] Predict (params MLFeature[] inputs) {
            // Predict
            var inputFeature = inputs[0].CreateNMLFeature(this.inputs[0]);
            var outputFeature = Predict(inputFeature)[0];
            // Copy logits
            var outputShape = ((MLArrayType)this.outputs[0]).shape;
            var classCount = outputShape.Aggregate(1, (a, b) => a * b);
            var logits = new float[classCount];
            Marshal.Copy(outputFeature.FeatureData(), logits, 0, logits.Length);
            // Release
            inputFeature.ReleaseFeature();
            outputFeature.ReleaseFeature();
            // Order descending
            return logits.Select((l, i) => (labels[i], l)).OrderByDescending(c => c.l).ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static async Task<string[]> LoadLabelsFromStreamingAssets (string relativePath) => File.ReadAllLines(await MLModelUtility.ModelPathFromStreamingAssets(relativePath));
        #endregion
    }
}