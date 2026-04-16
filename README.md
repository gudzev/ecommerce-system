# EcommerceSystem

## 🔗 Try out the App

Test the live app here:

- https://gudzev-store.netlify.app/

---

## 📋 Description

This is a full e-commerce system consisting of:
- React web application for customers
- WPF desktop application for store management
- ASP.NET Minimal API backend hosted on Azure
- SQL Server database

All components communicate through a shared REST API.

---

## 🛠️ Tech Stack

- React
- C# (ASP.NET Minimal API)
- WPF (.NET)
- SQL Server

---

## 🚧 To Do

- [ ] Add secure authentication for API access
- [ ] Add input validation on backend
- [ ] Organize desktop-app and back-end code in a better way

---

## 🔌 API Endpoints

### Products
- GET `/products?is_active={bool?}`
- POST `/products`
- PUT `/products`
- PATCH `/products/{productId}/status`

### Categories
- GET `/categories`
- POST `/categories`
- PUT `/categories`
- DELETE `/categories/{categoryId}`

### Delivery Options
- GET `/delivery-options`
- POST `/delivery-options`
- PUT `/delivery-options`
- DELETE `/delivery-options/{deliveryOptionID}`

### Orders
- GET `/orders?is_fulfilled={int}`
- GET `/orders/{id}`
- POST `/orders`
- PATCH `/orders`