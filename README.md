# EcommerceSystem

## 🔗 Try out the App

Test the live app here:

- CURRENTLY UNAVAILABLE

---

## 📋 Description

This is a full e-commerce system for PC hardware consisting of:
- React web application for customers
- WPF desktop application for store management
- ASP.NET Minimal API
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
- [ ] Create a PC configurator using store's components for customers
- [ ] Create separate table for storing multiple images for each product

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

---

## Setup instructions

- If you want to test out the website, along with the desktop application, you will have to import web_store.bacpac file in SQL server, and change the corresponding ConnectionStrings in desktop-app/appsettings.json and back-end/appsettings.json