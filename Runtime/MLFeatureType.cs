/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;

    /// <summary>
    /// ML input or output feature.
    /// </summary>
    public abstract class MLFeatureType {

        #region --Client API--
        /// <summary>
        /// Feature name.
        /// </summary>
        public readonly string name;

        /// <summary>
        /// Feature data type.
        /// This will typically be a numeric type.
        /// </summary>
        public readonly Type type;
        #endregion


        #region --Operations--

        protected internal MLFeatureType (string name, Type type) {
            this.name = name;
            this.type = type;
        }
        #endregion
    }
}