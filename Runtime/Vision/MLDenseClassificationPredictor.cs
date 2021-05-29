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
    public sealed class MLDenseClassificationPredictor : IMLPredictor<(string label, float confidence)[]> {

        #region --Client API--
        /// <summary>
        /// Classification labels.
        /// </summary>
        public readonly string[] labels;

        /// <summary>
        /// Create a dense classification predictor.
        /// </summary>
        /// <param name="model">Classification ML model.</param>
        /// <param name="labels">Classification labels.</param>
        public MLDenseClassificationPredictor (MLModel model, string[] labels) {
            // Save
            this.model = model;
            this.labels = labels;
            this.classes = ((MLArrayType)model.outputs.First()).shape.Aggregate(1, (a, b) => a * b);
            // Check
            if (labels.Length != classes)
                throw new ArgumentOutOfRangeException(
                    nameof(labels),
                    $"Classification predcitor received {labels.Length} labels but expected {classes}"
                );
        }

        /// <summary>
        /// Predict all classes on a feature.
        /// This will return the predicted labels in descending order of confidence scores.
        /// </summary>
        /// <param name="inputs">Input feature.</param>
        /// <returns>Output labels with unnormalized confidence values.</returns>
        public unsafe (string label, float confidence)[] Predict (params MLFeature[] inputs) {
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
            // Copy logits
            var logits = (float*)outputFeature.FeatureData();
            var pairs = new (string, float l)[classes];
            for (var i = 0; i < classes; ++i)
                pairs[i] = (labels[i], logits[i]);
            outputFeature.ReleaseFeature();
            // Order descending
            var result = pairs.OrderByDescending(c => c.l).ToArray();            
            return result;
        }
        #endregion


        #region --Operations--
        
        private readonly IMLModel model;
        private readonly int classes;

        void IDisposable.Dispose () { } // Nop
        #endregion
    }
}