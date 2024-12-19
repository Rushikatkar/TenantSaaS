
# Multi-Tenant SaaS Platform API Documentation

## Overview

This repository contains the API documentation for the Multi-Tenant SaaS Platform built using .NET Core Web API. The platform is designed to support multiple tenants, each with its own set of users, and provides features like CRUD operations, tenant isolation, and centralized logging.

### Key Features:
- **Multi-Tenant Architecture**: Each tenant has an isolated database.
- **Authentication and Authorization**: Handled using JWT tokens with role-based access control.
- **Centralized Logging**: Implemented using Serilog.
- **Dynamic Tenant Database Creation**: Automatic creation of a new database for each tenant upon registration.

---

## Application Workflow

1. **Tenant Management**:
   - Master Admin can register, update, deactivate, and manage tenants.
   - Each tenant gets an isolated database with the naming convention: `Tenant_<NameOfDatabase>`.
   - Connection strings are stored in the `MasterTenantDb`.

2. **Tenant Authentication**:
   - Login using email and password.
   - Upon successful login, a JWT token is generated, containing the `TenantId` for database access.

3. **User Management**:
   - User CRUD operations are isolated to each tenant’s database.
   - Role-based access control ensures proper permissions.

4. **Logging**:
   - Centralized logging is implemented using the Serilog framework.

---

## API Endpoints

### Tenant Controller

#### 1. **Register a Tenant**
`POST /api/tenant/register`  
Registers a new tenant and creates an isolated database.  
**Request Body**:
```json
{
  "tenantName": "string",
  "adminEmail": "string",
  "adminPasswordHash": "string",
  "adminPhoneNumber": 0,
  "adminFullName": "string"
}
```

#### 2. **Update a Tenant**
`PUT /api/tenant/update/{tenantId}`  
Updates tenant details.  
**Request Body**:
```json
{
  "tenantName": "string",
  "isActive": true,
  "adminEmail": "string",
  "adminPasswordHash": "string",
  "adminPhoneNumber": 0,
  "adminFullName": "string"
}
```

#### 3. **Deactivate a Tenant**
`PUT /api/tenant/deactivate/{tenantId}`  
Deactivates a tenant.

#### 4. **Login as a Tenant**
`POST /api/tenant/login`  
Authenticates tenant admin and returns a JWT token.  
**Request Body**:
```json
{
  "AdminEmail": "string",
  "AdminPassword": "string"
}
```

#### 5. **Delete a Tenant**
`DELETE /api/tenant/delete/{id}`  
Deletes a tenant.

---

### User Controller

#### 1. **Register a User**
`POST /api/user/register`  
Registers a user in the tenant’s database.  
**Request Body**:
```json
{
  "userName": "string",
  "email": "string",
  "password": "string",
  "role": "string"
}
```

#### 2. **Update a User**
`PUT /api/user/update/{userId}`  
Updates user details.  
**Request Body**:
```json
{
  "userName": "string",
  "email": "string",
  "role": "string",
  "isActive": true
}
```

#### 3. **Delete a User**
`DELETE /api/user/delete/{userId}`  
Deletes a user from the tenant’s database.

#### 4. **Get All Users**
`GET /api/user/getall`  
Retrieves all users in the tenant’s database.

#### 5. **Get User by ID**
`GET /api/user/byid/{userId}`  
Fetches a user by their ID.

---

## Logging

- **Framework**: Serilog  
- **Features**:
  - Tracks tenant-specific activities.
  - Provides centralized log management.

---

## Setup

1. Clone this repository.
2. Configure the database connection strings in the `appsettings.json` file.
3. Run the application using Visual Studio or the .NET CLI.

---

## License

This project is licensed under the MIT License. See the LICENSE file for details.
