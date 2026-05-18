using Core.Data;
Console.WriteLine("Hello, World!");
DataBase.Initialize();
Console.WriteLine("To sign up follow instructions.");
Console.WriteLine("Enter your details:");
string? name;
do
{
    Console.Write("UserName: *");
    name = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(name))
        Console.WriteLine("❌ Username cannot be empty. Please enter a valid username.");
} while (string.IsNullOrWhiteSpace(name));
string? password;
do
{
    Console.Write("Password: *");
    password = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(password))
        Console.WriteLine("❌ Password cannot be empty. Please enter a valid password.");
} while (string.IsNullOrWhiteSpace(password));
Console.Write("Email (optional):");
string? email = Console.ReadLine();
if (string.IsNullOrWhiteSpace(email))
    email = null;
bool success = DataBase.SignUp(name, password, email);
Console.WriteLine(success
    ? "✅ User created successfully!"
    : "❌ Username already exists or an error occurred.");