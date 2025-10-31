# 🐾 PetHome (Front Desk & Back Office Project)

---

## 🇪🇸 Descripción (Español)

**PetHome** es una aplicación web basada en **.NET** diseñada para ayudar a los usuarios a gestionar la estancia de mascotas en PetHome, su información y las operaciones relacionadas de manera eficiente.  
Este proyecto demuestra prácticas modernas de desarrollo backend con un enfoque en **arquitectura limpia (Clean Architecture)**, **mantenibilidad** y **seguridad**.

---
### 🚀 Tecnologías Utilizadas

- **.NET 9 / ASP.NET Core Web API**
- **Entity Framework Core (EF Core)** – ORM para operaciones con bases de datos
- **SQLlite** – SQLlite como bases de datos   
- **Clean Architecture** – separación de responsabilidades y alta capacidad de prueba  
- **MediatR** – implementación del patrón **CQRS**, simplificando el manejo de solicitudes y respuestas  
- **AutoMapper** – para el mapeo entre entidades y DTOs  
- **Identity Core - Autenticación y Seguridad** – autenticación basada en **JWT**, autorización por roles y endpoints seguros.
- **Fluent Validation** - para validacion de los requests.
- **Swagger** - para describir los REST APIs.

---
### ⚙️ Funcionalidades

- 🐶 Gestión de mascotas y registro  
- 🏠 Administración de dueños e información relacionada  
- 👥 Autenticación y autorización de usuarios (JWT)  
- 🧭 Operaciones CRUD con EF Core  
- 🧠 Patrón CQRS con MediatR  
- 🔁 Mapeo de objetos con AutoMapper
- ✔️ Validacion de los requests con Fluent Validation
- 🔢 Reportes en Excel usando CSVHelper
- 📸 Subida de imagenes con Cloudinary.

### 📝 Instrucciones
Un dueño (**Owner**) tiene 1 o varias mascotas (**Pets**). Cada **Pet** puede tener mas de una estancia temporal (**Stay**). Y cada **Stay** tiene una transaccion (**Transaction**)
1) Puedes usar swagger para ver los Rest APIs: http://localhost:5001/swagger/index.html
2) Usa el login con los siguientes credenciales: admin@gmail.com y Password123$
3) Usa el JsonWebToken que obtienes del response para colocarlo en el Authorize. Presiona Login y ahora tienes permisos de administrador.
4) Con los permisos de administrador puedes hacer uso de todos los Endpoints. CRUD.
5) Si quieres revisar como funciona la permisologia, registra un nuevo usuario y usa ese login para usar los Endpoints. Ese nuevo usuario no va a poder modificar, crear o eliminar.

---

## 🇺🇸 Description (English)

**PetHome** is a **.NET-based web application** designed to help users manage pet stays at PetHome, their information, and related operations efficiently.  
This project demonstrates modern backend development practices with a focus on **Clean Architecture**, **maintainability**, and **security**.

---

### 🚀 Tech Stack

- **.NET 9 / ASP.NET Core Web API**
- **Entity Framework Core (EF Core)** – ORM for database operations
- **SQLlite** – SQLlite as database   
- **Clean Architecture** – clear separation of concerns and high testability  
- **MediatR** – implements the **CQRS** pattern, simplifying request and response handling  
- **AutoMapper** – for mapping between entities and DTOs  
- **Identity Core – Authentication and Security** – JWT-based authentication, role-based authorization, and secure endpoints
- **Fluent Validation** - for validating the requests.
- **Swagger** - to describe the REST APIs.

---

### ⚙️ Features
- 🐶 Pet management and registration  
- 🏠 Owner management and related information  
- 👥 User authentication and authorization (JWT)  
- 🧭 CRUD operations with EF Core  
- 🧠 CQRS pattern using MediatR  
- 🔁 Object mapping with AutoMapper
- 🔢 Excel reports with CSVHelper
- 📸 Image upload with Cloudinary.

### 📝 Instructions
An (**Owner**) has one or more (**Pets**). Every **Pet** can stay once or more at PetHome. Each **Stay** requires a **Transaction**.
1) You can use Swagger to view the REST APIs: http://localhost:5001/swagger/index.html
2) Use the login with the following credentials: admin@gmail.com and Password123$
3) Use the JSON Web Token you get from the response to place it in Authorize. Press Login and now you have administrator permissions.
4) With administrator permissions, you can use all the endpoints. CRUD operations are available.
5) If you want to see how the permission system works, register a new user and use that login to access the other endpoints. They will not be able to modify, create, or delete.

