namespace NourhanRageb.G06.PL.Helpers
{
    public static class DocumentSettings
    {
        // 1.Upload 
        public static string UploadFile(IFormFile file , string FolderName) // Images
        {
            // 1. Get Location Folder Path

            // string FolderPath = $"C:\\Users\\ABO Ahmed\\source\\repos\\NourhanRageb.G06 Solution\\Company.G01 Solution\\Company.G01.PL\\wwwroot\\Files\\{FolderName}";

            //string FolderPath =  Directory.GetCurrentDirectory() + @"wwwroot\Files"+ FolderName; 

            string FolderPath = Path.Combine(Directory.GetCurrentDirectory() , @"wwwroot\Files" , FolderName);

            // 2. Get FileName Make It Unique 

            //string FileName = $"{Guid.NewGuid()}{file.FileName}";
            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Get File Path --> FolderPath + FileName

            string FilePath = Path.Combine(FolderPath, FileName);

            // 4. Save File as Stream : Data Per Time

            using var FileStream = new FileStream(FilePath, FileMode.Create);

            file.CopyTo(FileStream); 

            return FileName;


        }

        // 2.Delete
        public static void DeleteFile(string fileName , string FolderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", FolderName , fileName);

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
