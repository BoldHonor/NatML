/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct NMLTensorSpecification { // Loosely based on DLPack::DLTensor

        public readonly void* data;
        public readonly int* shape; // long?
        public readonly int dimensions;
        public readonly NMLFeatureType type;
        public readonly NMLTensorFlag flags;

        public unsafe NMLTensorSpecification (void* data, int* shape, int dimensions, NMLFeatureType type, NMLTensorFlag flags = 0) {
            this.data = data;
            this.shape = shape;
            this.dimensions = dimensions;
            this.type = type;
            this.flags = flags;
        }
    }
}