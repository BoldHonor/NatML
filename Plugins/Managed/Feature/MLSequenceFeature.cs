/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using System;

    /// <summary>
    /// ML sequence feature.
    /// </summary>
    public sealed class MLSequenceFeature : MLFeature { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// Sequence element feature.
        /// </summary>
        public readonly MLFeature value;
        #endregion

        #region --Operations--

        internal MLSequenceFeature (string name, Type type) : base(name, type) {
            
        }
        #endregion
    }
}