/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System;
    using System.Linq;
    using Internal;
    using Types;

    /// <summary>
    /// </summary>
    public class MLClassificationPredictor : IMLPredictor<(string label, float confidence)> {

        #region --Client API--
        /// <summary>
        /// Classification labels.
        /// </summary>
        public readonly string[] labels;

        /// <summary>
        /// Create a classification predictor.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="labels">Classification labels.</param>
        public MLClassificationPredictor (MLModel model, string[] labels) {
            // Save
            this.model = model;
            this.labels = labels;
            this.classes = (model.outputs.First() as MLArrayType).shape.Aggregate(1, (a, b) => a * b);
            // Check
            if (labels.Length != classes)
                throw new ArgumentOutOfRangeException(nameof(labels), $"Classifier predcitor received {labels.Length} labels but expected {classes}");
        }

        /// <summary>
        /// Classify a feature.
        /// This will return the most-likely label along with the confidence score.
        /// </summary>
        /// <param name="inputs">Input feature.</param>
        /// <returns>Output label along with unnormalized confidence value.</returns>
        public unsafe (string label, float confidence) Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"Classifier predictor expects a single feature", nameof(inputs));
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
        #endregion


        #region --Operations--

        private readonly IMLModel model;
        private readonly int classes;

        void IDisposable.Dispose () { } // Nop
        #endregion
    }
}