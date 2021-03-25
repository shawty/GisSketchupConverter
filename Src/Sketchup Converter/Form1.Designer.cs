
namespace Sketchup_Converter
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Dock = System.Windows.Forms.DockStyle.Top;
      this.label1.Location = new System.Drawing.Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(430, 72);
      this.label1.TabIndex = 0;
      this.label1.Text = "Please drag your Shape Files (*.shp) one at a time into the center of this window" +
    ", and the program will create a geographically referenced  2D spline ready to lo" +
    "ad into sketchup 2017.";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // label2
      // 
      this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.label2.Location = new System.Drawing.Point(0, 363);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(430, 44);
      this.label2.TabIndex = 1;
      this.label2.Text = "The sketchup compatible file will be saved in the same folder as your shape file " +
    "is in.";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // Form1
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(430, 407);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.Name = "Form1";
      this.Text = "GIS Shape to Sketchup 2D converter V1.00";
      this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
  }
}

