/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;

    /// <summary>
    /// </summary>
    public static class MLModel {

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="modelPath"></param>
        public static T Create<T> (string modelPath) where T : IMLModel {
            return default;
        }
        #endregion
    }
}