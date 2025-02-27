/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;

    /// <summary>
    /// ML feature type.
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
        public readonly Type dataType;
        #endregion


        #region --Operations--

        protected MLFeatureType (string name, Type type) => (this.name, this.dataType) = (name, type);
        #endregion
    }
}