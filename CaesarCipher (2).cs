using System.Text;

namespace PersonnelVault.Services
{
    // IMPORTANT (learning note): this is NOT secure encryption, only a simple,
    // educational "obfuscation". In a real system a password would never be stored like this -
    // it would need bcrypt/argon2 based hashing, together with a salt.
    public static class CaesarCipher
    {
        // The encoding function: shifts every letter in the alphabet by "shift".
        public static string Encode(string plainText, int shift)
        {
            return ShiftLetters(plainText, shift);
        }

        // The task asks for a SEPARATE decoding function.
        // It does essentially the same shifting, only in the opposite direction.
        public static string Decode(string encodedText, int shift)
        {
            return ShiftLetters(encodedText, -shift);
        }

        private static string ShiftLetters(string text, int shift)
        {
            var result = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                if (char.IsUpper(c))
                {
                    // '(c - 'A' + shift) % 26' would give the shift, but the % operator in C#
                    // can return a negative number for a negative input, so with the '+ 26) % 26' trick
                    // we make sure the result is always between 0 and 25 (e.g. when decoding with a negative shift).
                    result.Append((char)(((c - 'A' + shift) % 26 + 26) % 26 + 'A'));
                }
                else if (char.IsLower(c))
                {
                    result.Append((char)(((c - 'a' + shift) % 26 + 26) % 26 + 'a'));
                }
                else
                {
                    // Numbers, accented letters, spaces and symbols are left unchanged.
                    result.Append(c);
                }
            }

            return result.ToString();
        }
    }
}
