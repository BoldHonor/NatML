/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System;
    using System.Linq;
    using Features.Types;
    using Internal;

    /// <summary>
    /// </summary>
    public class MLDenseClassificationPredictor : IMLPredictor<(string label, float confidence)[]> {

        #region --Client API--
        /// <summary>
        /// Classification labels.
        /// </summary>
        public readonly string[] labels;

        /// <summary>
        /// Create a dense classification predictor.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="labels">List of labels which the classifier outputs.</param>
        public MLDenseClassificationPredictor (MLModel model, string[] labels) {
            // Save
            this.model = model;
            this.labels = labels;
            this.classes = ((MLArrayType)model.outputs.First()).shape.Aggregate(1, (a, b) => a * b);
            // Check
            if (labels.Length != classes)
                throw new ArgumentOutOfRangeException(nameof(labels), $"Classifier predcitor received {labels.Length} labels but expected {classes}");
        }

        /// <summary>
        /// Predict all classes on a feature.
        /// This will return the predicted labels in descending order of confidence scores.
        /// </summary>
        /// <param name="inputs">Input feature.</param>
        /// <returns></returns>
        public unsafe (string label, float confidence)[] Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"Classifier predictor expects a single feature", nameof(inputs));
            // Predict
            var inputFeature = (inputs.First() as IMLFeature).Create(model.inputs.First());
            var outputFeature = model.Predict(inputFeature).First();
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