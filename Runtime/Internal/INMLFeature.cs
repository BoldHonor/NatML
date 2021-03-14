/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using Features.Types;

    public interface INMLFeature {

        IntPtr CreateFeature (MLFeatureType featureType);
    }
}