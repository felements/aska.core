namespace aska.core.infrastructure.DataProtection
{
    public interface IDataEncryptor<T> where T : class, new()
    {
        string Encrypt(T entity, string passPhrase);
        T Decrypt(string cipherText, string passPhrase);
    }
}