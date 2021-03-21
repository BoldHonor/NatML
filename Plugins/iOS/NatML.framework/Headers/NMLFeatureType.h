//
//  NMLFeatureType.h
//  NatML
//
//  Created by Yusuf Olokoba on 3/14/21.
//  Copyright © 2021 Yusuf Olokoba. All rights reserved.
//

#pragma once

#include "NMLTypes.h"


#pragma region --Lifecycle--
/*!
 @struct NMLFeatureType
 
 @abstract Descriptor for an ML feature.

 @discussion Descriptor for an ML feature.
*/
struct NMLFeatureType;
typedef struct NMLFeatureType NMLFeatureType;

/*!
 @function NMLReleaseFeatureType

 @abstract Release an ML feature type.

 @discussion Release an ML feature type.

 @param type
 Feature type to release.
*/
BRIDGE EXPORT void APIENTRY NMLReleaseFeatureType (NMLFeatureType* type);
#pragma endregion


#pragma region --Introspection--
/*!
 @function NMLFeatureTypeName

 @abstract Get the name of a given feature type.

 @discussion Get the name of a given feature type.

 @param type
 Feature type.

 @param name
 Destination UTF-8 string.
*/
BRIDGE EXPORT void APIENTRY NMLFeatureTypeGetName (NMLFeatureType* type, char* name);

/*!
 @function NMLFeatureTypeDataType

 @abstract Get the data type of a given feature type.

 @discussion Get the data type of a given feature type.

 @param type
 Feature type.
*/
BRIDGE EXPORT NMLDataType APIENTRY NMLFeatureTypeGetDataType (NMLFeatureType* type);

/*!
 @function NMLFeatureTypeDimensions

 @abstract Get the number of dimensions for a given feature type.

 @discussion Get the number of dimensions for a given feature type.
 If the type is not a tensor, this function will return `0`.

 @param type
 Feature type.
*/
BRIDGE EXPORT int32_t APIENTRY NMLFeatureTypeGetDimensions (NMLFeatureType* type);

/*!
 @function NMLFeatureTypeShape

 @abstract Get the shape of a given feature type.

 @discussion Get the shape of a given feature type.
 The length of the shape array must be at least as large as the number of dimensions present in the type.

 @param type
 Feature type.

 @param shape
 Destination shape array.

 @param shapeLen
 Destination shape array length.
*/
BRIDGE EXPORT void APIENTRY NMLFeatureTypeGetShape (NMLFeatureType* type, int32_t* shape, int32_t shapeLen);
#pragma endregion
