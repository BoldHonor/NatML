/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal.Blitters {

    using System;

    /// <summary>
    /// Lightweight object which enables zero-copy inference on a given ML feature.
    /// This object is always bound to a specific `MLFeature`, and MUST be disposed once inference is complete.
    /// </summary>
    public interface IFeatureBlitter : IDisposable {

        /// <summary>
        /// </summary>
        NMLFeature feature { get; }
    }
}