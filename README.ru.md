# SqlTypeUtils
[![en](https://img.shields.io/badge/lang-en-red.svg)](README.md)
[![ru](https://img.shields.io/badge/lang-ru-blue.svg)](README.ru.md)

Библиотека для проверки возможности преобразования строкового значения в указанный SQL-тип данных и сопоставление между .NET-типами, DbType и SqlDbType.

## ✨ Возможности

- Валидация значений SQL-типов данных с поддержкой форматирования
- Сопоставление между .NET-типами, DbType и SqlDbType
- Поддержка всех основных SQL Server типов данных
- Настраиваемые сообщения об ошибках с поддержкой локализации
- Расширенная валидация для специальных SQL-значений

## 📥 Установка

```bash
git clone https://github.com/SergPavlyk/SqlTypeUtils.git
```

## 🚀 Использование

### Валидация SQL-значений

```csharp
// Инициализация валидатора с русскоязычными сообщениями
ISqlValueValidatorMsgProvider msgProvider = new SqlValValidMessRu();
SqlValueValidator validator = new SqlValueValidator(msgProvider);

// Пример валидации
var dateResult = validator.Validate(SqlDataType.Date, "2025.01.17");
Console.WriteLine($"Date: {dateResult.outValue}, Message: {dateResult.message}");
```

### Сопоставление типов

```csharp
// .NET Type -> SqlDataType
SqlDataType? sqlType = SqlTypeMapper.GetSqlDataType(typeof(byte));

// SqlDataType -> SqlDbType (для ADO.NET)
SqlDbType sqlDbType = SqlTypeMapper.GetSqlDbType(SqlDataType.BigInt);

// SqlDataType -> DbType (для Dapper/EF)
DbType dbType = SqlTypeMapper.GetDbType(SqlDataType.Money);
```

Полный список примеров использования библиотеки доступен в файле [Program.cs](https://github.com/SergPavlyk/SqlTypeUtils/blob/main/SqlTypeUtils_Example/Program.cs)

## 💻 Требования

[![.Net](https://img.shields.io/badge/.NET-6.0+-512BD4?style=flat&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)

## 📄 Лицензия

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](http://www.apache.org/licenses/LICENSE-2.0)

SqlTypeUtils распространяется под лицензией Apache License 2.0. Полный текст лицензии находится в файле [LICENSE.txt](LICENSE.txt)

## 📫 Контакты

[![Email](https://img.shields.io/badge/Email-serg.pavlyk89%40gmail.com-blue?style=flat&logo=gmail&logoColor=white)](mailto:serg.pavlyk89@gmail.com)
[![GitHub Issues](https://img.shields.io/badge/Issues-GitHub-green?style=flat&logo=github&logoColor=white)](https://github.com/SergPavlyk/SqlTypeUtils/issues)
