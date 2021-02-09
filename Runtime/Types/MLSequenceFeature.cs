/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature.Types {

    using System;

    /// <summary>
    /// Sequence feature type.
    /// </summary>
    public sealed class MLSequenceType : MLFeatureType { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// Sequence element feature.
        /// </summary>
        public readonly MLFeatureType value;
        #endregion

        #region --Operations--

        internal MLSequenceType (string name, Type type) : base(name, type) {
            
        }
        #endregion
    }
}