/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using System;

    /// <summary>
    /// ML sequence feature.
    /// </summary>
    public class MLSequenceFeature : MLFeature { // INCOMPLETE

        #region --Client API--
        public readonly MLFeature value;
        #endregion

        #region --Operations--

        internal MLSequenceFeature (string name, Type type) : base(name, type) {
            
        }
        #endregion
    }
}