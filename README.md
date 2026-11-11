# Warung POS: Desktop Point-of-Sale App (C# WinForms)

A desktop point-of-sale and inventory application for a small Indonesian shop (*warung*). It covers login, product stock management, sales transactions with a shopping cart, low-stock notifications and printable reports with **Crystal Reports**. Visual Programming course final project, built by a team of five.

## Features

| Module | Form | Description |
|---|---|---|
| Authentication | `FormLogin` | Shop-owner login (`pemilik_warung` table) |
| Dashboard | `FormDashboard` | Navigation hub and stock overview |
| Products | `FormBarang`, `FormTambah` | List, add, edit and delete products and stock |
| Transactions | `FormTransaksi` | Cart-based checkout (`KeranjangItem`), automatic stock deduction |
| Reports | `FormLaporan`, `LaporanTransaksi.rpt` | Transaction report rendered with Crystal Reports |
| Stock alerts | `notifikasi_stok` table | Low-stock notifications |

## Database

SQL Server database `warung` (script: [`database/warung.sql`](database/warung.sql)):

| Table | Purpose |
|---|---|
| `pemilik_warung` | Shop owners / users |
| `barang` | Products and stock |
| `transaksi` | Transaction headers |
| `detail_transaksi` | Transaction line items |
| `notifikasi_stok` | Low-stock notifications |

## Getting started

**Requirements:** Windows, Visual Studio 2019+ with the *.NET desktop development* workload, .NET Framework 4.7.2, SQL Server Express, and SAP Crystal Reports runtime for Visual Studio (for the report form).

1. Create the database: run `database/warung.sql` in SQL Server Management Studio.
2. Open `Warung.sln` in Visual Studio.
3. Check the connection string in `Warung/App.config` and `Warung/DbClient.cs`. It defaults to `Data Source=.\SQLEXPRESS;Initial Catalog=warung;Integrated Security=True`.
4. Build and run (F5). The SQL script seeds a demo owner account (`admin@gmail.com` / `12345678`) for the first login.

## Project structure

```
Warung.sln
Warung/
├── Program.cs              # entry point
├── DbClient.cs             # SQL Server data access
├── Form*.cs                # WinForms screens (login, dashboard, products, transactions, reports)
├── Barang.cs, Transaksi.cs, KeranjangItem.cs, PemilikWarung.cs   # domain models
├── LaporanTransaksi.rpt    # Crystal Reports template
└── warungDataSet.xsd       # typed dataset for reporting
database/warung.sql         # database schema
```

## Tech stack

C# · .NET Framework 4.7.2 · Windows Forms · ADO.NET · SQL Server · Crystal Reports
