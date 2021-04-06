/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    
    /// <summary>
    /// </summary>
    public interface INMLFeature {

        /// <summary>
        /// </summary>
        /// <param name="featureType"></param>
        /// <returns></returns>
        IntPtr CreateNativeFeature (MLFeatureType featureType);
    }
}