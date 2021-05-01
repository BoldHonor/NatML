/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Editor {

    using UnityEngine;
    using UnityEditor.Experimental.AssetImporters;
    using System.IO;

    /// <summary>
    /// ONNX model importer.
    /// </summary>
    [ScriptedImporter(1, "onnx")]
    public class MLImporter : ScriptedImporter {

        public override void OnImportAsset (AssetImportContext ctx) {
            var modelData = File.ReadAllBytes(ctx.assetPath);
            var serializedModel = ScriptableObject.CreateInstance<MLSerializedModel>();
            serializedModel.data = modelData;
            ctx.AddObjectToAsset("MLModel", serializedModel);
            ctx.SetMainObject(serializedModel);
        }
    }
}