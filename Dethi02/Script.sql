/*
Created		7/24/2023
Modified		7/24/2023
Project		
Model			
Company		
Author		
Version		
Database		MS SQL 2005 
*/


Drop table [Sanpham] 
go
Drop table [LoaiSP] 
go


Create table [LoaiSP]
(
	[MaLoai] Char(2) NOT NULL,
	[TenLoai] Nvarchar(30) NOT NULL,
Primary Key ([MaLoai])
) 
go

Create table [Sanpham]
(
	[MaSP] Char(6) NOT NULL,
	[TenSP] Nvarchar(30) NULL,
	[Ngaynhap] Datetime NULL,
	[MaLoai] Char(2) NOT NULL,
	[MaLoai] Char(2) NOT NULL,
Primary Key ([MaSP])
) 
go


Alter table [Sanpham] add  foreign key([MaLoai]) references [LoaiSP] ([MaLoai])  on update no action on delete no action 
go


Set quoted_identifier on
go


Set quoted_identifier off
go


