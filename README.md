# Software.Helper Class Library

The `Software.Helper` class library provides a namespace with utilities for supporting our Software Development

---

## `AppCustomException`

### Namespace
```csharp
namespace Software.Helper
```

### Description
The `AppCustomException` class provides a way to define custom exceptions for your application.

### Implementation
```csharp
public class AppCustomException : Exception
{
    public AppCustomException(string message) : base(message)
    {
    }
}
```

Use this class to throw application-specific errors, improving error handling and debugging.

---

## `PasswordManager`

### Namespace
```csharp
namespace Software.Helper
```

### Description
The `PasswordManager` class provides static methods for password security:
- `HashPassword`: Hashes a plaintext password.
- `VerifyPassword`: Verifies a plaintext password against a stored hash.

### Implementation
#### Constants
- **`SaltSize`**: Size of the salt in bytes (16 bytes = 128 bits).
- **`KeySize`**: Size of the derived key in bytes (32 bytes = 256 bits).
- **`Iterations`**: Number of PBKDF2 iterations (default: 10,000).

### Methods
#### `HashPassword`
```csharp
public static string HashPassword(string password)
```
Hashes a password using PBKDF2 with a randomly generated salt.

- **Parameters**:
  - `password` (string): The plaintext password to hash.
- **Returns**: A base64-encoded string in the format `{iterations}.{salt}.{hash}`.

#### Example
```csharp
string hashedPassword = PasswordManager.HashPassword("MySecurePassword");
Console.WriteLine(hashedPassword);
```

#### `VerifyPassword`
```csharp
public static bool VerifyPassword(string hashedPassword, string providedPassword)
```
Verifies a plaintext password against a stored hashed password.

- **Parameters**:
  - `hashedPassword` (string): The stored hashed password in the format `{iterations}.{salt}.{hash}`.
  - `providedPassword` (string): The plaintext password to verify.
- **Returns**: `true` if the password matches, otherwise `false`.

#### Example
```csharp
bool isValid = PasswordManager.VerifyPassword(hashedPassword, "MySecurePassword");
Console.WriteLine(isValid ? "Password is valid" : "Invalid password");
```

---

## Usage Example
```csharp
using System;
using Software.Helper;

class Program
{
    static void Main()
    {
        try
        {
            string password = "MySecurePassword";

            // Hash the password
            string hashedPassword = PasswordManager.HashPassword(password);
            Console.WriteLine("Hashed Password: " + hashedPassword);

            // Verify the password
            bool isPasswordValid = PasswordManager.VerifyPassword(hashedPassword, password);
            Console.WriteLine(isPasswordValid ? "Password is valid" : "Invalid password");
        }
        catch (AppCustomException ex)
        {
            Console.WriteLine("Custom Exception: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
    }
}
```

---

## Security Notes
1. **PBKDF2 Parameters**: The library uses 10,000 iterations by default, which provides a balance between security and performance. Adjust as needed based on your requirements.
2. **Secure Comparison**: Password verification uses `CryptographicOperations.FixedTimeEquals` to prevent timing attacks.
3. **Storage Format**: The hashed password format (`{iterations}.{salt}.{hash}`) is designed for easy storage and retrieval. Ensure that this value is stored securely (e.g., in a database).

---

## License
This library is license to Dachy Inc Only.

---

## Author
**Team Dachy**

