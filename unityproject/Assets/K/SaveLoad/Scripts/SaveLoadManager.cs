using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class SaveLoadManager
{
    private static SaveLoadManager instance;
    public static SaveLoadManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SaveLoadManager();
            return instance;
        }
    }

    public void Save(in string path, in string key, in string content, Risks risk)
    {
        try
        {
            File.WriteAllText(path, Encrypt(content, key));
        }
        catch
        {
            SendMessage(path, key, risk, true); ;
        }
    }

    public string Load(in string path, in string key, Risks risk)
    {
        try
        {
            return Decrypt(File.ReadAllText(path), key);
        }
        catch
        {
            SendMessage(path, key, risk, false);
            return "";
        }
    }

    private void SendMessage(in string tab, in string key, in Risks risk, bool save)
    {
        string stateStr = save ? "저장하지" : "불러오지";
        switch (risk)
        {
            case Risks.None:
                Debug.Log($"{tab} 파일을 {stateStr} 못했습니다.");
                break;
            case Risks.Warning:
                Debug.LogWarning($"{tab} 파일을 {stateStr} 못했습니다.");
                break;
            case Risks.Error:
                Debug.LogError($"{tab} 파일을 {stateStr} 못했습니다.");
                break;
        }
    }

    public enum Risks
    {
        None,
        Warning,
        Error
    }

    ///////////////////////////////////////////////////////////////////////////////////
    // 복사붙이기한 코드. 자료 주소 => https://intro0517.tistory.com/37
    // 복호화
    private string Decrypt(string textToDecrypt, string key)

    {

        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;

        rijndaelCipher.Padding = PaddingMode.PKCS7;



        rijndaelCipher.KeySize = 128;

        rijndaelCipher.BlockSize = 128;

        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

        byte[] keyBytes = new byte[16];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)

        {

            len = keyBytes.Length;

        }

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;

        rijndaelCipher.IV = keyBytes;

        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

        return Encoding.UTF8.GetString(plainText);

    }

    // 암호화
    private string Encrypt(string textToEncrypt, string key)

    {

        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;

        rijndaelCipher.Padding = PaddingMode.PKCS7;



        rijndaelCipher.KeySize = 128;

        rijndaelCipher.BlockSize = 128;

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

        byte[] keyBytes = new byte[16];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)

        {

            len = keyBytes.Length;

        }

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;

        rijndaelCipher.IV = keyBytes;

        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));

    }
    ///////////////////////////////////////////////////////////////////////////////////
}
