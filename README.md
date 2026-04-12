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

---

## 🔌 API Endpoints

### Products
- GET `/products?is_active={bool?}`
- POST `/add-product`
- PUT `/update-product`
- PATCH `/products/{productId}/status`

### Categories
- GET `/categories`
- POST `/add-category`
- PUT `/update-category`
- DELETE `/delete-category/{categoryId}`

### Delivery Options
- GET `/delivery-options`
- POST `/add-delivery-option`
- PUT `/update-delivery-option`
- DELETE `/delete-delivery-option/{deliveryOptionID}`

### Orders
- GET `/orders?is_fulfilled={int}`
- GET `/orders/{id}`
- POST `/add-order`
- PATCH `/update-order`