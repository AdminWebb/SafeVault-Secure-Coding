# SafeVault-Secure-Coding

## Project Overview
This repository contains the completed SafeVault application for the Secure Coding assignment. The project demonstrates input validation, SQL injection prevention, authentication, RBAC, XSS prevention, and unit testing using ASP.NET Core. 

## Security Implementation Summary

### 1. Input Validation and SQL Injection Prevention
**What was implemented:** 
I replaced all raw SQL string concatenations with parameterized queries using Entity Framework and `SqlParameter`. I also implemented strict input validation using Data Annotations (Regex allow-lists) on our DTO models to ensure usernames only contain alphanumeric characters.
**How Copilot assisted:** 
I used Copilot prompts like *"Refactor this SQL query to use parameterized inputs to prevent SQL injection"* and *"Generate a regex validation attribute that only allows alphanumeric characters."* Copilot successfully generated the `SqlParameter` objects and the `[RegularExpression]` tags.

### 2. Authentication and Authorization (RBAC)
**What was implemented:** 
I implemented secure authentication using `BCrypt.Net-Next` to hash passwords before database storage. I also configured JWT Bearer authentication and Role-Based Access Control (RBAC). Specific endpoints are locked behind `[Authorize(Roles = "Admin")]`.
**How Copilot assisted:** 
I prompted Copilot with *"Generate a password hashing function using BCrypt in C#"* and *"How do I implement JWT Role-Based Access Control (RBAC) on a .NET controller?"* Copilot provided the hashing logic and the exact middleware attributes to restrict routing.

### 3. Debugging and Resolving Security Vulnerabilities
**What was implemented:** 
During debugging, I found Cross-Site Scripting (XSS) risks where user input was returned raw. I fixed this by utilizing `System.Text.Encodings.Web.HtmlEncoder` to sanitize outputs. I also removed hardcoded database connection strings, migrating them to `appsettings.json` and environment variables.
**How Copilot assisted:** 
I asked Copilot, *"Are there XSS vulnerabilities in returning this string, and how can I fix it in C#?"* Copilot identified the risk and generated the `HtmlEncoder.Default.Encode()` wrapper to sanitize the data.

### 4. Generating and Executing Security Tests
**What was implemented:** 
I used the `xUnit` testing framework to write automated tests. The tests verify that BCrypt correctly hashes passwords (and rejects bad passwords), that the XSS sanitizer neutralizes `<script>` tags, and that invalid inputs fail model validation.
**How Copilot assisted:** 
I prompted Copilot, *"Generate xUnit tests for my AuthController to verify password hashing and XSS sanitization."* Copilot generated a clean testing suite with `Assert` statements that successfully proved the security of the methods.