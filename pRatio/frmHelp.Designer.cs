namespace pRatio
{
    partial class frmHelp
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHelp));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtAbout = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtBCommands = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAbout);
            this.groupBox1.Location = new System.Drawing.Point(16, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(588, 125);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "about pRatio";
            // 
            // txtAbout
            // 
            this.txtAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAbout.Location = new System.Drawing.Point(3, 16);
            this.txtAbout.Name = "txtAbout";
            this.txtAbout.ReadOnly = true;
            this.txtAbout.Size = new System.Drawing.Size(582, 106);
            this.txtAbout.TabIndex = 2;
            this.txtAbout.Text = resources.GetString("txtAbout.Text");
            this.txtAbout.TextChanged += new System.EventHandler(this.txtAbout_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtBCommands);
            this.groupBox2.Location = new System.Drawing.Point(16, 172);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(587, 178);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "use of pRatio";
            // 
            // txtBCommands
            // 
            this.txtBCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBCommands.Location = new System.Drawing.Point(3, 16);
            this.txtBCommands.Name = "txtBCommands";
            this.txtBCommands.ReadOnly = true;
            this.txtBCommands.Size = new System.Drawing.Size(581, 159);
            this.txtBCommands.TabIndex = 1;
            this.txtBCommands.Text = resources.GetString("txtBCommands.Text");
            // 
            // frmHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 383);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "frmHelp";
            this.Text = "frmHelp";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox txtBCommands;
        private System.Windows.Forms.RichTextBox txtAbout;
    }
}