# File Import
API to import CSV file

# Architectural overview
•	Layered Architecture
 
•	Presentation - FileImport.Api

•	Business Logic - FileImport.Application

•	Core/Domain Model - FileImport.Domain

•	Data Layer - FileImport.Persistence


# Explanation of solutions
•	API to file import: API will accept a csv file, perform basic validation and save the file to the configured location and then dispatch the file name to the File Processing Handler. Handler will parse the CSV and Normalize with the help of CSVParser and Normalize service. With the help of DataStorage repository, Handler then save data to two different data storage.

•	CSV data will be normalized as Products, Color & ColorCode (DeliveredIn, Q1, Size could be normalize as well)

•	Two data storage: MS SQL Server 2014 & JSON file on the Disk

•	Unit testing not done yet


# Tools and technologies
•	ASP.NET Core Web API 3.1

•	Entity Framework Core 3.1

•	Microsoft SQL Server 2014

•	.NET Core Native DI - Dependency Injection

•	TinyCsvParser -  to parse Csv and convert Csv data to model

•	NSwag for API Documentation

•	Newtonsoft.Json - To serialize and deserialize json

•	Serilog - for logging


# Improvement
•	Normalization could be apply for DeliveredIn, Q1, Size (Lookup table)

•	Global exception handling

•	Checking Id for Color & ColorCode in Database prior to populate Color & ColorCode from CSV to avoid duplication  

•	File processing could be run in worker process or backgroud using hangfire or Azure Web jobs or Functions

•	Separate validator to validate imported file using FluentValidation.

•	Scope to improve logging. 

•	Unit testing not done. 


