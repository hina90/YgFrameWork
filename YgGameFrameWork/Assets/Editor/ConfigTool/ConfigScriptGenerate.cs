using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ConfigScriptGenerate
{
    private static string GENERATE_SCRIPT_PATH = Application.dataPath + "/Scripts/Config/GanerateScripts/";
    private static string EDITOR_PATH = Application.dataPath + "/Editor/ConfigTool";
    private static string TEMPLATE_IDATABASE_PATH = "Assets/Editor/ConfigTool/Template_IDatabase.txt";
    private static string TEMPLATE_DATABASE_PATH = "Assets/Editor/ConfigTool/Template_ConfigData.txt";
    private static string TEMPLATE_DATABASEMANAGER_PATH = "Assets/Editor/ConfigTool/Template_ConfigManager.txt";
    private static string CONFIG_PATH = Application.dataPath + "/Resources/Config/";

    private static int DATA_ID;
    private static string REGISTER_LIST;
    private static string CONVERT_LIST;

    [MenuItem("Tools/Config/Gen_Script")]
    public static void GenerateScript()
    {
        Init();
        CreateIDatabaseScript();
        CreateDatabaseScript();
        CreateDatabaseManagerScript();

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 数据初始化
    /// </summary>
    private static void Init()
    {
        DATA_ID = 0;
        REGISTER_LIST = string.Empty;
        CONVERT_LIST = string.Empty;

        if (Directory.Exists(GENERATE_SCRIPT_PATH))
        {
            Directory.Delete(GENERATE_SCRIPT_PATH, true);
        }
        Directory.CreateDirectory(GENERATE_SCRIPT_PATH);
    }

    /// <summary>
    /// 获取模板文件
    /// </summary>
    private static string GetTemplateFile(string path)
    {
        TextAsset textAsset = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
        return textAsset.text;
    }

    /// <summary>
    /// 生成脚本
    /// </summary>
    private static void GenerateScript(string dataName, string data)
    {
        dataName = GENERATE_SCRIPT_PATH + dataName + ".cs";

        if (File.Exists(dataName))
            File.Delete(dataName);

        StreamWriter sw = File.CreateText(dataName);
        sw.WriteLine(data);
        sw.Close();
    }

    /// <summary>
    /// 生成IDatabase模板文件
    /// </summary>
    private static void CreateIDatabaseScript()
    {
        string template = GetTemplateFile(TEMPLATE_IDATABASE_PATH);
        GenerateScript("IDatabase", template);
    }

    /// <summary>
    /// 读取配置表CSV文件
    /// </summary>
    private static void CreateDatabaseScript()
    {
        string[] configPaths = Directory.GetFiles(CONFIG_PATH, "*.csv", SearchOption.AllDirectories);
        string assetPath = "";

        TextAsset textAsset = null;

        for (int i = 0; i < configPaths.Length; i++)
        {
            assetPath = "Assets" + configPaths[i].Replace(Application.dataPath, "").Replace("\\", "/");
            textAsset = (TextAsset)AssetDatabase.LoadAssetAtPath(assetPath, typeof(TextAsset));
            REGISTER_LIST += string.Format("RegisterDataType(new {0}Database());\n", textAsset.name);

            if (i != configPaths.Length - 1)
            {
                REGISTER_LIST += "\t\t\t";
                CONVERT_LIST += string.Format("CsvToJsonConverter.Convert<{0}Database>(\"{0}\");\n", textAsset.name);

                if (i != configPaths.Length - 1)
                {
                    CONVERT_LIST += "\t\t\t";
                }
            }
            EditorUtility.DisplayProgressBar("Analyse Csv Progress", $"Analyse {textAsset.name}.csv to {textAsset.name}.cs", i / configPaths.Length);
            CreateDatabaseScript(textAsset);
        }
        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 替换模板类的标注关键词
    /// </summary>
    /// <param name="textAsset"></param>
    private static void CreateDatabaseScript(TextAsset textAsset)
    {
        DATA_ID++;
        string template = GetTemplateFile(TEMPLATE_DATABASE_PATH);
        template = template.Replace("$DataClassName", textAsset.name + "Data");
        template = template.Replace("$DataAttributes", GetClassParmeters(textAsset));
        template = template.Replace("$CsvSerialize", GetCsvSerialize(textAsset));
        template = template.Replace("$DataKey", GetCsvKey(textAsset));
        template = template.Replace("$DataTypeName", textAsset.name + "Database");
        template = template.Replace("$DataID", DATA_ID.ToString());
        template = template.Replace("$DataPath", "\"Config/" + textAsset.name + "\"");

        GenerateScript(textAsset.name + "Database", template);
    }

    /// <summary>
    /// 获取配置文件属性
    /// </summary>
    /// <param name="textAsset"></param>
    /// <returns></returns>
    private static string GetClassParmeters(TextAsset textAsset)
    {
        string[] csvNote = CSVConverter.SerializeCSVNote(textAsset);
        string[] csvType = CSVConverter.SerializeCSVType(textAsset);
        string[] csvParameter = CSVConverter.SerializeCSVParameter(textAsset);
        int keyCount = csvParameter.Length;

        string classParamers = string.Empty;

        for (int i = 0; i < keyCount; i++)
        {
            classParamers += string.Format("/// <summary>\n\t\t///{0}\n\t\t/// </summary>\n", csvNote[i]);
            classParamers += string.Format("\t\tpublic {0} {1};", csvType[i], csvParameter[i]);

            if (i != keyCount - 1)
            {
                classParamers += "\n";
                classParamers += "\t\t";
            }
        }

        return classParamers;
    }

    private static string GetCsvKey(TextAsset textAsset)
    {
        string[] csvType = CSVConverter.SerializeCSVType(textAsset);
        string[] csvParameter = CSVConverter.SerializeCSVParameter(textAsset);

        if (csvType[0] == "string")
        {
            return string.Format("temp => temp.{0} == key", csvParameter[0]);
        }
        return string.Format("temp => temp.{0} == {1}.Parse(key)", csvParameter[0], csvType[0]);
    }

    private static string GetCsvSerialize(TextAsset textAsset)
    {
        string[] csvType = CSVConverter.SerializeCSVType(textAsset);
        string[] csvParameter = CSVConverter.SerializeCSVParameter(textAsset);

        int keyCount = csvParameter.Length;

        string csvSerialize = string.Empty;
        for (int i = 0; i < keyCount; i++)
        {
            string[] attributes = new string[] { csvType[i], csvParameter[i] };

            if (attributes[0] == "string")
            {
                csvSerialize += string.Format("m_tempData.{0}=m_datas[i][{1}];", attributes[1], i);
            }
            else if (attributes[0] == "bool")
            {
                csvSerialize += GetCsvSerialize(attributes, i, "0");
            }
            else if (attributes[0] == "int")
            {
                csvSerialize += GetCsvSerialize(attributes, i, "0");
            }
            else if (attributes[0] == "float")
            {
                csvSerialize += GetCsvSerialize(attributes, i, "0.0f");
            }
            else if (attributes[0] == "string[]")
            {
                csvSerialize += string.Format("m_tempData.{0}=CSVConverter.ConvertToArray<string>(m_datas[i][{1}].Trim());", attributes[1], i);
            }
            else if (attributes[0] == "bool[]")
            {
                csvSerialize += string.Format("m_tempData.{0}=CSVConverter.ConvertToArray<bool>(m_datas[i][{1}].Trim());", attributes[1], i);
            }
            else if (attributes[0] == "int[]")
            {
                csvSerialize += string.Format("m_tempData.{0}=CSVConverter.ConvertToArray<int>(m_datas[i][{1}].Trim());", attributes[1], i);
            }
            else if (attributes[0] == "float[]")
            {
                csvSerialize += string.Format("m_tempData.{0}=CSVConverter.ConvertToArray<float>(m_datas[i][{1}].Trim());", attributes[1], i);
            }

            if (i != keyCount - 1)
            {
                csvSerialize += "\n";
                csvSerialize += "\t\t\t\t\t";
            }
        }
        return csvSerialize;
    }

    private static string GetCsvSerialize(string[] attributes, int index, string value)
    {

        string csvSerialize = "";
        csvSerialize += string.Format("\n\t\t\t\tif (!{0}.TryParse(m_datas[i][{1}].Trim(),out m_tempData.{2}))\n", attributes[0], index, attributes[1]);

        csvSerialize += "\t\t\t\t{\n";
        csvSerialize += string.Format("\t\t\t\t\tm_tempData.{0}={1};\n", attributes[1], value);
        csvSerialize += "\t\t\t\t}\n";

        return csvSerialize;
    }

    /// <summary>
    /// 创建配置管理模板
    /// </summary>
    private static void CreateDatabaseManagerScript()
    {
        string template = GetTemplateFile(TEMPLATE_DATABASEMANAGER_PATH);
        template = template.Replace("$RegisterList", REGISTER_LIST);
        GenerateScript("ConfigDataManager", template);
    }
}
