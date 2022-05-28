namespace StartupFolderShortcut
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
      this.buttonCreate = new System.Windows.Forms.Button();
      this.buttonDelete = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // buttonCreate
      // 
      this.buttonCreate.Location = new System.Drawing.Point(12, 12);
      this.buttonCreate.Name = "buttonCreate";
      this.buttonCreate.Size = new System.Drawing.Size(131, 42);
      this.buttonCreate.TabIndex = 0;
      this.buttonCreate.Text = "Create Shortcut";
      this.buttonCreate.UseVisualStyleBackColor = true;
      this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
      // 
      // buttonDelete
      // 
      this.buttonDelete.Location = new System.Drawing.Point(12, 69);
      this.buttonDelete.Name = "buttonDelete";
      this.buttonDelete.Size = new System.Drawing.Size(131, 42);
      this.buttonDelete.TabIndex = 1;
      this.buttonDelete.Text = "Delete Shortcuts";
      this.buttonDelete.UseVisualStyleBackColor = true;
      this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(160, 129);
      this.Controls.Add(this.buttonDelete);
      this.Controls.Add(this.buttonCreate);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonCreate;
    private System.Windows.Forms.Button buttonDelete;
  }
}

