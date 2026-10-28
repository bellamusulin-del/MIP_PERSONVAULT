# MIP_Hazi2_Director-Manager-Employee
# PersonnelVault

A C# console application for managing company personnel. It loads people from a CSV file and lets you log in, view, update and delete them through a simple menu. There is no database: all data is kept in memory while the program runs.

## Features

- Loads employees, managers and one director from `Data/sample_people.csv` at startup
- Login with username and password
- Passwords are stored Caesar-encoded (shift of 5) and decoded when checking a login
- Change a person's username and password (the time of the last change is saved)
- View the time of the last password change for a person
- GDPR deletion: remove a person and log only the time of the deletion
- List all persons with their roles

## Class structure

- `Person`: abstract base class (id, name, username, encoded password)
- `Employee`: extends `Person`, adds department and salary
- `Manager`: extends `Employee`, adds team size
- `Director`: extends `Employee`, adds company. Only one Director can exist; creating a second throws an exception
- `CaesarCipher`: separate `Encode` and `Decode` methods
- `Vault`: keeps the persons in memory, imports the CSV, handles login, credential changes and deletion
- `Program`: console menu

## CSV format

name,role,department,salary,teamSize,company,username,password

Roles are `employee`, `manager` and `director`.

## How to run

Open `PersonnelVault.sln` in Visual Studio and press F5, or run `dotnet run` in the project folder.

Requires .NET 8 (change `TargetFramework` in `PersonnelVault.csproj` for another version).

## Note

The Caesar cipher is for educational purposes only and is not secure. A real system would use hashing (for example bcrypt) instead.
