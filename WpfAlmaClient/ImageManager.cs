using Microsoft.Win32;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAlmaClient.CrisisUnityService;

namespace WpfAlmaClient
{
    public class ImageManager
    {
        private static string imageDirectory;
        public static string ImageDirectory { get => imageDirectory; set => imageDirectory = value; }
        public static string[] GetAllPostImages(Post post)
        {
            DirectoryInfo directory = new DirectoryInfo(imageDirectory + post.ID + "\\");
            FileInfo[] files = directory.GetFiles("*.*");
            UnityClient service = new UnityClient();
            string[] serviceImages = service.GetImagesByPost(post.ID.ToString());
            List<string> clientImages = files.Select(file => file.Name).ToList();
            foreach (string file in serviceImages)
            {
                if (!clientImages.Contains(file))
                {
                    string path = GetImageFromService(file, post);
                    clientImages.Add(path);
                }
            }
            return clientImages.ToArray();
        }

        public static string GetImageFromService(string file, Post post)
        {
            UnityClient service = new UnityClient();
            byte[] imageArray = service.GetIamge(file, post.ID.ToString());
            MemoryStream stream = new MemoryStream(imageArray);
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

            string localFilePath = Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.Length-10)+@"\Images\Posts\"+ post.ID+"\\";
            localFilePath += file;
            image.Save(localFilePath);
            return localFilePath + file;
        }

        public static string Image_Dialog()
        {

            string filename = null;
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.Filter = "All Images | *.jpg;*.jpeg;*.tif;*.tiff;*.bmp;*.png|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
                filename = SaveImageToClient(filename); // save the Picture in LocalFolder
                                                        // (if not exist) rns return only the file name
            }
            return filename;
        }
        public static string SaveImageToClient(string sourcefileName)
        {
            string fileName = System.IO.Path.GetFileName(sourcefileName);
            string localFilePath = System.IO.Path.Combine(imageDirectory, fileName);
            if (!File.Exists(localFilePath))
            {
                byte[] imgArray = File.ReadAllBytes(sourcefileName);
                var stream = new MemoryStream(imgArray);
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);

                img.Save(localFilePath);
            }
            return fileName;
        }
        public static void SendImageToService(string image)
        {
            string path = System.IO.Path.Combine(imageDirectory, image);
            byte[] imgArray = File.ReadAllBytes(path);

           UnityClient service = new UnityClient();
            service.SaveImage(imgArray, image);
        }
    }
}
