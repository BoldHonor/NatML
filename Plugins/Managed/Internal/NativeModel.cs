/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {    

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class NativeModel : IMLModel, IClassifierModel {

        #region --Client API--

        public string this [string key] { // DEPLOY
            get {
                var result = new StringBuilder(2048);
                model.MetadataValue(key, result);
                return result.ToString();
            }
        }

        public NativeModel (IntPtr model) => this.model = model;

        public void Dispose () => model.DisposeModel(); // DEPLOY

        public MLFeature[] inputs => Enumerable.Range(0, model.InputFeatureCount()).Select(index => {
            // Fetch
            var nameBuffer = new StringBuilder(2048);
            var shapeBuffer = new long[10]; // should be large enough for most things
            model.InputFeature(index, nameBuffer, out var nativeType, out var dimensions, shapeBuffer);
            // Parse
            var name = nameBuffer.ToString();
            var type = TypeForNativeType(nativeType);
            var shape = shapeBuffer.Take(dimensions).Select(d => (int)d).ToArray();
            // Check special types
            switch (nativeType) {
                case 0: return null as MLFeature; // undefined
                case 7: return new MLSequenceFeature(name, type);
                case 8: return new MLDictionaryFeature(name, type, null);
                case var _ when shape.Length == 4: return new MLImageFeature(name, type, shape);
                default: return new MLTensorFeature(name, type, shape);
            }
        }).ToArray();

        public MLFeature[] outputs { // DEPLOY
            get {
                UnityEngine.Debug.Log("Output count: "+model.OutputFeatureCount());
                var result = Enumerable.Range(0, model.OutputFeatureCount()).Select(index => {
                    var nameBuffer = new StringBuilder(2048);
                    var shapeBuffer = new long[10]; // should be large enough for most things
                    model.OutputFeature(index, nameBuffer, out var type, out var dimensions, shapeBuffer);
                    UnityEngine.Debug.Log("Got output feature at index: "+index);
                    return default(MLFeature); //new MLFeature(nameBuffer.ToString(), TypeForNativeType(type), shapeBuffer);
                });
                return result.ToArray();
            }
        }

        public void Classify<T> (T[] pixelBuffer, int width, int height) where T : struct {
            var handle = GCHandle.Alloc(pixelBuffer, GCHandleType.Pinned);
            Classify(handle.AddrOfPinnedObject(), width, height);
            handle.Free();
        }

        public void Classify (IntPtr nativeBuffer, int width, int height) { // INCOMPLETE

        }
        #endregion


        #region --Operations--

        private readonly IntPtr model;

        IEnumerator<string> IEnumerable<string>.GetEnumerator () { // DEPLOY
            var count = model.MetadataKeyCount();
            UnityEngine.Debug.Log($"Metadata key count: {count}");
            var buffer = new StringBuilder(2048);
            for (var i = 0; i < count; i++) {
                model.MetadataKey(i, buffer);
                yield return buffer.ToString();
                buffer.Clear();
            }
        }

        IEnumerator IEnumerable.GetEnumerator () => (this as IEnumerable<string>).GetEnumerator();

        private static Type TypeForNativeType (int type) {
            switch (type) {
                case 0: goto case default;
                case 1: return typeof(short);
                case 2: return typeof(int);
                case 3: return typeof(long);
                case 4: return typeof(float);
                case 5: return typeof(double);
                case 6: return typeof(string);
                case 7: return typeof(IList);
                case 8: return typeof(IDictionary);
                default: return null;
            }
        }
        #endregion
    }
}