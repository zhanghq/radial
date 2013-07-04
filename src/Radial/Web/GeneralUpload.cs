using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Radial.Extensions;
using Newtonsoft.Json;

namespace Radial.Web
{
    /// <summary>
    /// Upload settings.
    /// </summary>
    public class UploadSettings
    {
        /// <summary>
        /// Gets or sets the root directory relative path, will point to website root ("/") if not set.
        /// </summary>
        public string RootDirectory { get; set; }

        /// <summary>
        /// Gets or sets the allowed extensions, separated by '|' (eg docx|exe).
        /// </summary>
        public string AllowedExtensions { get; set; }

        /// <summary>
        /// Gets or sets the max file size in KB (less than or equal to 0 means unlimited).
        /// </summary>
        public int MaxFileSize { get; set; }
    }

    /// <summary>
    /// Upload result.
    /// </summary>
    public struct UploadResult
    {
        /// <summary>
        /// Gets or sets the upload state.
        /// </summary>
        [JsonProperty("state")]
        public UploadState State { get; set; }

        /// <summary>
        /// Gets or sets the upload file relative path.
        /// </summary>
        [JsonProperty("file_path")]
        public string FilePath { get; set; }
    }

    /// <summary>
    /// Upload state.
    /// </summary>
    public enum UploadState
    {
        /// <summary>
        /// Unknown error
        /// </summary>
        UnknownError = -9,
        /// <summary>
        /// Permission denied
        /// </summary>
        PermissionDenied = -4,
        /// <summary>
        /// Invalid extension
        /// </summary>
        InvalidExtension = -3,
        /// <summary>
        /// Exceed max size
        /// </summary>
        ExceedMaxSize = -2,
        /// <summary>
        /// Illegal character
        /// </summary>
        IllegalCharacter = -1,
        /// <summary>
        /// Succeed
        /// </summary>
        Succeed = 1
    }

    /// <summary>
    /// General upload.
    /// </summary>
    public class GeneralUpload
    {
        static object SyncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralUpload"/> class.
        /// </summary>
        public GeneralUpload() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralUpload"/> class.
        /// </summary>
        /// <param name="settings">The upload settings.</param>
        public GeneralUpload(UploadSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        protected UploadSettings Settings { get; set; }


        /// <summary>
        /// Gets the HTTP server utility.
        /// </summary>
        protected HttpServerUtility HttpServerUtility
        {
            get
            {
                return HttpContext.Current.Server;
            }
        }

        /// <summary>
        /// Combines the relative path.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns></returns>
        protected string CombineRelativePath(params string[] paths)
        {
            IList<string> levels = new List<string>();

            if (paths != null)
            {
                foreach (string path in paths)
                {
                    if (!string.IsNullOrWhiteSpace(path))
                        levels.Add(path.Replace(@"\", "/").Trim('~', '/', ' '));
                }
            }

            levels.Remove(o => string.IsNullOrWhiteSpace(o));

            return "/" + string.Join("/", levels.ToArray());
        }

        /// <summary>
        /// Prepares the directory.
        /// </summary>
        /// <param name="relativeDirectory">The relative directory.</param>
        protected void PrepareDirectory(string relativeDirectory)
        {
            lock (SyncRoot)
            {
                string rootAP = Settings != null ? HttpServerUtility.MapPath(CombineRelativePath(Settings.RootDirectory)) : SystemSettings.BaseDirectory;

                //create root dir if not exist
                if (!Directory.Exists(rootAP))
                    Directory.CreateDirectory(rootAP);

                if (!string.IsNullOrWhiteSpace(relativeDirectory))
                {
                    string[] relativeDirectorySplit = relativeDirectory.Trim('~').Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string dir in relativeDirectorySplit)
                    {
                        //create relative dir if not exist
                        string dirAP = Path.Combine(rootAP, dir);
                        if (!Directory.Exists(dirAP))
                            Directory.CreateDirectory(dirAP);
                    }
                }

            }
        }

        /// <summary>
        /// Save posted file to the root directory.
        /// </summary>
        /// <param name="postedFile">The http posted file.</param>
        /// <param name="saveAsRandomName">A value indicating whether to save file using random name.</param>
        /// <returns>Upload result.</returns>
        public UploadResult Save(System.Web.HttpPostedFile postedFile, bool saveAsRandomName = false)
        {
            return Save(postedFile, null, saveAsRandomName);
        }

        /// <summary>
        /// Save posted file to the specified directory.
        /// </summary>
        /// <param name="postedFile">The http posted file.</param>
        /// <param name="storedDirectory">The relative directory where file will be stored.</param>
        /// <param name="saveAsRandomName">A value indicating whether to save file using random name.</param>
        /// <returns>Upload result.</returns>
        public UploadResult Save(System.Web.HttpPostedFile postedFile, string storedDirectory, bool saveAsRandomName = false)
        {
            Checker.Parameter(postedFile != null, "posted file can not be null");

            if ((Settings != null && Settings.RootDirectory.Any(o => Path.GetInvalidPathChars().Contains(o)))
                || (!string.IsNullOrWhiteSpace(storedDirectory) && storedDirectory.Any(o => Path.GetInvalidPathChars().Contains(o)))
                || postedFile.FileName.Any(o => Path.GetInvalidFileNameChars().Contains(o)))
                return new UploadResult { State = UploadState.IllegalCharacter };

            if (Settings != null && Settings.MaxFileSize > 0 && postedFile.ContentLength > Settings.MaxFileSize * 1024)
                return new UploadResult { State = UploadState.ExceedMaxSize };

            string fileExt = Path.GetExtension(postedFile.FileName).ToLower();

            string[] allowedExts = new string[] { };
            if (Settings != null && !string.IsNullOrWhiteSpace(Settings.AllowedExtensions))
                allowedExts = Settings.AllowedExtensions.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (allowedExts.Length > 0 && !allowedExts.Any(o => string.Compare(o.Trim('.', ' '), fileExt.Trim('.'), true) == 0))
                return new UploadResult { State = UploadState.InvalidExtension };

            string fileName = (saveAsRandomName ? Path.GetRandomFileName().Remove(8, 1) : Path.GetFileNameWithoutExtension(postedFile.FileName)) + fileExt;


            string fileRelativePath = CombineRelativePath(Settings != null ? Settings.RootDirectory : "/", storedDirectory, fileName);

            string fileAbsolutePath = HttpServerUtility.MapPath(fileRelativePath);

            try
            {
                PrepareDirectory(storedDirectory);
                postedFile.SaveAs(fileAbsolutePath);

                return new UploadResult { State = UploadState.Succeed, FilePath = fileRelativePath };
            }
            catch (Exception ex)
            {
                Logger.Default.Error(ex, "General Upload Error");

                if (ex is UnauthorizedAccessException)
                    return new UploadResult { State = UploadState.PermissionDenied };
            }

            return new UploadResult { State = UploadState.UnknownError };
        }

        /// <summary>
        /// Save posted file to the root directory.
        /// </summary>
        /// <param name="postedFile">The http posted file.</param>
        /// <param name="saveAsRandomName">A value indicating whether to save file using random name.</param>
        /// <returns>Upload result.</returns>
        public UploadResult Save(System.Web.HttpPostedFileBase postedFile, bool saveAsRandomName = false)
        {
            return Save(postedFile, null, saveAsRandomName);
        }

        /// <summary>
        /// Save posted file to the specified directory.
        /// </summary>
        /// <param name="postedFile">The http posted file.</param>
        /// <param name="storedDirectory">The relative directory where file will be stored.</param>
        /// <param name="saveAsRandomName">A value indicating whether to save file using random name.</param>
        /// <returns>Upload result.</returns>
        public UploadResult Save(System.Web.HttpPostedFileBase postedFile, string storedDirectory, bool saveAsRandomName = false)
        {
            Checker.Parameter(postedFile != null, "posted file can not be null");

            if ((Settings != null && Settings.RootDirectory.Any(o => Path.GetInvalidPathChars().Contains(o)))
                || (!string.IsNullOrWhiteSpace(storedDirectory) && storedDirectory.Any(o => Path.GetInvalidPathChars().Contains(o)))
                || postedFile.FileName.Any(o => Path.GetInvalidFileNameChars().Contains(o)))
                return new UploadResult { State = UploadState.IllegalCharacter };

            if (Settings != null && Settings.MaxFileSize > 0 && postedFile.ContentLength > Settings.MaxFileSize * 1024)
                return new UploadResult { State = UploadState.ExceedMaxSize };

            string fileExt = Path.GetExtension(postedFile.FileName).ToLower();

            string[] allowedExts = new string[] { };
            if (Settings != null && !string.IsNullOrWhiteSpace(Settings.AllowedExtensions))
                allowedExts = Settings.AllowedExtensions.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (allowedExts.Length > 0 && !allowedExts.Any(o => string.Compare(o.Trim('.', ' '), fileExt.Trim('.'), true) == 0))
                return new UploadResult { State = UploadState.InvalidExtension };

            string fileName = (saveAsRandomName ? Path.GetRandomFileName().Remove(8, 1) : Path.GetFileNameWithoutExtension(postedFile.FileName)) + fileExt;


            string fileRelativePath = CombineRelativePath(Settings != null ? Settings.RootDirectory : "/", storedDirectory, fileName);

            string fileAbsolutePath = HttpServerUtility.MapPath(fileRelativePath);

            try
            {
                PrepareDirectory(storedDirectory);
                postedFile.SaveAs(fileAbsolutePath);

                return new UploadResult { State = UploadState.Succeed, FilePath = fileRelativePath };
            }
            catch (Exception ex)
            {
                Logger.Default.Error(ex, "General Upload Error");

                if (ex is UnauthorizedAccessException)
                    return new UploadResult { State = UploadState.PermissionDenied };
            }

            return new UploadResult { State = UploadState.UnknownError };
        }


        /// <summary>
        /// Save upload file to the root directory.
        /// </summary>
        /// <param name="uploadFileName">The upload file name (include extension).</param>
        /// <param name="uploadFileBytes">The upload file bytes.</param>
        /// <param name="saveAsRandomName">A value indicating whether to save file using random name.</param>
        /// <returns>Upload result.</returns>
        public UploadResult Save(string uploadFileName, byte[] uploadFileBytes, bool saveAsRandomName = false)
        {
            return Save(uploadFileName, uploadFileBytes, null, saveAsRandomName);
        }

        /// <summary>
        /// Save upload file to the specified directory.
        /// </summary>
        /// <param name="uploadFileName">The upload file name (include extension).</param>
        /// <param name="uploadFileBytes">The upload file bytes.</param>
        /// <param name="storedDirectory">The relative directory.</param>
        /// <param name="saveAsRandomName">A value indicating whether to save file using random name.</param>
        /// <returns>Upload result.</returns>
        public UploadResult Save(string uploadFileName, byte[] uploadFileBytes, string storedDirectory, bool saveAsRandomName = false)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(uploadFileName), "upload file name can not be empty or null");
            Checker.Parameter(uploadFileBytes != null, "upload file bytes can not be null");

            if ((Settings != null && Settings.RootDirectory.Any(o => Path.GetInvalidPathChars().Contains(o)))
                || (!string.IsNullOrWhiteSpace(storedDirectory) && storedDirectory.Any(o => Path.GetInvalidPathChars().Contains(o)))
                || uploadFileName.Any(o => Path.GetInvalidFileNameChars().Contains(o)))
                return new UploadResult { State = UploadState.IllegalCharacter };

            if (Settings != null && Settings.MaxFileSize > 0 && uploadFileBytes.Length > Settings.MaxFileSize * 1024)
                return new UploadResult { State = UploadState.ExceedMaxSize };

            string fileExt = Path.GetExtension(uploadFileName).ToLower();

            string[] allowedExts = new string[] { };
            if (Settings != null && !string.IsNullOrWhiteSpace(Settings.AllowedExtensions))
                allowedExts = Settings.AllowedExtensions.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (allowedExts.Length > 0 && !allowedExts.Any(o => string.Compare(o.Trim('.', ' '), fileExt.Trim('.'), true) == 0))
                return new UploadResult { State = UploadState.InvalidExtension };

            string fileName = (saveAsRandomName ? Path.GetRandomFileName().Remove(8, 1) : Path.GetFileNameWithoutExtension(uploadFileName)) + fileExt;


            string fileRelativePath = CombineRelativePath(Settings != null ? Settings.RootDirectory : "/", storedDirectory, fileName);

            string fileAbsolutePath = HttpServerUtility.MapPath(fileRelativePath);

            try
            {
                PrepareDirectory(storedDirectory);

                File.WriteAllBytes(fileAbsolutePath, uploadFileBytes);

                return new UploadResult { State = UploadState.Succeed, FilePath = fileRelativePath };
            }
            catch (Exception ex)
            {
                Logger.Default.Error(ex, "General Upload Error");

                if (ex is UnauthorizedAccessException)
                    return new UploadResult { State = UploadState.PermissionDenied };
            }

            return new UploadResult { State = UploadState.UnknownError };

        }
    }
}
