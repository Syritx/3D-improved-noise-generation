using System;
using OpenTK;
using OpenTK.Input;

namespace terrain.gen
{
    public class Camera
    {
        public Vector3 position = new Vector3(-50,20,0);
        public Vector3 front = new Vector3(0.0f, 0.0f, -0.001f);
        public Vector3 up = new Vector3(0.0f, .01f, 0.0f);

        float rotSensitivity = 1;
        float moveSensitivity = 10;

        float yRotation;
        float xRotation;

        public Camera(GameWindow window) {
            window.KeyDown += onKeyPress;
        }

        public void update(object sender, FrameEventArgs e) {
            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(xRotation)) * (float)Math.Cos(MathHelper.DegreesToRadians(yRotation));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(xRotation));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(xRotation)) * (float)Math.Sin(MathHelper.DegreesToRadians(yRotation));

            front = Vector3.Normalize(front);
        }

        private void onKeyPress(object sender, KeyboardKeyEventArgs e) {
            if (e.Key == Key.W) position += front * moveSensitivity;
            if (e.Key == Key.S) position -= front * moveSensitivity;
            if (e.Key == Key.A) yRotation -= 5 * rotSensitivity;
            if (e.Key == Key.D) yRotation += 5 * rotSensitivity;
        }
    }
}