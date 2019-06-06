﻿using System;
using CUtility.Math;
using CUtility.Extension;

namespace CRender.Structure
{
    public struct Model : IAppliable<Model>
    {
        public readonly Vector4[] Vertices;

        public readonly IPrimitive[] Primitives;

        public readonly Vector2[] UVs;

        public readonly Vector3[] Normals;

        public Model(Vector4[] vertices, IPrimitive[] primitives, Vector2[] uvs, Vector3[] normals)//, Cuboid bound)
        {
            Vertices = vertices;
            Primitives = primitives;
            UVs = uvs;
            Normals = normals;
        }

        public Model GetInstanceToApply()
        {
            return new Model(Vertices.GetCopy(), Primitives, UVs?.GetCopy(), Normals?.GetCopy());//, Bound);
        }

        /// <summary>
        /// A cube without uv mapping
        /// </summary>
        public static Model Cube(float size = 1f, bool isWireframe = true, bool hasNormals = false)
        {
            size *= .5f;
            return new Model(
                vertices: new Vector4[] {
                    new Vector4(-size, -size, -size, 1f),
                    new Vector4(size, -size, -size, 1f),
                    new Vector4(size, size, -size, 1f),
                    new Vector4(-size, size, -size, 1f),
                    new Vector4(-size, -size, size, 1f),
                    new Vector4(size, -size, size, 1f),
                    new Vector4(size, size, size, 1f),
                    new Vector4(-size, size, size, 1f), },

                primitives: isWireframe ?
                new IPrimitive[] {
                    new LinePrimitive(0, 1),
                    new LinePrimitive(1, 2),
                    new LinePrimitive(2, 3),
                    new LinePrimitive(3, 0),
                    new LinePrimitive(0, 4),
                    new LinePrimitive(1, 5),
                    new LinePrimitive(2, 6),
                    new LinePrimitive(3, 7),
                    new LinePrimitive(4, 5),
                    new LinePrimitive(5, 6),
                    new LinePrimitive(6, 7),
                    new LinePrimitive(4, 7), }
                : new IPrimitive[] {
                    new TrianglePrimitive(0, 1, 2),
                    new TrianglePrimitive(2, 3, 0),
                    new TrianglePrimitive(0, 1, 5),
                    new TrianglePrimitive(1, 2, 6),
                    new TrianglePrimitive(2, 3, 7),
                    new TrianglePrimitive(3, 0, 4),
                    new TrianglePrimitive(4, 5, 6),
                    new TrianglePrimitive(6, 7, 4),
                    new TrianglePrimitive(4, 5, 1),
                    new TrianglePrimitive(5, 6, 2),
                    new TrianglePrimitive(6, 7, 3),
                    new TrianglePrimitive(7, 4, 0), },

                uvs: null,

                normals: hasNormals ?
                new Vector3[] {
                    new Vector3(-JMath.Sqrt3),
                    new Vector3(JMath.Sqrt3, -JMath.Sqrt3, -JMath.Sqrt3),
                    new Vector3(JMath.Sqrt3, JMath.Sqrt3, -JMath.Sqrt3),
                    new Vector3(-JMath.Sqrt3, JMath.Sqrt3, -JMath.Sqrt3),
                    new Vector3(-JMath.Sqrt3, -JMath.Sqrt3, JMath.Sqrt3),
                    new Vector3(JMath.Sqrt3, -JMath.Sqrt3, JMath.Sqrt3),
                    new Vector3(JMath.Sqrt3),
                    new Vector3(-JMath.Sqrt3, JMath.Sqrt3, JMath.Sqrt3), }
                : null
                );
        }
    }
}