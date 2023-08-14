using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTools.IO;
public class Paths
{
    static readonly HashSet<char> _allowedChars = new HashSet<char>
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        ' ', '.', '-', ',', '&', '(', ')', '_'
    };


    public static string RemoveInvalidChars(string name)
    {
        StringBuilder nameBuilder = new();
        foreach (char character in name)
        {
            if (_allowedChars.Contains(character))
            {
                nameBuilder.Append(character);
            }
        }

        return nameBuilder.ToString();
    }



    public static bool IsValid(string name)
    {
        foreach (char character in name)
        {
            if (!_allowedChars.Contains(character))
            {
                return false;
            }
        }

        return true;
    }

    public static string GenerateRandomName(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        StringBuilder builder = new StringBuilder();

        Random random = new Random();
        for (int i = 0; i < length; i++)
        {
            int index = random.Next(chars.Length);
            builder.Append(chars[index]);
        }

        return builder.ToString();
    }


}
