/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Runtime.InteropServices;

    public static class Bridge {

        private const string Assembly =
        #if UNITY_IOS && !UNITY_EDITOR
        @"__Internal";
        #else
        @"NatML";
        #endif

        [DllImport(Assembly, EntryPoint = @"NMLCreateModel")]
        public static extern IntPtr CreateModel ([MarshalAs(UnmanagedType.LPStr)] string modelPath);
        [DllImport(Assembly, EntryPoint = @"NMLDisposeModel")]
        public static extern void DisposeModel (this IntPtr model);
        [DllImport(Assembly, EntryPoint = @"NMLMetadataKeys")]
        public static extern void MetadataKeys (this IntPtr model, out IntPtr keys, out int keyCount);
        [DllImport(Assembly, EntryPoint = @"NMLMetadataValue")]
        public static extern void MetadataValue (this IntPtr model, [MarshalAs(UnmanagedType.LPStr)] string key, out IntPtr value);
    }
}