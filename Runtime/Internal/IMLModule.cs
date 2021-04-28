/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    public interface IMLModule : IDisposable, IReadOnlyDictionary<string, string> {

        /// <summary>
        /// Model input feature types.
        /// </summary>
        MLFeatureType[] inputs { get; }

        /// <summary>
        /// Model output feature types.
        /// </summary>
        MLFeatureType[] outputs { get; }
    }
}