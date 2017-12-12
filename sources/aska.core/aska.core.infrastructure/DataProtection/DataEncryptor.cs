namespace aska.core.infrastructure.DataProtection
{
    internal class DataEncryptor<T> : IDataEncryptor<T> where T: class, new()
    {
        public string Encrypt(T entity, string passPhrase)
        {
            var json = JsonConvert.SerializeObject(entity);
            return StringCipher.Encrypt(json, passPhrase);
        }

        public T Decrypt(string cipherText, string passPhrase)
        {
            var json = StringCipher.Decrypt(cipherText, passPhrase);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}