namespace Messenger.Models
{
    using System;

    public static class Crypto
    {
        public static string Encrypt(string message, TransferRequestAlgorithm algorithm, TransferTarget target)
        {
            switch (algorithm)
            {
                case TransferRequestAlgorithm.ClearText:
                    return message;
                default:
                    throw new NotImplementedException("Don't know algorithm: " + algorithm);
            }
        }

        public static string Decrypt(string message, TransferRequestAlgorithm algorithm, string privateKey)
        {
            switch (algorithm)
            {
                case TransferRequestAlgorithm.ClearText:
                    return message;
                default:
                    throw new NotImplementedException("Don't know algorithm: " + algorithm);
            }
        }
    }
}
