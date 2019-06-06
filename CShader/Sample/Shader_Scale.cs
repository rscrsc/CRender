﻿using CUtility;

using static CShader.ShaderValue;

using AppData = CShader.ShaderInOutDefault.AppData_Base;
using VOutData = CShader.ShaderInOutDefault.VOutData_Base;

namespace CShader.Sample
{
    /// <summary>
    /// Only supports VertexStage
    /// </summary>
    public unsafe class Shader_Scale : JSingleton<Shader_Scale>, IVertexShader
    {
        static Shader_Scale()
        {
            ShaderInterpreter<IVertexShader>.Interpret<Shader_Scale>();
        }

        public void Main(
            [ShaderInput(typeof(AppData))] void* inputPtr,
            [ShaderOutput(typeof(VOutData))] void* outputPtr, IShaderStage<IVertexShader> _)
        {
            AppData* appPtr = (AppData*)inputPtr;
            VOutData* vOutPtr = (VOutData*)outputPtr;

            vOutPtr->Vertex = ObjectToView * appPtr->Vertex * (SinTime * .3f + 1);
        }
    }
}