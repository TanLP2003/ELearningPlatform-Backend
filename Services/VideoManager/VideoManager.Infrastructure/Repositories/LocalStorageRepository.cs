using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoManager.Domain.Contracts;

namespace VideoManager.Infrastructure.Repositories;

public class LocalStorageRepository : ILocalStorageRepository
{
    public string CreateLectureFolderIfNotExist(string userUploadFolder, Guid lectureId)
    {
        var lectureFolder = Path.Combine(userUploadFolder, lectureId.ToString());
        if(!Directory.Exists(lectureFolder)) Directory.CreateDirectory(lectureFolder);  
        return lectureFolder;
    }

    public string CreateVideosFolderForLecture(string lectureFolderPath)
    {
        var videoFolderPath = Path.Combine(lectureFolderPath, "video");
        if(!Directory.Exists(videoFolderPath)) Directory.CreateDirectory(videoFolderPath);  
        return videoFolderPath;
    }
    public string CreateResourceFolderForLecture(string lectureFolderPath)
    {
        var resourceFolderPath = Path.Combine(lectureFolderPath, "resource");
        if(!Directory.Exists(resourceFolderPath)) Directory.CreateDirectory(resourceFolderPath);
        return resourceFolderPath;
    }

    public string CreateUserUploadFolderIfNotExist(string storateAbsolutePath, Guid UserId)
    {   
        var userUploadFolder = Path.Combine(storateAbsolutePath, UserId.ToString());
        if(!Directory.Exists(userUploadFolder)) Directory.CreateDirectory(userUploadFolder);
        return userUploadFolder;
    }
}