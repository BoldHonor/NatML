/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NatSuite.ML.Editor")]
namespace NatSuite.ML {

    using UnityEngine;

    /// <summary>
    /// </summary>
    public sealed class MLSerializedModel : ScriptableObject {
        
        #region --Client API--
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public MLModel Deserialize () => new MLModel(data);
        #endregion
        

        #region --Operations--
        [SerializeField, HideInInspector] internal byte[] data;
        #endregion
    }
}