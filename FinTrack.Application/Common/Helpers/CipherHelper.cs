using System.Security.Cryptography;
using System.Text;

namespace FinTrack.Application.Common.Helpers;

public static class CipherHelper
{
    // Gerçek projede bu key ve iv değerleri appsettings.json içinde gizlenir.
    //todo: burası yorumda belirtildiği gibi appsettings.json'dan okunacak şekilde güncellenmeli.
    // Şimdilik test amaçlı sabit 32 byte'lık bir anahtar ve 16 byte'lık IV tanımlıyoruz.
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("FinTrackSecureKey32BytesLong123!");
    private static readonly byte[] Iv = Encoding.UTF8.GetBytes("FinTrackIV16Byte");

    /// <summary>
    /// İçerideki tamsayı ID'yi dış dünyaya göndermek üzere şifreler.
    /// </summary>
    public static string EncryptId(int id)
    {
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = Iv;

        using MemoryStream ms = new();
        using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        using (StreamWriter sw = new(cs))
        {
            sw.Write(id);
        }

        return Convert.ToBase64String(ms.ToArray())
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('='); // URL uyumlu hale getirmek için temizlik yapıyoruz
    }

    /// <summary>
    /// Dış dünyadan gelen şifreli metni çözüp içerideki tamsayı ID'ye dönüştürür.
    /// </summary>
    public static int DecryptId(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return 0;

        // URL temizliğini geri alıyoruz
        string base64 = cipherText.Replace('-', '+').Replace('_', '/');
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        try
        {
            byte[] buffer = Convert.FromBase64String(base64);

            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = Iv;

            using MemoryStream ms = new(buffer);
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader sr = new(cs);

            string decrypted = sr.ReadToEnd();
            return int.TryParse(decrypted, out int id) ? id : 0;
        }
        catch
        {
            return 0; // Şifre çözülemezse geçersiz ID (0) döner
        }
    }
}