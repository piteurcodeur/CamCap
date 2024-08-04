
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using AForge.Video;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using GroupDocs.Metadata.Standards.Exif;
using GroupDocs.Metadata;
//using SharpDX.WIC;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace CamCap
{
    public partial class Form1 : Form
    {
        private bool folderOpen = false;

        private bool DeviceExist = false;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice? videoSource = null;
        private VideoCapabilities[] videoCapabilities;
        private FormSettings frmSettings1 = new();
        private ToolStripItem? toolStripItem = null;
        private string actualPath;
        public string defaultDirectoryPath = @"CamCapTemp/repertoire/";

        public Form1()
        {
            InitializeComponent();

        }


        private void Form1_Load_1(object sender, EventArgs e)
        {

            actualPath = frmSettings1.LastFolderOpen;
            imp_camList();
            button1.Enabled = false;
            listView1.ItemActivate += ListView1_ItemActivate;
            pictureBox1.BackColor = Color.DarkGray;
            if (!Directory.Exists(@"CamCapTemp/repertoire"))
            {
                Directory.CreateDirectory(@"CamCapTemp/repertoire");
            }
            try
            {
                AcquireImage(actualPath);
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(@"CamCapTemp/images"))
                {
                    Directory.CreateDirectory(@"CamCapTemp/images");
                    AcquireImage(@"CamCapTemp/images");
                }
            }
            /*
             * images => dossier par défaut pour sauvegarder les images
             * répertoire => dossier qui permet de stocker les images pendant leur modification
             */


        }

        public void AcquireImage(string path)
        {
            //permet de charger les images du dossier actuel dans la liste d'image
            listView1.Clear();
            imageList1.Dispose();
            string[] files = Directory.GetFiles(path);
            foreach (string item in files)
            {
                if (item.EndsWith(".jpg"))
                {
                    Image image = Image.FromFile(item);
                    imageList1.Images.Add(image);
                    imageList1.ImageSize = new Size(91, 64);

                    listView1.LargeImageList = imageList1;
                    listView1.View = View.LargeIcon;
                    int indexElt = listView1.Items.Count;
                    listView1.Items.Add(new ListViewItem(Path.GetFileName(actualPath + @"\" + item), indexElt));
                    image.Dispose();
                }


            }

        }


        public void imp_camList()//liste des caméras dans le sous-menu Device
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                DeviceExist = true;
                foreach (FilterInfo device in videoDevices)
                {
                    ToolStripItem subItem = new ToolStripMenuItem(device.Name);
                    MenuDevice.DropDownItems.Add(subItem);
                    MenuDevice.DropDownItems[MenuDevice.DropDownItems.IndexOf(subItem)].Click += SelectCam;
                }
            }
            catch (ApplicationException)
            {
                DeviceExist = false;
                MessageBox.Show("No capture device on your system");
            }
        }

        public void SelectCam(object? sender, EventArgs e)
        {
            toolStripItem = (ToolStripItem)sender;
        }

        private void CloseVideoSource() //close the device safely
        {
            if (!(videoSource == null))
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs) //eventhandler if new frame is ready
        {
            System.Drawing.Bitmap img = (System.Drawing.Bitmap)eventArgs.Frame.Clone();
            img.RotateFlip(RotateFlipType.RotateNoneFlipX);
            pictureBox1.Image = img;
        }

        private void Form1_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            CloseVideoSource();
            frmSettings1.Save();
        }

        private void capture_Click_1(object sender, EventArgs e)
        {
            Capture_image();
        }

        private void start_Click_1(object sender, EventArgs e)
        {
            Start_video();
        }

        public void Capture_image()
        {
            if (start.Text == "Stop")
            {
                if (!Directory.Exists(@"CamCapTemp/images"))
                {
                    Directory.CreateDirectory(@"CamCapTemp/images");
                    MessageBox.Show("Image folder Created...");
                }
                //listView1.Items.Count
                string path = actualPath + @"\" + "Capture-" + Convert.ToString(get_NumberImage()) + ".jpg";
                Image copy = pictureBox1.Image;

                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                ImageCodecInfo jpegEncoder = GetEncoder(ImageFormat.Jpeg);

                copy.Save(path);
                imageList1.Images.Add(copy);
                imageList1.ImageSize = new Size(91, 64);


                listView1.LargeImageList = imageList1;
                listView1.View = View.LargeIcon;
                int indexElt = listView1.Items.Count;
                listView1.Items.Add(new ListViewItem(Path.GetFileName(path), indexElt));
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            //les images capturées sont par défaut des .png, cette méthode permet de les convertir en .jpg pour pouvoir accéder ensuite aux EXIF
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        public void Start_video()
        {
            if (start.Text == "Start")
            {
                if (DeviceExist && toolStripItem != null)
                {
                    button1.Enabled = true;
                    //videoSource = new VideoCaptureDevice(videoDevices[comboBox1.SelectedIndex].MonikerString);
                    videoSource = new VideoCaptureDevice(videoDevices[MenuDevice.DropDownItems.IndexOf(toolStripItem)].MonikerString);
                    videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                    CloseVideoSource();
                    videoSource.Start();
                    start.Text = "Stop";
                    MenuVideo.DropDownItems[0].Text = "Stop";
                    start.BackColor = Color.Red;
                    getResolutionList();
                }
                else
                {
                    MessageBox.Show(" Error : No Device selected ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                button1.Enabled = false;
                pictureBox1.Image = null;
                //pictureBox1.Dispose();
                MenuVideo.DropDownItems[0].Text = "Start";
                if (videoSource.IsRunning)
                {
                    CloseVideoSource();
                    start.Text = "Start";
                    start.BackColor = Color.Gray;
                }
            }
        }


        private void ListView1_ItemActivate(object sender, EventArgs e)
        {

            System.Windows.Forms.ListView listView = (System.Windows.Forms.ListView)sender;
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView.SelectedItems[0];
                //Image image = selectedItem.ImageList.Images[selectedItem.ImageIndex];
                Image image = Image.FromFile(actualPath + "/" + selectedItem.Text);
                activate_imageLargeView(selectedItem.Text, image);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            videoSource.VideoResolution = videoCapabilities[comboBox2.SelectedIndex];
        }

        private void activate_imageLargeView(string imageName, Image image)
        {
            Form2 form = new Form2(imageName, image, actualPath);
            form.Show();
        }

        private void getResolutionList() //liste des résolutions pour la caméra sélectionnée
        {
            if (videoSource != null)
            {
                videoCapabilities = videoSource.VideoCapabilities;

                foreach (VideoCapabilities capability in videoCapabilities)
                {
                    ToolStripItem subItem = new ToolStripMenuItem(capability.FrameSize.Width + "x" + capability.FrameSize.Height + " " + capability.MaximumFrameRate + "fps");
                    MenuResolution.DropDownItems.Add(subItem);
                    MenuResolution.DropDownItems[MenuResolution.DropDownItems.IndexOf(subItem)].Click += SetResolution;
                }
                //videoSource.VideoResolution = videoCapabilities[MenuResolution.SelectedIndex];
            }
        }

        public void SetResolution(object sender, EventArgs e)
        {
            //videoSource.VideoResolution = videoCapabilities[MenuResolution.DropDownItems.IndexOf((ToolStripItem)sender)];
            /*
            string message = $"Résolution : {videoSource.VideoResolution.FrameSize.Width}x{videoSource.VideoResolution.FrameSize.Height}\n";
            message += $"Fréquence d'images maximale : {videoSource.VideoResolution.MaximumFrameRate}fps";
            MessageBox.Show(message, "Capacité vidéo choisie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            */

            //à dévelloper 
            //actuellement on ne peut pas choisir une résolution
            //elles s'affichent seulement
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getResolutionList();
        }



        public int get_NumberImage()
        {
            //incrémente un numéro qui permet de nommer les captures
            frmSettings1.NumberImage += 1;
            frmSettings1.Save();
            return frmSettings1.NumberImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisplaySettings();
        }

        public void DisplaySettings()
        {
            //affiche la page de paramètres correspondants à ceux du driver de la caméra sélectionnée
            if (videoSource != null)
            {
                videoSource.DisplayPropertyPage(IntPtr.Zero);
            }

        }

        private void resolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplaySettings();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Start_video();
        }

        private void captureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Capture_image();
        }

        public string OpenFile()
        {
            //ouvrir un fichier
            //méthode non utilisée
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }
            return filePath;
        }

        public string OpenFolder()
        {
            //ouvrir un dossier dans l'application
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string foldername = folderBrowserDialog.SelectedPath;
                frmSettings1.LastFolderOpen = foldername;
                return foldername;
            }
            return null;
        }

        private void SousMenuFileOpenFolder_Click(object sender, EventArgs e)
        {
            string folder = OpenFolder();
            if (folder != null)
            {
                actualPath = folder;
                AcquireImage(folder);
            }

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    contextMenuStrip1.Show(listView1, e.Location);
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //permet de supprimer une image directement dans la liste d'image 
            string imageClicked = listView1.SelectedItems[0].Text;
            listView1.Items.Clear();
            imageList1.Dispose();

            File.Delete(actualPath + "/" + imageClicked);

            AcquireImage(actualPath);

        }

        private void actualiserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //permet d'actualiser la liste d'image pour que la dernière image créée apparaisse
            //vérifier d'abord que le chemin existe
            if (!Directory.Exists(actualPath))
            {
                return;
            }
            AcquireImage(actualPath);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //permet de renommer une capture directement dans la liste d'image
            string userInput = ShowInputDialog("Entrez votre texte :");
            try
            {
                if (userInput != "")
                {
                    //File.Copy(actualPath + listView1.SelectedItems[0].Name, actualPath + userInput + ".jpg", true);
                    Image image = Image.FromFile(actualPath + "/" + listView1.SelectedItems[0].Text);
                    image.Save(actualPath + "/" + userInput + ".jpg");
                    image.Dispose();
                    File.Delete(actualPath + "/" + listView1.SelectedItems[0].Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(actualPath + listView1.SelectedItems[0].Text);
            }
            AcquireImage(actualPath);
        }

        static string ShowInputDialog(string prompt)
        {
            //boite de dialogue pour renommer l'image
            Form inputBox = new Form();
            Label label = new Label();
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Button buttonOk = new System.Windows.Forms.Button();
            System.Windows.Forms.Button buttonCancel = new System.Windows.Forms.Button();

            inputBox.Text = "Entrée utilisateur";
            label.Text = prompt;
            buttonOk.Text = "OK";
            buttonCancel.Text = "Annuler";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            inputBox.ClientSize = new System.Drawing.Size(396, 107);
            inputBox.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            inputBox.ClientSize = new System.Drawing.Size(Math.Max(300, label.Right + 10), inputBox.ClientSize.Height);
            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.StartPosition = FormStartPosition.CenterScreen;
            inputBox.MinimizeBox = false;
            inputBox.MaximizeBox = false;
            inputBox.AcceptButton = buttonOk;
            inputBox.CancelButton = buttonCancel;

            DialogResult result = inputBox.ShowDialog();
            if (result == DialogResult.OK)
            {
                return textBox.Text;
            }
            else
            {
                return string.Empty;
            }
        }


    }


    sealed class FormSettings : ApplicationSettingsBase
    {
        /*
         * Paramètres de l'application, sauvegardés même après la fermeture du programme
         * NumberImage est un nombre qui s'incrémente à chaque capture, pour nommer la catpture suivzante différemment
         * LastFolderOpen est le chemin d'accès du dossier ouvert dans l'application
         * par défaut, c'est le dossier images créé par l'application qui est sélectionné
         * Ce setting permet au programme de se souvenir du dernier dossier ouvert avant sa fermeture
         */
        [UserScopedSettingAttribute()]
        public int NumberImage
        {
            get
            {
                if (this["NumberImage"] != null)
                {
                    return (int)this["NumberImage"];
                }
                else
                {
                    return 0;
                }
            }
            set { this["NumberImage"] = value; }
        }
        [UserScopedSettingAttribute()]
        public string LastFolderOpen
        {
            get
            {
                if (this["LastFolderOpen"] != null)
                {
                    return (string)this["LastFolderOpen"];
                }
                else
                {
                    return "CamCapTemp/images/";
                }
            }
            set { this["LastFolderOpen"] = value; }
        }

    }
}