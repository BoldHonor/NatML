/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Types {

    using System;

    /// <summary>
    /// ML dictionary feature type.
    /// </summary>
    public sealed class MLDictionaryType : MLFeatureType {

        #region --Client API--
        /// <summary>
        /// Feature key type.
        /// </summary>
        public Type key => dataType;

        /// <summary>
        /// Feature value.
        /// </summary>
        public readonly MLFeatureType value;

        /// <summary>
        /// Create an ML dictionary feature type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public MLDictionaryType (string name, Type type, MLFeatureType value) : base(name, type) => this.value = value;
        #endregion


        #region --Operations--

        #endregion
    }
}