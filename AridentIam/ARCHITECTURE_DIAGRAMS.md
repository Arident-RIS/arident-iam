# 🔐 AridentIAM User Secrets — Visual Architecture

---

## Configuration Loading Flow

```
┌─────────────────────────────────────────────────────────────────┐
│           Application Startup in Development Mode               │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  Program.cs: WebApplication.CreateBuilder(args)                │
│  (Automatically loads User Secrets for Development environment) │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────────────────┐
        │   Configuration Sources (in loading order):         │
        ├─────────────────────────────────────────────────────┤
        │                                                     │
        │  1️⃣  appsettings.json                              │
        │     ├─ Database:Host = ...                         │
        │     ├─ Database:Port = 1433                        │
        │     └─ Other defaults                              │
        │                                                     │
        │  2️⃣  appsettings.Development.json                  │
        │     ├─ Database:Username = null  ← PLACEHOLDER     │
        │     ├─ Database:Password = null  ← PLACEHOLDER     │
        │     ├─ Jwt:SecretKey = ""        ← PLACEHOLDER     │
        │     └─ Cors:AllowedOrigins = []                    │
        │                                                     │
        │  3️⃣  User Secrets ⭐ (HIGHEST PRIORITY!)          │
        │     ├─ Database:Username = "aridentris"  ✅ FOUND! │
        │     ├─ Database:Password = "Ar!dentRI5"  ✅ FOUND! │
        │     └─ Jwt:SecretKey = "N7x9kL2m..."    ✅ FOUND! │
        │                                                     │
        │  4️⃣  Environment Variables (if set)                │
        │                                                     │
        └─────────────────────────────────────────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────────────────┐
        │   Final Configuration (User Secrets Override)      │
        ├─────────────────────────────────────────────────────┤
        │ Database:Username = "aridentris"  ← FROM USER SECRETS
        │ Database:Password = "Ar!dentRI5"  ← FROM USER SECRETS
        │ Jwt:SecretKey = "N7x9kL2m..."     ← FROM USER SECRETS
        └─────────────────────────────────────────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────────────────┐
        │     Application Ready to Connect to Database       │
        │     and Use JWT Token Validation                   │
        └─────────────────────────────────────────────────────┘
```

---

## File Structure & Storage Locations

```
💻 Your Development Machine
├─ E:\IAM\arident-iam (GIT REPOSITORY)
│  │
│  ├─ AridentIam.WebApi
│  │  ├─ appsettings.json              ✅ In Git (safe)
│  │  ├─ appsettings.Development.json  ✅ In Git (safe - null values)
│  │  ├─ appsettings.Local.json        ✅ In Git (safe - null values)
│  │  ├─ appsettings.Production.json   ✅ In Git (safe - no secrets)
│  │  └─ AridentIam.WebApi.csproj      ✅ In Git
│  │
│  ├─ .gitignore
│  └─ .git (repository)
│
└─ C:\Users\{YourUsername}\AppData\Roaming\Microsoft\UserSecrets
   │
   └─ {UserSecretsId} (GUID from .csproj)  ❌ NOT in Git (machine-local)
      │
      └─ secrets.json                      🔐 Your Secrets!
         ├─ "Database:Username": "aridentris"
         ├─ "Database:Password": "Ar!dentRI5"
         └─ "Jwt:SecretKey": "N7x9kL2m..."
```

---

## Data Flow: Where Secrets Come From

```
┌──────────────────────────────────┐
│   appsettings.Development.json   │
│                                  │
│  "Username": null    ─────┐      │
│  "Password": null    ─────┤      │
│  "SecretKey": ""     ─────┤      │
└──────────────────────────────────┘
                               │
                               │ (null/empty - not valid)
                               │
                               ▼
┌──────────────────────────────────────────────────────────────┐
│         Configuration Resolver: "Is Username set?"           │
│                                                              │
│         NO → Check next configuration source                │
└──────────────────────────────────────────────────────────────┘
                               │
                               ▼
┌──────────────────────────────────────────────────────────────┐
│   User Secrets (Local Machine)                               │
│                                                              │
│   "Database:Username": "aridentris"    ◄──── FOUND!         │
│   "Database:Password": "Ar!dentRI5"    ◄──── FOUND!         │
│   "Jwt:SecretKey": "N7x9kL2m..."       ◄──── FOUND!         │
│                                                              │
│   ✅ Configuration resolved successfully!                   │
└──────────────────────────────────────────────────────────────┘
                               │
                               ▼
┌──────────────────────────────────────────────────────────────┐
│        Application (IConfiguration injection)               │
│                                                              │
│  config["Database:Username"]  → "aridentris"  ✅           │
│  config["Database:Password"]  → "Ar!dentRI5"  ✅           │
│  config["Jwt:SecretKey"]      → "N7x9kL2m..."  ✅          │
└──────────────────────────────────────────────────────────────┘
                               │
                               ▼
                    ✅ Database Connected
                    ✅ JWT Validation Ready
```

---

## Security Comparison

```
┌──────────────────────────────────────────────────────────────┐
│                    BEFORE (Insecure)                         │
├──────────────────────────────────────────────────────────────┤
│                                                              │
│  appsettings.Development.json (IN GIT)                      │
│  ├─ Username: "aridentris"      ❌ Everyone sees it         │
│  ├─ Password: "Ar!dentRI5"      ❌ EXPOSED!                 │
│  └─ SecretKey: "REPLACE_WITH..." ❌ In source control       │
│                                                              │
│  ⚠️  Risk: If repo is stolen, passwords are compromised    │
│                                                              │
└──────────────────────────────────────────────────────────────┘

                              UPGRADE

┌──────────────────────────────────────────────────────────────┐
│                    AFTER (Secure)                            │
├──────────────────────────────────────────────────────────────┤
│                                                              │
│  appsettings.Development.json (IN GIT)                      │
│  ├─ Username: null              ✅ Placeholder              │
│  ├─ Password: null              ✅ Placeholder              │
│  └─ SecretKey: ""               ✅ Placeholder              │
│                                                              │
│  secrets.json (NOT IN GIT - Machine Local)                  │
│  ├─ "Database:Username": "aridentris"                       │
│  ├─ "Database:Password": "Ar!dentRI5"                       │
│  └─ "Jwt:SecretKey": "N7x9kL2m..."                          │
│                                                              │
│  ✅ Risk: Repo can be publicly shared safely!              │
│                                                              │
└──────────────────────────────────────────────────────────────┘
```

---

## Environment-Specific Flows

### 👨‍💻 Development (Local Machine)

```
┌─────────────────┐
│ Dev Machine     │
│                 │
│ Env: Development│
│                 │
└────────┬────────┘
         │
         ▼
┌────────────────────────────────┐
│ Program.cs                     │
│ CreateBuilder()                │
│ Automatically loads:           │
│  1. appsettings.json          │
│  2. appsettings.Development   │
│  3. User Secrets ⭐           │
│  4. Env vars                  │
└────────┬───────────────────────┘
         │
         ▼
┌────────────────────────────────┐
│ Reads Credentials From:        │
│ C:\Users\...\UserSecrets\...   │
│ secrets.json                   │
└────────┬───────────────────────┘
         │
         ▼
✅ App runs with correct creds
```

### 🚀 Production (Azure App Service)

```
┌─────────────────┐
│ Azure App Svc   │
│                 │
│ Env: Production │
│                 │
└────────┬────────┘
         │
         ▼
┌────────────────────────────────┐
│ Program.cs                     │
│ CreateBuilder()                │
│ Loads:                         │
│  1. appsettings.json          │
│  2. Env variables (from Azure)│
│  3. Key Vault (if configured) │
│                               │
│ ❌ NO User Secrets loaded!    │
│    (Only for Development)     │
└────────┬───────────────────────┘
         │
         ▼
┌────────────────────────────────┐
│ Reads Credentials From:        │
│ Environment Variables          │
│ (Set in App Service settings)  │
│         OR                     │
│ Azure Key Vault                │
└────────┬───────────────────────┘
         │
         ▼
✅ App runs with prod creds
```

---

## Team Collaboration Flow

```
┌─────────────────────────────────────────────────────────────┐
│  GitHub Repository (Public)                                 │
│  ├─ appsettings.Development.json  ← null/empty values      │
│  └─ README with setup instructions                          │
└─────────────────────────────────────────────────────────────┘
                          │
                ┌─────────┼─────────┐
                │         │         │
                ▼         ▼         ▼
         ┌──────────┐ ┌──────────┐ ┌──────────┐
         │ Developer│ │Developer2│ │Developer3│
         │   1      │ │          │ │          │
         │          │ │          │ │          │
         │ Sets own │ │ Sets own │ │ Sets own │
         │ secrets: │ │ secrets: │ │ secrets: │
         │          │ │          │ │          │
         │ secrets. │ │ secrets. │ │ secrets. │
         │ json     │ │ json     │ │ json     │
         │ (Local)  │ │ (Local)  │ │ (Local)  │
         │          │ │          │ │          │
         │ Password │ │ Password │ │ Password │
         │ Manager: │ │ Manager: │ │ Manager: │
         │ Shared   │ │ Shared   │ │ Shared   │
         │ Creds    │ │ Creds    │ │ Creds    │
         └──────────┘ └──────────┘ └──────────┘
```

---

## User Secrets File Structure (JSON)

```json
{
  "Database:Username": "aridentris",
  "Database:Password": "Ar!dentRI5",
  "Jwt:SecretKey": "N7x9kL2mP8qR4sT5uV6wX7yZ9aB1cD2eF3gH4iJ5kL6mN7oP8qR9sT0uV1wX2yZ3a"
}
```

**Located at (Windows):**
```
C:\Users\YourUsername\AppData\Roaming\Microsoft\UserSecrets\{UserSecretsId}\secrets.json
```

**Readable as:**
```powershell
dotnet user-secrets list
# Output:
# Database:Password = Ar!dentRI5
# Database:Username = aridentris
# Jwt:SecretKey = N7x9kL2m...
```

---

## Command Flow

```
┌──────────────────────────────────────────────────────┐
│  You Run: dotnet user-secrets set "Key" "value"     │
└──────────────────────────────────────────────────────┘
                        │
                        ▼
┌──────────────────────────────────────────────────────┐
│  .NET CLI reads: AridentIam.WebApi.csproj           │
│  Finds: <UserSecretsId>12345678-abcd-...</Usr...>   │
└──────────────────────────────────────────────────────┘
                        │
                        ▼
┌──────────────────────────────────────────────────────┐
│  Stores secret in:                                  │
│  ~/Microsoft/UserSecrets/12345678.../secrets.json   │
└──────────────────────────────────────────────────────┘
                        │
                        ▼
┌──────────────────────────────────────────────────────┐
│  When app runs, Config API reads from this file     │
│  and injects values via Dependency Injection        │
└──────────────────────────────────────────────────────┘
                        │
                        ▼
              ✅ Application Works!
```

---

## Summary Diagram

```
┌────────────────────────────────────────────────────────────────┐
│                     APPLICATION STARTUP                        │
└────────────────────────────────────────────────────────────────┘
                              │
                ┌─────────────┴──────────────┐
                │                            │
                ▼                            ▼
        ┌──────────────────┐        ┌──────────────────┐
        │ Config Files     │        │ User Secrets     │
        │ (In Git ✅)      │        │ (Local Only ✅)  │
        │                  │        │                  │
        │ ✅ Safe          │        │ ✅ Secure        │
        │ ✅ Shared        │        │ ✅ Machine Local │
        │ ✅ Default Values│        │ ✅ Per Developer │
        └──────────────────┘        └──────────────────┘
                │                            │
                └─────────────┬──────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │ Merged Config    │
                    │                  │
                    │ Credentials +    │
                    │ Settings         │
                    └─────────┬────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │    Database      │
                    │    Connected ✅  │
                    │                  │
                    │ JWT Validation   │
                    │    Ready ✅      │
                    └──────────────────┘
```

---

**Status:** ✅ Configured  
**Security:** ✅ Enhanced  
**Ready:** ✅ YES
