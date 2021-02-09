/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    /// <summary>
    /// </summary>
    public interface IMLInputFeature {
        
        /// <summary>
        /// </summary>
        NMLTensorSpecification Lock ();

        /// <summary>
        /// </summary>
        void Unlock ();
    }
}