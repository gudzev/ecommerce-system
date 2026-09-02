# E-Commerce System

## 🔗 Try out the App

Test the live app here:

- CURRENTLY UNAVAILABLE

---

## 📋 Description

This is a full e-commerce system that includes a website for customers to make orders on, desktop app for management of products, categories, orders and more, shared REST API and a database.

---

## 🛠️ Tech Stack

- React as front end
- .NET Minimal API as back end
- WPF as a desktop app
- SQL Server for database

---

## 🚧 To Do

- [ ] Add secure authentication and rate limiting to API
- [ ] Create a PC configurator using store's components for customers
- [✓] Organize desktop-app and back-end code in a better way
- [✓] Create separate table for storing multiple images for each product
- [✓] Add product pagination

---

## 🔌 API Endpoints

### Products

* GET `/products?is_active={bool}&category_id={int}&search_text={string}&product_ids={int[]}&page={int}&products_per_page={int}`
* GET `/products/{productId}`
* POST `/products`
* PUT `/products`
* PATCH `/products/{productId}/status?isActive={bool}`

### Product Pages

* GET `/product-pages?products_per_page={int}&category_id={int}&search_text={string}`

### Categories

* GET `/categories`
* POST `/categories`
* PUT `/categories`
* DELETE `/categories/{categoryId}`

### Delivery Options

* GET `/delivery-options`
* POST `/delivery-options`
* PUT `/delivery-options`
* DELETE `/delivery-options/{int}`

### Orders

* GET `/orders?is_fulfilled={int}`
* GET `/orders/{int}`
* POST `/orders`
* PATCH `/orders`

### Category Specifications

* GET `/category_specifications?category_id={int}`
* POST `/category_specifications`
* PUT `/category_specifications`
* DELETE `/category_specifications?category_specification_id={int}`

---

## Setup instructions

1. Run command "git clone https://github.com/gudzev/ecommerce-system.git" or download project files in a different way.
2. Import ecommerce-system .bacpac file from ecommerce-system/db/ directory in SQL server.
3. Open ecommerce-system/front-end and run command "npm install" (Node.JS must be installed for this to work).
4. Change connection strings in ecommerce-system/back-end/appsettings.json and ecommerce-system/desktop-app/appsettings.json to valid ones.
5. Using Visual Studio (or any other IDE), run .NET Minimal API (back-end) and run command "npm run dev" inside ecommerce-system/front-end, after which website will be displayed.