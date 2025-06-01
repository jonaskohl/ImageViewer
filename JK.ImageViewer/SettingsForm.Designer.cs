using JK.ImageViewer.Controls;

namespace JK.ImageViewer
{
    partial class SettingsForm
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
            tableLayoutPanel1 = new TableLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            okButton = new Button();
            cancelButton = new Button();
            applyButton = new Button();
            iconListBox1 = new IconListBox();
            columnHeader1 = new ColumnHeader();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 1);
            tableLayoutPanel1.Controls.Add(iconListBox1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(578, 368);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Right;
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.SetColumnSpan(flowLayoutPanel1, 2);
            flowLayoutPanel1.Controls.Add(okButton);
            flowLayoutPanel1.Controls.Add(cancelButton);
            flowLayoutPanel1.Controls.Add(applyButton);
            flowLayoutPanel1.Location = new Point(353, 359);
            flowLayoutPanel1.Margin = new Padding(0, 6, 0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(225, 9);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // okButton
            // 
            okButton.AutoSize = true;
            okButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            okButton.DialogResult = DialogResult.OK;
            okButton.FlatStyle = FlatStyle.System;
            okButton.Location = new Point(3, 0);
            okButton.Margin = new Padding(3, 0, 0, 0);
            okButton.MinimumSize = new Size(72, 0);
            okButton.Name = "okButton";
            okButton.Size = new Size(72, 9);
            okButton.TabIndex = 0;
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.AutoSize = true;
            cancelButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.FlatStyle = FlatStyle.System;
            cancelButton.Location = new Point(78, 0);
            cancelButton.Margin = new Padding(3, 0, 0, 0);
            cancelButton.MinimumSize = new Size(72, 0);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(72, 9);
            cancelButton.TabIndex = 1;
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // applyButton
            // 
            applyButton.AutoSize = true;
            applyButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            applyButton.FlatStyle = FlatStyle.System;
            applyButton.Location = new Point(153, 0);
            applyButton.Margin = new Padding(3, 0, 0, 0);
            applyButton.MinimumSize = new Size(72, 0);
            applyButton.Name = "applyButton";
            applyButton.Size = new Size(72, 9);
            applyButton.TabIndex = 2;
            applyButton.UseVisualStyleBackColor = true;
            applyButton.Click += applyButton_Click;
            // 
            // iconListBox1
            // 
            iconListBox1.Dock = DockStyle.Fill;
            iconListBox1.DrawMode = DrawMode.OwnerDrawFixed;
            iconListBox1.ItemHeight = 20;
            iconListBox1.Location = new Point(0, 0);
            iconListBox1.Margin = new Padding(0, 0, 3, 0);
            iconListBox1.Name = "iconListBox1";
            iconListBox1.Size = new Size(197, 353);
            iconListBox1.TabIndex = 0;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "";
            columnHeader1.Width = 25;
            // 
            // SettingsForm
            // 
            AcceptButton = applyButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new Size(602, 392);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            Padding = new Padding(12);
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button okButton;
        private Button cancelButton;
        private Button applyButton;
        private IconListBox iconListBox1;
        private ColumnHeader columnHeader1;
    }
}