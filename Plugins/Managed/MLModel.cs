/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using Internal;

    /// <summary>
    /// </summary>
    public sealed class MLModel : MLModelBase {

        #region --Client API--
        /// <summary>
        /// Create an ML model.
        /// </summary>
        /// <param name="modelPath">Path to ONNX model.</param>
        public MLModel (string modelPath) : base(Bridge.CreateModel(modelPath)) { }

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        public MLTensor Predict (params MLTensor[] inputs) {
            // Check input count
            if (inputs.Length != this.inputs.Count)
                throw new ArgumentException(@"Incorrect number of inputs provided", nameof(inputs));
            // Do stuff
            return default;
        }
        #endregion


        #region --Operations--

    
        #endregion
    }
}