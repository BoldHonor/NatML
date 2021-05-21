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
        /// <param name="model">Classification ML model.</param>
        /// <param name="labels">Classification labels.</param>
        public MLClassificationPredictor (MLModel model, string[] labels) {
            // Save
            this.model = model;
            this.labels = labels;
            this.classes = (model.outputs.First() as MLArrayType).shape.Aggregate(1, (a, b) => a * b);
            // Check
            if (labels.Length != classes)
                throw new ArgumentOutOfRangeException(
                    nameof(labels),
                    $"Classification predictor received {labels.Length} labels but expected {classes}"
                );
        }

        /// <summary>
        /// Classify a feature.
        /// This will return the most-likely label along with the confidence score.
        /// </summary>
        /// <param name="inputs">Input feature.</param>
        /// <returns>Output label with unnormalized confidence value.</returns>
        public unsafe (string label, float confidence) Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"Classification predictor expects a single feature", nameof(inputs));
            // Check type
            var input = inputs[0];
            if (!(input.type is MLArrayType))
                throw new ArgumentException(@"Classification predictor expects an an array or image feature", nameof(inputs));
            // Predict
            var inputType = model.inputs[0];
            var inputFeature = (input as IMLFeature).Create(inputType);
            var outputFeature = model.Predict(inputFeature)[0];
            inputFeature.ReleaseFeature();
            // Find label
            var logits = (float*)outputFeature.FeatureData();
            var argMax = 0;
            for (var i = 1; i < classes; ++i)
                if (logits[i] > logits[argMax])
                    argMax = i;
            outputFeature.ReleaseFeature();
            // Return
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