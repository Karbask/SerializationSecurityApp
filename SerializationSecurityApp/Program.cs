using System.Text.Json;
using System.Security.Cryptography;
using System.Text;  

public class Program
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string GenerateHash() // Method to generate a hash of the password
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(Password));
                return Convert.ToBase64String(bytes);
               
            }
        }

        public void EncryptData() // Method to encrypt the password (for demonstration purposes, using Base64 encoding)
        {
            Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(Password));
        }   
    }

    public static User DeserializeUserData(string jsonData,bool isTrustedSource)
    {
        if (!isTrustedSource)// Check if the source of the data is trusted before deserialization
        {
            Console.WriteLine("Deserialization blocked: Untrusted source.");
            return null;
        }
        return JsonSerializer.Deserialize<User>(jsonData); // Safe deserialization
    }

    public static string SerializeUserdata(User user)
    {
        if(string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            Console.WriteLine("Serialization blocked: User data is incomplete.");
            return string.Empty;
        }
        user.EncryptData(); // Encrypt the password before serialization
        return JsonSerializer.Serialize(user); // Safe serialization
    }

    public static void Main(string[] args)
    {
        User user = new User
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "SecurePassword123"
        };

        string generatedHash = user.GenerateHash(); // Generate a hash of the password

        string serializedData = SerializeUserdata(user);
        User deserializedUser = DeserializeUserData(serializedData, true); // Assuming the source is trusted
        Console.WriteLine("Serialized User Data: " + generatedHash); // Output the generated hash instead of the actual password
    }
}
