using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;

namespace PasswordlessAuth.Helpers;

public class RSAEncryption
{
    private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
    private RSAParameters _privateKey;
    private RSAParameters _publicKey;

    public string GetPublicKey()
    {
        var sw = new StringWriter();
        var xs = new XmlSerializer(typeof(RSAParameters));
        xs.Serialize(sw, _publicKey);
        return sw.ToString();
    }

    public string Encrypt(string plainText)
    {
        csp = new RSACryptoServiceProvider();
        csp.ImportParameters(_publicKey);
        var data = Encoding.Unicode.GetBytes(plainText);
        var cipher = csp.Encrypt(data, false);
        return Convert.ToBase64String(cipher);
    }

    public string EncryptWithPublicKey(string plainText, string publicKeyString)
    {
        publicKeyString = "LS0tLS1CRUdJTiBQVUJMSUMgS0VZLS0tLS0KTUlJQklqQU5CZ2txaGtpRzl3MEJBUUVGQUFPQ0FROEFNSUlCQ2dLQ0FRRUF4WWNRT3VLanA0c05RdUttUktFcgovRDJmRnl4RngvWnM3SDhsbTFody9McXZXNmoydTZ5RmRjVlRHb2Q1eW95Wk9vYXQ3bUFTL2ZtRjY5aWFmQ21rCm5PN2UvYlQ1eVRKT1BuT1ZINDNyZWM0cDRnS2VDbEJGdGpxK3dxZmNGQXFNTHNZd0lsL1hZU3lRdURYbWJOeHcKT2xrZWcwb3lhOEt2TjJvWTNrNGQ2U0dSaEoraW5Rd1BMV2Y1RDVqVHdmcjlhUDF5S1pacTMvQ3lSaGRPTzFYTgpwL0JpekJlZFpwY2VKbFdDM3ZlSVRvQmg0Rzh0WEZMOXJiNkxPR1J4LzRTczQvc29DTVlubXVGbjlDT1BNbFhUCjdhbXhBZTlwMUl3QXBKdUU0eHN2elYvejRqYzlkSmJtWFpnaUdVdFdmREsrUkUzWTNnb3NXVm1QQjliejJ2bkEKWlFJREFRQUIKLS0tLS1FTkQgUFVCTElDIEtFWS0tLS0t";

        byte[] pbKeyStringByte = Convert.FromBase64String(publicKeyString);
        publicKeyString = Encoding.UTF8.GetString(pbKeyStringByte);

        // Read the public key using a PemReader
        PemReader pr = new PemReader(new StringReader(publicKeyString));
        AsymmetricKeyParameter publicKey = (AsymmetricKeyParameter)pr.ReadObject();

        // Convert the public key to RSA parameters
        RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKey);

        //// Create a new instance of the RSA class and load the public key
        //using RSA rsa = RSA.Create();
        //rsa.ImportParameters(rsaParams);

        csp.ImportParameters(rsaParams);
        var pbKeyStringBytes = Encoding.Unicode.GetBytes(plainText);
        var cipher = csp.Encrypt(pbKeyStringBytes, false);
        return Convert.ToBase64String(cipher);
    }

    public string Decrypt(string cipherText)
    {
        var dataBytes = Convert.FromBase64String(cipherText);
        csp.ImportParameters(_privateKey);
        var plainText = csp.Decrypt(dataBytes, false);
        return Encoding.Unicode.GetString(plainText);
    }
}