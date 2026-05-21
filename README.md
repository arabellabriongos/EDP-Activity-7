# EDP-Activity-7

## Brew & Bite Cafe - Sales and Inventory Management System

A Windows Forms desktop application built with C# and .NET for managing the daily operations of Brew & Bite Cafe.


## Features

### Dashboard
Displays today's overview showing: total sales, number of orders, number of customers, and products sold for the day. 
Also includes a sales line chart (viewable by Week, Month, or Year), a Top Selling This Month bar chart, and a Recent Orders table showing Order ID, customer name, date, items, total, and payment method.

### Products
Manage cafe menu items. Add, edit, or delete products with their category, price, and stock information.

### Orders
View and manage all customer orders. Shows order details including customer, items ordered, total amount, and payment method.

### Customers
Manage customer records including name and contact information.

### Reports
Generate three types of reports filtered by date range:
- **Order Report** - lists all orders with customer, product, quantity, amount, and date
- **Sales Report** - summarizes sales per order with payment method and total sales
- **Inventory Report** - shows all products with stock levels and status (In Stock, Low Stock, Out of Stock)

All reports can be exported to Excel.

### Users *(Admin only)*
Manage staff and admin accounts - add, edit, activate, or deactivate users.

### About
Displays information about the application.


## Technologies Used
- C# / .NET 10 (Windows Forms)
- MySQL Database
- ClosedXML (Excel export)
- MySql.Data connector

## Setup
1. Clone the repository
2. Set up the MySQL database using `database_setup.sql`
3. Update the connection string in `DatabaseConnection.cs`
4. Build and run with `dotnet run`


*Updated by abbriongos - collaborator*