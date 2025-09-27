//using BuildingBlocksClient.Shared.Interfaces;

//namespace Identity.API.Infrastructure.Storage
//{
//    public class FileService : IFileService
//    {
//        public async Task<string> HandleFileUploads(IFormFileCollection files)
//        {
//            List<ImageUpload> uploadResults = new List<ImageUpload>();
//            var filePaths = new List<string>();

//            foreach (var file in files)
//            {
//                var uploadResult = new ImageUpload();

//                var uploadedFileName = file.FileName;
//                var extension = Path.GetExtension(file.FileName);
//                var storedFileName = Guid.NewGuid().ToString() + extension;
//                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", storedFileName);

//                await using FileStream fs = new(filePath, FileMode.Create);
//                await file.CopyToAsync(fs);

//                uploadResult.UploadedFileName = uploadedFileName;
//                uploadResult.StoredFileName = storedFileName;
//                uploadResult.ImagePath = filePath;
//                uploadResult.ContentType = file.ContentType;

//                uploadResults.Add(uploadResult);
//                filePaths.Add(filePath);
//            }

//            var imagePaths = string.Join(",", filePaths);

//            return imagePaths;
//        }

//    }
//}
