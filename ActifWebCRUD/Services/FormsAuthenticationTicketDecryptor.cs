using ActifWebCRUD.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace ActifWebCRUD.Services
{
    /// <summary>
    /// Servicio para descifrar y deserializar tickets de autenticación de FormsAuthentication.
    /// Compatible con las cookies generadas por ASP.NET Web Forms (PowerDashboard).
    /// </summary>
    public class FormsAuthenticationTicketDecryptor
    {
        private readonly byte[] _decryptionKey;
        private readonly byte[] _validationKey;
        private readonly ILogger<FormsAuthenticationTicketDecryptor> _logger;

        public FormsAuthenticationTicketDecryptor(
            IConfiguration configuration,
            ILogger<FormsAuthenticationTicketDecryptor> logger)
        {
            _logger = logger;

            // Lee las claves desde appsettings.json (deben coincidir con web.config de PowerDashboard)
            var decryptionKeyHex = configuration["MachineKey:DecryptionKey"]
                ?? throw new InvalidOperationException("MachineKey:DecryptionKey no configurada");
            var validationKeyHex = configuration["MachineKey:ValidationKey"]
                ?? throw new InvalidOperationException("MachineKey:ValidationKey no configurada");

            _decryptionKey = HexStringToByteArray(decryptionKeyHex);
            _validationKey = HexStringToByteArray(validationKeyHex);

            _logger.LogInformation("FormsAuthenticationTicketDecryptor inicializado");
            _logger.LogInformation("DecryptionKey: {Length} bytes", _decryptionKey.Length);
            _logger.LogInformation("ValidationKey: {Length} bytes", _validationKey.Length);
        }

        /// <summary>
        /// Descifra un ticket de FormsAuthentication desde el valor de la cookie.
        /// </summary>
        public CustomPrincipalSerializeModel? DecryptTicket(string encryptedTicket)
        {
            if (string.IsNullOrEmpty(encryptedTicket))
            {
                _logger.LogWarning("Ticket vacío recibido");
                return null;
            }

            _logger.LogInformation("=== INICIANDO DESCIFRADO DE TICKET ===");
            _logger.LogInformation("Ticket length: {Length} caracteres", encryptedTicket.Length);
            _logger.LogInformation("Primeros 50 chars: {Chars}",
                encryptedTicket.Length > 50 ? encryptedTicket.Substring(0, 50) : encryptedTicket);

            try
            {
                // Paso 1: Decodificar de Hex a bytes
                byte[] encryptedBytes = HexStringToByteArray(encryptedTicket);
                _logger.LogInformation("Paso 1: Decodificado hex -> {Length} bytes", encryptedBytes.Length);

                // Paso 2: Descifrar
                byte[] decryptedBytes = DecryptTicketBytes(encryptedBytes);
                _logger.LogInformation("Paso 2: Descifrado -> {Length} bytes", decryptedBytes.Length);

                // Paso 3: Parsear el ticket
                var userData = ParseTicket(decryptedBytes);
                _logger.LogInformation("Paso 3: UserData extraído -> {Length} chars", userData?.Length ?? 0);

                if (string.IsNullOrEmpty(userData))
                {
                    _logger.LogWarning("No se pudo extraer UserData del ticket");
                    return null;
                }

                _logger.LogInformation("UserData JSON completo:");
                _logger.LogInformation("{JSON}", userData);

                // Paso 4: Deserializar JSON
                try
                {
                    var model = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(userData);
                    _logger.LogInformation("Paso 4: Modelo deserializado exitosamente - User: {User}", model?.Login);
                    return model;
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "Error al deserializar JSON. UserData: {UserData}", userData);

                    // Intenta limpiar el JSON de caracteres extraños
                    var cleanedJson = CleanJsonString(userData);
                    if (cleanedJson != userData)
                    {
                        _logger.LogInformation("JSON limpiado, reintentando deserialización: {Cleaned}", cleanedJson);
                        try
                        {
                            var model = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(cleanedJson);
                            _logger.LogInformation("Deserialización exitosa con JSON limpiado");
                            return model;
                        }
                        catch (Exception ex2)
                        {
                            _logger.LogError(ex2, "Falló incluso con JSON limpiado");
                        }
                    }

                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR al descifrar ticket");
                return null;
            }
        }

        private byte[] DecryptTicketBytes(byte[] encryptedData)
        {
            _logger.LogInformation("--- Iniciando descifrado AES ---");
            _logger.LogInformation("Datos encriptados: {Length} bytes", encryptedData.Length);

            // Probar diferentes formatos según la configuración de compatibilityMode
            // Framework20SP2 puede usar diferentes layouts

            // INTENTO 1: [HMAC-SHA256 (32 bytes)][IV (16 bytes)][Encrypted Data]
            try
            {
                _logger.LogInformation("INTENTO 1: Formato estándar [HMAC][IV][Data]");
                return DecryptWithFormat1(encryptedData);
            }
            catch (CryptographicException ex1)
            {
                _logger.LogWarning(ex1, "Intento 1 falló, probando formato 2");

                // INTENTO 2: [IV (16 bytes)][HMAC-SHA256 (32 bytes)][Encrypted Data]
                try
                {
                    _logger.LogInformation("INTENTO 2: Formato alternativo [IV][HMAC][Data]");
                    return DecryptWithFormat2(encryptedData);
                }
                catch (CryptographicException ex2)
                {
                    _logger.LogWarning(ex2, "Intento 2 falló, probando formato 3");

                    // INTENTO 3: [IV (16 bytes)][Encrypted Data][HMAC (al final)]
                    try
                    {
                        _logger.LogInformation("INTENTO 3: Formato con HMAC al final [IV][Data][HMAC]");
                        return DecryptWithFormat3(encryptedData);
                    }
                    catch (CryptographicException ex3)
                    {
                        _logger.LogWarning(ex3, "Intento 3 falló, probando formato 4");

                        // INTENTO 4: Sin HMAC, solo [IV][Data]
                        try
                        {
                            _logger.LogInformation("INTENTO 4: Sin HMAC, solo [IV][Data]");
                            return DecryptWithFormat4(encryptedData);
                        }
                        catch (CryptographicException ex4)
                        {
                            _logger.LogError(ex4, "Todos los formatos fallaron - problema con las claves o datos corruptos");
                            throw;
                        }
                    }
                }
            }
        }

        private byte[] DecryptWithFormat1(byte[] encryptedData)
        {
            // Formato: [HMAC-SHA256 (32 bytes)][IV (16 bytes)][Encrypted Data]
            const int hmacLength = 32;
            const int ivLength = 16;

            if (encryptedData.Length < hmacLength + ivLength)
            {
                throw new ArgumentException($"Datos muy cortos: {encryptedData.Length} bytes");
            }

            byte[] hmac = new byte[hmacLength];
            byte[] iv = new byte[ivLength];
            int cipherLength = encryptedData.Length - hmacLength - ivLength;
            byte[] cipherText = new byte[cipherLength];

            Buffer.BlockCopy(encryptedData, 0, hmac, 0, hmacLength);
            Buffer.BlockCopy(encryptedData, hmacLength, iv, 0, ivLength);
            Buffer.BlockCopy(encryptedData, hmacLength + ivLength, cipherText, 0, cipherLength);

            _logger.LogInformation("  HMAC: 32 bytes, IV: {IV}, Cipher: {Cipher} bytes",
                BitConverter.ToString(iv, 0, Math.Min(4, iv.Length)), cipherLength);

            return DecryptAesData(iv, cipherText);
        }

        private byte[] DecryptWithFormat2(byte[] encryptedData)
        {
            // Formato: [IV (16 bytes)][HMAC-SHA256 (32 bytes)][Encrypted Data]
            const int hmacLength = 32;
            const int ivLength = 16;

            if (encryptedData.Length < hmacLength + ivLength)
            {
                throw new ArgumentException($"Datos muy cortos: {encryptedData.Length} bytes");
            }

            byte[] iv = new byte[ivLength];
            byte[] hmac = new byte[hmacLength];
            int cipherLength = encryptedData.Length - hmacLength - ivLength;
            byte[] cipherText = new byte[cipherLength];

            Buffer.BlockCopy(encryptedData, 0, iv, 0, ivLength);
            Buffer.BlockCopy(encryptedData, ivLength, hmac, 0, hmacLength);
            Buffer.BlockCopy(encryptedData, ivLength + hmacLength, cipherText, 0, cipherLength);

            _logger.LogInformation("  IV: {IV}, HMAC: 32 bytes, Cipher: {Cipher} bytes",
                BitConverter.ToString(iv, 0, Math.Min(4, iv.Length)), cipherLength);

            return DecryptAesData(iv, cipherText);
        }

        private byte[] DecryptWithFormat3(byte[] encryptedData)
        {
            // Formato: [IV (16 bytes)][Encrypted Data][HMAC-SHA256 (32 bytes)]
            const int hmacLength = 32;
            const int ivLength = 16;

            if (encryptedData.Length < hmacLength + ivLength)
            {
                throw new ArgumentException($"Datos muy cortos: {encryptedData.Length} bytes");
            }

            byte[] iv = new byte[ivLength];
            int cipherLength = encryptedData.Length - hmacLength - ivLength;
            byte[] cipherText = new byte[cipherLength];
            byte[] hmac = new byte[hmacLength];

            Buffer.BlockCopy(encryptedData, 0, iv, 0, ivLength);
            Buffer.BlockCopy(encryptedData, ivLength, cipherText, 0, cipherLength);
            Buffer.BlockCopy(encryptedData, ivLength + cipherLength, hmac, 0, hmacLength);

            _logger.LogInformation("  IV: {IV}, Cipher: {Cipher} bytes, HMAC: 32 bytes al final",
                BitConverter.ToString(iv, 0, Math.Min(4, iv.Length)), cipherLength);

            return DecryptAesData(iv, cipherText);
        }

        private byte[] DecryptWithFormat4(byte[] encryptedData)
        {
            // Formato simple: [IV (16 bytes)][Encrypted Data] (sin HMAC)
            const int ivLength = 16;

            if (encryptedData.Length < ivLength)
            {
                throw new ArgumentException($"Datos muy cortos: {encryptedData.Length} bytes");
            }

            byte[] iv = new byte[ivLength];
            int cipherLength = encryptedData.Length - ivLength;
            byte[] cipherText = new byte[cipherLength];

            Buffer.BlockCopy(encryptedData, 0, iv, 0, ivLength);
            Buffer.BlockCopy(encryptedData, ivLength, cipherText, 0, cipherLength);

            _logger.LogInformation("  IV: {IV}, Cipher: {Cipher} bytes (sin HMAC)",
                BitConverter.ToString(iv, 0, Math.Min(4, iv.Length)), cipherLength);

            return DecryptAesData(iv, cipherText);
        }

        private byte[] DecryptAesData(byte[] iv, byte[] cipherText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _decryptionKey;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] decrypted = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                    _logger.LogInformation("  Descifrado exitoso: {Length} bytes", decrypted.Length);
                    return decrypted;
                }
            }
        }

        private string? ParseTicket(byte[] ticketData)
        {
            _logger.LogInformation("--- Parseando estructura del ticket ---");

            try
            {
                using (var ms = new MemoryStream(ticketData))
                using (var reader = new BinaryReader(ms, Encoding.UTF8))
                {
                    // Byte 0: Version
                    byte version = reader.ReadByte();
                    _logger.LogInformation("Version: {Version}", version);

                    // Bytes 1-8: Issue date (ticks)
                    long issueTicks = reader.ReadInt64();
                    DateTime issueDate = new DateTime(issueTicks);
                    _logger.LogInformation("Issue Date: {Date}", issueDate);

                    // Byte 9: Spacer
                    reader.ReadByte();

                    // Bytes 10-17: Expiration (ticks)
                    long expirationTicks = reader.ReadInt64();
                    DateTime expiration = new DateTime(expirationTicks);
                    _logger.LogInformation("Expiration: {Date}", expiration);

                    // Byte 18: Is Persistent
                    byte persistentByte = reader.ReadByte();
                    bool isPersistent = persistentByte == 1;
                    _logger.LogInformation("Is Persistent: {Persistent} (byte: {Byte})", isPersistent, persistentByte);

                    // Name (7-bit encoded length + string)
                    int nameLength = Read7BitEncodedInt(reader);
                    string name = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));
                    _logger.LogInformation("Name: '{Name}' ({Length} chars)", name, nameLength);

                    // UserData (7-bit encoded length + string) - ESTE ES EL JSON QUE NECESITAMOS
                    int userDataLength = Read7BitEncodedInt(reader);
                    string userData = Encoding.UTF8.GetString(reader.ReadBytes(userDataLength));
                    _logger.LogInformation("UserData: {Length} chars", userDataLength);

                    // Cookie Path (7-bit encoded length + string)
                    int pathLength = Read7BitEncodedInt(reader);
                    string path = Encoding.UTF8.GetString(reader.ReadBytes(pathLength));
                    _logger.LogInformation("Path: '{Path}'", path);

                    return userData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al parsear ticket estructurado - intentando método alternativo");

                // Método alternativo: buscar JSON directamente
                string dataString = Encoding.UTF8.GetString(ticketData);
                int jsonStart = dataString.IndexOf('{');
                int jsonEnd = dataString.LastIndexOf('}');

                if (jsonStart >= 0 && jsonEnd > jsonStart)
                {
                    string json = dataString.Substring(jsonStart, jsonEnd - jsonStart + 1);
                    _logger.LogInformation("JSON extraído por método alternativo: {Length} chars", json.Length);
                    return json;
                }

                _logger.LogError("No se pudo extraer UserData por ningún método");
                return null;
            }
        }

        private int Read7BitEncodedInt(BinaryReader reader)
        {
            int result = 0;
            int shift = 0;

            while (shift < 35)
            {
                byte b = reader.ReadByte();
                result |= (b & 0x7F) << shift;
                shift += 7;

                if ((b & 0x80) == 0)
                {
                    return result;
                }
            }

            throw new FormatException("Invalid 7-bit encoded integer");
        }

        private static byte[] HexStringToByteArray(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                throw new ArgumentNullException(nameof(hex));
            }

            hex = hex.Replace("-", "").Replace(" ", "");

            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Hex string debe tener un número par de caracteres");
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        private string CleanJsonString(string json)
        {
            if (string.IsNullOrEmpty(json))
                return json;

            // Elimina caracteres nulos y de control
            var cleaned = new StringBuilder();
            foreach (char c in json)
            {
                if (c >= 32 || c == '\t' || c == '\n' || c == '\r')
                {
                    cleaned.Append(c);
                }
            }

            // Trim espacios
            return cleaned.ToString().Trim();
        }
    }
}
