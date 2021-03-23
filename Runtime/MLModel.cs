/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Internal;

    /// <summary>
    /// ML model.
    /// </summary>
    public class MLModel : IDisposable, IReadOnlyDictionary<string, string> {

        #region --Client API--
        /// <summary>
        /// Model inputs.
        /// </summary>
        public readonly MLFeatureType[] inputs;

        /// <summary>
        /// Model outputs.
        /// </summary>
        public readonly MLFeatureType[] outputs;

        /// <summary>
        /// Get a value in the model's metadata dictionary.
        /// </summary>
        /// <param name="key">Metadata key.</param>
        public string this [string key] {
            get {
                var result = new StringBuilder(2048);
                model.MetadataValue(key, result);
                return result.ToString();
            }
        }

        /// <summary>
        /// Create an ML model.
        /// </summary>
        /// <param name="path">Path to ONNX model.</param>
        public MLModel (string path) {
            // Create model
            Bridge.CreateModel(path, out this.model);
            // Marshal inputs
            this.inputs = new MLFeatureType[model.InputFeatureCount()];
            for (var i = 0; i < inputs.Length; i++) {
                model.InputFeatureType(i, out var nativeType);
                inputs[i] = nativeType.MarshalFeatureType();
            }
            // Marshal outputs
            this.outputs = new MLFeatureType[model.OutputFeatureCount()];
            for (var i = 0; i < outputs.Length; i++) {
                model.OutputFeatureType(i, out var nativeType);
                outputs[i] = nativeType.MarshalFeatureType();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public unsafe MLFeature[] Predict (params MLFeature[] inputs) {
            // Check input count
            if (inputs.Length != this.inputs.Length)
                throw new ArgumentException(@"Incorrect number of inputs provided", nameof(inputs));
            // Predict
            var inputFeatures = inputs.Select((f, i) => f.CreateNMLFeature(this.inputs[i])).ToArray();
            var outputFeatures = Predict(inputFeatures);
            // Copy outputs
            var outputs = outputFeatures.Select(f => f.MarshalFeature()).ToArray();
            return outputs;
        }

        /// <summary>
        /// Dispose the model and release resources.
        /// </summary>
        public virtual void Dispose () => model.ReleaseModel();
        #endregion


        #region --Operations--
        /// <summary>
        /// </summary>
        protected readonly IntPtr model;

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        protected IntPtr[] Predict (params IntPtr[] inputs) { // Inputs are released
            // Predict
            var outputs = new IntPtr[this.outputs.Length];
            model.Predict(inputs, outputs);
            // Release
            foreach (var feature in inputs)
                feature.ReleaseFeature();
            return outputs;
        }

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator () {
            foreach (var key in (this as IReadOnlyDictionary<string, string>).Keys)
                yield return new KeyValuePair<string, string>(key, this[key]);
        }

        IEnumerable<string> IReadOnlyDictionary<string, string>.Keys {
            get {
                var count = model.MetadataCount();
                var buffer = new StringBuilder(2048);
                for (var i = 0; i < count; i++) {
                    buffer.Clear();
                    model.MetadataKey(i, buffer);
                    yield return buffer.ToString();
                }
            }
        }

        IEnumerable<string> IReadOnlyDictionary<string, string>.Values {
            get {
                foreach (var key in (this as IReadOnlyDictionary<string, string>).Keys)
                    yield return this[key];
            }
        }

        int IReadOnlyCollection<KeyValuePair<string, string>>.Count => model.MetadataCount();

        bool IReadOnlyDictionary<string, string>.ContainsKey (string key) => (this as IReadOnlyDictionary<string, string>).Keys.Contains(key);

        bool IReadOnlyDictionary<string, string>.TryGetValue (string key, out string value) {
            var validKey = (this as IReadOnlyDictionary<string, string>).Keys.Contains(key);
            value = validKey ? this[key] : default;
            return validKey;
        }

        IEnumerator IEnumerable.GetEnumerator () => (this as IEnumerable<KeyValuePair<string, string>>).GetEnumerator();
        #endregion
    }
}