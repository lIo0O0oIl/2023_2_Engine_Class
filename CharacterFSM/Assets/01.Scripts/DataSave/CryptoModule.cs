using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class CryptoModule
{
    private static readonly string _PASSWORD = "GGMisGreatestOfAllTime1234abcdefgqwnjebaslwqe12"; //�ּ� 32���ڴ� �Ǿ�� ��

    private byte[] _saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
    public string AESEncrypt256(string plainText)
    {
        try
        {
            //��ȣȭ�� ������ �ִ´�.
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(_PASSWORD, _saltBytes);
            Aes myAES = Aes.Create();
            myAES.Mode = CipherMode.CBC;
            myAES.Padding = PaddingMode.PKCS7;
            myAES.KeySize = 256;
            myAES.BlockSize = 128;

            myAES.Key = key.GetBytes(myAES.KeySize / 8);
            myAES.IV = key.GetBytes(myAES.BlockSize / 8);

            MemoryStream memoryStream = new MemoryStream();

            //��ũ���͸� ������ְ� �ش���ũ���Ϳ� �ۼ��� Ű�� IV�� �ִ´�. 
            ICryptoTransform encryptor = myAES.CreateEncryptor(myAES.Key, myAES.IV);

            //��ȣȭ ��Ʈ���� �޸𸮽�Ʈ���� ��ũ���͸� �־��ְ� ���ش�.
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            cryptoStream.FlushFinalBlock();

            //������ �޸𸮸� ����Ʈ ��̷� �ް�
            byte[] encryptBytes = memoryStream.ToArray();
            //�޾ƿ� ����Ʈ ��̸� base64�� ���ڵ��Ѵ�.
            string encryptString = Convert.ToBase64String(encryptBytes);

            cryptoStream.Close();
            memoryStream.Close();

            return encryptString;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public string Decrypt(string cryptedText)
    {
        try
        {
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(_PASSWORD, _saltBytes);
            Aes myAES = Aes.Create();
            myAES.Mode = CipherMode.CBC;
            myAES.Padding = PaddingMode.PKCS7;
            myAES.KeySize = 256;
            myAES.BlockSize = 128;

            myAES.Key = key.GetBytes(myAES.KeySize / 8);
            myAES.IV = key.GetBytes(myAES.BlockSize / 8);

            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform decryptor = myAES.CreateDecryptor();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write);

            byte[] crytedBytes = Convert.FromBase64String(cryptedText); //base64���ڵ��ؼ� ��ȣȭ�� ���� �������� 
            cryptoStream.Write(crytedBytes, 0, crytedBytes.Length); //�ؼ��⸦ �����ؼ� �޸𸮿� �ؼ��ؼ� ���ְ�
            cryptoStream.FlushFinalBlock(); //���ۿ� �ִ°� �о��

            byte[] buffer = memoryStream.ToArray();
            string Output = Encoding.UTF8.GetString(buffer); //�ؼ��� ���ۿ� �ִ� �������������� �ٲ� ���� 
            return Output;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}