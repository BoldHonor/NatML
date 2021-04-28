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
    public class MLClassifier : IMLPredictor<(string label, float confidence)> {

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
        public MLClassifier (MLModel model, string[] labels) {
            this.model = model;
            this.labels = labels;
            this.classes = ((MLArrayType)model.outputs.First()).shape.Aggregate(1, (a, b) => a * b);
        }

        /// <summary>
        /// Classify a feature.
        /// This will return the most-likely label along with the confidence score.
        /// </summary>
        /// <param name="inputs">Input features.</param>
        /// <returns>Output label along with unnormalized confidence value.</returns>
        public unsafe (string label, float confidence) Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"MLClassifier expects a single feature", nameof(inputs));
            // Predict
            var inputFeature = (inputs.First() as IMLFeature).Create(model.inputs.First());
            var outputFeature = model.Predict(inputFeature).First();
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
        /// Load classification labels from a plain text file.
        /// </summary>
        /// <param name="relativePath">Relative path to label file in `StreamingAssets` folder.</param>
        /// <returns>Array of class labels read from file.</returns>
        public static async Task<string[]> LoadLabelsFromStreamingAssets (string relativePath) {
            var absolutePath = await MLModelUtility.StreamingAssetsToAbsolute(relativePath);
            return File.ReadAllLines(absolutePath);
        }
        #endregion


        #region --Operations--

        private readonly IMLModel model;
        private readonly int classes;

        void IDisposable.Dispose () { } // Nop
        #endregion
    }
}