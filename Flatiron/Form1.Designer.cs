namespace Flatiron
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            RC.D3DControls.Components.Camera camera2 = new RC.D3DControls.Components.Camera();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.d3DSimpleSurface1 = new RC.D3DControls.D3DSimpleSurface();
            this.SuspendLayout();
            // 
            // d3DSimpleSurface1
            // 
            camera2.Active = false;
            camera2.AspectRatio = 1.333333F;
            camera2.FarPlane = 1500F;
            camera2.FieldOfView = 0.7853982F;
            camera2.Height = 480F;
            camera2.NearPlane = 1F;
            camera2.Orthogonal = false;
            camera2.Position = ((Microsoft.DirectX.Vector3)(resources.GetObject("camera2.Position")));
            camera2.Target = ((Microsoft.DirectX.Vector3)(resources.GetObject("camera2.Target")));
            camera2.TargetX = 0F;
            camera2.TargetY = 0F;
            camera2.TargetZ = 0F;
            camera2.UpVector = ((Microsoft.DirectX.Vector3)(resources.GetObject("camera2.UpVector")));
            camera2.UseFieldOfView = true;
            camera2.Width = 640F;
            camera2.X = 0F;
            camera2.Y = 3F;
            camera2.Z = -5F;
            this.d3DSimpleSurface1.ActiveCamera = camera2;
            this.d3DSimpleSurface1.BackBufferFormat = Microsoft.DirectX.Direct3D.Format.Unknown;
            this.d3DSimpleSurface1.DepthStencilFormat = Microsoft.DirectX.Direct3D.DepthFormat.Unknown;
            this.d3DSimpleSurface1.Location = new System.Drawing.Point(47, 58);
            this.d3DSimpleSurface1.MultiSample = Microsoft.DirectX.Direct3D.MultiSampleType.None;
            this.d3DSimpleSurface1.Name = "d3DSimpleSurface1";
            this.d3DSimpleSurface1.OnRenderRaiseEventOnly = false;
            this.d3DSimpleSurface1.PresentIntervalImmediate = false;
            this.d3DSimpleSurface1.SetViewProjTransformsAutomatically = true;
            this.d3DSimpleSurface1.Size = new System.Drawing.Size(150, 150);
            this.d3DSimpleSurface1.SuspendRender = false;
            this.d3DSimpleSurface1.TabIndex = 0;
            this.d3DSimpleSurface1.UseDefaultLighting = true;
            this.d3DSimpleSurface1.VertexProcessingFlag = Microsoft.DirectX.Direct3D.CreateFlags.SoftwareVertexProcessing;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.d3DSimpleSurface1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private RC.D3DControls.D3DSimpleSurface d3DSimpleSurface1;
    }
}

