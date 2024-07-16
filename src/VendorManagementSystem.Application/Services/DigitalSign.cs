using Microsoft.AspNetCore.Http;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;

namespace VendorManagementSystem.Application.Services
{
    public class DigitalSign : IDigitalSign
    {
       /* private readonly IKeyVaultService _keyVaultService;
        public DigitalSign(IKeyVaultService keyVaultService)
        {
            _keyVaultService = keyVaultService;
        }*/
        private (RSAParameters publicKey, RSAParameters privateKey) GetKeys()
        {

            /*var strPublicKey = _keyVaultService.GetSecret("RSAPublicKey");
            var strPrivateKey = _keyVaultService.GetSecret("RSAPrivateKey");
            RSAParameters publicKey = ImportKeyFromXML(strPublicKey.Data, false);
            RSAParameters privateKey = ImportKeyFromXML(strPrivateKey.Data, true);*/
            RSAParameters publicKey  = ImportKeyFromXML("<RSAKeyValue><Modulus>wwaQuDda8bdwY1JyRbi1ffZyZ7jlWNKhbkMCUHOWUYHNdu3Mqgyp989Gws0DWGVZv/O52tfLNoOm4o+iAQME06+8RtnDC45ED+bJpdUcAg4ZHf5EIQl+DLfTKW/jIV+1fNK3epHTPfs2YSGBqBST01NMKUaU22l+cc3fm7jp+TS33gAcemhroXjXcp2iLBfIZc0N5n7XxmiWWPLd2bZGQWyRKy6c3meOUq/x/zYJGEn/x+k/YUFIt5YMTh3QJmhs4iJiQWmYJcJdrsZFMw3ZXD8zoouFfk6jJ/QY8p3UXew59ICejHVwV5gBpHPVjCQhBa+dTOhkqzGdQYWpPtxPLQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", false);
            RSAParameters privateKey = ImportKeyFromXML("<RSAKeyValue><Modulus>wwaQuDda8bdwY1JyRbi1ffZyZ7jlWNKhbkMCUHOWUYHNdu3Mqgyp989Gws0DWGVZv/O52tfLNoOm4o+iAQME06+8RtnDC45ED+bJpdUcAg4ZHf5EIQl+DLfTKW/jIV+1fNK3epHTPfs2YSGBqBST01NMKUaU22l+cc3fm7jp+TS33gAcemhroXjXcp2iLBfIZc0N5n7XxmiWWPLd2bZGQWyRKy6c3meOUq/x/zYJGEn/x+k/YUFIt5YMTh3QJmhs4iJiQWmYJcJdrsZFMw3ZXD8zoouFfk6jJ/QY8p3UXew59ICejHVwV5gBpHPVjCQhBa+dTOhkqzGdQYWpPtxPLQ==</Modulus><Exponent>AQAB</Exponent><P>5g8XU7Smc1MyrA3BklhM5Ya5FTXDyFosEjPu5EqOevTjN7NNFJATJD8/LK9Nigs0XA7gU9OIcGiZlxMgPS0hdmkB+mR0i8Y7+hBACtvhCklxdTumh7G7L+uTQFTVyw++im6gkKZHdC2FST82XTIs3LR3p+Sgbp34QRJhLY8iCWs=</P><Q>2QQy3WJLaAC3V0m+nmyAmwvBYk7+Fy5DOIIrlukug/9M0RaymOZmi3MgmEoPCieppK2Lyxa+jtIsq41yvTRpYX1PRW2QK36cBLfyVJ2Sy7ESC+dAfRFx+UB8P5ju8tYFQ/aeeu+/aZLocG4SNuEgMHfJw0Mt0C24Sd6VPdVjN8c=</Q><DP>xJEIYWDtB4SCuzdVuXDw4vxlj5XMnpdNKJBvAWgirTQoICN3LKaddE8F72wpWFWSe0XKrlUDMuhsswSIezzgZof6RLMoUXUjMdpInf6ZHWz7ICvDchWN1rf1rPXPZh0htK4pSu6IBuAODjOQg7inVDxuMGnMGjenMhI+LesqLdk=</DP><DQ>ge8woTqEGY4sN9gQiHxAeBSOwdS375skZkYR4TZWQnPuQ1ZKsp9okF/rCSN2Y8chnFbIV12T1KFkW4bCRySFX/iOKOyToGca9PxJ3H3H8atgOb5I71ktm2YPvmhL0RtxZaLYepTnT0fFYeOVhkQ+aKagKAgl+voU2C2rv6zuzes=</DQ><InverseQ>23rl47VIR5PoE8yj4k1m4Yytgrb+PQK1EY6nbdOtR3UkBvrKbzuSqK2oDYCKBwfHiO25Lu5QvnNM3KRRLDv0Uh4q8b/CEcTMCEHbNX1OQvJ9eBM7KnW46kPooFKH34K/+3Iq5uvwX73Ze7P/Y4QqcstwkQpDdmAR5Rbi38HsGkQ=</InverseQ><D>XxeNavU6t89c0mD57PsCK5gF8oMZ+PRC2DN+JmIlsjpbR5jtiSDBo14Cv6sc7XYFi+23+nfr0vngXmnwcRPav6jZYaZ+Tt0gXKyIN/6wOGRNZO18pKsvNd5P7M1sbvJu2J0Z7365BGi//B5NTPozlf/wi9Opc05949mhJRnFo8dxvc10J7xA4kA5Nbtmdxml6elEAAYW+Ijck1t0xj7EwGk6ikBLZV57halyOn7sW40L/7sUIE8rk9rxGFgwoVv1b89qPexvJdvBSCZnBdeNvJK7lzdLuXK6OtMctVtwdEgpIeG0WcpSWbi4/FCwDM66E/OCKcsVGmSh3HvridGWtQ==</D></RSAKeyValue>", true);


            return (publicKey, privateKey);

            /*using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                RSAParameters publicKey = rsa.ExportParameters(false);
                RSAParameters privateKey = rsa.ExportParameters(true);
                return (publicKey, privateKey);
            }*/

        }

        public  byte[] SignDocument(string documentContent)
        {
            // compute digest of the data
            byte[] digest = ComputeHash(documentContent);

            (RSAParameters publicKey, RSAParameters privateKey) = GetKeys();


            // sign the digest using senders private key.
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.ImportParameters(privateKey);
                return rsa.SignData(digest, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        public byte[] SignDocument(byte[] documentContent)
        {
            // compute digest of the data
            byte[] digest = ComputeHash(documentContent);

            (RSAParameters publicKey, RSAParameters privateKey) = GetKeys();
            
            

            /*string pub = ExportKeyToXml(publicKey, false);
            string pri = ExportKeyToXml(privateKey, true);*/
            
            // sign the digest using senders private key.
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.ImportParameters(privateKey);
                return rsa.SignData(documentContent, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }


        public async Task<ApplicationResponseDto<bool>> VerifySignature(IFormFile document)
        {
            var response = new ApplicationResponseDto<bool>();
            try
            {
                if (document == null || document.Length == 0)
                {
                    response.Data = false;
                    response.Error = new Error() { Message = ["no document found"] };
                    return response;
                }
                byte[] documentContent;
                byte[] signature = null;
                using (var stream = new MemoryStream())
                {
                    await document.CopyToAsync(stream);
                    documentContent = stream.ToArray();

                    stream.Position = 0;
                    PdfDocument incomigPdf = PdfReader.Open(stream, PdfDocumentOpenMode.ReadOnly);
                    if (incomigPdf.Info.Elements.TryGetValue("/Signature", out PdfItem signatureItem))
                    {
                        if (signatureItem is PdfString signatureString)
                        { 
                            Console.WriteLine(signatureString.Value);

                            signature = Convert.FromBase64String(signatureString.Value);
                        }


                    }

                }
                if (signature == null)
                {
                    response.Data = false;
                    response.Error = new Error() { Message = ["no signature in the document"] };
                    return response;
                }
                // Compute the hash (digest) of the document content

                //byte[] digest = ComputeHash(documentContent);
                /*var strPublicKey = _keyVaultService.GetSecret("RSAPublicKey");
                RSAParameters publicKey = ImportKeyFromXML(strPublicKey.Data, false);*/
                RSAParameters publicKey = ImportKeyFromXML("<RSAKeyValue><Modulus>wwaQuDda8bdwY1JyRbi1ffZyZ7jlWNKhbkMCUHOWUYHNdu3Mqgyp989Gws0DWGVZv/O52tfLNoOm4o+iAQME06+8RtnDC45ED+bJpdUcAg4ZHf5EIQl+DLfTKW/jIV+1fNK3epHTPfs2YSGBqBST01NMKUaU22l+cc3fm7jp+TS33gAcemhroXjXcp2iLBfIZc0N5n7XxmiWWPLd2bZGQWyRKy6c3meOUq/x/zYJGEn/x+k/YUFIt5YMTh3QJmhs4iJiQWmYJcJdrsZFMw3ZXD8zoouFfk6jJ/QY8p3UXew59ICejHVwV5gBpHPVjCQhBa+dTOhkqzGdQYWpPtxPLQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", false);

                // Verify the signature using the public key
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
                {
                    rsa.ImportParameters(publicKey);
                    response.Data =  rsa.VerifyData(documentContent, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    response.Message = "verification status";
                }
                return response;
            }catch(Exception ex)
            {
                response.Error = new Error()
                {
                    Message = [ex.Message, "exception occured"]
                };
                return response;
            }
        }


        private string ExportKeyToXml(RSAParameters key, bool isPrivate)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(key);
                return rsa.ToXmlString(isPrivate);
            }
        }

        private RSAParameters ImportKeyFromXML(string xmlKey, bool isPrivate)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlKey);
                return rsa.ExportParameters(isPrivate);
            }
        }

        private static byte[] ComputeHash(string content)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));

                return hash;
            }
        }

        private static byte[] ComputeHash(byte[] content)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(content);

                return hash;
            }
        }
    }
}
