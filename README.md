# GMapTestTask
**Тестовое задание на вакансию Junior С#**
## Задание
На языке C# необходимо разработать приложение Windows Forms, которое на главной форме содержит карту из библиотеки GMap.NET, загружает из базы данных Microsoft SQL Server географические координаты условных единиц техники и отображает их на карте в виде маркеров. Так же необходимо реализовать перемещение маркеров с помощью мыши (Drag&Drop, то есть нажать на маркер и перенести в другую точку карты) и сохранение новых координат в базу данных, чтобы после перезапуска приложения маркеры были расположены так же, как и до закрытия приложения.

Выполненным заданием будет считаться архив с проектом Visual Studio, а также SQL-скриптом для создания базы данных. В базе данных уже должны содержаться маркеры. При работе с базой данных нельзя пользоваться никакими фреймворками (EntityFramework и т.д.), необходимо использовать T-SQL.

## Запуск:
Для создания БД необходимо выпонить скрипт CreateDB.sql в Microsoft SQL Server Management Studio, скачать файлы: Code -> Download ZIP, распаковать и запустить GMap.sln в Microsoft Visual Studio, или из Microsoft Visual Studio выполнить клонирование репозитория: расположение https://github.com/Katerina-Seren/GMapTestTask.git -> клонировать. 
##
В Map.cs измените 37 строка unitService = new UnitService(@"Data Source=ваша строка подключения");
При запуске может не подгрузиться пакет GMap.NET.WindowsForms, в таком случае удалите его и загрузите снова через NuGet.
