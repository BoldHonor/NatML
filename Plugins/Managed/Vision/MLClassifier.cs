/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    public sealed class MLClassifier : MLModel { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="modelPath">Path to ONNX model.</param>
        /// <param name="labels">List of labels which the classifier outputs.</param>
        public MLClassifier (string modelPath, IReadOnlyList<string> labels) : base(modelPath) => this.labels = labels;

        /// <summary>
        /// </summary>
        public (string label, float confidence) Classify (MLTensor input) {
            return default;
        }
        #endregion


        #region --Operations--

        private readonly IReadOnlyList<string> labels;
        #endregion
    }
}