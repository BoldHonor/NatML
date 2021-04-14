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
    public abstract class MLModule<TOutput> : IDisposable, IReadOnlyDictionary<string, string> {

        #region --Client API--
        /// <summary>
        /// Model input feature types.
        /// </summary>
        public readonly MLFeatureType[] inputs;

        /// <summary>
        /// Model output feature types.
        /// </summary>
        public readonly MLFeatureType[] outputs;

        /// <summary>
        /// Get a value in the model metadata.
        /// </summary>
        /// <param name="key">Metadata key.</param>
        /// <returns>Metadata value or `null` if key is not present.</returns>
        public string this [string key] {
            get {
                var buffer = new StringBuilder(2048);
                model.MetadataValue(key, buffer);
                var result = buffer.ToString();
                return !string.IsNullOrEmpty(result) ? result : null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public abstract TOutput Predict (params MLFeature[] inputs);

        /// <summary>
        /// Dispose the model and release resources.
        /// </summary>
        public virtual void Dispose () => model.ReleaseModel();
        #endregion


        #region --Operations--

        private readonly IntPtr model; // We don't want anyone to touch this

        protected MLModule (string path) {
            // Create model
            Bridge.CreateModel(path, out this.model);
            // Marshal inputs
            this.inputs = new MLFeatureType[model.InputFeatureCount()];
            for (var i = 0; i < inputs.Length; ++i) {
                model.InputFeatureType(i, out var nativeType);
                inputs[i] = nativeType.MarshalFeatureType();
                nativeType.ReleaseFeatureType();
            }
            // Marshal outputs
            this.outputs = new MLFeatureType[model.OutputFeatureCount()];
            for (var i = 0; i < outputs.Length; ++i) {
                model.OutputFeatureType(i, out var nativeType);
                outputs[i] = nativeType.MarshalFeatureType();
                nativeType.ReleaseFeatureType();
            }
        }

        protected IntPtr[] Predict (params IntPtr[] inputs) { // Black box
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