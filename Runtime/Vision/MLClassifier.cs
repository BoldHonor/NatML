/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    public sealed class MLClassifier : IDisposable { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// </summary>
        public readonly MLModel model;

        /// <summary>
        /// </summary>
        public readonly IReadOnlyList<string> labels;

        /// <summary>
        /// </summary>
        /// <param name="modelPath">Path to ONNX model.</param>
        /// <param name="labels">List of labels which the classifier outputs.</param>
        public MLClassifier (string modelPath, IReadOnlyList<string> labels) {
            this.model = new MLModel(modelPath);
            this.labels = labels;
        }

        /// <summary>
        /// </summary>
        public (string label, float confidence) Classify (MLFeature input) {
            return default;
        }

        /// <summary>
        /// </summary>
        public void Dispose () => model.Dispose();
        #endregion


        #region --Operations--

        #endregion
    }
}