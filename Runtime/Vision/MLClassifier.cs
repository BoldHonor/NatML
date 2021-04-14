/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Features.Types;
    using Internal;

    /// <summary>
    /// </summary>
    public sealed class MLClassifier : MLModule<(string label, float confidence)[]> {

        #region --Client API--
        /// <summary>
        /// Classification labels.
        /// </summary>
        public readonly string[] labels;

        /// <summary>
        /// Create a classifier.
        /// </summary>
        /// <param name="path">Path to ONNX model.</param>
        /// <param name="labels">List of labels which the classifier outputs.</param>
        public MLClassifier (string path, string[] labels) : base(path) {
            this.labels = labels;
            // Allocate logits
            var outputShape = ((MLArrayType)this.outputs.First()).shape;
            this.classes = outputShape.Aggregate(1, (a, b) => a * b);
        }

        /// <summary>
        /// Classify a feature.
        /// This will return the most-likely label for the given feature.
        /// </summary>
        /// <param name="input">Input feature.</param>
        /// <returns>Output label along with unnormalized confidence value.</returns>
        public unsafe (string label, float confidence) Classify (MLFeature input) {
            // Predict
            var inputFeature = (input as INMLFeature).CreateNativeFeature(this.inputs[0]);
            var outputFeature = Predict(inputFeature).First();
            // Find label
            var logits = (float*)outputFeature.FeatureData();
            var argMax = 0;
            for (var i = 1; i < classes; ++i)
                if (logits[i] > logits[argMax])
                    argMax = i;
            // Release
            inputFeature.ReleaseFeature();
            outputFeature.ReleaseFeature();
            return (labels[argMax], logits[argMax]);
        }

        /// <summary>
        /// </summary>
        /// <param name="inputs">Input features.</param>
        /// <returns></returns>
        public unsafe override (string label, float confidence)[] Predict (params MLFeature[] inputs) { // Slower than ::Classify
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"MLClassifier expects a single feature", nameof(inputs));
            // Predict
            var inputFeature = (inputs[0] as INMLFeature).CreateNativeFeature(this.inputs[0]);
            var outputFeature = Predict(inputFeature).First();
            // Copy logits
            var logits = (float*)outputFeature.FeatureData();
            var pairs = new (string, float l)[classes];
            for (var i = 0; i < classes; ++i)
                pairs[i] = (labels[i], logits[i]);
            // Order descending
            var result = pairs.OrderByDescending(c => c.l).ToArray();
            // Release
            inputFeature.ReleaseFeature();
            outputFeature.ReleaseFeature();
            return result;
        }

        /// <summary>
        /// Load classification labels from a plain text file.
        /// </summary>
        /// <param name="relativePath">Relative path to label file in `StreamingAssets` folder.</param>
        /// <returns>Array of class labels read from file.</returns>
        public static async Task<string[]> LoadLabelsFromStreamingAssets (string relativePath) => File.ReadAllLines(await MLModelUtility.ModelPathFromStreamingAssets(relativePath));
        #endregion


        #region --Operations--

        private readonly int classes;
        #endregion
    }
}