/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class Bridge {

        private const string Assembly =
        #if UNITY_IOS && !UNITY_EDITOR
        @"__Internal";
        #else
        @"NatML";
        #endif


        #region --Lifecycle--
        [DllImport(Assembly, EntryPoint = @"NMLCreateModel")]
        public static extern IntPtr CreateModel ([MarshalAs(UnmanagedType.LPStr)] string modelPath);
        [DllImport(Assembly, EntryPoint = @"NMLDisposeModel")]
        public static extern void DisposeModel (this IntPtr model);
        [DllImport(Assembly, EntryPoint = @"NMLDisposeFeature")]
        public static extern void DisposeFeature (this IntPtr feature);
        #endregion


        #region --Metadata--
        [DllImport(Assembly, EntryPoint = @"NMLMetadataKeyCount")]
        public static extern int MetadataKeyCount (this IntPtr model);
        [DllImport(Assembly, EntryPoint = @"NMLMetadataKey")]
        public static extern void MetadataKey (this IntPtr model, int index, [MarshalAs(UnmanagedType.LPStr)] StringBuilder dest);
        [DllImport(Assembly, EntryPoint = @"NMLMetadataValue")]
        public static extern void MetadataValue (this IntPtr model, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] StringBuilder dest);
        #endregion


        #region --Introspection--
        [DllImport(Assembly, EntryPoint = @"NMLInputFeatureCount")]
        public static extern int InputFeatureCount (this IntPtr model);
        [DllImport(Assembly, EntryPoint = @"NMLOutputFeatureCount")]
        public static extern int OutputFeatureCount (this IntPtr model);
        [DllImport(Assembly, EntryPoint = @"NMLInputFeature")]
        public static extern void InputFeature (this IntPtr model, int index, [MarshalAs(UnmanagedType.LPStr)] StringBuilder dest, out NMLFeatureType type, out int dimensions, [Out] long[] shape);
        [DllImport(Assembly, EntryPoint = @"NMLOutputFeature")]
        public static extern void OutputFeature (this IntPtr model, int index, [MarshalAs(UnmanagedType.LPStr)] StringBuilder dest, out NMLFeatureType type, out int dimensions, [Out] long[] shape);
        #endregion


        #region --Inference--
        [DllImport(Assembly, EntryPoint = @"NMLPredict")]
        public static extern IntPtr Predict (this IntPtr model, [In] NMLFeature[] inputs, [Out] NMLFeature[] outputs);
        #endregion
    }
}