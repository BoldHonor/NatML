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
    public class MLDenseClassifier : IMLPredictor<(string label, float confidence)[]> { // DOC // Slower than `MLClassifier`

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
        public MLDenseClassifier (MLModel model, string[] labels) {
            this.model = model;
            this.labels = labels;
            this.classes = ((MLArrayType)model.outputs.First()).shape.Aggregate(1, (a, b) => a * b);
        }

        /// <summary>
        /// Predict all classes on a features.
        /// This will return the predicted labels in descending order of confidence scores.
        /// </summary>
        /// <param name="inputs">Input features.</param>
        /// <returns></returns>
        public unsafe (string label, float confidence)[] Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"MLDenseClassifier expects a single feature", nameof(inputs));
            // Predict
            var inputFeature = (inputs.First() as IMLFeature).Create(model.inputs.First());
            var outputFeature = model.Predict(inputFeature).First();
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
        #endregion


        #region --Operations--
        
        private readonly IMLModel model;
        private readonly int classes;

        void IDisposable.Dispose () { } // Nop
        #endregion
    }
}