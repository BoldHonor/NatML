//
//  NMLFeature.h
//  NatML
//
//  Created by Yusuf Olokoba on 3/14/2021.
//  Copyright © 2021 Yusuf Olokoba. All rights reserved.
//

#pragma once

#include "NMLFeatureType.h"


#pragma region --Lifecycle--
/*!
 @struct NMLFeature
 
 @abstract Model input or output feature.

 @discussion Model input or output feature. This is loosely based on `DLPack::DLTensor`.
*/
struct NMLFeature;
typedef struct NMLFeature NMLFeature;

/*!
 @function NMLReleaseFeature

 @abstract Release an ML feature.

 @discussion Release an ML feature.

 @param feature
 Feature to release.
*/
BRIDGE EXPORT void APIENTRY NMLReleaseFeature (NMLFeature* feature);
#pragma endregion


#pragma region --Introspection--
/*!
 @function NMLFeatureGetType

 @abstract Get the feature type.

 @discussion Get the feature type.

 @param feature
 Feature.

 @param type
 Output feature type. This type should be released once it is no longer in use.
*/
BRIDGE EXPORT void APIENTRY NMLFeatureGetType (NMLFeature* feature, NMLFeatureType** type);

/*!
 @function NMLFeatureGetData

 @abstract Get the feature data.

 @discussion Get the feature data.

 @param feature
 Feature.

 @returns Opaque pointer to feature data.
*/
BRIDGE EXPORT void* APIENTRY NMLFeatureGetData (NMLFeature* feature);
#pragma endregion


#pragma region --Constructors--
/*!
 @function NMLCreateFeature

 @abstract Create a feature from a data buffer.

 @discussion Create a feature from a data buffer.
 The data will not be released when the feature is released.

 @param data
 Feature data.

 @param shape
 Feature shape.

 @param dims
 Number of dimensions in shape.

 @param dtype
 Feature data type.

 @param feature
 Destination pointer to created feature.
*/
BRIDGE EXPORT void APIENTRY NMLCreateFeature (
    const void* data,
    const int32_t* shape,
    int32_t dims,
    NMLDataType dtype,
    NMLFeature** feature
);

/*!
 @function NMLCreateFeatureFromPixelBuffer

 @abstract Create a feature from a pixel buffer.

 @discussion Create a feature from a pixel buffer.
 The pixel buffer MUST have an RGBA8888 layout (32 bits per pixel).

 @param pixelBuffer
 Pixel buffer to convert to a feature.

 @param width
 Pixel buffer width.

 @param height
 Pixel buffer height.

 @param shape
 Feature shape.

 @param dtype
 Feature data type.

 @param aspectMode
 Pixel buffer aspect mode when converting to feature.

 @param mean
 Normalization [R, G, B] channel mean when converting to feature. Can be `NULL`.

 @param std
 Normalization [R, G, B] channel standard deviation when converting to feature. Can be `NULL`.

 @param feature
 Destination pointer to created feature.
*/
BRIDGE EXPORT void APIENTRY NMLCreateFeatureFromPixelBuffer (
    const uint8_t* pixelBuffer,
    int32_t width,
    int32_t height,
    const int32_t* shape,
    NMLDataType dtype,
    NMLAspectMode aspectMode,
    float* mean,
    float* std,
    NMLFeature** feature
);

/*!
 @function NMLCreateInputFeature

 @abstract Create an feature from an existing feature.

 @discussion Create an feature from an existing feature.

 When a feature is provided to a model, the model will typically create an 
 input feature, one which has any transformations applied.

 With this function, the input feature is immediately converted to a feature which can be directly 
 consumed by a model, removing the need for any pre-processing.

 Note that the source feature MUST NOT be released before the created input feature.

 @param feature
 Feature to convert to a model input feature.

 @param outFeature
 Output converted feature.
*/
BRIDGE EXPORT void APIENTRY NMLCreateInputFeature (NMLFeature* feature, NMLFeature** outFeature);
#pragma endregion
