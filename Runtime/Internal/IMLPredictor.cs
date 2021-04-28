/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;

    /// <summary>
    /// </summary>
    public interface IMLPredictor<TOutput> : IDisposable {

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        TOutput Predict (params MLFeature[] inputs);
    }
}