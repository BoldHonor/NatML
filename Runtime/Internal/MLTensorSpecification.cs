/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MLTensorSpecification { // Loosely based on DLPack::DLTensor

        public readonly void* data;
        public readonly int* shape; // long?
        public readonly int dimensions;
        public readonly MLFeatureType type;
        public readonly MLTensorFlag flags;

        public unsafe MLTensorSpecification (void* data, int* shape, int dimensions, MLFeatureType type, MLTensorFlag flags = 0) {
            this.data = data;
            this.shape = shape;
            this.dimensions = dimensions;
            this.type = type;
            this.flags = flags;
        }
    }
}