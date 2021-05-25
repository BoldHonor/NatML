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
    public class MLModel : IMLModel {

        #region --Client API--
        /// <summary>
        /// Model input feature types.
        /// </summary>
        public MLFeatureType[] inputs { get; private set; }

        /// <summary>
        /// Model output feature types.
        /// </summary>
        public MLFeatureType[] outputs { get; private set; }

        /// <summary>
        /// Get a value in the model metadata.
        /// </summary>
        /// <param name="key">Metadata key.</param>
        /// <returns>Metadata value. Throws exception if key is not present.</returns>
        public string this [string key] {
            get {
                // Check
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                // Fetch
                var buffer = new StringBuilder(2048);
                model.MetadataValue(key, buffer, buffer.Capacity);
                // Return
                if (buffer.Length > 0)
                    return buffer.ToString();
                else
                    throw new KeyNotFoundException($"MLModel metadata does not contain the requested key: {key}");
            }
        }

        /// <summary>
        /// Create an ML model.
        /// </summary>
        /// <param name="path">Path to ONNX model.</param>
        public MLModel (string path) : this(Create(path)) { }

        /// <summary>
        /// Dispose the model and release resources.
        /// </summary>
        public void Dispose () => model.ReleaseModel();
        #endregion


        #region --Operations--

        private readonly IntPtr model;

        internal MLModel (byte[] modelData) : this(Create(modelData)) { } // Devs don't need access to this just yet

        private MLModel (IntPtr model) {
            // Save
            this.model = model;
            // Marshal input types
            this.inputs = new MLFeatureType[model.InputFeatureCount()];
            for (var i = 0; i < inputs.Length; ++i) {
                model.InputFeatureType(i, out var nativeType);
                inputs[i] = nativeType.MarshalFeatureType();
                nativeType.ReleaseFeatureType();
            }
            // Marshal output types
            this.outputs = new MLFeatureType[model.OutputFeatureCount()];
            for (var i = 0; i < outputs.Length; ++i) {
                model.OutputFeatureType(i, out var nativeType);
                outputs[i] = nativeType.MarshalFeatureType();
                nativeType.ReleaseFeatureType();
            }
        }

        private static IntPtr Create (string path) {
            Bridge.CreateModel(path, out var model);
            if (model != IntPtr.Zero)
                return model;
            else
                throw new ArgumentException($"Failed to create MLModel from path: {path}", nameof(path));
        }

        private static unsafe IntPtr Create (byte[] data) {
            var model = IntPtr.Zero;
            fixed (void* buffer = data)
                Bridge.CreateModel(buffer, data.Length, out model);
            if (model != IntPtr.Zero)
                return model;
            else
                throw new ArgumentException(@"Failed to create MLModel from data", nameof(data));
        }

        IntPtr[] IMLModel.Predict (params IntPtr[] inputs) => Predict(inputs);

        private protected virtual IntPtr[] Predict (params IntPtr[] inputs) {
            var outputs = new IntPtr[this.outputs.Length];
            model.Predict(inputs, outputs);
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
                for (var i = 0; i < count; ++i) {
                    buffer.Clear();
                    model.MetadataKey(i, buffer, buffer.Capacity);
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