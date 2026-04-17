# ✅ User Secrets Setup — Complete Guide for AridentIAM

---

## 🎯 What Was Done

Your `appsettings.Development.json` has been **updated** to remove hardcoded credentials:

### ✅ Changes Made

**BEFORE (❌ INSECURE):**
```json
"Username": "aridentris",
"Password": "Ar!dentRI5",
"SecretKey": "REPLACE_WITH_SECURE_256BIT_KEY_IN_PRODUCTION"
```

**AFTER (✅ SECURE):**
```json
"Username": null,
"Password": null,
"SecretKey": ""
```

**Status:** ✅ Already applied to your file!

---

## 🔧 Next Steps: Set Your User Secrets

Follow these steps to store credentials securely on your machine.

### Step 1: Initialize User Secrets (Choose One Method)

#### Method A: Visual Studio GUI (Easiest) ✨
1. Open **Visual Studio 2026**
2. **Right-click** `AridentIam.WebApi` project in Solution Explorer
3. Select **"Manage User Secrets"**
   - A `secrets.json` file opens
   - `UserSecretsId` is automatically added to your `.csproj` file
4. ✅ Done!

#### Method B: PowerShell CLI
```powershell
# Navigate to WebApi directory
cd "E:\IAM\arident-iam\AridentIam\AridentIam.WebApi"

# Initialize user secrets
dotnet user-secrets init
```

---

### Step 2: Store Your Secrets

Open **Package Manager Console** in Visual Studio (or PowerShell from WebApi directory) and run:

#### Store Database Credentials
```powershell
dotnet user-secrets set "Database:Username" "aridentris"
dotnet user-secrets set "Database:Password" "Ar!dentRI5"
```

#### Store JWT Secret (256+ bits required!)

**Generate a secure random key:**
```powershell
# Run this to generate a 256-bit (32-byte) base64 key:
[Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
```

Example output:
```
N7x9kL2mP8qR4sT5uV6wX7yZ9aB1cD2eF3gH4iJ5kL6mN7oP8qR9sT0uV1wX2yZ3a
```

Store it:
```powershell
dotnet user-secrets set "Jwt:SecretKey" "N7x9kL2mP8qR4sT5uV6wX7yZ9aB1cD2eF3gH4iJ5kL6mN7oP8qR9sT0uV1wX2yZ3a"
```

---

### Step 3: Verify Secrets Are Stored

```powershell
dotnet user-secrets list
```

**Expected output:**
```
Database:Username = aridentris
Database:Password = Ar!dentRI5
Jwt:SecretKey = N7x9kL2mP8qR4sT5uV6wX7yZ9aB1cD2eF3gH4iJ5kL6mN7oP8qR9sT0uV1wX2yZ3a
```

---

### Step 4: Run Your Application

```powershell
# Option 1: Visual Studio
Press F5

# Option 2: Command line
dotnet run

# Option 3: PowerShell with explicit environment
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run
```

**Expected result:** App starts without credential errors! ✅

---

## 🔍 How It Works

### Configuration Priority (Reading Order)

When running in **Development** environment, your app reads configuration in this order:

1. **`appsettings.json`** (defaults)
2. **`appsettings.Development.json`** (dev-specific overrides) ← Currently has `null`/`""`
3. **User Secrets** (highest priority) ← Your credentials come from here! ✅
4. **Environment variables** (can override everything)

### Example: Reading Database Password

When the code accesses `configuration["Database:Password"]`:

```
1. Check appsettings.json            → not set
2. Check appsettings.Development    → null (empty)
3. Check User Secrets               → "Ar!dentRI5" ← USES THIS! ✅
4. Check environment variables      → not set
Result: "Ar!dentRI5"
```

### How Program.cs Loads User Secrets

Your existing `Program.cs` already handles this automatically:

```csharp
var builder = WebApplication.CreateBuilder(args);

// ✅ CreateBuilder() automatically includes User Secrets
//    for Development environment (no additional code needed!)
```

**Behind the scenes:**
```csharp
// CreateBuilder internally does this for Development:
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddUserSecrets<Program>()  // ← Automatic!
    .AddEnvironmentVariables();
```

---

## 💾 Where Secrets Are Stored

User Secrets are stored **on your machine**, outside the repository:

### Windows
```
C:\Users\{YourUsername}\AppData\Roaming\Microsoft\UserSecrets\{UserSecretsId}\secrets.json
```

### macOS / Linux
```
~/.microsoft/usersecrets/{UserSecretsId}/secrets.json
```

**Key point:** These paths are **excluded from Git** automatically. ✅

---

## ✅ Verification

### Verify appsettings.Development.json Is Clean

Open the file and confirm secrets are removed:

```json
"Username": null,      // ✅ Not "aridentris"
"Password": null,      // ✅ Not "Ar!dentRI5"
"SecretKey": ""        // ✅ Not the actual key
```

### Verify User Secrets Are Set

```powershell
dotnet user-secrets list
```

Should show all 3 secrets are present.

### Verify App Reads Secrets Correctly

Run the app and check logs for:
```
Database migrations applied successfully
```

If you see database connection errors, the secrets weren't read. See troubleshooting below.

---

## ❌ Troubleshooting

### Error: "JWT SecretKey is not configured"

```
InvalidOperationException: JWT SecretKey is not configured.
```

**Cause:** Secret not set or app not in Development environment.

**Fix:**
```powershell
# Check environment
echo $env:ASPNETCORE_ENVIRONMENT  # Should be "Development"

# Set it if needed
$env:ASPNETCORE_ENVIRONMENT = "Development"

# Check if secret exists
dotnet user-secrets list | Select-String "Jwt:SecretKey"

# If not found, set it
dotnet user-secrets set "Jwt:SecretKey" "your-256-bit-key"
```

---

### Error: "Login failed for user 'aridentris'"

```
SqlException: Login failed for user 'aridentris'
```

**Cause:** Database credentials not in User Secrets.

**Fix:**
```powershell
# Verify they're set
dotnet user-secrets list

# Must show:
# Database:Username = aridentris
# Database:Password = Ar!dentRI5

# If missing:
dotnet user-secrets set "Database:Username" "aridentris"
dotnet user-secrets set "Database:Password" "Ar!dentRI5"
```

---

### Error: "UserSecretsId not found"

```
InvalidOperationException: No UserSecretsId found
```

**Cause:** User Secrets not initialized for this project.

**Fix:**
```powershell
cd "E:\IAM\arident-iam\AridentIam\AridentIam.WebApi"
dotnet user-secrets init
```

Or in Visual Studio: Right-click project → "Manage User Secrets"

---

### Error: "No secrets configured"

```
No secrets configured for this application.
```

**Cause:** Secrets were never set (init was done but set wasn't).

**Fix:**
```powershell
dotnet user-secrets set "Database:Username" "aridentris"
dotnet user-secrets set "Database:Password" "Ar!dentRI5"
dotnet user-secrets set "Jwt:SecretKey" "your-256-bit-key"
```

---

## 🚀 For Team Collaboration

When your team members clone the repo, they'll see:

```
AridentIam.WebApi/
├── appsettings.json
├── appsettings.Development.json    (has null values - safe to commit)
├── appsettings.Local.json
├── appsettings.Production.json
└── ... (NO secrets.json here!)
```

Each team member needs to:

1. Initialize User Secrets in their own environment
2. Set the same secrets with their own (or shared) values
3. The app will work without any issues

**Optional:** Create a `SECRETS_SETUP.md` in the repo documenting what secrets need to be set:

```markdown
# User Secrets Setup

Each developer must set these:

```powershell
dotnet user-secrets set "Database:Username" "aridentris"
dotnet user-secrets set "Database:Password" "..." # Get from team
dotnet user-secrets set "Jwt:SecretKey" "..." # Get from team
```
```

---

## 📋 Checklist

Verify everything is set up:

- [ ] Opened Visual Studio > Right-click `AridentIam.WebApi` > "Manage User Secrets" (or ran `dotnet user-secrets init`)
- [ ] Set `Database:Username` secret
- [ ] Set `Database:Password` secret
- [ ] Set `Jwt:SecretKey` secret (generated 256+ bits)
- [ ] Ran `dotnet user-secrets list` and saw all 3 secrets
- [ ] Verified `appsettings.Development.json` has `null` and `""` (no hardcoded secrets)
- [ ] App starts without credential errors (`F5` or `dotnet run`)
- [ ] Git status shows only config structure changes (no secrets)
- [ ] Ready to commit changes! ✅

---

## 🔐 Security Benefits

| What | Before | After |
|------|--------|-------|
| Secrets in Git | ❌ YES | ✅ NO |
| Team sees passwords | ❌ YES | ✅ NO |
| Risk if repo leaked | ❌ HIGH | ✅ LOW |
| Easy for developers | ✅ YES | ✅ YES (same) |

---

## 📚 For Production

**DO NOT use User Secrets in production!**

For production deployments, use:

### Azure Key Vault (Recommended)
```csharp
// In Program.cs for Production:
if (app.Environment.IsProduction())
{
    var keyVaultUrl = new Uri("https://your-keyvault.vault.azure.net/");
    builder.Configuration.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());
}
```

### Environment Variables (Alternative)
Set in Azure App Service, Docker, etc.:
```
Database:Username=aridentris
Database:Password=secure_value
Jwt:SecretKey=production_256bit_key
```

See: [Azure Key Vault Configuration Provider](https://docs.microsoft.com/aspnet/core/security/key-vault-configuration)

---

## 📞 Quick Reference Commands

```powershell
# List all secrets
dotnet user-secrets list

# Set a secret
dotnet user-secrets set "Key" "value"

# Remove a secret
dotnet user-secrets remove "Key"

# Clear all secrets
dotnet user-secrets clear

# For a specific project
dotnet user-secrets set "Key" "value" --project "path/to/project"
```

---

## ✨ You're All Set!

Your project is now:
- ✅ Secure (no credentials in source control)
- ✅ Developer-friendly (easy to run locally)
- ✅ Team-ready (each dev has their own secrets)
- ✅ Production-ready (separate secret management strategy)

**Next:** Commit your updated `appsettings.Development.json` and inform the team about the User Secrets setup.

---

## 📖 References

- [Safe storage of app secrets in development - ASP.NET Core](https://docs.microsoft.com/aspnet/core/security/app-secrets)
- [Secret Manager tool - ASP.NET Core](https://docs.microsoft.com/aspnet/core/security/app-secrets?view=aspnetcore-8.0#enable-secret-storage)
- [Azure Key Vault Configuration Provider](https://docs.microsoft.com/aspnet/core/security/key-vault-configuration)
- [.NET Microservices: Secure app secrets storage](https://docs.microsoft.com/dotnet/architecture/microservices/secure-net-microservices-web-applications/developer-app-secrets-storage)
