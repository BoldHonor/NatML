/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct NMLFeature { // Loosely based on DLPack::DLTensor
        public void* data;
        public int* shape;
        public int dimensions;
        public NMLDataType dataType;
        public NMLFeatureFlag flags;
        public void* context;
    }
}