/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Features {

    using System;

    /// <summary>
    /// </summary>
    public sealed class MLDictionaryFeature : MLFeature {

        #region --Operations--

        internal MLDictionaryFeature () : base(null) {

        }

        protected internal override IntPtr CreateNMLFeature (MLFeatureType featureType) {
            return IntPtr.Zero;
        }
        #endregion
    }
}