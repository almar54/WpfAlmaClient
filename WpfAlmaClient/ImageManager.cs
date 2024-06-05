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
            string[] serviceImages = null;
            UnityClient service = new UnityClient();
            serviceImages = service.GetImagesByPost(post.ID.ToString());

            List<string> localImage = new List<string>();
            string localFolder = System.IO.Path.Combine(imageDirectory, post.ID.ToString());
            if (Directory.Exists(localFolder))
                localImage = Directory.GetFiles(localFolder).ToList();
           
            if (serviceImages == null) return null;
            foreach (string file in serviceImages)
            {
                if (localImage.Find(img=>img.Contains(file))==null)
                {
                    string path = GetImageFromService(file, post);
                    localImage.Add(path);
                }
            }
            return localImage.ToArray();
        }

        public static string GetImageFromService(string file, Post post)
        {
            UnityClient service = new UnityClient();
            byte[] imageArray = service.GetIamge(file, post.ID.ToString());

            MemoryStream stream = new MemoryStream(imageArray);
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

            Directory.CreateDirectory(System.IO.Path.Combine(imageDirectory, post.ID.ToString()));
            string localFilePath = System.IO.Path.Combine(imageDirectory, post.ID.ToString(), file);
            image.Save(localFilePath);
            return localFilePath;
        }

        public static string[] Image_Dialog()
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif)|*.jpg; *.jpeg; *.png; *.gif";
            dlg.Multiselect = true;
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                return dlg.FileNames;
            }
            return null;
        }
        public static string SaveImageToClient(string sourcefileName,string folderName)
        {
            //getting new location to save image
            Directory.CreateDirectory(System.IO.Path.Combine(imageDirectory, folderName));
            string fileName = System.IO.Path.GetFileName(sourcefileName);
            string localFilePath = System.IO.Path.Combine(imageDirectory, folderName, fileName);
            System.IO.File.Copy(sourcefileName, localFilePath, true);
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
