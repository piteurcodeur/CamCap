namespace CamCap
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            start = new Button();
            capture = new Button();
            pictureBox1 = new PictureBox();
            imageList1 = new ImageList(components);
            listView1 = new ListView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            deleteToolStripMenuItem = new ToolStripMenuItem();
            renameToolStripMenuItem = new ToolStripMenuItem();
            button1 = new Button();
            menuStrip1 = new MenuStrip();
            MenuFile = new ToolStripMenuItem();
            SousMenuFileOpenFolder = new ToolStripMenuItem();
            actualiserToolStripMenuItem = new ToolStripMenuItem();
            MenuDevice = new ToolStripMenuItem();
            MenuResolution = new ToolStripMenuItem();
            MenuSettings = new ToolStripMenuItem();
            MenuVideo = new ToolStripMenuItem();
            SousMenuVideoStart = new ToolStripMenuItem();
            SousMenuVideoCapture = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // start
            // 
            start.BackColor = Color.DarkGray;
            start.Location = new Point(182, 47);
            start.Margin = new Padding(3, 2, 3, 2);
            start.Name = "start";
            start.Size = new Size(82, 32);
            start.TabIndex = 2;
            start.Text = "Start";
            start.UseVisualStyleBackColor = false;
            start.Click += start_Click_1;
            // 
            // capture
            // 
            capture.BackColor = Color.DarkGray;
            capture.Location = new Point(182, 94);
            capture.Margin = new Padding(3, 2, 3, 2);
            capture.Name = "capture";
            capture.Size = new Size(82, 32);
            capture.TabIndex = 3;
            capture.Text = "Capture";
            capture.UseVisualStyleBackColor = false;
            capture.Click += capture_Click_1;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackColor = Color.LightGray;
            pictureBox1.Cursor = Cursors.Cross;
            pictureBox1.Location = new Point(285, 36);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(887, 472);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // listView1
            // 
            listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listView1.BackColor = Color.FromArgb(54, 57, 63);
            listView1.ForeColor = Color.White;
            listView1.Location = new Point(12, 47);
            listView1.Margin = new Padding(3, 2, 3, 2);
            listView1.Name = "listView1";
            listView1.Size = new Size(157, 462);
            listView1.TabIndex = 6;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.MouseClick += listView1_MouseClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { deleteToolStripMenuItem, renameToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(118, 48);
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(117, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.Size = new Size(117, 22);
            renameToolStripMenuItem.Text = "Rename";
            renameToolStripMenuItem.Click += renameToolStripMenuItem_Click;
            // 
            // button1
            // 
            button1.Location = new Point(182, 140);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(82, 30);
            button1.TabIndex = 7;
            button1.Text = "settings";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.ActiveCaption;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { MenuFile, MenuDevice, MenuResolution, MenuSettings, MenuVideo });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(1183, 24);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // MenuFile
            // 
            MenuFile.DropDownItems.AddRange(new ToolStripItem[] { SousMenuFileOpenFolder, actualiserToolStripMenuItem });
            MenuFile.Name = "MenuFile";
            MenuFile.Size = new Size(37, 20);
            MenuFile.Text = "File";
            // 
            // SousMenuFileOpenFolder
            // 
            SousMenuFileOpenFolder.Name = "SousMenuFileOpenFolder";
            SousMenuFileOpenFolder.Size = new Size(137, 22);
            SousMenuFileOpenFolder.Text = "Open folder";
            SousMenuFileOpenFolder.Click += SousMenuFileOpenFolder_Click;
            // 
            // actualiserToolStripMenuItem
            // 
            actualiserToolStripMenuItem.Name = "actualiserToolStripMenuItem";
            actualiserToolStripMenuItem.Size = new Size(137, 22);
            actualiserToolStripMenuItem.Text = "Actualiser";
            actualiserToolStripMenuItem.Click += actualiserToolStripMenuItem_Click;
            // 
            // MenuDevice
            // 
            MenuDevice.Name = "MenuDevice";
            MenuDevice.Size = new Size(54, 20);
            MenuDevice.Text = "Device";
            // 
            // MenuResolution
            // 
            MenuResolution.Name = "MenuResolution";
            MenuResolution.Size = new Size(75, 20);
            MenuResolution.Text = "Resolution";
            MenuResolution.Click += resolutionToolStripMenuItem_Click;
            // 
            // MenuSettings
            // 
            MenuSettings.Name = "MenuSettings";
            MenuSettings.Size = new Size(61, 20);
            MenuSettings.Text = "Settings";
            MenuSettings.Click += settingsToolStripMenuItem_Click;
            // 
            // MenuVideo
            // 
            MenuVideo.DropDownItems.AddRange(new ToolStripItem[] { SousMenuVideoStart, SousMenuVideoCapture });
            MenuVideo.Name = "MenuVideo";
            MenuVideo.Size = new Size(49, 20);
            MenuVideo.Text = "Video";
            // 
            // SousMenuVideoStart
            // 
            SousMenuVideoStart.Name = "SousMenuVideoStart";
            SousMenuVideoStart.Size = new Size(116, 22);
            SousMenuVideoStart.Text = "Start";
            SousMenuVideoStart.Click += startToolStripMenuItem_Click;
            // 
            // SousMenuVideoCapture
            // 
            SousMenuVideoCapture.Name = "SousMenuVideoCapture";
            SousMenuVideoCapture.Size = new Size(116, 22);
            SousMenuVideoCapture.Text = "Capture";
            SousMenuVideoCapture.Click += captureToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(47, 49, 54);
            ClientSize = new Size(1183, 518);
            Controls.Add(button1);
            Controls.Add(listView1);
            Controls.Add(pictureBox1);
            Controls.Add(capture);
            Controls.Add(start);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "CamCap";
            FormClosed += Form1_FormClosed_1;
            Load += Form1_Load_1;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox comboBox1;
        private Button start;
        private Button capture;
        private PictureBox pictureBox1;
        private ComboBox comboBox2;
        private ImageList imageList1;
        private ListView listView1;
        private Button button1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem MenuFile;
        private ToolStripMenuItem SousMenuFileOpenFolder;
        private ToolStripMenuItem MenuDevice;
        private ToolStripMenuItem MenuResolution;
        private ToolStripMenuItem MenuSettings;
        private ToolStripMenuItem MenuVideo;
        private ToolStripMenuItem SousMenuVideoStart;
        private ToolStripMenuItem SousMenuVideoCapture;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem actualiserToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
    }
}