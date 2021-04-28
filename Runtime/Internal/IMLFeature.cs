/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    
    /// <summary>
    /// </summary>
    public interface IMLFeature {

        /// <summary>
        /// </summary>
        /// <param name="featureType"></param>
        /// <returns></returns>
        IntPtr Create (MLFeatureType featureType);
    }
}