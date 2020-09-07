using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace terrain.gen
{
    public class Game : GameWindow
    {
        List<Vector3> vertices;

        int length;
        Camera camera;

        public Game(List<double> heightMaps, int width, int height)
            : base(width, height, GraphicsMode.Default, "noise test") {

            camera = new Camera(this as GameWindow);
            vertices = new List<Vector3>();

            length = (int)Math.Sqrt(heightMaps.Count);
            int c = 0;

            for (int x = 0; x < length; x++) {
                for (int z = 0; z < length; z++) {

                    Vector3 vertex = new Vector3((x-length/2) * 10, (float)heightMaps[c], (z-length / 2) * 10);
                    vertices.Add(vertex);
                    c++;
                }
            }

            start();
        }

        void start()
        {
            UpdateFrame += camera.update;
            RenderFrame += render;
            Resize += resize;
            Load += load;

            Run(60);
        }

        void render(object sender, EventArgs e)
        {

            var view = Matrix4.LookAt(camera.position, camera.position + camera.front, camera.up);
            GL.LoadMatrix(ref view);
            GL.MatrixMode(MatrixMode.Modelview);

            //------------------------------------//
            // FOG
            //------------------------------------//

            GL.Enable(EnableCap.Fog);

            float[] colors = { 230, 230, 230 };
            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            GL.Hint(HintTarget.FogHint, HintMode.Nicest);
            GL.Fog(FogParameter.FogColor, colors);

            GL.Fog(FogParameter.FogStart, (float)1000 / 100.0f);
            GL.Fog(FogParameter.FogEnd, 550.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int length = (int)Math.Sqrt(vertices.Count);
            int count = 0;

            //------------------------------------//
            // TERRAIN RENDERING
            //------------------------------------//

            for (int v = 0; v < length*length; v++)
            {
                GL.Begin(PrimitiveType.Quads);
                GL.Color3((double)114 / 255, (double)179 / 255, (double)29 / 255);
                GL.Vertex3(vertices[v]);

                try {
                    if ((int)vertices[v].X == (int)vertices[v+1].X)
                        GL.Vertex3(vertices[v+1]);

                    int x1 = (int)vertices[v+length].X,
                        x2 = (int)vertices[v+length+1].X;

                    if (x1 == x2) {
                        GL.Vertex3(vertices[v+length+1]);
                        GL.Vertex3(vertices[v+length]);
                    }
                }
                catch(Exception exception) {}

                GL.End();
            }

            float waterHeight = -0.1f;

            //------------------------------------//
            // WATER RENDERING
            //------------------------------------//

            for (int v = 0; v < length*length; v++)
            {
                GL.Begin(PrimitiveType.Quads);
                GL.Color3((double)5 / 255, (double)94 / 255, (double)227 / 255);
                GL.Vertex3(new Vector3(vertices[v].X, waterHeight, vertices[v].Z));

                try {
                    if ((int)vertices[v].X == (int)vertices[v+1].X)
                        GL.Vertex3(new Vector3(vertices[v+1].X, waterHeight, vertices[v+1].Z));

                    int x1 = (int)vertices[v+length].X,
                        x2 = (int)vertices[v+length+1].X;

                    if (x1 == x2) {
                        GL.Vertex3(new Vector3(vertices[v+length+1].X, waterHeight, vertices[v+length+1].Z));
                        GL.Vertex3(new Vector3(vertices[v+length].X, waterHeight, vertices[v+length].Z));
                    }
                }
                catch(Exception exception) {}

                GL.End();
            }

            SwapBuffers();
        }

        void resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Matrix4 perspectiveMatrix =
                Matrix4.CreatePerspectiveFieldOfView(1, Width / Height, 1.0f, 2000.0f);

            GL.LoadMatrix(ref perspectiveMatrix);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.End();
        }

        void load(object sender, EventArgs e)
        {
            GL.ClearColor(0, 0, 0, 0);
            GL.Enable(EnableCap.DepthTest);
        }
    }
}
