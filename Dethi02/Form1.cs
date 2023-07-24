using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dethi02
{
    public partial class frmSanpham : Form
    {
        SPContextDB dbcontext = new SPContextDB();
        List<Sanpham> listsanpham;
        public frmSanpham()
        {
            InitializeComponent();
        }

        private void frmSanpham_Load(object sender, EventArgs e)
        {
            listsanpham = dbcontext.Sanphams.ToList();
            List<LoaiSP> listloai = dbcontext.LoaiSPs.ToList();

            DanhSachSanPham(listsanpham);
            listloaisp(listloai);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string masanpham = txtMaSP.Text;
                Sanpham them = dbcontext.Sanphams.FirstOrDefault(s => s.MaSP == masanpham);

                if (them == null)
                {
                    Sanpham s = new Sanpham()
                    {
                        MaSP = txtMaSP.Text,
                        TenSP = txtTenSP.Text,
                        Ngaynhap = DateTime.Parse(dtNgaynhap.Text),
                        MaLoai = cboLoaiSP.Text,
                    };
                    dbcontext.Sanphams.Add(s);
                    dbcontext.SaveChanges();
                    DanhSachSanPham(dbcontext.Sanphams.ToList());
                    MessageBox.Show("Thêm dữ liệu thành công !");
                }
                else
                {
                    MessageBox.Show("Dữ liệu đã tồn tại !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi:" + ex.Message);
            }
        }
        public void DanhSachSanPham(List<Sanpham> listsanpham)
        {
            lvSanpham.Items.Clear();
            foreach (Sanpham sp in listsanpham)
            {
                ListViewItem lv = new ListViewItem(sp.MaSP);
                lv.SubItems.Add(sp.TenSP);
                lv.SubItems.Add(sp.Ngaynhap.ToString());
                lv.SubItems.Add(sp.MaLoai);
                lvSanpham.Items.Add(lv);
            }
        }
        public void listloaisp(List<LoaiSP> listloaisanpham)
        {
            cboLoaiSP.DataSource = listloaisanpham;
            cboLoaiSP.DisplayMember = "MaLoai";
            cboLoaiSP.ValueMember = "TenLoai";
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (lvSanpham.SelectedItems.Count > 0)
            {
                try
                {
                    string maSP = lvSanpham.SelectedItems[0].Text;
                    Sanpham sanPham = dbcontext.Sanphams.FirstOrDefault(sp => sp.MaSP == maSP);

                    if (sanPham != null)
                    {
                        
                        sanPham.TenSP = txtTenSP.Text;
                        sanPham.Ngaynhap = dtNgaynhap.Value;
                        sanPham.MaLoai = cboLoaiSP.Text;

                        dbcontext.SaveChanges();

                        
                       listsanpham = dbcontext.Sanphams.ToList();
                        DanhSachSanPham(listsanpham);

                        MessageBox.Show("CẬP NHẬT THÔNG TIN THÀNH CÔNG!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("SẢN PHẨM KHÔNG TỒN TẠI TRONG CƠ SỞ DỮ LIỆU!", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("LỖI: " + ex.Message, "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("VUI LÒNG CHỌN SẢN PHẨM ĐỂ CHỈNH SỮA!", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void lvSanpham_Click(object sender, EventArgs e)
        {
            if (lvSanpham.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvSanpham.SelectedItems[0];
                string maSP = selectedItem.SubItems[0].Text;
                Sanpham sanPhamChon = dbcontext.Sanphams.FirstOrDefault(sp => sp.MaSP == maSP);

                if (sanPhamChon != null)
                {

                    txtMaSP.Text = sanPhamChon.MaSP;
                    txtTenSP.Text = sanPhamChon.TenSP;
                    dtNgaynhap.Value = sanPhamChon.Ngaynhap.HasValue ? sanPhamChon.Ngaynhap.Value : DateTime.Now;
                    cboLoaiSP.Text = sanPhamChon.MaLoai;
                }
            }
            else
            {

                txtMaSP.Clear();
                txtTenSP.Clear();
                dtNgaynhap.Value = DateTime.Now;
                cboLoaiSP.SelectedIndex = -1;
            }
        }

        private void txtTimkiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimkiem.Text.ToLower().Trim();

            
            List<Sanpham> ketQuaTimKiem = dbcontext.Sanphams.Where(sp => sp.MaSP.ToLower().Contains(keyword) || sp.TenSP.ToLower().Contains(keyword)).ToList();

            if (ketQuaTimKiem.Count > 0)
            {
             
                DanhSachSanPham(ketQuaTimKiem);
                MessageBox.Show("TÌM KIẾM THÀNH CÔNG!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("KHÔNG TÌM THẤY SẢN PHẨM!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if(lvSanpham.SelectedItems.Count > 0)
            {
                try
                {
                    string maSP = lvSanpham.SelectedItems[0].Text;
                    Sanpham sanPhamXoa = dbcontext.Sanphams.FirstOrDefault(sp => sp.MaSP == maSP);

                    if (sanPhamXoa != null)
                    {

                        DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {

                            dbcontext.Sanphams.Remove(sanPhamXoa);
                            dbcontext.SaveChanges();


                            listsanpham = dbcontext.Sanphams.ToList();
                            DanhSachSanPham(listsanpham);

                            MessageBox.Show("XÓA SẢN PHẨM THÀNH CÔNG!", " THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("SẢN PHẨM KHÔNG TỒN TẠI TRONG CƠ SỞ DỮ LIỆU!", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("LỖI: " + ex.Message, "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("VUI LÒNG CHỌN SẢN PHẨM MUỐN XÓA!", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
