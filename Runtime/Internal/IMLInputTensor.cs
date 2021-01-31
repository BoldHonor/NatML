/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    /// <summary>
    /// </summary>
    public unsafe interface IMLInputTensor {
        
        /// <summary>
        /// </summary>
        MLTensorSpecification Lock ();

        /// <summary>
        /// </summary>
        void Unlock ();
    }
}