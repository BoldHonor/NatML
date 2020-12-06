/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base interface implemented by all ML models.
    /// </summary>
    public interface IMLModel : IDisposable, IEnumerable<string> { // CHECK // Enumerable on metadata keys

        /// <summary>
        /// Get a value in the model's metadata dictionary.
        /// </summary>
        string this [string key] { get; }

        /// <summary>
        /// Model inputs.
        /// </summary>
        MLFeature[] inputs { get; }

        /// <summary>
        /// Model outputs.
        /// </summary>
        MLFeature[] outputs { get; }
    }
}