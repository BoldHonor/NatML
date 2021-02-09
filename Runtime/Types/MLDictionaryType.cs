/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature.Types {

    using System;

    /// <summary>
    /// Dictionary feature type.
    /// </summary>
    public sealed class MLDictionaryType : MLFeatureType { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// Feature key type.
        /// </summary>
        public Type key => type;

        /// <summary>
        /// Feature value.
        /// </summary>
        public readonly MLFeatureType value;
        #endregion


        #region --Operations--

        internal MLDictionaryType (string name, Type type, MLFeatureType value) : base(name, type) => this.value = value;
        #endregion
    }
}