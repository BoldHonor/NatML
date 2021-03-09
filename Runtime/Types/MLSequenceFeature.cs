/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Features.Types {

    using System;

    /// <summary>
    /// Sequence feature type.
    /// </summary>
    public sealed class MLSequenceType : MLFeatureType { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// Sequence element feature.
        /// </summary>
        public readonly MLFeatureType element;
        #endregion

        #region --Operations--

        public MLSequenceType (string name, Type type) : base(name, type) {
            
        }
        #endregion
    }
}