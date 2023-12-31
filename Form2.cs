﻿using System;
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
using WinFormsApp2;
using System;
using System.Reflection;
using System.Reflection.Metadata;
using GroupDocs.Metadata;
using GroupDocs.Metadata.Standards.Exif;
using Aspose.Imaging.ImageOptions;

namespace WinFormsApp2
{


    public partial class Form2 : Form
    {
        public Form2(string imageName, Image image, string actualpath)
        {
            InitializeComponent();
            //private ImageMetaExtensions ImageMetaExtensions = new ImageMetaExtensions();

            ImageName.value = imageName;  
            ImageName.doc_path = actualpath + @"/";
            //int nouvelleLargeur = 500;
            //int nouvelleHauteur = 350;
            //Image imageRedimensionnee = image.GetThumbnailImage(nouvelleLargeur, nouvelleHauteur, null, IntPtr.Zero);
            pictureBox1.Image = image;
            label1.Text = imageName;
            textBox2.Text = imageName.Split(".")[0];

        }
        public string defaultDirectoryPath = @"WinFormsApp2/repertoire/";

        private void button1_Click(object sender, EventArgs e)
        {
            String comment = textBox1.Text; //commentaire à ajouter aux EXIF
            Image im = pictureBox1.Image;
            /*Bitmap bitmap = new Bitmap(im);
            Bitmap modifiee = Commenter(bitmap, comment);
            //
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
            ImageCodecInfo jpegEncoder = GetEncoder(ImageFormat.Jpeg);
            */
            try
            {
                if (comment != "")
                {
                    Image modifiee = EditExif(im, comment);
                    string imagePath = ImageName.doc_path + textBox2.Text + ".jpg";
                    if (textBox2.Text != "")
                    {
                        CreateDefaultDirectory();
                        modifiee.Save(defaultDirectoryPath + textBox2.Text + ".jpg");
                    }
                    else
                    {
                        MessageBox.Show("Vous devez donner un nom à votre fichier");
                    }

                    im.Dispose();
                    //bitmap.Dispose();
                    modifiee.Dispose();
                }
                else
                {
                    Image modifiee = im;
                    modifiee.Save(ImageName.doc_path + textBox2.Text + ".jpg");
                    im.Dispose();
                    //bitmap.Dispose();
                    modifiee.Dispose();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Vous devez choisir un nouveau nom de fichier " + ex);
            }

            File.Copy(defaultDirectoryPath + textBox2.Text + ".jpg", ImageName.doc_path + textBox2.Text + ".jpg", true);
            File.Delete(defaultDirectoryPath + textBox2.Text + ".jpg");
            Close();

        }

        private void CreateDefaultDirectory()
        {
            //Créer un répertoire pour enregistrer l'image temporairement pendant les modifications, s'il n'existe pas déjà
            if (!Directory.Exists(defaultDirectoryPath))
            {
                Directory.CreateDirectory(defaultDirectoryPath);
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
            string[] files = Directory.GetFiles(defaultDirectoryPath);
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
            PropertyItem propertyItem = (PropertyItem)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            propertyItem.Id = id;
            return propertyItem;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            using (Image image = Image.FromFile(ImageName.doc_path + ImageName.value))
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
    }



    static class ImageName
    {
        /*
         * une classe qui permet de récupérer le nom de l'image ouverte, et son chemin d'accès
         */

        public static string value;
        //public static string path = @"WinFormsApp2/images";
        public static string doc_path = @"WinFormsApp2/images/";
        
    }

}

