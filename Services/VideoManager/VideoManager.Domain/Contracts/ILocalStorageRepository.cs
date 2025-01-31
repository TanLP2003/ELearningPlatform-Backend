using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoManager.Domain.Contracts;

public interface ILocalStorageRepository
{
    string CreateUserUploadFolderIfNotExist(string storateAbsolutePath, Guid UserId);
    string CreateLectureFolderIfNotExist(string userUploadFolder, Guid lectureId);
    string CreateVideosFolderForLecture(string lectureFolderPath);
    string CreateResourceFolderForLecture(string lectureFolderPath);
}