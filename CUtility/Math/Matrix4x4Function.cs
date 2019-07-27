﻿using System;
using static CUtility.Extension.MarshalExtension;

namespace CUtility.Math
{
    public unsafe partial struct Matrix4x4
    {
        public readonly static Matrix4x4* ARITHMETIC_TEMP;

        private readonly static Matrix4x4* MATRIX_TEMP;

        private readonly static Vector4* VECTOR4_TEMP;

        static Matrix4x4()
        {
            MATRIX_TEMP = Alloc<Matrix4x4>(2);
            VECTOR4_TEMP = Alloc<Vector4>();
            ARITHMETIC_TEMP = MATRIX_TEMP + 1;
            FreeWhenExit(MATRIX_TEMP);
            FreeWhenExit(VECTOR4_TEMP);
        }

        public static Matrix4x4* Mul(Matrix4x4* matrix, Matrix4x4* transform)
        {
            *MATRIX_TEMP = *matrix;
            Mul(MATRIX_TEMP, transform, matrix);
            return matrix;
        }

        /// <summary>
        /// <paramref name="result"/> must differ from <paramref name="matrix"/>, otherwise invoke <see cref="Mul(Matrix4x4*, Matrix4x4*)"/> instead
        /// </summary>
        public static void Mul(Matrix4x4* matrix, Matrix4x4* transform, Matrix4x4* result)
        {
            result->M11 = matrix->M11 * transform->M11 + matrix->M21 * transform->M12 + matrix->M31 * transform->M13 + matrix->M41 * transform->M14;
            result->M12 = matrix->M12 * transform->M11 + matrix->M22 * transform->M12 + matrix->M32 * transform->M13 + matrix->M42 * transform->M14;
            result->M13 = matrix->M13 * transform->M11 + matrix->M23 * transform->M12 + matrix->M33 * transform->M13 + matrix->M43 * transform->M14;
            result->M14 = matrix->M14 * transform->M11 + matrix->M24 * transform->M12 + matrix->M34 * transform->M13 + matrix->M44 * transform->M14;

            result->M21 = matrix->M11 * transform->M21 + matrix->M21 * transform->M22 + matrix->M31 * transform->M23 + matrix->M41 * transform->M24;
            result->M22 = matrix->M12 * transform->M21 + matrix->M22 * transform->M22 + matrix->M32 * transform->M23 + matrix->M42 * transform->M24;
            result->M23 = matrix->M13 * transform->M21 + matrix->M23 * transform->M22 + matrix->M33 * transform->M23 + matrix->M43 * transform->M24;
            result->M24 = matrix->M14 * transform->M21 + matrix->M24 * transform->M22 + matrix->M34 * transform->M23 + matrix->M44 * transform->M24;

            result->M31 = matrix->M11 * transform->M31 + matrix->M21 * transform->M32 + matrix->M31 * transform->M33 + matrix->M41 * transform->M34;
            result->M32 = matrix->M12 * transform->M31 + matrix->M22 * transform->M32 + matrix->M32 * transform->M33 + matrix->M42 * transform->M34;
            result->M33 = matrix->M13 * transform->M31 + matrix->M23 * transform->M32 + matrix->M33 * transform->M33 + matrix->M43 * transform->M34;
            result->M34 = matrix->M14 * transform->M31 + matrix->M24 * transform->M32 + matrix->M34 * transform->M33 + matrix->M44 * transform->M34;

            result->M41 = matrix->M11 * transform->M41 + matrix->M21 * transform->M42 + matrix->M31 * transform->M43 + matrix->M41 * transform->M44;
            result->M42 = matrix->M12 * transform->M41 + matrix->M22 * transform->M42 + matrix->M32 * transform->M43 + matrix->M42 * transform->M44;
            result->M43 = matrix->M13 * transform->M41 + matrix->M23 * transform->M42 + matrix->M33 * transform->M43 + matrix->M43 * transform->M44;
            result->M44 = matrix->M14 * transform->M41 + matrix->M24 * transform->M42 + matrix->M34 * transform->M43 + matrix->M44 * transform->M44;
        }

        public static Vector4* Mul(Matrix4x4* transform, Vector4* vector)
        {
            *VECTOR4_TEMP = *vector;
            Mul(transform, VECTOR4_TEMP, vector);
            return vector;
        }

        /// <summary>
        /// <paramref name="result"/> must differ from <paramref name="vector"/>, otherwise invoke <see cref="Mul(Matrix4x4*, Vector4*)"/> instead
        /// </summary>
        public static void Mul(Matrix4x4* transform, Vector4* vector, Vector4* result)
        {
            result->X = vector->X * transform->M11 + vector->Y * transform->M21 + vector->Z * transform->M31 + vector->W * transform->M41;
            result->Y = vector->X * transform->M12 + vector->Y * transform->M22 + vector->Z * transform->M32 + vector->W * transform->M42;
            result->Z = vector->X * transform->M13 + vector->Y * transform->M23 + vector->Z * transform->M33 + vector->W * transform->M43;
            result->W = vector->X * transform->M14 + vector->Y * transform->M24 + vector->Z * transform->M34 + vector->W * transform->M44;
        }

        public static Matrix4x4* Transpose(Matrix4x4* matrix)
        {
            matrix->M21 = matrix->M12; matrix->M31 = matrix->M13; matrix->M41 = matrix->M14;
            matrix->M12 = matrix->M21; matrix->M32 = matrix->M23; matrix->M42 = matrix->M24;
            matrix->M13 = matrix->M31; matrix->M23 = matrix->M32; matrix->M43 = matrix->M34;
            matrix->M14 = matrix->M41; matrix->M24 = matrix->M42; matrix->M34 = matrix->M43;
            return matrix;
        }

        public static Matrix4x4* Translation(Vector3 vector, Matrix4x4* result)
        {
            result->M11 = 1; result->M21 = 0; result->M31 = 0; result->M41 = vector.X;
            result->M12 = 0; result->M22 = 1; result->M32 = 0; result->M42 = vector.Y;
            result->M13 = 0; result->M23 = 0; result->M33 = 1; result->M43 = vector.Z;
            result->M14 = 0; result->M24 = 0; result->M34 = 0; result->M44 = 1;
            return result;
        }

        public static Matrix4x4* RotationEuler(Vector3 rotation, Matrix4x4* result)
        {
            float sinX = MathF.Sin(rotation.X), sinY = MathF.Sin(rotation.Y), sinZ = MathF.Sin(rotation.Z);
            float cosX = MathF.Cos(rotation.X), cosY = MathF.Cos(rotation.Y), cosZ = MathF.Cos(rotation.Z);
            float sinXsinY = sinX * sinY;
            float cosXcosZ = cosX * cosZ;

            //It's really complex!
            result->M11 = cosY * cosZ;
            result->M21 = sinXsinY * cosZ - cosX * sinZ;
            result->M31 = cosXcosZ * sinY + sinX * sinZ;
            result->M12 = cosY * sinZ;
            result->M22 = cosXcosZ + sinXsinY * sinZ;
            result->M32 = -sinX * cosZ;
            result->M13 = -sinY;
            result->M23 = sinX * cosY;
            result->M33 = cosX * cosY;
            result->M14 = 0; result->M24 = 0; result->M34 = 0;
            result->M41 = 0; result->M42 = 0; result->M43 = 0; result->M44 = 1;
            return result;
        }

        public static Matrix4x4* RotationEulerInverse(Vector3 rotation, Matrix4x4* result)
        {
            float sinX = MathF.Sin(rotation.X), sinY = MathF.Sin(rotation.Y), sinZ = MathF.Sin(rotation.Z);
            float cosX = MathF.Cos(rotation.X), cosY = MathF.Cos(rotation.Y), cosZ = MathF.Cos(rotation.Z);
            float sinXsinY = sinX * sinY;
            float cosXcosZ = cosX * cosZ;
            float cosXsinZ = cosX * sinZ;

            //!xelpmoc yllaer s'tI
            result->M11 = cosZ * cosY;
            result->M21 = sinZ * cosY;
            result->M31 = -sinY;
            result->M12 = -cosXsinZ + cosZ * sinXsinY;
            result->M22 = cosXcosZ + sinZ * sinXsinY;
            result->M32 = cosY * sinX;
            result->M13 = cosXcosZ * sinY + sinX * sinZ;
            result->M23 = -cosZ * sinX + cosXsinZ * sinY;
            result->M33 = cosY * cosX;
            result->M14 = 0; result->M24 = 0; result->M34 = 0;
            result->M41 = 0; result->M42 = 0; result->M43 = 0; result->M44 = 1;
            return result;
        }

        public static Matrix4x4* Scale(Vector3 scale, Matrix4x4* result)
        {
            return Scale(scale.X, scale.Y, scale.Z, result);
        }

        public static Matrix4x4* Scale(float x,float y,float z,Matrix4x4* result)
        {
            result->M11 = x; result->M21 = 0; result->M31 = 0; result->M41 = 0;
            result->M12 = 0; result->M22 = y; result->M32 = 0; result->M42 = 0;
            result->M13 = 0; result->M23 = 0; result->M33 = z; result->M43 = 0;
            result->M14 = 0; result->M24 = 0; result->M34 = 0; result->M44 = 1;
            return result;
        }

        public static Matrix4x4* Scale(float scale, Matrix4x4* result)
        {
            result->M11 = scale; result->M21 = 0; result->M31 = 0; result->M41 = 0;
            result->M12 = 0; result->M22 = scale; result->M32 = 0; result->M42 = 0;
            result->M13 = 0; result->M23 = 0; result->M33 = scale; result->M43 = 0;
            result->M14 = 0; result->M24 = 0; result->M34 = 0; result->M44 = 1;
            return result;
        }

        /// <summary>
        /// Positive X oriented, Y is <paramref name="width"/>,Z is <paramref name="height"/>
        /// </summary>
        public static Matrix4x4* Orthographic(float width, float height, float near, float far, Matrix4x4* result)
        {
            float scaleX = 2f / (far - near);
            result->M11 = scaleX; result->M21 = 0; result->M31 = 0; result->M41 = -(near + far) * .5f * scaleX;
            result->M12 = 0; result->M22 = 2f / width; result->M32 = 0; result->M42 = 0;
            result->M13 = 0; result->M23 = 0; result->M33 = 2f / height; result->M43 = 0;
            result->M14 = 0; result->M24 = 0; result->M34 = 0; result->M44 = 1;
            return result;
        }
    }
}
