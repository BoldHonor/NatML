/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Feature;
    using Feature.Types;

    internal class MLFeatureCollection : IReadOnlyList<MLFeatureType> {
        
        #region --Client API--
        MLFeatureType IReadOnlyList<MLFeatureType>.this[int index] { // INCOMPLETE // Nested types
            get {
                // Fetch
                var nameBuffer = new StringBuilder(2048);
                var shapeBuffer = new long[10]; // should be large enough for most things
                Feature(index, nameBuffer, out var nativeType, out var dimensions, shapeBuffer);
                // Parse
                var name = nameBuffer.ToString();
                var type = nativeType.ManagedType();
                var shape = shapeBuffer.Take(dimensions).Select(d => (int)d).ToArray();
                // Check special types
                switch (nativeType) {
                    case 0: return null as MLFeatureType; // undefined
                    case NMLFeatureType.Sequence: return new MLSequenceType(name, type);
                    case NMLFeatureType.Dictionary: return new MLDictionaryType(name, type, null);
                    case var _ when shape.Length == 4: return new MLImageType(name, type, shape); // safe assumption
                    default: return new MLTensorType(name, type, shape);
                }
            }
        }

        int IReadOnlyCollection<MLFeatureType>.Count => count;

        IEnumerator<MLFeatureType> IEnumerable<MLFeatureType>.GetEnumerator () {
            var indexer = this as IReadOnlyList<MLFeatureType>;
            for (var i = 0; i < count; ++i)
                yield return indexer[i];
        }

        IEnumerator IEnumerable.GetEnumerator () => (this as IEnumerable<MLFeatureType>).GetEnumerator();
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

        private void Feature (int index, StringBuilder nameBuffer, out NMLFeatureType nativeType, out int dimensions, long[] shapeBuffer) {
            if (input)
                model.InputFeature(index, nameBuffer, out nativeType, out dimensions, shapeBuffer);
            else
                model.OutputFeature(index, nameBuffer, out nativeType, out dimensions, shapeBuffer);
        }
        #endregion
    }
}