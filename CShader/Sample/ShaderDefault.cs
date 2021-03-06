﻿using System;
using CShader.Attribute;
using CShader.Interpret;
using CUtility;
using CUtility.Math;

using static CShader.ShaderValue;
using static CUtility.Math.Matrix4x4;

using App = CShader.ShaderInOutDefault.App_Base;
using FIn = CShader.ShaderInOutDefault.FIn_Base;
using FOut = CShader.ShaderInOutDefault.FOut_Base;
using VOut = CShader.ShaderInOutDefault.VOut_Base;

namespace CShader
{
    /// <summary>
    /// Only supports VertexStage
    /// </summary>
    public class ShaderDefault : JSingleton<ShaderDefault>, IVertexShader, IFragmentShader
    {
        static ShaderDefault()
        {
            ShaderInterpreter<IVertexShader, ShaderInOutPatternDefault>.Interpret<ShaderDefault>();
            ShaderInterpreter<IFragmentShader, ShaderInOutPatternDefault>.Interpret<ShaderDefault>();
        }

        public unsafe void Main(
            [ShaderInput(typeof(App))] void* inputPtr,
            [ShaderOutput(typeof(VOut))] void* outputPtr, IShaderStage<IVertexShader> _)
        {
            App* appPtr = (App*)inputPtr;
            VOut* vOutPtr = (VOut*)outputPtr;

            Mul(ObjectToScreen, &appPtr->Vertex, &vOutPtr->Vertex);
        }

        public unsafe void Main(
            [ShaderInput(typeof(FIn))] void* inputPtr,
            [ShaderOutput(typeof(FOut))] void* outputPtr, IShaderStage<IFragmentShader> _)
        {
            FIn* fInPtr = (FIn*)inputPtr;
            FOut* fOutPtr = (FOut*)outputPtr;

            float absSinTime = MathF.Abs(CosTime);
            fOutPtr->Color = new Vector4(absSinTime, absSinTime, absSinTime, 1f);
        }
    }
}
