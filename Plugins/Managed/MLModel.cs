/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Internal;

    /// <summary>
    /// </summary>
    public sealed class MLModel : IDisposable, IEnumerable<string> {

        #region --Client API--
        /// <summary>
        /// Model inputs.
        /// </summary>
        public readonly IReadOnlyList<MLFeature> inputs;

        /// <summary>
        /// Model outputs.
        /// </summary>
        public readonly IReadOnlyList<MLFeature> outputs;

        /// <summary>
        /// Get a value in the model's metadata dictionary.
        /// </summary>
        /// <param name="key">Metadata key.</param>
        public string this [string key] { // DEPLOY
            get {
                var result = new StringBuilder(2048);
                model.MetadataValue(key, result);
                return result.ToString();
            }
        }
        
        /// <summary>
        /// Create an ML model.
        /// </summary>
        /// <param name="modelPath">Path to ONNX model.</param>
        public MLModel (string modelPath) {
            this.model = Bridge.CreateModel(modelPath);
            this.inputs = new MLFeatureCollection(model, true);
            this.outputs = new MLFeatureCollection(model, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        public MLTensor Predict (params MLTensor[] inputs) { // INCOMPLETE
            // Check input count
            if (inputs.Length != this.inputs.Count)
                throw new ArgumentException(@"Incorrect number of inputs provided", nameof(inputs));
            // Get data
            
            return default;
        }

        /// <summary>
        /// Dispose the model and release resources.
        /// </summary>
        public void Dispose () => model.DisposeModel(); // DEPLOY
        #endregion


        #region --Operations--

        private readonly IntPtr model;

        IEnumerator<string> IEnumerable<string>.GetEnumerator () { // DEPLOY
            var count = model.MetadataKeyCount();
            var buffer = new StringBuilder(2048);
            for (var i = 0; i < count; i++) {
                model.MetadataKey(i, buffer);
                yield return buffer.ToString();
                buffer.Clear();
            }
        }

        IEnumerator IEnumerable.GetEnumerator () => (this as IEnumerable<string>).GetEnumerator();
        #endregion
    }
}