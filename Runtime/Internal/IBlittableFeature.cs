/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using Blitters;

    /// <summary>
    /// To support high performance inference, we want to minimize the number of memory copies
    /// performed when running a model inference. This interface is implemented by `MLFeature`
    /// implementations, allowing for zero-copy model inference for managed and native input buffers.
    /// </summary>
    public interface IBlittableFeature {
        
        /// <summary>
        /// Create a blitter to allow for zero-copy ML inference.
        /// </summary>
        IFeatureBlitter CreateBlitter ();
    }
}