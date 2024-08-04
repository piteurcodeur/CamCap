using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using CamCap;
using System;
using System.Reflection;
using System.Reflection.Metadata;
using GroupDocs.Metadata;
using GroupDocs.Metadata.Standards.Exif;
using Aspose.Imaging.ImageOptions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;







namespace CamCap
{


    public partial class Form2 : Form 
    {
        public Form2(string imageName, Image image, string actualpath)
        {
            InitializeComponent();
            //private ImageMetaExtensions ImageMetaExtensions = new ImageMetaExtensions();

            ImageName.value = imageName;
            ImageName.actualPath = actualpath + @"/";
            //int nouvelleLargeur = 500;
            //int nouvelleHauteur = 350;
            //Image imageRedimensionnee = image.GetThumbnailImage(nouvelleLargeur, nouvelleHauteur, null, IntPtr.Zero);
            pictureBox1.Image = image;
            label1.Text = imageName;
            textBox2.Text = imageName.Split(".")[0];

        }
        /*
         * Retrouver le chemin de AppData 
         * 
         * defaultDirectoryPath => repertoire
         */
        static string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string repertoirePath = Path.Combine(AppDataPath, @"CamCapTemp/repertoire/");
        public string imagesPath = Path.Combine(AppDataPath, @"CamCapTemp/images/");
        

        private void button1_Click(object sender, EventArgs e)
        {
            String comment = textBox1.Text; //commentaire à ajouter aux EXIF
            Image im = pictureBox1.Image;
            

            
            try
            {
                if (comment != "") //commentaire non nul
                {
                    Image modifiee = EditExif(im, comment);
                    string imagePath = ImageName.actualPath + textBox2.Text + ".jpg";
                    if (textBox2.Text != "")
                    {
                        if (Directory.Exists(ImageName.actualPath))
                        {
                            modifiee.Save(repertoirePath + textBox2.Text + ".jpg");
                        }
                        else
                        {
                            CreateDefaultDirectory();
                            modifiee.Save(repertoirePath + textBox2.Text + ".jpg");
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Vous devez donner un nom à votre fichier");
                    }
                    im.Dispose();
                    File.Delete(ImageName.actualPath + ImageName.value); //supprimer l'image précédente
                    modifiee.Dispose();
                }
                else //commentaire nul
                {
                    Image modifiee = im;
                    if(!Directory.Exists(ImageName.actualPath + textBox2.Text + ".jpg"))
                    {
                        modifiee.Save(repertoirePath + textBox2.Text + ".jpg"); //save dans le repertoire temp
                        
                    }
                    modifiee.Dispose();
                    im.Dispose();
                }
                
                File.Delete(ImageName.actualPath + ImageName.value); //supprimer l'image précédente

            }
            catch (Exception ex)
            {
                MessageBox.Show("Vous devez choisir un nouveau nom de fichier " + ex);
            }

            File.Copy(repertoirePath + textBox2.Text + ".jpg", ImageName.actualPath + textBox2.Text + ".jpg", true);
            File.Delete(repertoirePath + textBox2.Text + ".jpg");
            Close();

        }

        private void CreateDefaultDirectory()
        {
            //Créer un répertoire pour enregistrer l'image temporairement pendant les modifications, s'il n'existe pas déjà
            if (!Directory.Exists(repertoirePath))
            {
                Directory.CreateDirectory(repertoirePath);
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
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

        public Bitmap Commenter(Bitmap bitmap, string comment)
        {


            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            Brush brush = new SolidBrush(Color.FromKnownColor(KnownColor.White));

            // Déclarer le pinceau en spécifiant la couleur
            Pen pen = new Pen(Color.FromKnownColor(KnownColor.Black), 1);

            // Définir la police du texte
            Font arial = new Font("Arial", 15, FontStyle.Regular);

            // Définir le texte
            string text = comment;
            Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, 40);
            graphics.DrawRectangle(pen, rectangle);

            // Dessiner du texte
            graphics.DrawString(text, arial, brush, rectangle);

            return bitmap;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBox1.Image.Dispose();
            //supprimer les images du repertoire
            string[] files = Directory.GetFiles(repertoirePath);
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        public Image EditExif(Image image, string comment)
        {
            // Créer un nouveau PropertyItem pour le commentaire EXIF
            PropertyItem commentProperty = CreateCommentPropertyItem(comment);

            // Ajouter le PropertyItem à la liste des PropertyItems de l'image
            image.SetPropertyItem(commentProperty);

            // Enregistrer l'image modifiée dans un nouveau fichier
            return image;

        }
        private static PropertyItem CreateCommentPropertyItem(string comment)
        {
            PropertyItem propertyItem = ImagePropertyItem(37510); // Identifiant pour le commentaire EXIF

            // Convertir le commentaire en tableau de bytes
            byte[] commentBytes = System.Text.Encoding.ASCII.GetBytes(comment + '\0');

            // Définir les propriétés du PropertyItem
            propertyItem.Type = 2; // Chaîne ASCII (null-terminated)
            propertyItem.Len = commentBytes.Length;
            propertyItem.Value = commentBytes;

            return propertyItem;
        }
        private static PropertyItem ImagePropertyItem(int id)
        {
            PropertyItem propertyItem = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            propertyItem.Id = id;
            return propertyItem;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            using (Image image = Image.FromFile(ImageName.actualPath + ImageName.value))
            {
                // Lire les PropertyItems de l'image
                PropertyItem[] propertyItems = image.PropertyItems;

                // Rechercher le PropertyItem correspondant au commentaire EXIF (identifiant 37510)
                PropertyItem commentProperty = FindCommentPropertyItem(propertyItems);

                if (commentProperty != null)
                {
                    // Convertir les données du commentaire en une chaîne de caractères
                    string comment = Encoding.ASCII.GetString(commentProperty.Value).TrimEnd('\0');

                    textBox1.Text = comment;
                }

            }
        }

        private static PropertyItem FindCommentPropertyItem(PropertyItem[] propertyItems)
        {
            foreach (PropertyItem propertyItem in propertyItems)
            {
                if (propertyItem.Id == 37510) // Identifiant pour le commentaire EXIF
                {
                    return propertyItem;
                }
            }

            return null; // Commentaire EXIF non trouvé
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }



    static class ImageName
    {
        /*
         * une classe qui permet de récupérer le nom de l'image ouverte, et son chemin d'accès
         */

        public static string value;
        //public static string path = @"CamCapTemp/images";
        static string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string actualPath = AppDataPath +  @"CamCapTemp/images/";

    }

}

