# PhoneBook Project by C# Academy
## Project Overview

The PhoneBook Project allows users to create and manage contacts in a phonebook application. Users can categorize contacts, add their details, and send emails directly from the app.

Project Link: [PhoneBook](https://www.thecsharpacademy.com/project/16/phonebook)

## Project Requirements
This application allows users to record contacts along with their phone numbers.

### Key features include:

- This is an application where you should record contacts with their phone numbers.
- Users should be able to Add, Delete, Update and Read from a database, using the console.
- You need to use Entity Framework, raw SQL isn't allowed.
- Your code should contain a base Contact class with AT LEAST {Id INT, Name STRING, Email STRING and Phone Number(STRING)}
- You should validate e-mails and phone numbers and let the user know what formats are expected
- You should use Code-First Approach, which means EF will create the database schema for you.
- You should use SQL Server, not SQLite

### Additional Challenges

- Create a functionality that allows users to add the contact's e-mail address and send an e-mail message from the app.
- Expand the app by creating categories of contacts (i.e. Family, Friends, Work, etc).
- What if you want to send not only e-mails but SMS?

## Lessons Learned

**1. Entity Framework**
  - Though I had a basic understanding, Started using MS tutorial and few videos on youtube. 
  - Even though I was able to get job done I took my time and spend hours about learnig as much as possible about this framework. 
  -  This helped me to discover more featured and helped during implementation of challenges.

**2. Secret Management:**
  - Learned how to handle sensitive information securely via the secrets.json file.

**3. SMTP Library:**
  - Gained experience with the SMTP library and integrated its functions into the Email Service.

**4. SMS:**
  - Briefly explored SMS capabilities but decided to postpone the implementation to focus on more important topics for future projects.

## Areas for Improvement

**1. Asynchronous Code:**
  - The code could be refactored to use asynchronous methods for better performance.
    
**2. SMS Functionality:**
  - Adding SMS functionality would enhance the application’s feature set.
    
**3. UI Service:**
  - The UI service could be separated into different components for better maintainability and scalability.

## Main Resources Used
Articles and videos about EF Core Framework.
SMTP client resources for email functionality.

## Packages Used
| Package | Version |
|---------|---------|
| Microsoft.EntityFrameworkCore.Design | 9.0.3 |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.3 |
| Microsoft.EntityFrameworkCore.Tools | 9.0.3 |
| Microsoft.Extensions.Configuration.UserSecrets | 6.0.1 |
| Microsoft.Extensions.DependencyInjection | 9.0.3 |
| Spectre.Console | 0.49.1 |
