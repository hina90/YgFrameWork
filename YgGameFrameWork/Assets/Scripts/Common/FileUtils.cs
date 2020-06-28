using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class FileUtils : Singleton<FileUtils>
{
    /// <summary>
    /// 获取可写目录路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string GetWritePath(string path)
    {
        StringBuilder sb = new StringBuilder();
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            sb.Append(Application.streamingAssetsPath).Append("/").Append(path);
        }
        else
        {
            sb.Append(Application.persistentDataPath).Append("/").Append(path);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 获取打包资源路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string GetResPath(string path)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(Application.persistentDataPath).Append("/Resources/").Append(path);

        return sb.ToString();
    }

    /// <summary>
    /// 删除指定目录下所有文件
    /// </summary>
    /// <param name="path"></param>
    public bool DeleteAllFile(string fullPath)
    {
        //获取指定路径下面的所有资源文件  然后进行删除
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                string FilePath = fullPath + "/" + files[i].Name;
                File.Delete(FilePath);
            }
            return true;
        }
        return false;
    }
}
