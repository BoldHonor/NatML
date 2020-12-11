/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Feature;

    internal class MLFeatureCollection : IReadOnlyList<MLFeature> {
        
        #region --Client API--
        MLFeature IReadOnlyList<MLFeature>.this[int index] { // INCOMPLETE // Nested types
            get {
                // Fetch
                var nameBuffer = new StringBuilder(2048);
                var shapeBuffer = new long[10]; // should be large enough for most things
                Feature(index, nameBuffer, out var nativeType, out var dimensions, shapeBuffer);
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
            }
        }

        int IReadOnlyCollection<MLFeature>.Count => count;

        IEnumerator<MLFeature> IEnumerable<MLFeature>.GetEnumerator () {
            var indexer = this as IReadOnlyList<MLFeature>;
            for (var i = 0; i < count; ++i)
                yield return indexer[i];
        }

        IEnumerator IEnumerable.GetEnumerator () => (this as IEnumerable<MLFeature>).GetEnumerator();
        #endregion


        #region --Operations--

        private readonly IntPtr model;
        private readonly int count;
        private readonly bool input;

        internal MLFeatureCollection (IntPtr model, bool input) {
            this.model = model;
            this.count = input ? model.InputFeatureCount() : model.OutputFeatureCount();
            this.input = input;
        }

        private void Feature (int index, StringBuilder nameBuffer, out int nativeType, out int dimensions, long[] shapeBuffer) {
            if (input)
                model.InputFeature(index, nameBuffer, out nativeType, out dimensions, shapeBuffer);
            else
                model.OutputFeature(index, nameBuffer, out nativeType, out dimensions, shapeBuffer);
        }

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