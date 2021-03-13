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
    using Features.Types;

    internal abstract class MLFeatureMap : IReadOnlyList<MLFeatureType> {
        
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
                    case NMLDataType.Sequence: return new MLSequenceType(name, type);
                    case NMLDataType.Dictionary: return new MLDictionaryType(name, type, null);
                    case var _ when shape.Length == 4: return new MLImageType(name, type, shape); // safe assumption
                    default: return new MLArrayType(name, type, shape);
                }
            }
        }

        int IReadOnlyCollection<MLFeatureType>.Count => size;

        IEnumerator<MLFeatureType> IEnumerable<MLFeatureType>.GetEnumerator () {
            var indexer = this as IReadOnlyList<MLFeatureType>;
            for (var i = 0; i < size; ++i)
                yield return indexer[i];
        }

        IEnumerator IEnumerable.GetEnumerator () => (this as IEnumerable<MLFeatureType>).GetEnumerator();
        #endregion


        #region --Operations--

        protected readonly int size;

        protected MLFeatureMap (int size) => this.size = size;

        protected abstract void Feature (int index, StringBuilder nameBuffer, out NMLDataType nativeType, out int dimensions, long[] shapeBuffer);
        #endregion
    }

    internal sealed class MLInputFeatureMap : MLFeatureMap {

        private readonly IntPtr model;

        internal MLInputFeatureMap (IntPtr model) : base(model.InputFeatureCount()) => this.model = model;

        protected override void Feature (int index, StringBuilder nameBuffer, out NMLDataType nativeType, out int dimensions, long[] shapeBuffer) {
            model.InputFeature(index, nameBuffer, out nativeType, out dimensions, shapeBuffer);
        }
    }

    internal sealed class MLOutputFeatureMap : MLFeatureMap {

        private readonly IntPtr model;

        internal MLOutputFeatureMap (IntPtr model) : base(model.OutputFeatureCount()) => this.model = model;

        protected override void Feature (int index, StringBuilder nameBuffer, out NMLDataType nativeType, out int dimensions, long[] shapeBuffer) {
            model.OutputFeature(index, nameBuffer, out nativeType, out dimensions, shapeBuffer);
        }
    }
}