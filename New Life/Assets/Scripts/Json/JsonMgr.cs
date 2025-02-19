using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public enum JsonType
{
    JsonUtlity,
    LitJson
}
/// <summary>
/// Json���ݹ�������Ҫ���ڽ��� Json�����л��洢��Ӳ�� �� �����л���Ӳ���ж�ȡ���ڴ���
/// </summary>
public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;
    private JsonMgr() { }

    //�洢json���� ���л�
    public void SavaData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        //ȷ�ϴ洢·��
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        //���л� �õ�json�ַ���
        string jsonStr = "";
        switch (type)
        {
            case JsonType.JsonUtlity:
                jsonStr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(data);
                break;
        }
        //�����л���Json�ַ��� �洢��ָ��·�����ļ���
        File.WriteAllText(path, jsonStr);
    }

    //��ȡָ���ļ��е� Json���� �����л�
    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        //ȷ�����ĸ�·����ȡ
        //�������ж� Ĭ�������ļ������Ƿ���������Ҫ������ ����� �ʹ��л�ȡ
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        //���ж� �Ƿ��������ļ�
        //���������Ĭ���ļ� �ʹ� ��д�ļ�����ȥѰ��
        if (!File.Exists(path))
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";
        }
        Debug.Log(path);
        //�����д�ļ����ж���û�� �Ǿͷ���һ��Ĭ�϶���
        if (!File.Exists(path))
            return new T();

        string jsonStr = File.ReadAllText(path);
        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(jsonStr);
                break;
        }
        return data;
    }
}