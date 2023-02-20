using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace TemplateGenerator.Utilities
{
    public class FolderScanner
    {
        public List<int> DefaultIDs { get; }

        public FolderScanner(string folderName)
        {
            DefaultIDs = new List<int>();
            
            // Retrieve the file names in the folder
            string[] imageFiles = Directory.GetFiles(folderName, "*.*", SearchOption.TopDirectoryOnly)
                                           .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png"))
                                           .ToArray();

            // Parse the file names and store the integers in a HashSet
            foreach (string fileName in imageFiles)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                if (int.TryParse(fileNameWithoutExtension, out int result))
                {
                    DefaultIDs.Add(result);
                }
            }
            DefaultIDs.Sort();
        }
    }
}
