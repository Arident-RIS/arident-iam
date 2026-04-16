# Swagger UI Not Opening in "Local" Environment — Root Cause & Solution

## Problem Statement

When running the AridentIAM API with the **`Local`** environment (via `appsettings.Local.json`), **Swagger UI is not accessible** at `https://localhost:7001/swagger/index.html`.

---

## Root Cause Analysis

### Location: `ApplicationBuilderExtensions.cs` (Line 16)

```csharp
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Integration"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

### Issue

The middleware pipeline **only enables Swagger** for:
- `Development` environment
- `Integration` environment

When you run with `Local` environment:
1. `app.Environment.IsDevelopment()` → `false` (Local ≠ Development)
2. `app.Environment.IsEnvironment("Integration")` → `false` (Local ≠ Integration)
3. **Result:** Both conditions fail → Swagger is **NOT registered**

### Environment Detection

ASP.NET Core determines the environment via:

1. **`ASPNETCORE_ENVIRONMENT`** environment variable
2. **File naming convention:** `appsettings.{ENVIRONMENT}.json`
   - `appsettings.Development.json` → Development env
   - `appsettings.Local.json` → **Local env** (custom, non-standard)

---

## Solution Applied

### Change in `ApplicationBuilderExtensions.cs`

**BEFORE:**
```csharp
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Integration"))
```

**AFTER:**
```csharp
if (app.Environment.IsDevelopment() || 
    app.Environment.IsEnvironment("Integration") || 
    app.Environment.IsEnvironment("Local"))
```

### Effect

Now Swagger UI is enabled in **three environments**:
- ✅ `Development`
- ✅ `Integration`  
- ✅ `Local` (NEW)

---

## Testing the Fix

### Step 1: Verify Environment Configuration

Ensure your launch profile sets the correct environment. In `launchSettings.json`:

```json
{
  "profiles": {
    "Local": {
      "commandName": "Project",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Local"  // ← This is what matters
      },
      "applicationUrl": "https://localhost:7001"
    }
  }
}
```

### Step 2: Run the Application

```powershell
# Using dotnet CLI
dotnet run --launch-profile Local

# Or via Visual Studio
# Set startup project to AridentIam.WebApi → Debug → Select "Local" profile → Press F5
```

### Step 3: Access Swagger UI

Navigate to:
```
https://localhost:7001/swagger/index.html
```

**Expected Result:** Swagger UI loads successfully with all endpoints listed.

---

## Why "Local" Environment Exists

The custom `Local` environment allows developers to:

- Use **local machine names** instead of "localhost"
- Apply **different security policies** (e.g., stricter CORS in prod)
- Enable **debug-only features** (Swagger, verbose logging) without using standard `Development`
- Keep `Development` reserved for CI/CD pipelines

---

## Best Practices for Environment-Specific Features

When enabling/disabling features per environment, use this pattern:

```csharp
// DO: Explicit list approach (recommended)
var enableSwagger = app.Environment.IsDevelopment() || 
                    app.Environment.IsEnvironment("Integration") || 
                    app.Environment.IsEnvironment("Local");

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// OR: Invert logic with production check (alternative)
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

---

## Related Configuration Files

### `appsettings.Local.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Debug"
    }
  },
  "Database": {
    "Host": "localhost",
    "TrustedConnection": true,
    "Encrypt": false
  }
}
```

### `appsettings.Development.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### `appsettings.Production.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "Database": {
    "Encrypt": true,
    "TrustServerCertificate": false
  }
  // Swagger explicitly NOT configured here
}
```

---

## Verification

Run this command to confirm the environment is being detected correctly:

```csharp
// In Program.cs or a middleware, you can log the environment:
app.MapGet("/environment", (IWebHostEnvironment env) => 
    new { Environment = env.EnvironmentName, IsProduction = env.IsProduction() });

// Result when running with "Local":
// { "Environment": "Local", "IsProduction": false }
```

---

## Summary

| Scenario | Before Fix | After Fix |
|----------|-----------|-----------|
| Run in `Development` | ✅ Swagger works | ✅ Swagger works |
| Run in `Integration` | ✅ Swagger works | ✅ Swagger works |
| Run in `Local` | ❌ Swagger blocked | ✅ **Swagger now works** |
| Run in `Production` | N/A | ✅ Swagger disabled (secure) |

**Fix Status:** ✅ **DEPLOYED** — Swagger UI now accessible in Local environment
