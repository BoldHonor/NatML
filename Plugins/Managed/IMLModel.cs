/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;

    /// <summary>
    /// </summary>
    public interface IMLModel : IDisposable {

        /// <summary>
        /// Get a value in the model's metadata dictionary.
        /// </summary>
        string this [string key] { get; }
    }
}