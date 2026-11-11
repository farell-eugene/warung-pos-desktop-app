-- CREATE DATABASE
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'warung')
BEGIN
    CREATE DATABASE warung;
END
GO

GO

-- DROP TABLES (urutan dari dependent ke independen)
IF OBJECT_ID('notifikasi_stok', 'U') IS NOT NULL DROP TABLE notifikasi_stok;
IF OBJECT_ID('detail_transaksi', 'U') IS NOT NULL DROP TABLE detail_transaksi;
IF OBJECT_ID('transaksi', 'U') IS NOT NULL DROP TABLE transaksi;
IF OBJECT_ID('barang', 'U') IS NOT NULL DROP TABLE barang;
IF OBJECT_ID('pemilik_warung', 'U') IS NOT NULL DROP TABLE pemilik_warung;
GO

-- TABLE: pemilik_warung
CREATE TABLE pemilik_warung (
    id_pemilik INT PRIMARY KEY IDENTITY(1,1),
    nama NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) UNIQUE NOT NULL,
    password NVARCHAR(255) NOT NULL
);
GO

-- INSERT admin user
INSERT INTO pemilik_warung (nama, email, password)
VALUES ('Admin', 'admin@gmail.com', '12345678');
GO

-- TABLE: barang
CREATE TABLE barang (
    id_barang INT PRIMARY KEY IDENTITY(1,1),
    nama_barang NVARCHAR(100) NOT NULL,
    kategori NVARCHAR(50),
    harga DECIMAL(10,2) NOT NULL,
    stok INT NOT NULL,
    id_pemilik INT,
    FOREIGN KEY (id_pemilik) REFERENCES pemilik_warung(id_pemilik)
        ON DELETE CASCADE
);
GO

-- TABLE: transaksi
CREATE TABLE transaksi (
    id_transaksi INT PRIMARY KEY IDENTITY(1,1),
    tanggal DATETIME NOT NULL DEFAULT GETDATE(),
    total_harga DECIMAL(12,2) NOT NULL,
    id_pemilik INT,
    FOREIGN KEY (id_pemilik) REFERENCES pemilik_warung(id_pemilik)
        ON DELETE CASCADE
);
GO

CREATE TABLE detail_transaksi (
    id_detail INT PRIMARY KEY IDENTITY(1,1),
    id_transaksi INT,
    id_barang INT,
    jumlah INT NOT NULL,
    total_harga DECIMAL(12,2) NOT NULL,
    FOREIGN KEY (id_transaksi) REFERENCES transaksi(id_transaksi)
        ON DELETE CASCADE,
    FOREIGN KEY (id_barang) REFERENCES barang(id_barang)
        ON DELETE NO ACTION
);

GO

-- TABLE: notifikasi_stok
CREATE TABLE notifikasi_stok (
    id_notifikasi INT PRIMARY KEY IDENTITY(1,1),
    id_barang INT,
    tanggal DATETIME DEFAULT GETDATE(),
    pesan NVARCHAR(MAX),
    FOREIGN KEY (id_barang) REFERENCES barang(id_barang)
        ON DELETE CASCADE
);
GO
