# Sleep Tracker

## Project Description
Sleep Tracker is a web-based ASP.NET Core MVC application for monitoring and analyzing sleep habits.  
Users can create sleep logs, record sleep-related factors, and review statistics about sleep quality and duration.

## Main Features
- Create, read, update, and delete sleep logs
- Create, read, update, and delete sleep factors
- Automatic sleep duration calculation
- Sleep quality rating from 1 to 5
- Statistics for:
    - average sleep duration for the last 7 days
    - average sleep duration for the last 30 days
    - best and worst night
    - factor analysis
    - sleep consistency
- Basic Admin/User role logic
- Validation for correct sleep input

## Technologies Used
- ASP.NET Core MVC
- Entity Framework Core
- SQLite
- LINQ
- Bootstrap

## Database Structure
The project uses Code First approach with the following main entities:
- User
- SleepLog
- SleepFactor

Relationships:
- One User -> Many SleepLogs
- One SleepLog -> Many SleepFactors

## OOP and Architecture
- BaseEntity is used as a base class for all models
- ISleepService and SleepService demonstrate abstraction and polymorphism
- Dependency Injection is used to inject services into controllers
- DTO is used to transfer sleep log data safely

## Security
- Basic Admin/User role logic is implemented
- Delete operation is protected for Admin only

## Validation
- Required fields
- Range validation for Quality (1-5)
- Custom validation through service:
    - sleep start cannot be in the future
    - sleep end cannot be in the future
    - sleep end must be after sleep start

## How to Run
1. Open the project in Rider or Visual Studio
2. Restore packages
3. Apply migrations
4. Run the project

## Author
Pavlo Kikavsky