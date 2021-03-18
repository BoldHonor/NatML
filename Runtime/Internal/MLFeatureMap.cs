/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Features.Types;

    internal abstract class MLFeatureMap : IReadOnlyList<MLFeatureType> {
        
        #region --Client API--

        MLFeatureType IReadOnlyList<MLFeatureType>.this[int index] {
            get {
                GetFeatureType(index, out var nativeType);
                return nativeType.MarshalFeatureType();
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

        protected abstract void GetFeatureType (int index, out IntPtr type);
        #endregion
    }

    internal sealed class MLInputFeatureMap : MLFeatureMap {

        private readonly IntPtr model;

        internal MLInputFeatureMap (IntPtr model) : base(model.InputFeatureCount()) => this.model = model;

        protected override void GetFeatureType (int index, out IntPtr type) => model.InputFeatureType(index, out type);
    }

    internal sealed class MLOutputFeatureMap : MLFeatureMap {

        private readonly IntPtr model;

        internal MLOutputFeatureMap (IntPtr model) : base(model.OutputFeatureCount()) => this.model = model;

        protected override void GetFeatureType (int index, out IntPtr type) => model.OutputFeatureType(index, out type);
    }
}