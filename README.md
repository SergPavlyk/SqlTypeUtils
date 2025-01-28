# SqlTypeUtils
[![en](https://img.shields.io/badge/lang-en-red.svg)](README.md)
[![ru](https://img.shields.io/badge/lang-ru-blue.svg)](README.ru.md)


A library for validating string value conversions to specified SQL data types and mapping between .NET types, DbType, and SqlDbType.

## ✨ Features
- SQL data type value validation with formatting support
- Mapping between .NET types, DbType, and SqlDbType
- Support for all major SQL Server data types
- Customizable error messages with localization support
- Advanced validation for special SQL values

## 📥 Installation
```bash
git clone https://github.com/SergPavlyk/SqlTypeUtils.git
```

## 🚀 Usage
### SQL Value Validation
```csharp
// Initialize validator with English messages
ISqlValueValidatorMsgProvider msgProvider = new SqlValValidMessEn();
SqlValueValidator validator = new SqlValueValidator(msgProvider);

// Validation example
var dateResult = validator.Validate(SqlDataType.Date, "2025.01.17");
Console.WriteLine($"Date: {dateResult.outValue}, Message: {dateResult.message}");
```

### Type Mapping
```csharp
// .NET Type -> SqlDataType
SqlDataType? sqlType = SqlTypeMapper.GetSqlDataType(typeof(byte));

// SqlDataType -> SqlDbType (for ADO.NET)
SqlDbType sqlDbType = SqlTypeMapper.GetSqlDbType(SqlDataType.BigInt);

// SqlDataType -> DbType (for Dapper/EF)
DbType dbType = SqlTypeMapper.GetDbType(SqlDataType.Money);
```

For a complete list of library usage examples, see [Program.cs](https://github.com/SergPavlyk/SqlTypeUtils/blob/main/SqlTypeUtils_Example/Program.cs)

## 💻 Requirements
[![.Net](https://img.shields.io/badge/.NET-6.0+-512BD4?style=flat&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)

## 📄 License
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](http://www.apache.org/licenses/LICENSE-2.0)

SqlTypeUtils is distributed under the Apache License 2.0. The complete license text can be found in the [LICENSE.txt](LICENSE.txt) file.

## 📫 Contact
[![Email](https://img.shields.io/badge/Email-serg.pavlyk89%40gmail.com-blue?style=flat&logo=gmail&logoColor=white)](mailto:serg.pavlyk89@gmail.com)
[![GitHub Issues](https://img.shields.io/badge/Issues-GitHub-green?style=flat&logo=github&logoColor=white)](https://github.com/SergPavlyk/SqlTypeUtils/issues)
