using Login_register.Dtos;
using Login_register.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Login_register.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users; //Represents a MongoDB collection (like a SQL table) where documents of type User are stored.
        private readonly IConfiguration _config; //It allows you to read values from appsettings.json, environment variables, Connection strings, JWT secret keys etc.
        public UserService(IConfiguration config) // Injects the IConfiguration dependency into your service.IConfiguration allows you to read values from appsettings.json or environment variables.
        {
            _config = config; // Injects the IConfiguration dependency into your service. IConfiguration allows you to read values from appsettings.json or environment variables.
            var client = new MongoClient(_config["MongoDB:ConnectionString"]); // Creates a new MongoDB client. It gets the MongoDB connection string from appsettings.json using the key "MongoDb"
            var database = client.GetDatabase(_config["MongoDB:Database"]); // Gets the MongoDB database. It uses the database name from appsettings.json using the key "MongoDb:Database".
            _users = database.GetCollection<User>("Users"); // Gets the "Users" collection from the MongoDB database. This is where user documents will be stored.
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return (false, "User already exists.");
            }
            var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = hashed, // Hash the password before storing it
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _users.InsertOneAsync(newUser);
            return (true, "User registered successfully.");
        }

        public async Task<(bool Success, string Message, string Token, User? user)> LoginAsync(LoginDto dto)
        {
            var user = await _users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                return (false, "User not found.", string.Empty, null);
            }
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return (false, "Invalid password.", string.Empty, null);
            }
            // Generate JWT token here (not implemented in this example)
            var token = CreateJwtToken(user); // Placeholder for actual JWT generation logic
            return (true, "Login successful.", token, user);
        }
        private string CreateJwtToken(User user)
        {
            var claims = new[] // Claims are key-value pairs that represent user information and are used to create the JWT token.These values can be extracted from the token later for authorization or personalization.
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
            };

            //Retrieves the secret from appsettings.json via _config["Jwt:Secret"].Converts the secret into a SymmetricSecurityKey. Uses HmacSha256 to sign the token, making it tamper-proof.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Creates a new JWT token with the specified claims, expiration time, and signing credentials.
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            // Serializes the JWT token to a string format that can be sent to the client.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
