/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using System;

    /// <summary>
    /// ML dictionary feature.
    /// </summary>
    public sealed class MLDictionaryFeature : MLFeature { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// Feature key type.
        /// </summary>
        public Type key => type;

        /// <summary>
        /// Feature value.
        /// </summary>
        public readonly MLFeature value;
        #endregion


        #region --Operations--

        internal MLDictionaryFeature (string name, Type type, MLFeature value) : base(name, type) => this.value = value;
        #endregion
    }
}