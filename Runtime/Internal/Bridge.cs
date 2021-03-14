/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using Vision;

    public static class Bridge {

        private const string Assembly =
        #if UNITY_IOS && !UNITY_EDITOR
        @"__Internal";
        #else
        @"NatML";
        #endif


        #region --Model Lifecycle--
        [DllImport(Assembly, EntryPoint = @"NMLCreateModel")]
        public static extern void CreateModel ([MarshalAs(UnmanagedType.LPStr)] string modelPath, out IntPtr model);
        [DllImport(Assembly, EntryPoint = @"NMLReleaseModel")]
        public static extern void ReleaseModel (this IntPtr model);
        #endregion


        #region --Feature Lifecycle--
        [DllImport(Assembly, EntryPoint = @"NMLCreateFeature")]
        public static unsafe extern void CreateFeature (void* data, [In] int[] shape, int dims, NMLDataType dtype, out IntPtr feature);
        [DllImport(Assembly, EntryPoint = @"NMLCreateFeatureFromPixelBuffer")]
        public static unsafe extern void CreateFeatureFromPixelBuffer (void* pixelBuffer, int width, int height, [In] int[] shape, NMLDataType dtype, MLAspectMode aspect, out IntPtr feature);
        [DllImport(Assembly, EntryPoint = @"NMLReleaseFeature")]
        public static extern void ReleaseFeature (this IntPtr feature);
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
        public static extern void InputFeature (this IntPtr model, int index, [MarshalAs(UnmanagedType.LPStr)] StringBuilder dest, out NMLDataType type, out int dimensions, [Out] long[] shape);
        [DllImport(Assembly, EntryPoint = @"NMLOutputFeature")]
        public static extern void OutputFeature (this IntPtr model, int index, [MarshalAs(UnmanagedType.LPStr)] StringBuilder dest, out NMLDataType type, out int dimensions, [Out] long[] shape);
        #endregion


        #region --Inference--
        [DllImport(Assembly, EntryPoint = @"NMLPredict")]
        public static extern void Predict (this IntPtr model, [In] IntPtr[] inputs, [Out] IntPtr[] outputs);
        #endregion
    }
}