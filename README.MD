# Поле для регистрации на C#
Это минималистичное приложение на blazor для регистрации пользователей с сохранением данных в PostgreSQL (pgAdmin4).
# Требования
+ NET Framework 4.7.2 или новее
+ PostgreSQL сервер
+ gAdmin4 для управления БД
+ Npgsql (PostgreSQL .NET Data Provider)
# Установка
1. Клонируйте репозиторий
2. Установите Npgsql через NuGet
   ``` C#
   Install-Package Npgsql -Version 4.1.10
   ```
3. Перейти в папку с проектом, где лежит файл docker-compose.yml.
    ```cmd
    cd путь к файлу
    ```
5. Выполнить команду:
   ```cmd
   docker-compose up --build
   ```
# Открыть приложение в браузере
После запуска приложение будет доступно по адресу:
``` 
   http://localhost5000:80
   ```