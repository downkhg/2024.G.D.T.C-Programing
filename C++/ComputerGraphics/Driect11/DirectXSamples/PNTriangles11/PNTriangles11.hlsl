//--------------------------------------------------------------------------------------
// File: PNTriangles11.hlsl
//
// These shaders implement the PN-Triangles tessellation technique
//
// Contributed by the AMD Developer Relations Team
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//--------------------------------------------------------------------------------------

#include "AdaptiveTessellation.hlsl"


//--------------------------------------------------------------------------------------
// Constant buffer
//--------------------------------------------------------------------------------------

cbuffer cbPNTriangles : register( b0 )
{
    float4x4    g_f4x4World;                // World matrix for object
    float4x4    g_f4x4ViewProjection;       // View * Projection matrix
    float4x4    g_f4x4WorldViewProjection;  // World * View * Projection matrix
    float4      g_f4LightDir;               // Light direction vector
    float4      g_f4Eye;                    // Eye
    float4      g_f4ViewVector;             // View Vector
    float4      g_f4TessFactors;            // Tessellation factors ( x=Edge, y=Inside, z=MinDistance, w=Range )
    float4      g_f4ScreenParams;           // Screen resolution ( x=Current width, y=Current height )
    float4      g_f4GUIParams1;             // GUI params1 ( x=BackFace Epsilon, y=Silhouette Epsilon, z=Range scale, w=Edge size )
    float4      g_f4GUIParams2;             // GUI params2 ( x=Screen resolution scale, y=View Frustum Epsilon )
    float4      g_f4ViewFrustumPlanes[4];   // View frustum planes ( x=left, y=right, z=top, w=bottom )
}

// Some global lighting constants
static float4 g_f4MaterialDiffuseColor  = float4( 1.0f, 1.0f, 1.0f, 1.0f );
static float4 g_f4LightDiffuse          = float4( 1.0f, 1.0f, 1.0f, 1.0f );
static float4 g_f4MaterialAmbientColor  = float4( 0.2f, 0.2f, 0.2f, 1.0f );

// Some global epsilons for adaptive tessellation
static float g_fMaxScreenWidth = 2560.0f;
static float g_fMaxScreenHeight = 1600.0f;


//--------------------------------------------------------------------------------------
// Buffers, Textures and Samplers
//--------------------------------------------------------------------------------------

// Textures
Texture2D g_txDiffuse : register( t0 );

// Samplers
SamplerState g_SamplePoint  : register( s0 );
SamplerState g_SampleLinear : register( s1 );


//--------------------------------------------------------------------------------------
// Shader structures
//--------------------------------------------------------------------------------------

struct VS_RenderSceneInput
{
    float3 f3Position   : POSITION;  
    float3 f3Normal     : NORMAL;     
    float2 f2TexCoord   : TEXCOORD;
};

struct HS_Input
{
    float3 f3Position   : POSITION;
    float3 f3Normal     : NORMAL;
    float2 f2TexCoord   : TEXCOORD;
};

struct HS_ConstantOutput
{
    // Tess factor for the FF HW block
	//FF HW 블록의 테스 팩터.
    float fTessFactor[3]    : SV_TessFactor;
    float fInsideTessFactor : SV_InsideTessFactor;
    
    // Geometry cubic generated control points
	// 형상 입방체는 제어점을 생성합니다.
    float3 f3B210    : POSITION3;
    float3 f3B120    : POSITION4;
    float3 f3B021    : POSITION5;
    float3 f3B012    : POSITION6;
    float3 f3B102    : POSITION7;
    float3 f3B201    : POSITION8;
    float3 f3B111    : CENTER;
    
    // Normal quadratic generated control points
	//정상적인 2 차 생성 된 제어점.
    float3 f3N110    : NORMAL3;      
    float3 f3N011    : NORMAL4;
    float3 f3N101    : NORMAL5;
};

struct HS_ControlPointOutput
{
    float3 f3Position    : POSITION;
    float3 f3Normal      : NORMAL;
    float2 f2TexCoord    : TEXCOORD;
};

struct DS_Output
{
    float4 f4Position   : SV_Position;
    float2 f2TexCoord   : TEXCOORD0;
    float4 f4Diffuse    : COLOR0;
};

struct PS_RenderSceneInput
{
    float4 f4Position   : SV_Position;
    float2 f2TexCoord   : TEXCOORD0;
    float4 f4Diffuse    : COLOR0;
};

struct PS_RenderOutput
{
    float4 f4Color      : SV_Target0;
};


//--------------------------------------------------------------------------------------
// This vertex shader computes standard transform and lighting, with no tessellation stages following
//이 버텍스 쉐이더는 테셀레이션 단계가없는 표준 변환 및 조명을 계산합니다.
//--------------------------------------------------------------------------------------
PS_RenderSceneInput VS_RenderScene( VS_RenderSceneInput I )
{
    PS_RenderSceneInput O;
    float3 f3NormalWorldSpace;
    
    // Transform the position from object space to homogeneous projection space
	//객체 공간에서 균일 한 투영 공간으로 위치를 변환합니다. 
    O.f4Position = mul( float4( I.f3Position, 1.0f ), g_f4x4WorldViewProjection );
    
    // Transform the normal from object space to world space  
	//오브젝트 공간에서 월드 공간으로 법선을 변환하십시오.
    f3NormalWorldSpace = normalize( mul( I.f3Normal, (float3x3)g_f4x4World ) );
    
    // Calc diffuse color.
	// Calc 확산 색상.
    O.f4Diffuse.rgb = g_f4MaterialDiffuseColor * g_f4LightDiffuse * max( 0, dot( f3NormalWorldSpace, g_f4LightDir.xyz ) ) + g_f4MaterialAmbientColor;  
    O.f4Diffuse.a = 1.0f;
    
    // Pass through texture coords
	//텍스처 좌표를 전달하십시오.
    O.f2TexCoord = I.f2TexCoord; 
    
    return O;    
}


//--------------------------------------------------------------------------------------
// This vertex shader is a pass through stage, with HS, tessellation, and DS stages following
//이 버텍스 쉐이더는 통과 단계이며, HS, 테셀레이션 및 DS 단계가 뒤 따른다.
//--------------------------------------------------------------------------------------
HS_Input VS_RenderSceneWithTessellation( VS_RenderSceneInput I )
{
    HS_Input O;
    
    // Pass through world space position
	// 월드공간 위치 패스
    O.f3Position = mul( I.f3Position, (float3x3)g_f4x4World );
    
    // Pass through normalized world space normal    
	// 정규화 된 월드 공간 정상 패스
    O.f3Normal = normalize( mul( I.f3Normal, (float3x3)g_f4x4World ) );
        
    // Pass through texture coordinates
	// 텍스처 좌표를 패스
    O.f2TexCoord = I.f2TexCoord;
    
    return O;    
}


//--------------------------------------------------------------------------------------
// This hull shader passes the tessellation factors through to the HW tessellator, and the 10 (geometry), 6 (normal) control points of the PN-triangular patch to the domain shader.
//이 덮개 셰이더는 테셀레이션 요소를 HW 테셀레이터로 전달하고 도메인 셰이더에 대한 PN 삼각형 패치의 10 개 (기하 구조), 6 개 (정상) 제어점을 전달합니다.
//--------------------------------------------------------------------------------------
HS_ConstantOutput HS_PNTrianglesConstant( InputPatch<HS_Input, 3> I )
{
    HS_ConstantOutput O = (HS_ConstantOutput)0;
    bool bViewFrustumCull = false;
    bool bBackFaceCull = false;
    float fEdgeDot[3];
    
    #ifdef USE_VIEW_FRUSTUM_CULLING

        // Perform view frustum culling test
		//뷰 프러스 텀 컬링 테스트 수행
        bViewFrustumCull = ViewFrustumCull( I[0].f3Position, I[1].f3Position, I[2].f3Position, g_f4ViewFrustumPlanes, g_f4GUIParams2.y );
                    
    #endif

    #ifdef USE_BACK_FACE_CULLING

        // Perform back face culling test
        //후면 컬링 테스트 수행
        // Aquire patch edge dot product between patch edge normal and view vector 
		// "patch edge"노말과 view vector 사이의 "patch edge"내적을 얻습니다.
        fEdgeDot[0] = GetEdgeDotProduct( I[2].f3Normal, I[0].f3Normal, g_f4ViewVector.xyz );
        fEdgeDot[1] = GetEdgeDotProduct( I[0].f3Normal, I[1].f3Normal, g_f4ViewVector.xyz );
        fEdgeDot[2] = GetEdgeDotProduct( I[1].f3Normal, I[2].f3Normal, g_f4ViewVector.xyz );

        // If all 3 fail the test then back face cull
		// 만약 3번의 테스트를 모두 실패하면, 후면컬링함.
        bBackFaceCull = BackFaceCull( fEdgeDot[0], fEdgeDot[1], fEdgeDot[2], g_f4GUIParams1.x );

    #endif

    // Skip the rest of the function if culling
	// 컬링하는 경우 나머지 기능은 건너 뜁니다.
    if( !bViewFrustumCull && !bBackFaceCull )
    {
        // Use the tessellation factors as defined in constant space 
		//일정한 공간에 정의 된 테셀레이션 계수 사용
        O.fTessFactor[0] = O.fTessFactor[1] = O.fTessFactor[2] = g_f4TessFactors.x;
        float fAdaptiveScaleFactor;
                
        #if defined( USE_SCREEN_SPACE_ADAPTIVE_TESSELLATION )

            // Get the screen space position of each control point, so we can compute the 
            // desired tess factor based upon an ideal primitive size
		    //각 제어점의 화면 공간 위치를 가져와 알맞는 원본 크기를 기반으로 원하는 테스 팩터를 계산할 수 있습니다.
            float2 f2EdgeScreenPosition0 = GetScreenSpacePosition( I[0].f3Position, g_f4x4ViewProjection,  g_f4ScreenParams.x,  g_f4ScreenParams.y );
            float2 f2EdgeScreenPosition1 = GetScreenSpacePosition( I[1].f3Position, g_f4x4ViewProjection,  g_f4ScreenParams.x,  g_f4ScreenParams.y );
            float2 f2EdgeScreenPosition2 = GetScreenSpacePosition( I[2].f3Position, g_f4x4ViewProjection,  g_f4ScreenParams.x,  g_f4ScreenParams.y );
            // Edge 0
            fAdaptiveScaleFactor = GetScreenSpaceAdaptiveScaleFactor( f2EdgeScreenPosition2, f2EdgeScreenPosition0, g_f4TessFactors.x, g_f4GUIParams1.w );
            O.fTessFactor[0] = lerp( 1.0f, O.fTessFactor[0], fAdaptiveScaleFactor ); 
            // Edge 1
            fAdaptiveScaleFactor = GetScreenSpaceAdaptiveScaleFactor( f2EdgeScreenPosition0, f2EdgeScreenPosition1, g_f4TessFactors.x, g_f4GUIParams1.w );
            O.fTessFactor[1] = lerp( 1.0f, O.fTessFactor[1], fAdaptiveScaleFactor ); 
            // Edge 2
            fAdaptiveScaleFactor = GetScreenSpaceAdaptiveScaleFactor( f2EdgeScreenPosition1, f2EdgeScreenPosition2, g_f4TessFactors.x, g_f4GUIParams1.w );
            O.fTessFactor[2] = lerp( 1.0f, O.fTessFactor[2], fAdaptiveScaleFactor ); 

        #else
        
            #if defined( USE_DISTANCE_ADAPTIVE_TESSELLATION )
        
                // Perform distance adaptive tessellation per edge
				// 테셀레이션 당 엣지의 거리를 조정을 수행합니다.
                // Edge 0
                fAdaptiveScaleFactor = GetDistanceAdaptiveScaleFactor(    g_f4Eye.xyz, I[2].f3Position, I[0].f3Position, g_f4TessFactors.z, g_f4TessFactors.w * g_f4GUIParams1.z );
                O.fTessFactor[0] = lerp( 1.0f, O.fTessFactor[0], fAdaptiveScaleFactor ); 
                // Edge 1
                fAdaptiveScaleFactor = GetDistanceAdaptiveScaleFactor(    g_f4Eye.xyz, I[0].f3Position, I[1].f3Position, g_f4TessFactors.z, g_f4TessFactors.w * g_f4GUIParams1.z );
                O.fTessFactor[1] = lerp( 1.0f, O.fTessFactor[1], fAdaptiveScaleFactor ); 
                // Edge 2
                fAdaptiveScaleFactor = GetDistanceAdaptiveScaleFactor(    g_f4Eye.xyz, I[1].f3Position, I[2].f3Position, g_f4TessFactors.z, g_f4TessFactors.w * g_f4GUIParams1.z );
                O.fTessFactor[2] = lerp( 1.0f, O.fTessFactor[2], fAdaptiveScaleFactor ); 
            
            #endif

            #if defined( USE_SCREEN_RESOLUTION_ADAPTIVE_TESSELLATION )

                // Use screen resolution as a global scaling factor
				// 화면 해상도를 전역 배율 인수로 사용하십시오.
                // Edge 0
                fAdaptiveScaleFactor = GetScreenResolutionAdaptiveScaleFactor( g_f4ScreenParams.x, g_f4ScreenParams.y, g_fMaxScreenWidth * g_f4GUIParams2.x, g_fMaxScreenHeight * g_f4GUIParams2.x );
                O.fTessFactor[0] = lerp( 1.0f, O.fTessFactor[0], fAdaptiveScaleFactor ); 
                // Edge 1
                fAdaptiveScaleFactor = GetScreenResolutionAdaptiveScaleFactor( g_f4ScreenParams.x, g_f4ScreenParams.y, g_fMaxScreenWidth * g_f4GUIParams2.x, g_fMaxScreenHeight * g_f4GUIParams2.x );
                O.fTessFactor[1] = lerp( 1.0f, O.fTessFactor[1], fAdaptiveScaleFactor ); 
                // Edge 2
                fAdaptiveScaleFactor = GetScreenResolutionAdaptiveScaleFactor( g_f4ScreenParams.x, g_f4ScreenParams.y, g_fMaxScreenWidth * g_f4GUIParams2.x, g_fMaxScreenHeight * g_f4GUIParams2.x );
                O.fTessFactor[2] = lerp( 1.0f, O.fTessFactor[2], fAdaptiveScaleFactor ); 

            #endif

        #endif

        #ifdef USE_ORIENTATION_ADAPTIVE_TESSELLATION

            #ifndef USE_BACK_FACE_CULLING

                // If back face culling is not used, then aquire patch edge dot product
                // between patch edge normal and view vector 
				// 후면 컬링 (back culling)이 사용되지 않는다면,
				//"patch edge"노말과 view vector 사이의 "patch edge"내적을 얻습니다.
                fEdgeDot[0] = GetEdgeDotProduct( I[2].f3Normal, I[0].f3Normal, g_f4ViewVector.xyz );
                fEdgeDot[1] = GetEdgeDotProduct( I[0].f3Normal, I[1].f3Normal, g_f4ViewVector.xyz );
                fEdgeDot[2] = GetEdgeDotProduct( I[1].f3Normal, I[2].f3Normal, g_f4ViewVector.xyz );    

            #endif

            // Scale the tessellation factors based on patch orientation with respect to the viewing vector
			// 보기 벡터에 대한 패치 방향을 기반으로 테셀레이션 계수를 조정합니다.
            // Edge 0
            fAdaptiveScaleFactor = GetOrientationAdaptiveScaleFactor( fEdgeDot[0], g_f4GUIParams1.y );
            float fTessFactor0 = lerp( 1.0f, g_f4TessFactors.x, fAdaptiveScaleFactor ); 
            // Edge 1
            fAdaptiveScaleFactor = GetOrientationAdaptiveScaleFactor( fEdgeDot[1], g_f4GUIParams1.y );
            float fTessFactor1 = lerp( 1.0f, g_f4TessFactors.x, fAdaptiveScaleFactor ); 
            // Edge 2
            fAdaptiveScaleFactor = GetOrientationAdaptiveScaleFactor( fEdgeDot[2], g_f4GUIParams1.y );
            float fTessFactor2 = lerp( 1.0f, g_f4TessFactors.x, fAdaptiveScaleFactor ); 

            #if defined( USE_SCREEN_SPACE_ADAPTIVE_TESSELLATION ) || defined( USE_DISTANCE_ADAPTIVE_TESSELLATION )

                O.fTessFactor[0] = ( O.fTessFactor[0] + fTessFactor0 ) / 2.0f;    
                O.fTessFactor[1] = ( O.fTessFactor[1] + fTessFactor1 ) / 2.0f;    
                O.fTessFactor[2] = ( O.fTessFactor[2] + fTessFactor2 ) / 2.0f;    

            #else
            
                O.fTessFactor[0] = fTessFactor0;    
                O.fTessFactor[1] = fTessFactor1;    
                O.fTessFactor[2] = fTessFactor2;    

            #endif
                                            
        #endif
        
        // Now setup the PNTriangle control points...
		// 이제 PNTriangle 제어점을 설정하십시오 ...

        // Assign Positions
		// 위치 지정
        float3 f3B003 = I[0].f3Position;
        float3 f3B030 = I[1].f3Position;
        float3 f3B300 = I[2].f3Position;
        // And Normals
		// 그리고 노말라이즈
        float3 f3N002 = I[0].f3Normal;
        float3 f3N020 = I[1].f3Normal;
        float3 f3N200 = I[2].f3Normal;
            
        // Compute the cubic geometry control points
		// "cubic geometry"제어점 계산.
        // Edge control points
		// 엣지 위치 조정?
        O.f3B210 = ( ( 2.0f * f3B003 ) + f3B030 - ( dot( ( f3B030 - f3B003 ), f3N002 ) * f3N002 ) ) / 3.0f;
        O.f3B120 = ( ( 2.0f * f3B030 ) + f3B003 - ( dot( ( f3B003 - f3B030 ), f3N020 ) * f3N020 ) ) / 3.0f;
        O.f3B021 = ( ( 2.0f * f3B030 ) + f3B300 - ( dot( ( f3B300 - f3B030 ), f3N020 ) * f3N020 ) ) / 3.0f;
        O.f3B012 = ( ( 2.0f * f3B300 ) + f3B030 - ( dot( ( f3B030 - f3B300 ), f3N200 ) * f3N200 ) ) / 3.0f;
        O.f3B102 = ( ( 2.0f * f3B300 ) + f3B003 - ( dot( ( f3B003 - f3B300 ), f3N200 ) * f3N200 ) ) / 3.0f;
        O.f3B201 = ( ( 2.0f * f3B003 ) + f3B300 - ( dot( ( f3B300 - f3B003 ), f3N002 ) * f3N002 ) ) / 3.0f;
        // Center control point
		// 중앙 위치 제어?
        float3 f3E = ( O.f3B210 + O.f3B120 + O.f3B021 + O.f3B012 + O.f3B102 + O.f3B201 ) / 6.0f;
        float3 f3V = ( f3B003 + f3B030 + f3B300 ) / 3.0f;
        O.f3B111 = f3E + ( ( f3E - f3V ) / 2.0f );
        
        // Compute the quadratic normal control points, and rotate into world space
		// "quadratic normal"제어점을 계산하고 월드 공간으로 회전합니다.
        float fV12 = 2.0f * dot( f3B030 - f3B003, f3N002 + f3N020 ) / dot( f3B030 - f3B003, f3B030 - f3B003 );
        O.f3N110 = normalize( f3N002 + f3N020 - fV12 * ( f3B030 - f3B003 ) );
        float fV23 = 2.0f * dot( f3B300 - f3B030, f3N020 + f3N200 ) / dot( f3B300 - f3B030, f3B300 - f3B030 );
        O.f3N011 = normalize( f3N020 + f3N200 - fV23 * ( f3B300 - f3B030 ) );
        float fV31 = 2.0f * dot( f3B003 - f3B300, f3N200 + f3N002 ) / dot( f3B003 - f3B300, f3B003 - f3B300 );
        O.f3N101 = normalize( f3N200 + f3N002 - fV31 * ( f3B003 - f3B300 ) );
    }
    else
    {
        // Cull the patch
		// 패치를 제거하십시오.
        O.fTessFactor[0] = 0.0f;
        O.fTessFactor[1] = 0.0f;
        O.fTessFactor[2] = 0.0f;
    }

    // Inside tess factor is just the average of the edge factors
	// 테스인자 내부는 단지 엣지인자들의 평균이다.
    O.fInsideTessFactor = ( O.fTessFactor[0] + O.fTessFactor[1] + O.fTessFactor[2] ) / 3.0f;
               
    return O;
}

[domain("tri")]
[partitioning("fractional_odd")]
[outputtopology("triangle_cw")]
[patchconstantfunc("HS_PNTrianglesConstant")]
[outputcontrolpoints(3)]
[maxtessfactor(9)]
HS_ControlPointOutput HS_PNTriangles( InputPatch<HS_Input, 3> I, uint uCPID : SV_OutputControlPointID )
{
    HS_ControlPointOutput O = (HS_ControlPointOutput)0;

    // Just pass through inputs = fast pass through mode triggered
	// 그냥 입력을 통과 = 트리거모드사이로 빠른 패스
    O.f3Position = I[uCPID].f3Position;
    O.f3Normal = I[uCPID].f3Normal;
    O.f2TexCoord = I[uCPID].f2TexCoord;
    
    return O;
}


//--------------------------------------------------------------------------------------
// This domain shader applies contol point weighting to the barycentric coords produced by the FF tessellator 
//이 도메인 셰이더는 FF 테셀레이터에 의해 생성 된 중심점에 contol point 가중치를 적용합니다.
//--------------------------------------------------------------------------------------
[domain("tri")]
DS_Output DS_PNTriangles( HS_ConstantOutput HSConstantData, const OutputPatch<HS_ControlPointOutput, 3> I, float3 f3BarycentricCoords : SV_DomainLocation )
{
    DS_Output O = (DS_Output)0;

    // The barycentric coordinates
	// 무게중심좌표
    float fU = f3BarycentricCoords.x;
    float fV = f3BarycentricCoords.y;
    float fW = f3BarycentricCoords.z;

    // Precompute squares and squares * 3 
	//사각형 및 사각형 사전 계산 * 3
    float fUU = fU * fU;
    float fVV = fV * fV;
    float fWW = fW * fW;
    float fUU3 = fUU * 3.0f;
    float fVV3 = fVV * 3.0f;
    float fWW3 = fWW * 3.0f;
    
    // Compute position from cubic control points and barycentric coords
	// 큐빅 제어점 및 중심점 좌표에서 위치를 계산합니다.
    float3 f3Position = I[0].f3Position * fWW * fW +
                        I[1].f3Position * fUU * fU +
                        I[2].f3Position * fVV * fV +
                        HSConstantData.f3B210 * fWW3 * fU +
                        HSConstantData.f3B120 * fW * fUU3 +
                        HSConstantData.f3B201 * fWW3 * fV +
                        HSConstantData.f3B021 * fUU3 * fV +
                        HSConstantData.f3B102 * fW * fVV3 +
                        HSConstantData.f3B012 * fU * fVV3 +
                        HSConstantData.f3B111 * 6.0f * fW * fU * fV;
    
    // Compute normal from quadratic control points and barycentric coords
	// 큐빅 제어점 및 중심점 좌표에서 위치를 계산합니다.
    float3 f3Normal =   I[0].f3Normal * fWW +
                        I[1].f3Normal * fUU +
                        I[2].f3Normal * fVV +
                        HSConstantData.f3N110 * fW * fU +
                        HSConstantData.f3N011 * fU * fV +
                        HSConstantData.f3N101 * fW * fV;

    // Normalize the interpolated normal    
	// 보간 된 법선을 표준화합니다.
    f3Normal = normalize( f3Normal );

    // Linearly interpolate the texture coords
	// 텍스처 좌표를 선형으로 보간합니다.
    O.f2TexCoord = I[0].f2TexCoord * fW + I[1].f2TexCoord * fU + I[2].f2TexCoord * fV;

    // Calc diffuse color    
    O.f4Diffuse.rgb = g_f4MaterialDiffuseColor * g_f4LightDiffuse * max( 0, dot( f3Normal, g_f4LightDir.xyz ) ) + g_f4MaterialAmbientColor;  
    O.f4Diffuse.a = 1.0f; 

    // Transform model position with view-projection matrix
	// 뷰 프로젝션 행렬을 사용하여 모델 위치를 변환합니다.
    O.f4Position = mul( float4( f3Position.xyz, 1.0 ), g_f4x4ViewProjection );
        
    return O;
}


//--------------------------------------------------------------------------------------
// This shader outputs the pixel's color by passing through the lit diffuse material color & modulating with the diffuse texture
//이 쉐이더는 "확산 된 텍스처 색상"및 "확산 텍스처"로 변조하여 "픽셀의 색상"을 출력합니다.
//--------------------------------------------------------------------------------------
PS_RenderOutput PS_RenderSceneTextured( PS_RenderSceneInput I )
{
    PS_RenderOutput O;
    
    O.f4Color = g_txDiffuse.Sample( g_SampleLinear, I.f2TexCoord ) * I.f4Diffuse;
    //O.f4Color = g_txDiffuse.Sample(g_SamplePoint, I.f2TexCoord) * I.f4Diffuse;
    
    return O;
}


//--------------------------------------------------------------------------------------
// This shader outputs the pixel's color by passing through the lit diffuse material color
// 이 셰이더는 "확산 된 머테리얼 색상"를 통과하여 "픽셀의 색"을 출력합니다.
//--------------------------------------------------------------------------------------
PS_RenderOutput PS_RenderScene( PS_RenderSceneInput I )
{
    PS_RenderOutput O;
    
    O.f4Color = I.f4Diffuse;
    
    return O;
}


//--------------------------------------------------------------------------------------
// EOF
//--------------------------------------------------------------------------------------
