/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Types {

    using System;

    /// <summary>
    /// ML sequence feature type.
    /// </summary>
    public sealed class MLSequenceType : MLFeatureType { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// Sequence element feature.
        /// </summary>
        public readonly MLFeatureType element;

        /// <summary>
        /// Create an ML sequence feature type.
        /// </summary>
        /// <param name="name">Feature name.</param>
        /// <param name="type">Sequence element data type.</param>
        public MLSequenceType (string name, Type type) : base(name, type) {
            
        }
        #endregion

        #region --Operations--

        #endregion
    }
}