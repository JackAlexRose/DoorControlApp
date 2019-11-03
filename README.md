# DoorControlApp
A WPF application writted with a SQL database to report and control the current status of multiple doors.

The create table query in the root folder will generate a suitable table. Feel free to add or remove elements from the table, the window will resize and the elements will re-index to match.

Click on the status of a door to switch between open and closed, this will also update the SQL database.

Currently, the application will poll the database once every three seconds, a future goal is to implement a realtime event listener which responds to changes in the database. Unfortunately I couldn't work out the functionality of the SQL Dependency class.

There is an executable in the binary files of the application.
