using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace terrain.gen
{
    public class Game : GameWindow
    {
        List<double> heightMaps;
        List<Vector3> vertices;

        int length;

        public Game(List<double> heightMaps, int width, int height)
            : base(width, height, GraphicsMode.Default, "noise test") {

            GL.Translate(0, -20, 0);

            this.heightMaps = heightMaps;
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
            RenderFrame += render;
            Resize += resize;
            Load += load;

            Run(60);
        }

        void render(object sender, EventArgs e)
        {
            GL.Enable(EnableCap.Fog);
            // Fog
            float[] colors = { 230, 230, 230 };
            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            GL.Hint(HintTarget.FogHint, HintMode.Nicest);
            GL.Fog(FogParameter.FogColor, colors);

            GL.Fog(FogParameter.FogStart, (float)1000 / 100.0f);
            GL.Fog(FogParameter.FogEnd, 550.0f);

            GL.Rotate(.5, 0, 1, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int length = (int)Math.Sqrt(heightMaps.Count);
            int count = 0;

            for (int v = 0; v < length*length; v++)
            {
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3((double)114 / 255, (double)179 / 255, (double)29 / 255);
                GL.Vertex3(vertices[v]);

                try {
                    if ((int)vertices[v].X == (int)vertices[v+1].X)
                        GL.Vertex3(vertices[v+1]);

                    int x1 = (int)vertices[v+(int)Math.Sqrt(vertices.Count)].X,
                        x2 = (int)vertices[v+(int)Math.Sqrt(vertices.Count)+1].X;

                    if (x1 == x2) {
                        GL.Vertex3(vertices[v+(int)Math.Sqrt(vertices.Count)+1]);
                        GL.Vertex3(vertices[v+(int)Math.Sqrt(vertices.Count)]);
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
