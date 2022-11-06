using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Mariuc
{
    class ImmediateMode : GameWindow
    {

        private const int XYZ_SIZE = 75;
        private int[,] vertsFromFile = new int[3,3];
        private Random rnd = new Random();
        private Color randomColorV1 = new Color();
        private Color randomColorV2 = new Color();
        private Color randomColorV3 = new Color();

        public ImmediateMode() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;

            Console.WriteLine("OpenGl versiunea: " + GL.GetString(StringName.Version));
            Title = "OpenGl versiunea: " + GL.GetString(StringName.Version) + " (mod imediat)";
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string text = System.IO.File.ReadAllText(@"pos.txt");
            int i = 0;
            foreach (string vert in text.Split(' '))
            {
                int j = 0;
                foreach(string coord in vert.Split(','))
                {
                    Int32.TryParse(coord, out vertsFromFile[i, j]);
                    j++;
                }
                i++;
            }

            GL.ClearColor(Color.Blue);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            double aspect_ratio = Width / (double)Height;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            Matrix4 lookat = Matrix4.LookAt(30, 30, 30, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyboard[Key.Escape])
            {
                Exit();
            }
            if (keyboard[Key.Z])
            {
                randomColorV1 = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                Console.WriteLine("Color for vertex[ 1 ]: " + randomColorV1);
            }
            if (keyboard[Key.X])
            {
                randomColorV2 = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                Console.WriteLine("Color for vertex[ 2 ]: " + randomColorV2);
            }
            if (keyboard[Key.C])
            {
                randomColorV3 = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                Console.WriteLine("Color for vertex[ 3 ]: " + randomColorV3);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            DrawAxes();
            DrawObjects();

            SwapBuffers();
        }

        private void DrawAxes()
        {
            // Desenează axa Ox (cu roșu).
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(XYZ_SIZE, 0, 0);
            GL.End();

            // Desenează axa Oy (cu galben).
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Yellow);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, XYZ_SIZE, 0); ;
            GL.End();

            // Desenează axa Oz (cu verde).
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, XYZ_SIZE);
            GL.End();
        }

        private void DrawObjects()
        {
            //Deseneaza triunghi
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(randomColorV1);
            GL.Vertex3(vertsFromFile[0,0], vertsFromFile[0, 1], vertsFromFile[0, 2]);
            GL.Color3(randomColorV2);
            GL.Vertex3(vertsFromFile[1, 0], vertsFromFile[1, 1], vertsFromFile[1, 2]);
            GL.Color3(randomColorV3);
            GL.Vertex3(vertsFromFile[2, 0], vertsFromFile[2, 1], vertsFromFile[2, 2]);
            GL.End();
        }


        [STAThread]
        static void Main(string[] args)
        {
            using (ImmediateMode example = new ImmediateMode())
            {
                example.Run(30.0, 0.0);
            }
        }
    }

}
