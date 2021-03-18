/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;

    public interface INMLFeature {

        IntPtr CreateFeature (MLFeatureType featureType);
    }
}